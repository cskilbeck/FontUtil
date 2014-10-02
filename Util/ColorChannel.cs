using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FontUtil
{
	public enum ColorChannel : int
	{
		Blue = 0,
		Green = 1,
		Red = 2,
		Alpha = 3
	}

	public class ColorChannelHolder
	{
		public ColorChannel channel; // ARGB = 3210

		public static string[] ChannelNames = new string[] { "Blue", "Green", "Red", "Alpha" };

		public ColorChannelHolder(ColorChannel channel)
		{
			this.channel = channel;
		}

		public ColorChannelHolder(int channel)
		{
			this.channel = (ColorChannel)channel;
		}

		public static implicit operator ColorChannelHolder(ColorChannel channel)
		{
			return new ColorChannelHolder(channel);
		}

		public static implicit operator ColorChannelHolder(int channel)
		{
			return new ColorChannelHolder(channel);
		}

		public override string ToString()
		{
			return ChannelNames[(int)channel];
		}

		public static implicit operator int(ColorChannelHolder channel)
		{
			return (int)channel.channel;
		}
	}
}
