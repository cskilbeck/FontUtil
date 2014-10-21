using System;
using System.Drawing;

using Graphic = FontUtil.Graphic;
using ChannelMask = FontUtil.ChannelMask;
using ColorChannel = FontUtil.ColorChannel;
using PluginAttribute = FontUtil.PluginAttribute;
using PluginDefaults = FontUtil.PluginDefaults;
using RawBitmap = FontUtil.RawBitmap;
using RangedPointF = FontUtil.RangedPointF;

namespace MyPlugins
{
	public static unsafe class Ops
	{
		class CopyChannelDefaults
		{
			ColorChannel Source { get { return ColorChannel.Alpha; } }
		}

		[Plugin("Copy one color channel to another", true)]
		[PluginDefaults(typeof(CopyChannelDefaults))]
		public static Graphic CopyChannel(Graphic graphic, ColorChannel Source, ColorChannel Destination = ColorChannel.Alpha)
		{
			if (Source != Destination)
			{
				unsafe
				{
					int w = graphic.bmp.Width;
					int h = graphic.bmp.Height;

					int dc = (int)Destination;
					int sc = (int)Source;

					using (RawBitmap rawsrc = new RawBitmap(graphic.bmp))
					{
						for (int y = 0; y < h; ++y)
						{
							byte* ptr = rawsrc[0, y];
							for (int x = 0; x < w; ++x)
							{
								ptr[dc] = ptr[sc];
								ptr += 4;
							}
						}
					}
				}
			}
			return graphic;
		}

		class SubtractImageDefaults
		{
			RangedPointF offset { get { return new RangedPointF(new PointF(-50, -50), new PointF(50, 50), PointF.Empty); } }
		}

		[Plugin("Subtract one iamge from another", true)]
		[PluginDefaults(typeof(SubtractImageDefaults))]
		public static Graphic SubImage(Graphic source, Graphic mask, RangedPointF offset)
		{
			Graphic newGraphic = new Graphic(source);
			unsafe
			{
				int w = Math.Min(source.bmp.Width, mask.bmp.Width);
				int h = Math.Min(source.bmp.Height, mask.bmp.Height);

				int srcOffsetX = Math.Abs(source.bmp.Width - mask.bmp.Width) / 2;
				int srcOffsetY = Math.Abs(source.bmp.Width - mask.bmp.Width) / 2;

				int maskOffsetX = 0;
				int maskOffsetY = 0;

				if (source.bmp.Width < mask.bmp.Width)
				{
					maskOffsetX = srcOffsetX;
					srcOffsetX = 0;
				}

				if (source.bmp.Height < mask.bmp.Height)
				{
					maskOffsetY = srcOffsetY;
					srcOffsetY = 0;
				}

				Random r = new Random();

				using (RawBitmap rawsrc = new RawBitmap(newGraphic.bmp))
				{
					using (RawBitmap rawmask = new RawBitmap(mask.bmp))
					{
						for (int y = 0; y < h; ++y)
						{
							for (int x = 0; x < w; ++x)
							{
								byte* s = rawsrc[x + srcOffsetX, y + srcOffsetY];
								byte* m = rawmask[x + maskOffsetX, y + maskOffsetY];

								for (int z = 0; z < 4; ++z)
								{
									int sp = s[z];
									int mp = m[z];
									int rp = sp - mp;
									if (rp < 0)
									{
										rp = 0;
									}
									s[z] = (byte)rp;
								}
							}
						}
					}
				}
			}
			return newGraphic;
		}

		class ClipDefaults
		{
			Color Color { get { return Color.FromArgb(128, 128, 128, 128); } }
		}

		[Plugin("Clip below threshold", true)]
		[PluginDefaults(typeof(ClipDefaults))]
		public static Graphic Clip(Graphic graphic, Color Color)
		{
			Graphic newGraphic = new Graphic(graphic);
			unsafe
			{
				using (RawBitmap raw = new RawBitmap(newGraphic.bmp))
				{
					int r = Color.R;
					int g = Color.G;
					int b = Color.B;
					int a = Color.A;

					int w = raw.Width;
					int h = raw.Height;

					for (int y = 0; y < h; ++y)
					{
						byte* ptr = raw[0, y];

						for (int x = 0; x < w; ++x)
						{
							if (ptr[0] < b) ptr[0] = 0;
							if (ptr[1] < g) ptr[1] = 0;
							if (ptr[2] < r) ptr[2] = 0;
							if (ptr[3] < a) ptr[3] = 0;

							ptr += 4;
						}
					}
				}
			}
			return newGraphic;
		}

		class MultiplyDefaults
		{
			Color Color { get { return Color.Black; } }
		}

		[Plugin("Multiply by a Color", true)]
		[PluginDefaults(typeof(MultiplyDefaults))]
		public static Graphic Multiply(Graphic graphic, Color Color)
		{
			int rm = Color.R;
			int gm = Color.G;
			int bm = Color.B;
			int am = Color.A;
			Graphic newGraphic = new Graphic(graphic);
			unchecked
			{
				unsafe
				{
					using (RawBitmap raw = new RawBitmap(newGraphic.bmp))
					{
						for (int y = 0; y < raw.Height; ++y)
						{
							byte* p = raw[0, y];
							for (int x = 0; x < raw.Width; ++x)
							{
								p[0] = (byte)(p[0] * bm >> 8);
								p[1] = (byte)(p[1] * gm >> 8);
								p[2] = (byte)(p[2] * rm >> 8);
								p[3] = (byte)(p[3] * am >> 8);
								p += 4;
							}
						}
					}
				}
			}
			return newGraphic;
		}

