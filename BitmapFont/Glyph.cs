using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace FontUtil
{
	public class Glyph
	{
		public char character;
		public float advance;
		public int penWidth;
		public Bitmap originalBitmap;
		public List<KerningValue> kerningTable;
		public List<Graphic> imageTable;
		public BitmapFont font;

		public Glyph(BitmapFont font)
		{
			this.font = font;
			imageTable = new List<Graphic>();
		}

		public Glyph(BitmapFont font, char chr)
		{
			this.font = font;
			imageTable = new List<Graphic>();
			character = chr;
		}

		public void AddImage(BitmapFont font, char c, Bitmap b, PointF drawOffset)
		{
			Graphic i = new Graphic(this, b, drawOffset);
			imageTable.Add(i);
		}

		public void RemoveImage(Graphic i)
		{
			imageTable.Remove(i);
		}

		public void Place(BitmapFont font, Bitmap bmp)
		{
			//TODO
		}

		public void AddKerningEntry(char first, int amount)
		{
			if (kerningTable == null)
			{
				kerningTable = new List<KerningValue>();
			}
			kerningTable.Add(new KerningValue(first, amount));
		}

		public void Process(BitmapFont font)
		{
			if (imageTable.Count > 0 && imageTable[0] != null && imageTable[0].bmp != null)
			{
				imageTable[0] = Util.CopyChannel(imageTable[0], ColorChannel.Blue, ColorChannel.Alpha);
				Util.MaskAndSet(imageTable[0].bmp, 0xff000000, 0x00ffffff);
			}
		}

		public override string ToString()
		{
			return new string(character, 1);
		}

		public void DrawPreview(Graphic graphic, Graphics destination, PointF pos, Color color)
		{
			if (graphic.bmp != null)
			{
				Graphic temp = new Graphic(graphic);
				temp = Util.Multiply(temp, color.A, color.R, color.G, color.B);
				destination.DrawImage(temp.bmp, pos.Add(graphic.drawOffset));
			}
		}

		public float Draw(BitmapFont font, Bitmap dest, PointF pos, char previous, int layerIndex)
		{
            float rc = 0;
            FontLayer layer = font.layers[layerIndex];

			if (imageTable.Count > layerIndex)
			{
                Graphic i = imageTable[layerIndex];

				if (i.pageIndex >= 0 && i.pageIndex < font.pages.Count)
				{
                    if (previous != 0 && kerningTable != null)
                    {
                        foreach (KerningValue k in kerningTable)
                        {
                            if (k.otherChar == previous)
                            {
                                if (font.useKerning)
                                {
                                    rc = k.kerningAmount;
                                }
                            }
                        }
                    }
                    
                    Page p = font.pages[i.pageIndex];
					using (Graphics d = Graphics.FromImage(dest))
					{
                        d.PixelOffsetMode = PixelOffsetMode.HighQuality;
						d.SmoothingMode = SmoothingMode.AntiAlias;
						RectangleF r = new RectangleF((float)i.glyphPosition.Left,
                                                        (float)i.glyphPosition.Top,
                                                        (float)i.glyphPosition.Width,
                                                        (float)i.glyphPosition.Height);
		
						float x = pos.X + i.drawOffset.X + rc + (float)layer.offset.X;
						float y = pos.Y + i.drawOffset.Y + (float)layer.offset.Y;

                        PointF[] pf = new PointF[3]
                        {
                            new PointF(x, y),
                            new PointF(x + r.Width, y),
                            new PointF(x, y + r.Height)
                        };

                        float red = layer.color.R / 255.0f;
                        float green = layer.color.G / 255.0f;
                        float blue = layer.color.B / 255.0f;
                        float alpha = layer.color.A / 255.0f;

                        ColorMatrix m = new ColorMatrix();

                        m.Matrix00 = red;
                        m.Matrix11 = green;
                        m.Matrix22 = blue;
                        m.Matrix33 = alpha;

                        ImageAttributes ia = new ImageAttributes();
                        ia.SetColorMatrix(m);

                        d.DrawImage(p.bitmap, pf, r, GraphicsUnit.Pixel, ia);
					}
				}
			}
			rc += this.advance;

			return rc;
		}
	}
}
