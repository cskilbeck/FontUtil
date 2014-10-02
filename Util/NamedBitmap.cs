using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FontUtil
{
	public class NamedBitmap
	{
		public string Name;
		public Bitmap Bitmap;

		public NamedBitmap(string filename, Bitmap bmp = null)
		{
			this.Name = filename;
			if (bmp == null)
			{
				// work around the locking problem
				using (Bitmap b = new Bitmap(filename))
				{
					this.Bitmap = new Bitmap(b);
				}
			}
			else
			{
				this.Bitmap = bmp;
			}
		}

		public static implicit operator Bitmap(NamedBitmap bmp)
		{
			return bmp.Bitmap;
		}
	}
}
