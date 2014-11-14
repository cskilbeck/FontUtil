using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace FontUtil
{
	enum PixelSize : int
	{
		InBytes = 4,
		InUInt32s = 1
	}

	public static class Util
	{
		public static void DrawRoundRect(Graphics g, Brush brush, Pen p, float X, float Y, float width, float height, float radius)
		{
			GraphicsPath gp = new GraphicsPath();
			gp.AddLine(X + radius, Y, X + width - (radius * 2), Y);
			gp.AddArc(X + width - (radius * 2), Y, radius * 2, radius * 2, 270, 90);
			gp.AddLine(X + width, Y + radius, X + width, Y + height - (radius * 2));
			gp.AddArc(X + width - (radius * 2), Y + height - (radius * 2), radius * 2, radius * 2, 0, 90);
			gp.AddLine(X + width - (radius * 2), Y + height, X + radius, Y + height);
			gp.AddArc(X, Y + height - (radius * 2), radius * 2, radius * 2, 90, 90);
			gp.AddLine(X, Y + height - (radius * 2), X, Y + radius);
			gp.AddArc(X, Y, radius * 2, radius * 2, 180, 90);
			gp.CloseFigure();
			g.FillPath(brush, gp);
			g.DrawPath(p, gp);
			gp.Dispose();
		}
		
		public static Rectangle Dimensions(this Bitmap bmp)
		{
			return new Rectangle(Point.Empty, bmp.Size);
		}

		public static void Fill(this Bitmap bmp, Color color)
		{
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.FillRectangle(new SolidBrush(color), bmp.Dimensions());
			}
		}

		public static Point Add(this Point a, Point b)
		{
			return new Point(a.X + b.X, a.Y + b.Y);
		}

		public static Point Subtract(this Point a, Point b)
		{
			return new Point(a.X - b.X, a.Y - b.Y);
		}

		public static PointF Add(this PointF a, PointF b)
		{
			return new PointF(a.X + b.X, a.Y + b.Y);
		}

		public static PointF Subtract(this PointF a, PointF b)
		{
			return new PointF(a.X - b.X, a.Y - b.Y);
		}


		public static bool Intersects(this Rectangle a, Rectangle b)
		{
			return ! (a.Left >= b.Right || b.Left >= a.Right || a.Top >= b.Bottom || b.Top >= a.Bottom);
		}

		public static int DistanceTo(this Point a, Point b)
		{
			int xd = a.X - b.X;
			int yd = a.Y - b.Y;
			return (int)Math.Sqrt(xd * xd + yd * yd);
		}

		public static float DistanceTo(this PointF a, PointF b)
		{
			float xd = a.X - b.X;
			float yd = a.Y - b.Y;
			return (float)Math.Sqrt(xd * xd + yd * yd);
		}

		public static int DistanceTo(this Point a, int x, int y)
		{
			int xd = a.X - x;
			int yd = a.Y - y;
			return (int)Math.Sqrt(xd * xd + yd * yd);
		}

		public static double Length(this Point a)
		{
			return Math.Sqrt(a.X * a.X + a.Y * a.Y);
		}

		public static double Length(this PointF a)
		{
			return Math.Sqrt(a.X * a.X + a.Y * a.Y);
		}

		public static void CopyRect(this Bitmap bmp, Bitmap srcBmp, int srcX, int srcY, int destX, int destY, int w, int h)
		{
			if (bmp != null)
			{
				unsafe
				{
					using (RawBitmap rawsrc = new RawBitmap(srcBmp))
					{
						using (RawBitmap rawdest = new RawBitmap(bmp))
						{
							for (int y = 0; y < h; ++y)
							{
								UInt32* src = (UInt32*)rawsrc[srcX, srcY + y];
								UInt32* dst = (UInt32*)rawdest[destX, destY + y];
								for (int x = 0; x < w; ++x)
								{
									*dst++ = *src++;
								}
							}
						}
					}
				}
			}
		}

		public static void MaskAndSet(this Bitmap bmp, UInt32 mask, UInt32 set)
		{
			using (RawBitmap rawsrc = new RawBitmap(bmp))
			{
				unsafe
				{
					int w = bmp.Width;
					int h = bmp.Height;
					for (int y = 0; y < h; ++y)
					{
						UInt32* ptr = (UInt32*)rawsrc[0, y];
						for (int x = 0; x < w; ++x)
						{
							*ptr = (*ptr & mask) | set;
							++ptr;
						}
					}
				}
			}
		}

		public static Graphic Multiply(Graphic graphic, int a, int r, int g, int b)
		{
			if (graphic.bmp != null)
			{
				unsafe
				{
					int w = graphic.bmp.Width;
					int h = graphic.bmp.Height;
					using (RawBitmap rawsrc = new RawBitmap(graphic.bmp))
					{
						for (int y = 0; y < h; ++y)
						{
							byte* ptr = rawsrc[0, y];
							for (int x = 0; x < w; ++x)
							{
								int ib = (int)ptr[0] * b / 255;
								int ig = (int)ptr[1] * g / 255;
								int ir = (int)ptr[2] * r / 255;
								int ia = (int)ptr[3] * a / 255;

								if(ib < 0) ib = 0;
								if(ig < 0) ig = 0;
								if(ir < 0) ir = 0;
								if(ia < 0) ia = 0;

								if(ib > 255) ib = 255;
								if(ig > 255) ig = 255;
								if(ir > 255) ir = 255;
								if(ia > 255) ia = 255;

								ptr[0] = (byte)ib;
								ptr[1] = (byte)ig;
								ptr[2] = (byte)ir;
								ptr[3] = (byte)ia;

								ptr += 4;
							}
						}
					}
				}
			}
			return graphic;
		}

		public static Graphic CopyChannel(Graphic graphic, ColorChannelHolder srcChannel, ColorChannelHolder dstChannel)
		{
			if (graphic.bmp != null && srcChannel != dstChannel)
			{
				unsafe
				{
					int w = graphic.bmp.Width;
					int h = graphic.bmp.Height;

					int dc = (int)dstChannel;
					int sc = (int)srcChannel;

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
	}
}
