using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FontUtil
{
	public class Page
	{
		public RectanglePacker packer;
		public Bitmap bitmap;
		public Bitmap nonAlphaBitmap;
		public int index;
		public Size extent;

		public Page(int w, int h, int index)
		{
			extent = Size.Empty;
			this.index = index;
			bitmap = new Bitmap(w, h);
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				g.FillRectangle(new SolidBrush(Color.Empty), 0, 0, w, h);
			}

			switch (BitmapFont.FontPackingType)
			{
				case PackingType.Cygon:
					packer = new CygonRectanglePacker(w - 1, h - 1);
					break;

				case PackingType.Arevalo:
					packer = new ArevaloRectanglePacker(w - 1, h - 1);
					break;
			}
		}

		// keep halving the bitmap vertically to make it as small as it can be...
		public void ShrinkToFit()
		{
			int x = bitmap.Width;
			int y = bitmap.Height;
			while (extent.Height < y / 2 && y > 8)
			{
				y /= 2;
			}
			while (extent.Width < x / 2 && x > 8)
			{
				x /= 2;
			}

			if (y < bitmap.Height || x < bitmap.Width)
			{
				Bitmap newBmp = new Bitmap(x, y);
				Util.CopyRect(newBmp, bitmap, 0, 0, 0, 0, x, y);
				bitmap = newBmp;
			}

			nonAlphaBitmap = new Bitmap(bitmap.Width, bitmap.Height);
			using (Graphics g = Graphics.FromImage(nonAlphaBitmap))
			{
				g.FillRectangle(Brushes.Blue, bitmap.Dimensions());
				g.DrawImage(bitmap, new Point(0, 0));
			}
			//nonAlphaBitmap.CopyRect(bitmap, 0, 0, 0, 0, bitmap.Width, bitmap.Height);
			nonAlphaBitmap.MaskAndSet(0xffffffff, 0xff000000);
		}

		public bool Place(Bitmap bmp, ref Rectangle rect, bool singlePixelBorder)
		{
			Point pos;
			int w = bmp.Width;
			int h = bmp.Height;

			int b = singlePixelBorder ? 1 : 0;

			if (packer.TryPack(w + b, h + b, out pos))
			{
				pos.X += b;
				pos.Y += b;

				bitmap.CopyRect(bmp, 0, 0, pos.X, pos.Y, w, h);

				rect.X = pos.X;
				rect.Y = pos.Y;
				rect.Width = w;
				rect.Height = h;

				extent.Width = Math.Max(extent.Width, rect.Right);
				extent.Height = Math.Max(extent.Height, rect.Bottom);

				return true;
			}
			return false;
		}
	}

}
