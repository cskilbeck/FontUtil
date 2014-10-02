using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FontUtil
{
	public class EdgeDetector : ConvolutionMask
	{
		public static int[] edgeMask = new int[3 * 3]
		{
			-1,	-1,	-1,
			-1,	 8,	-1,
			-1,	-1,	-1,
		};

		public EdgeDetector()
			: base(edgeMask, 3, 3)
		{
		}
	}
}