		class OutlineDefaults
		{
			Color Color { get { return Color.Black; } }
		}

		[Plugin("Set any non-empty pixel to a certain color", true)]
		public static Graphic Set(Graphic graphic, Color color)
		{
			Graphic newGraphic = graphic.Clone();
			UInt32 col = (UInt32)color.ToArgb();
			using (RawBitmap r = new RawBitmap(newGraphic.bmp))
			{
				for(int y=0; y<newGraphic.bmp.Height; ++y)
				{
					UInt32 *p = (UInt32 *)r[0,y];
					for (int x = 0; x < newGraphic.bmp.Width; ++x )
					{
						if ((p[x] & 0xff000000) != 0)
						{
							p[x] = col;
						}
					}
				}
			}
			return newGraphic;
		}

		[Plugin("Create an outline, using Alpha channel as input", true)]
		[PluginDefaults(typeof(OutlineDefaults))]
		public static Graphic Outline(Graphic graphic, Color Color, RangedPointF Offset)
		{
			Graphic newGraphic = graphic.AddBorders(2, 2, 1, 1);
			PointF offset = Offset;
			newGraphic.drawOffset.X += offset.X;
			newGraphic.drawOffset.Y += offset.Y;
			if (newGraphic.bmp != null)
			{
				Bitmap newBmp = newGraphic.bmp;
				int w = graphic.bmp.Width;
				int h = graphic.bmp.Height;
				Color o = Color.FromArgb(255, Color);
				for (int y = 0; y < h; ++y)
				{
					for (int x = 0; x < w; ++x)
					{
						int a = graphic.bmp.GetPixel(x, y).A;
						if (a != 0)
						{
							newBmp.SetPixel(x + 0, y + 0, o);
							newBmp.SetPixel(x + 0, y + 1, o);
							newBmp.SetPixel(x + 0, y + 2, o);
							newBmp.SetPixel(x + 1, y + 0, o);
							newBmp.SetPixel(x + 1, y + 1, o);
							newBmp.SetPixel(x + 1, y + 2, o);
							newBmp.SetPixel(x + 2, y + 0, o);
							newBmp.SetPixel(x + 2, y + 1, o);
							newBmp.SetPixel(x + 2, y + 2, o);
						}
					}
				}
			}
			return newGraphic;
		}

		[Plugin("Combine 2 together", true)]
		public static Graphic Combine(Graphic a, Graphic b)
		{
			double left = Math.Min(a.drawOffset.X, b.drawOffset.X);
			double right = Math.Max(a.drawOffset.X + a.bmp.Width, b.drawOffset.X + b.bmp.Width);
			double top = Math.Min(a.drawOffset.Y, b.drawOffset.Y);
			double bottom = Math.Max(a.drawOffset.Y + a.bmp.Height, b.drawOffset.Y + b.bmp.Height);
			double w = right - left;
			double h = bottom - top;
			w = Math.Round(w + 0.5);
			h = Math.Round(h + 0.5);
			Bitmap newBitmap = new Bitmap((int)w, (int)h);

			float xd = Math.Max(a.drawOffset.X - b.drawOffset.X, 0);
			float yd = Math.Max(a.drawOffset.Y - b.drawOffset.Y, 0);
			PointF newDrawOffset = new PointF(a.drawOffset.X - xd, a.drawOffset.Y - yd);

			Graphic g = new Graphic(a.glyph, newBitmap, newDrawOffset);
			using (Graphics graphics = Graphics.FromImage(newBitmap))
			{
				graphics.DrawImage(a.bmp, new PointF((float)(a.drawOffset.X - left), (float)(a.drawOffset.Y - top)));
				graphics.DrawImage(b.bmp, new PointF((float)(b.drawOffset.X - left), (float)(b.drawOffset.Y - top)));
			}
			return g;
		}

		[Plugin("Apply mask (clears out some of RGBA channels)", true)]
		public static Graphic Mask(Graphic graphic, ChannelMask Mask = ChannelMask.Red | ChannelMask.Green | ChannelMask.Blue)
		{
			UInt32 mask = (UInt32)Mask;
			Graphic newg = new Graphic(graphic);
			if (graphic.bmp != null)
			{
				unsafe
				{
					int w = graphic.bmp.Width;
					int h = graphic.bmp.Height;

					using (RawBitmap rawsrc = new RawBitmap(newg.bmp))
					{
						for (int y = 0; y < h; ++y)
						{
							UInt32* ptr = (UInt32*)rawsrc[0, y];
							for (int x = 0; x < w; ++x)
							{
								ptr[x] = ptr[x] & mask;
							}
						}
					}
				}
			}
			return newg;
		}

		class OffsetDefaults
		{
			RangedPointF Offset { get { return new RangedPointF(new PointF(-50, -50), new PointF(50, 50), new PointF(0, 0)); } }
		}

		[Plugin("Offset the graphic", false)]
		[PluginDefaults(typeof(OffsetDefaults))]
		public static Graphic Offset(Graphic graphic, RangedPointF Offset)
		{
			Bitmap b = new Bitmap(graphic.bmp);
			PointF offset = new PointF(Offset.Value.X, Offset.Value.Y);
			return new Graphic(graphic.glyph, b, new PointF(graphic.drawOffset.X + offset.X, graphic.drawOffset.Y + offset.Y));
		}
	}
}
