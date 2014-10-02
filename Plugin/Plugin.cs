using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FontUtil
{
	public class Plugin
	{
		public List<Function> functions;
		public string Name { get; set; }

		public Plugin(string name)
		{
			functions = new List<Function>();
			Name = name;
		}

		public void AddFunction(Function function)
		{
			function.parent = this;
			functions.Add(function);
		}

		public Function FindFunction(string name)
		{
			return functions.Single(f => name == f.name);
		}

		public override string ToString()
		{
			return Name;
		}
	}

}
