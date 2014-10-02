using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace FontUtil
{
	public class Graphic : IComparable
	{
		public PointF drawOffset;			// where to plot this image relative to cursospos
		public Bitmap bmp;					// temp. bitmap

		public Rectangle glyphPosition;		// where on the page this image is
		public int hash32;				// for dup detection
		public int pageIndex;				// which page it's on
		public Glyph glyph;
		public bool counted;

		public Bitmap PageBitmap
		{
			get
			{
				if (pageIndex != -1 && glyph != null)
				{
					Bitmap f = glyph.font.pages[pageIndex].bitmap;
					Bitmap n = new Bitmap(glyphPosition.Width, glyphPosition.Height);
					using (Graphics g = Graphics.FromImage(n))
					{
						g.DrawImage(f, new Rectangle(Point.Empty, n.Size), glyphPosition, GraphicsUnit.Pixel);
					}
					return n;
				}
				return null;
			}
		}

		public Graphic()
		{
			pageIndex = -1;
		}

		public Graphic(Graphic other)
		{
			glyph = other.glyph;
			counted = other.counted;
			hash32 = other.hash32;
			pageIndex = other.pageIndex;
			drawOffset = other.drawOffset;
			if (other.bmp != null)
			{
				bmp = (Bitmap)other.bmp.Clone();
			}
		}

		public int CompareTo(object o)
		{
			Graphic i = o as Graphic;
			if (bmp == null || i.bmp == null)
			{
				return 0;
			}
			if (BitmapFont.FontPackingType == PackingType.Arevalo)
			{
				return -Math.Max(bmp.Height, bmp.Width).CompareTo(Math.Max(i.bmp.Width, i.bmp.Height));
			}
			else
			{
				int c = bmp.Height.CompareTo(i.bmp.Height);
				if (c == 0)
				{
					c = bmp.Width.CompareTo(i.bmp.Width);
				}
				return -c;
			}
		}

		public Graphic Clone()
		{
			return new Graphic(glyph, new Bitmap(bmp), drawOffset);
		}

		public Graphic AddBorders(int xborder, int yborder, int xoffset, int yoffset)
		{
			if (bmp != null)
			{
				Bitmap n = new Bitmap(bmp.Width + xborder, bmp.Height + yborder);
				using (Graphics g = Graphics.FromImage(n))
				{
					Brush b = new SolidBrush(Color.FromArgb(0));
					g.FillRectangle(b, 0, 0, n.Width, n.Height);
				}
				n.CopyRect(bmp, 0, 0, xoffset, yoffset, bmp.Width, bmp.Height);
				return new Graphic(glyph, n, new PointF(drawOffset.X - xoffset, drawOffset.Y - yoffset));
			}
			else
			{
				return null;
			}
		}

		private static void ScanBitmap(Bitmap bmp, int xs, int ys, int xe, int ye, ref int left, ref int top, ref int right, ref int bottom, ref bool empty)
		{
			right = -1;
			left = -1;
			top = -1;
			bottom = -1;

			empty = true;

			for (int y = ys; y < ye; ++y)
			{
				for (int x = xs; x < xe; ++x)
				{
					if (bmp.GetPixel(x, y).A != 0)	// Scans Alpha only - needs to do color also?
					{
						empty = false;
						if (x < left || left == -1)
						{
							left = x;
						}
						if (x > right || right == -1)
						{
							right = x;
						}
						if (y < top || top == -1)
						{
							top = y;
						}
						if (y > bottom || bottom == -1)
						{
							bottom = y;
						}
					}
				}
			}
		}

		private void ShrinkToFit()
		{
			int left = 0, right = 0, top = 0, bottom = 0;
			bool empty = false;
			ScanBitmap(bmp, 0, 0, bmp.Width, bmp.Height, ref left, ref top, ref right, ref bottom, ref empty);
			if (!empty)
			{
				drawOffset.X += left;
				drawOffset.Y += top;
				int newWidth = right - left + 1;
				int newHeight = bottom - top + 1;
				if (newWidth < bmp.Width || newHeight < bmp.Height)
				{
					Bitmap newBmp = new Bitmap(newWidth, newHeight);
					using (Graphics gr = Graphics.FromImage(newBmp))
					{
						gr.FillRectangle(Brushes.Magenta, newBmp.Dimensions());
					}
					newBmp.CopyRect(bmp, left, top, 0, 0, newWidth, newHeight);
					bmp = newBmp;
				}
			}
			else
			{
				bmp = null;
			}
		}

		public static int ComputeHash(Bitmap bmp)
		{
			if (bmp != null)
			{
				Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
				BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);

				int hash;

				unchecked
				{
					const int p = 16777619;
					hash = (int)2166136261;

					unsafe
					{
						for (int y = 0; y < bmp.Height; ++y)
						{
							byte* data = (byte*)bmpData.Scan0 + y * bmpData.Stride;
							for (int x = 0; x < bmp.Width * 4; x++)
							{
								hash = (hash ^ data[x]) * p;
							}
						}
					}
				}
				bmp.UnlockBits(bmpData);
				return hash;
			}
			else
			{
				return 0;
			}
		}

		// From some arbitrary bmp
		public Graphic(Glyph glyph, Bitmap bmp, PointF drawOffset)
		{
			this.glyph = glyph;
			pageIndex = -1;
			SetBitmap(bmp, drawOffset);
		}

		public void SetBitmap(Bitmap bmp, PointF drawOffset)
		{
			this.drawOffset = drawOffset;
			this.bmp = bmp;
		}

		public void Bake()
		{
			if (bmp != null)
			{
				ShrinkToFit();	// do this for all glyphs at the end of processing
				hash32 = ComputeHash(bmp);
			}
		}

		private static bool BitmapsAreIdentical(Bitmap a, Bitmap b)
		{
			int w = a.Width;
			int h = a.Height;
			for (int y = 0; y < h; ++y)
			{
				for (int x = 0; x < w; ++x)
				{
					if (a.GetPixel(x, y).ToArgb() != b.GetPixel(x, y).ToArgb())
					{
						return false;
					}
				}
			}
			return true;
		}

		// this should be in BitmapFont really
		public void Place(BitmapFont font, bool singlePixelBorder)
		{
			bool placed = false;

			if (bmp != null)
			{
				foreach (Graphic i in font.globalImageList)
				{
					if (i.bmp != null &&					// the other one is not empty and
						hash32 == i.hash32 &&				// hashes are the same and
						!i.Equals(this) &&
						i.pageIndex != -1 &&				// the other one has been placed and
						bmp.Width == i.bmp.Width &&		// they have the same dimensions
						bmp.Height == i.bmp.Height)
					{
						if (BitmapsAreIdentical(bmp, i.bmp))	// then do exhaustive compare
						{
							// duplicate
							glyphPosition = i.glyphPosition;
							pageIndex = i.pageIndex;
							placed = true;
							//Console.WriteLine("'{0}' is a dup of '{1}'", g, g2);
							break;
						}
						else
						{
							Console.WriteLine("Hash collision between '{0}' and '{1}'", this.glyph.character, i.glyph.character);
						}
					}
				}

				Rectangle pos = new Rectangle();

				while (!placed)
				{
					foreach (Page p in font.pages)
					{
						if (p.Place(bmp, ref pos, singlePixelBorder))
						{
							BitmapFont.glyphSize += pos.Width * pos.Height;
							glyphPosition = pos;
							pageIndex = p.index;
							placed = true;
							break;
						}
					}
					if (!placed)
					{
						Page p = new Page(font.pageWidth, font.pageHeight, font.pages.Count);
						font.pages.Add(p);
					}
				}
			}
		}
	}
}
