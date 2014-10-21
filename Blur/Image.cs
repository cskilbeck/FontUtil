using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using BitmapUtilities;

namespace FontUtil
{
	public static class Image
	{
		[Plugin("Blur a bitmap with a given radius", true)]
		public static Graphic Blur(Graphic image, int BlurRadius = 5)
		{
			Graphic n = image.AddBorders(BlurRadius * 2, BlurRadius * 2, BlurRadius, BlurRadius);
			GaussianBlur g = new GaussianBlur(BlurRadius);
			n.bmp = g.ProcessImage(n.bmp);
			return n;
		}

		private class Offset
		{
			public int x, y;
			public int l;
			public Color color;

			public Offset(int x, int y, Color color)
			{
				this.color = color;
				this.x = x;
				this.y = y;
				l = (int)Math.Sqrt(x * x + y * y);
			}
		}

		public static class GlowifyDefaults
		{
			public static NamedBitmap Bitmap
			{
				get
				{
					return new NamedBitmap("Default", new Bitmap(10,10));
				}
			}
		}

		[Plugin("Glowify using a source texture", true)]
		[PluginDefaults(typeof(GlowifyDefaults))]
		public unsafe static Graphic Glowify(Graphic image, NamedBitmap Bitmap, ChannelMask ChannelMask = ChannelMask.Alpha)
		{
			Bitmap bmp = Bitmap;
			// result bmp
			Bitmap dest = new Bitmap(image.bmp.Width + bmp.Width, image.bmp.Height + bmp.Height);
			dest.Fill(Color.FromArgb(0, 0, 0, 0));

			// thick outline of pixels that need proper checking
			Bitmap expandedSrc = new Bitmap(dest.Width, dest.Height);
			expandedSrc.Fill(Color.FromArgb(0, 0xff, 0, 0));

			// scan glow bitmap for list of non-empty pixels
			List<Offset> offsets = new List<Offset>();

			int halfWidth = bmp.Width / 2;
			int halfHeight = bmp.Height / 2;

			for (int y = 0; y < bmp.Height; ++y)
			{
				for (int x = 0; x < bmp.Width; ++x)
				{
					Color c = bmp.GetPixel(x, y);
					if (c != Color.Empty)
					{
						offsets.Add(new Offset(x - halfWidth, y - halfHeight, c));
					}
				}
			}

			// ordered by decreasing length
			Offset[] offsets1 = offsets.OrderBy(x => -x.l).ToArray();

			// and increasing length
			Offset[] offsets2 = offsets.OrderBy(x => x.l).ToArray();

			int numOffsets = offsets1.Count();

			int srcWidth = image.bmp.Width;
			int srcHeight = image.bmp.Height;
			int dstWidth = expandedSrc.Width;
			int dstHeight = expandedSrc.Height;

			UInt32 mask = (UInt32)ChannelMask;

			using (RawBitmap rawSrc = new RawBitmap(image.bmp))
			{
				// expand around the source initially
				using (RawBitmap rawSrcExpanded = new RawBitmap(expandedSrc))
				{
					for (int y = 0; y < dstHeight; ++y)
					{
						for (int x = 0; x < dstWidth; ++x)
						{
							for (int i = 0; i < numOffsets; ++i)
							{
								Offset o = offsets1[i];

								int px = x + o.x - halfWidth;
								int py = y + o.y - halfHeight;

								if (px > 0 && py > 0 && px < srcWidth && py < srcHeight)
								{
									if ((*(uint*)rawSrc[px, py] & mask) != 0)
									{
										*((uint*)rawSrcExpanded[x, y]) = 0xffffffff;
										break;
									}
								}
							}
						}
					}
				}

				// then fill in the actual results
				using (RawBitmap rawSrcExpanded = new RawBitmap(expandedSrc))
				{
					using (RawBitmap rawDest = new RawBitmap(dest))
					{
						for (int y = 0; y < dstHeight; ++y)
						{
							for (int x = 0; x < dstWidth; ++x)
							{
								if (*(uint*)rawSrcExpanded[x, y] != 0)
								{
									for (int i = 0; i < numOffsets; ++i)
									{
										Offset o = offsets2[i];

										int px = x + o.x - halfWidth;
										int py = y + o.y - halfHeight;

										if (px > 0 && py > 0 && px < srcWidth && py < srcHeight)
										{
											if ((*(uint*)rawSrc[px, py] & mask) != 0)
											{
												*((int*)rawDest[x, y]) = o.color.ToArgb();
												break;
											}
										}
									}
								}
							}
						}
					}
				}
			}

			return new Graphic(image.glyph, dest, image.drawOffset.Subtract(new Point(bmp.Width / 2, bmp.Height / 2)));
		}

		[Plugin("Apply a texture", true)]
		public unsafe static Graphic Texturize(Graphic image, NamedBitmap Bitmap)
		{
			Bitmap bmp = Bitmap;
			Bitmap newBmp = new Bitmap(image.bmp.Width, image.bmp.Height);

			using (RawBitmap rawSrc = new RawBitmap(image.bmp))
			{
				using (RawBitmap rawDst = new RawBitmap(newBmp))
				{
					using (RawBitmap rawTexture = new RawBitmap(bmp))
					{
						// notwithstanding offsets yet

						int h = newBmp.Height;
						int w = newBmp.Width;

						int th = bmp.Height;
						int tw = bmp.Width;

						byte *tex = rawTexture.Begin;

						float oy = Math.Abs(image.drawOffset.Y);
						float ox = Math.Abs(image.drawOffset.X);

						int ty = (int)oy % bmp.Height;

						for (int y = 0; y < h; ++y)
						{
							byte* src = rawSrc[0, y];
							byte* dst = rawDst[0, y];

							int tx = (int)ox % bmp.Width;

							tex = rawTexture[tx, ty];

							for (int x = 0; x < w; ++x)
							{
								int sb = *src++;
								int sg = *src++;
								int sr = *src++;
								int sa = *src++;

								int tb = *tex++;
								int tg = *tex++;
								int tr = *tex++;
								int ta = *tex++;

								int da = sa;
								int dr = sr * tr >> 8;
								int dg = sg * tg >> 8;
								int db = sb * tb >> 8;

								*dst++ = (byte)db;
								*dst++ = (byte)dg;
								*dst++ = (byte)dr;
								*dst++ = (byte)da;

								if (++tx == tw)
								{
									tx = 0;
									tex = rawTexture[tx, ty];
								}
							}
							if (++ty == th)
							{
								ty = 0;
							}
						}
					}
				}
			}

			Graphic newGraphic = new Graphic(image.glyph, newBmp, image.drawOffset);
			return newGraphic;
		}
	}
}
