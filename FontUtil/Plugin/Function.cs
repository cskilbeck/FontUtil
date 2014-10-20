using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace FontUtil
{
	public class Function
	{
		public string name;
		public string description;
		public Plugin parent;
		public MethodInfo method;
		public List<Parameter> parameters;
		public int graphInputs;

		public override string ToString()
		{
			return name;
		}
	}
}
