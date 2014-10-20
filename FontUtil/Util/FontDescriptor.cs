using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FontUtil
{
	public class FontSizeDescriptor
	{
		public int size;

		public FontSizeDescriptor(int s)
		{
			size = s;
		}

		public override string ToString()
		{
			return size.ToString();
		}
	}

	public class FontDescriptor
	{
		public int outline;
		public bool antiAlias;
		public bool rounded;

		public FontDescriptor(int outline, bool antiAlias, bool rounded)
		{
			this.outline = outline;
			this.antiAlias = antiAlias;
			this.rounded = rounded;
		}
	}
}
