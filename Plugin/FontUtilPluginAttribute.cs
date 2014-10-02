using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FontUtil
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class PluginAttribute : Attribute
	{
		public PluginAttribute(string Description, bool IsBitmapModifier)
		{
			this.Description = Description;
			this.IsBitmapModifier = IsBitmapModifier;
		}

		public string Description { get; set; }
		public bool IsBitmapModifier { get; set; }
	}

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class PluginDefaults : Attribute
	{
		public PluginDefaults(Type type)
		{
			this.Type = type;
		}

		public Type Type { get; set; }
	}
}
