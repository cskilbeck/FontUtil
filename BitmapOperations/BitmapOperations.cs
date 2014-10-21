using System;
using System.Drawing;

using Graphic = FontUtil.BitmapFont.Glyph.Graphic;
using ChannelMask = FontUtil.ChannelMask;
using ColorChannel = FontUtil.ColorChannel;
using PluginAttribute = FontUtil.PluginAttribute;
using PluginDefaults = FontUtil.PluginDefaults;
using RawBitmap = FontUtil.RawBitmap;
using RangedPoint = FontUtil.RangedPoint;

namespace MyPlugins
{
	public static class Ops
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

		[Plugin("Create an outline, using Alpha channel as input", true)]
		[PluginDefaults(typeof(OutlineDefaults))]
		public static Graphic Outline(Graphic graphic, Color Color, RangedPoint Offset)
		{
			Graphic newGraphic = graphic.AddBorders(2, 2, 1, 1);
			Point offset = Offset;
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
			int left = Math.Min(a.drawOffset.X, b.drawOffset.Y);
			int right = Math.Max(a.drawOffset.X + a.bmp.Width, b.drawOffset.Y + b.bmp.Width);
			int top = Math.Min(a.drawOffset.Y, b.drawOffset.Y);
			int bottom = Math.Max(a.drawOffset.Y + a.bmp.Height, b.drawOffset.Y + b.bmp.Height);
			int w = right - left;
			int h = bottom - top;
			Bitmap newBitmap = new Bitmap(w, h);

			Graphic g = new Graphic(a.glyph, newBitmap, a.drawOffset);
			using (Graphics graphics = Graphics.FromImage(newBitmap))
			{
				graphics.DrawImage(a.bmp, new Point(a.drawOffset.X - left, a.drawOffset.Y - top));
				graphics.DrawImage(b.bmp, new Point(b.drawOffset.X - left, b.drawOffset.Y - top));
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
			RangedPoint Offset { get { return new RangedPoint(new Point(-50, -50), new Point(50, 50), new Point(0, 0)); } }
		}

		[Plugin("Offset the graphic", false)]
		[PluginDefaults(typeof(OffsetDefaults))]
		public static Graphic Offset(Graphic graphic, RangedPoint Offset)
		{
			Bitmap b = new Bitmap(graphic.bmp);
			Point offset = Offset;
			return new Graphic(graphic.glyph, b, new Point(graphic.drawOffset.X + offset.X, graphic.drawOffset.Y + offset.Y));
		}
	}
}
