using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FontUtil
{
	[FlagsAttribute]
	public enum ChannelMask : uint
	{
		Alpha = 0xff000000,
		Blue = 0x00ff0000,
		Red = 0x0000ff00,
		Green = 0x000000ff
	}
}
