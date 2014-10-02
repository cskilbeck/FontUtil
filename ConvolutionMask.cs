using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;

namespace FontUtil
{
	public class ConvolutionMask
	{
		public int[] mask;
		public int width;
		public int height;

		public ConvolutionMask(int[] mask, int width, int height)
		{
			this.mask = mask;
			this.width = width;
			this.height = height;
			Debug.Assert((width & 1) == 1 && (height & 1) == 1);
		}

		/*
	       //-------X GRADIENT APPROXIMATION------
	       for(I=-1; I<=1; I++) 
		  {
			for(J=-1; J<=1; J++) 
		 * {
		      sumX = sumX + (int)( (*(originalImage.data + X + I + 
                             (Y + J)*originalImage.cols)) * GX[I+1][J+1]);
		   }
	       }

	       //-------Y GRADIENT APPROXIMATION-------
	       for(I=-1; I<=1; I++)  {
		   for(J=-1; J<=1; J++)  {
		       sumY = sumY + (int)( (*(originalImage.data + X + I + 
                              (Y + J)*originalImage.cols)) * GY[I+1][J+1]);
		   }
	       }

	       /*---GRADIENT MAGNITUDE APPROXIMATION (Myler p.218)----
               SUM = abs(sumX) + abs(sumY);
		*/

		public Bitmap ApplyHorizontal(Bitmap bmp)
		{
			return null;
		}

		public Bitmap ApplyVertical(Bitmap bmp)
		{
			return null;
		}

		public Bitmap ApplyAbsSumXY(Bitmap bmp)	// For Sobel edge detect
		{
			return null;
		}

		public Bitmap ApplyHV(Bitmap bmp)
		{
			return ApplyVertical(ApplyHorizontal(bmp));
		}

		public Bitmap ApplyVH(Bitmap bmp)
		{
			return ApplyHorizontal(ApplyVertical(bmp));
		}

		public Bitmap Apply(Bitmap bmp)
		{
			Bitmap newBmp = null;
			if (bmp != null)
			{
				newBmp = new Bitmap(bmp.Width, bmp.Height);

				using (RawBitmap rawSource = new RawBitmap(bmp))
				{
					using (RawBitmap rawDest = new RawBitmap(newBmp))
					{
						int hw = width / 2;
						int hh = height / 2;

						int w = bmp.Width - hw;
						int h = bmp.Height - hh;

						for (int y = hh; y < h; ++y)
						{
							unsafe
							{
								for (int x = hw; x < w; ++x)
								{
									byte* dst = rawDest[x + hw, y];

									int bt = 0;
									int gt = 0;
									int rt = 0;
									int at = 0;

									int mx = 0;

									for (int yy = -hh; yy <= hh; ++yy)
									{
										byte* src = rawSource[x, y + yy];

										for (int xx = -hw; xx <= hw; ++xx)
										{
											int m = mask[mx++];
											bt += src[(int)ColorChannel.Channel.Blue] * m;
											gt += src[(int)ColorChannel.Channel.Green] * m;
											rt += src[(int)ColorChannel.Channel.Red] * m;
											at += src[(int)ColorChannel.Channel.Alpha] * m;
											src += 4;
										}
									}

									if (bt < 0) bt = 0;
									if (gt < 0) gt = 0;
									if (rt < 0) rt = 0;
									if (at < 0) at = 0;

									if (bt > 255) bt = 255;
									if (gt > 255) gt = 255;
									if (rt > 255) rt = 255;
									if (at > 255) at = 255;

									dst[(int)ColorChannel.Channel.Blue] = (byte)bt;
									dst[(int)ColorChannel.Channel.Green] = (byte)gt;
									dst[(int)ColorChannel.Channel.Red] = (byte)rt;
									dst[(int)ColorChannel.Channel.Alpha] = (byte)at;
								}
							}
						}
					}
				}
			}
			return newBmp;
		}
	}
}
