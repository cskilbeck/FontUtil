using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FontUtil
{
	public class KerningValue
	{
		public char otherChar;
		public int kerningAmount;

		public KerningValue(char otherChar, int kerningAmount)
		{
			this.otherChar = otherChar;
			this.kerningAmount = kerningAmount;
		}
	}

}
