using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Diagnostics;

using RawBitmap = FontUtil.RawBitmap;
using Util = FontUtil.Util;

namespace BitmapUtilities
{
    [Serializable]
    public enum BlurType
    {
        Both,
        HorizontalOnly,
        VerticalOnly,
    }

    [Serializable]
    public class GaussianBlur
    {
        private int _radius = 1;
        private int[] _kernel;
        private int _kernelSum;
        private int[,] _multable;
        private BlurType _blurType;

        public GaussianBlur()
        {
            PreCalculateSomeStuff();
        }

        public GaussianBlur(int radius)
        {
            _radius = radius;
            PreCalculateSomeStuff();
        }

        private void PreCalculateSomeStuff()
        {
            int sz = _radius * 2 + 1;
            _kernel = new int[sz];
            _multable = new int[sz, 256];
            for (int i = 1; i <= _radius; i++)
            {
                int szi = _radius - i;
                int szj = _radius + i;
                _kernel[szj] = _kernel[szi] = (szi + 1) * (szi + 1);
                _kernelSum += (_kernel[szj] + _kernel[szi]);
                for (int j = 0; j < 256; j++)
                {
                    _multable[szj, j] = _multable[szi, j] = _kernel[szj] * j;
                }
            }
            _kernel[_radius] = (_radius + 1) * (_radius + 1);
            _kernelSum += _kernel[_radius];
            for (int j = 0; j < 256; j++)
            {
                _multable[_radius, j] = _kernel[_radius] * j;
            }
        }

        public long t1;
        public long t2;
        public long t3;
        public long t4;

