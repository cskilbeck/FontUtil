using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace FontUtil
{
    public unsafe sealed class RawBitmap : IDisposable
    {
        private Bitmap _originBitmap;
        private BitmapData _bitmapData;
        private byte* _begin;

        public RawBitmap(Bitmap originBitmap)
        {
            _originBitmap = originBitmap;
            _bitmapData = _originBitmap.LockBits(new Rectangle(0, 0, _originBitmap.Width, _originBitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            _begin = (byte*)(void*)_bitmapData.Scan0;
        }

		public void CopyRectangle(Bitmap src, Rectangle srcRect, Point destination)
		{
			using (RawBitmap srcRaw = new RawBitmap(src))
			{
				for (int y = 0; y < srcRect.Height; ++y)
				{
					uint* dstPtr = (uint*)this[destination.X, destination.Y + y];
					uint* srcPtr = (uint*)srcRaw[srcRect.X, srcRect.Y + y];

					for (int x = 0; x < srcRect.Width; ++x)
					{
						*dstPtr++ = *srcPtr++;
					}
				}
			}
		}

        #region IDisposable Members

        public void Dispose()
        {
            _originBitmap.UnlockBits(_bitmapData);
        }

        #endregion

        public unsafe byte* Begin
        {
            get { return _begin; }
        }

        public unsafe byte* this[int x,int y]
        {
            get
            {
                return _begin + y * (_bitmapData.Stride) + x * 4;
            }
        }

        public unsafe byte* this[int x, int y, int offset]
        {
            get
            {
                return _begin + y * (_bitmapData.Stride) + x * 4 + offset;
            }
        }

        public int Stride
        {
            get { return _bitmapData.Stride; }
        }

        public int Width
        {
            get { return _bitmapData.Width; }
        }

        public int Height
        {
            get { return _bitmapData.Height; }
        }

        public int GetOffset()
        {
            return _bitmapData.Stride - _bitmapData.Width * 4;
        }

        public Bitmap OriginBitmap
        {
            get { return _originBitmap; }
        }
    }
}