        public Bitmap ProcessImage(Bitmap inputImage)
        {
            Bitmap blurred = new Bitmap(inputImage.Width, inputImage.Height);

            using (RawBitmap src = new RawBitmap(inputImage))
            {
                using (RawBitmap dest = new RawBitmap(blurred))
                {
                    int pixelCount = src.Width * src.Height;
                    //Stopwatch sw = new Stopwatch();
                    //sw.Start();
                    int[] b = new int[pixelCount];
                    int[] g = new int[pixelCount];
					int[] r = new int[pixelCount];
					int[] a = new int[pixelCount];

                    int[] b2 = new int[pixelCount];
                    int[] g2 = new int[pixelCount];
					int[] r2 = new int[pixelCount];
					int[] a2 = new int[pixelCount];

					//sw.Stop();
                    //t1 = sw.ElapsedMilliseconds;

                    int offset = src.GetOffset();
                    int index = 0;
                    unsafe
                    {
                        //sw.Reset();
                        //sw.Start();

                        byte* ptr = src.Begin;
                        for (int i = 0; i < src.Height; i++)
                        {
                            for (int j = 0; j < src.Width; j++)
                            {
                                b[index] = *ptr++;
                                g[index] = *ptr++;
								r[index] = *ptr++;
								a[index] = *ptr++;

                                ++index;
                            }
                            ptr += offset;
                        }

                        //sw.Stop();
                        //t2 = sw.ElapsedMilliseconds;

                        int bsum;
                        int gsum;
						int rsum;
						int asum;
						int read;
                        int start = 0;
                        index = 0;

                        //sw.Reset();
                        //sw.Start();

                        if (_blurType != BlurType.VerticalOnly)
                        {
							int w = src.Width;
							int h = src.Height;
							int l = _kernel.Length;
                            for (int i = 0; i < h; i++)
                            {
                                for (int j = 0; j < w; j++)
                                {
                                    bsum = gsum = rsum = asum = 0;
                                    read = index - _radius;

                                    for (int z = 0; z < l; z++)
                                    {
                                        //if (read >= start && read < start + src.Width)
                                        //{
                                        //    bsum += _multable[z, b[read]];
                                        //    gsum += _multable[z, g[read]];
										//    rsum += _multable[z, r[read]];
										//    asum += _multable[z, a[read]];
										//    sum += _kernel[z];
                                        //}

                                        if (read < start)
                                        {
                                            bsum += _multable[z, b[start]];
                                            gsum += _multable[z, g[start]];
											rsum += _multable[z, r[start]];
											asum += _multable[z, a[start]];
										}
                                        else if (read > start + w - 1)
                                        {
                                            int idx = start + w - 1;
                                            bsum += _multable[z, b[idx]];
                                            gsum += _multable[z, g[idx]];
											rsum += _multable[z, r[idx]];
											asum += _multable[z, a[idx]];
										}
                                        else
                                        {
                                            bsum += _multable[z, b[read]];
                                            gsum += _multable[z, g[read]];
											rsum += _multable[z, r[read]];
											asum += _multable[z, a[read]];
										}
                                        ++read;
                                    }

                                    //b2[index] = (bsum / sum);
                                    //g2[index] = (gsum / sum);
									//r2[index] = (rsum / sum);
									//a2[index] = (asum / sum);

                                    b2[index] = (bsum / _kernelSum);
                                    g2[index] = (gsum / _kernelSum);
									r2[index] = (rsum / _kernelSum);
									a2[index] = (asum / _kernelSum);

                                    if (_blurType == BlurType.HorizontalOnly)
                                    {
                                        //byte* pcell = dest[j, i];
                                        //*pcell = (byte)(bsum / sum);
                                        //pcell++;
                                        //*pcell = (byte)(gsum / sum);
                                        //pcell++;
										//*pcell = (byte)(rsum / sum);
										//pcell++;
										//*pcell = (byte)(asum / sum);
										//pcell++;

                                        byte* pcell = dest[j, i];
                                        *pcell++ = (byte)(bsum / _kernelSum);
                                        *pcell++ = (byte)(gsum / _kernelSum);
										*pcell++ = (byte)(rsum / _kernelSum);
										*pcell++ = (byte)(asum / _kernelSum);
									}

                                    ++index;
                                }
                                start += w;
                            }
                        }
                        if (_blurType == BlurType.HorizontalOnly)
                        {
                            return blurred;
                        }

                        //sw.Stop();
                        //t3 = sw.ElapsedMilliseconds;

                        //sw.Reset();
                        //sw.Start();

                        int tempy;
                        for (int i = 0; i < src.Height; i++)
                        {
                            int y = i - _radius;
                            start = y * src.Width;
                            for (int j = 0; j < src.Width; j++)
                            {
                                bsum = gsum = rsum = asum = 0;
                                read = start + j;
                                tempy = y;
                                for (int z = 0; z < _kernel.Length; z++)
                                {
                                    //if (tempy >= 0 && tempy < src.Height)
                                    //{
                                    //    if (_blurType == BlurType.VerticalOnly)
                                    //    {
                                    //        bsum += _multable[z, b[read]];
                                    //        gsum += _multable[z, g[read]];
									//        rsum += _multable[z, r[read]];
									//        asum += _multable[z, a[read]];
									//    }
                                    //    else
                                    //    {
                                    //        bsum += _multable[z, b2[read]];
                                    //        gsum += _multable[z, g2[read]];
									//        rsum += _multable[z, r2[read]];
									//        asum += _multable[z, a2[read]];
									//    }
                                    //    sum += _kernel[z];
                                    //}

                                    if (_blurType == BlurType.VerticalOnly)
                                    {
                                        if (tempy < 0)
                                        {
                                            bsum += _multable[z, b[j]];
                                            gsum += _multable[z, g[j]];
											rsum += _multable[z, r[j]];
											asum += _multable[z, a[j]];
										}
                                        else if (tempy > src.Height - 1)
                                        {
                                            int idx = pixelCount - (src.Width - j);
                                            bsum += _multable[z, b[idx]];
                                            gsum += _multable[z, g[idx]];
											rsum += _multable[z, r[idx]];
											asum += _multable[z, a[idx]];
										}
                                        else
                                        {
                                            bsum += _multable[z, b[read]];
                                            gsum += _multable[z, g[read]];
											rsum += _multable[z, r[read]];
											asum += _multable[z, a[read]];
										}
                                    }
                                    else
                                    {
                                        if (tempy < 0)
                                        {
                                            bsum += _multable[z, b2[j]];
                                            gsum += _multable[z, g2[j]];
											rsum += _multable[z, r2[j]];
											asum += _multable[z, a2[j]];
										}
                                        else if (tempy > src.Height - 1)
                                        {
                                            int idx = pixelCount - (src.Width - j);
                                            bsum += _multable[z, b2[idx]];
                                            gsum += _multable[z, g2[idx]];
											rsum += _multable[z, r2[idx]];
											asum += _multable[z, a2[idx]];
										}
                                        else
                                        {
                                            bsum += _multable[z, b2[read]];
                                            gsum += _multable[z, g2[read]];
											rsum += _multable[z, r2[read]];
											asum += _multable[z, a2[read]];
										}
                                    }


                                    read += src.Width;
                                    ++tempy;
                                }

                                byte* pcell = dest[j, i];

                                //pcell[0] = (byte)(bsum / sum);
                                //pcell[1] = (byte)(gsum / sum);
								//pcell[2] = (byte)(rsum / sum);
								//pcell[3] = (byte)(asum / sum);

                                pcell[0] = (byte)(bsum / _kernelSum);
                                pcell[1] = (byte)(gsum / _kernelSum);
								pcell[2] = (byte)(rsum / _kernelSum);
								pcell[3] = (byte)(asum / _kernelSum);
							}
                        }
                        //sw.Stop();
                        //t4 = sw.ElapsedMilliseconds;
                    }
                }
            }

            return blurred;
        }

        public int Radius
        {
            get { return _radius; }
            set
            {
                if (value < 1)
                {
                    throw new InvalidOperationException("Radius must be greater then 0");
                }
                _radius = value;
                PreCalculateSomeStuff();
                //RaisePropertyChanged(this, new PropertyChangedEventArgs("Radius"));
            }
        }

        public BlurType BlurType
        {
            get { return _blurType; }
            set
            {
                _blurType = value;
                //RaisePropertyChanged(this, new PropertyChangedEventArgs("BlurType"));
            }
        }
    }
}
