using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;

namespace FontUtil
{
	public class FunctionNode : Node
	{
		public Function function;

		public FunctionNode() : base()
		{
		}

		public FunctionNode(Function function, List<Parameter> inputParameters = null)
			: base()
		{
			this.function = function;

			if (function != null)
			{
				// make a default copy of the parameters from the function. The output is always a Glyph.Graphic
				parameters = new List<Parameter>();

				int n = function.graphInputs + 1;

				// create the input pins from any parameters which are Graphics
				int index = 0;
				int inputIndex = 0;
				int socketID = 0;
				foreach (Parameter p in function.parameters)
				{
					Parameter param = Parameter.CreateParameter(p.name, p.type, index++, p.defaultValue);

					if (param.defaultValue != null)
					{
						param.value = param.defaultValue;
					}
					parameters.Add(param);
					if (param.IsGraphInput)
					{
						InputSocket inputSocket = new InputSocket(new Point(0, index * Height / n - Pin.height / 2), this);
						inputSocket.id = socketID++;
						inputSocket.parameter = param;
						inputSocket.PropertyChanged += new EventHandler<EventArgs>(inputSocket_Changed);
					}
					else
					{
						if (inputParameters != null)
						{
							param.value = inputParameters[inputIndex++].value;
						}

						param.ParameterChanged += new EventHandler<EventArgs>(param_ParameterChanged);
					}
				}
			}
			Position = new Point(100, 100);
		}

		public override string Caption
		{
			get
			{
				return function.name;
			}
		}

		void param_ParameterChanged(object sender, EventArgs e)
		{
			UpdateValues();
			Dirty = true;
			GetGraphic();
			RaiseChanged();
		}

		void inputSocket_Changed(object sender, EventArgs e)
		{
			Dirty = true;
			GetGraphic();
			RaiseChanged();
		}

		public override string Description
		{
			get
			{
				return function.description;
			}
		}

		public override void WriteExtraAttributes(XmlTextWriter stream)
		{
			stream.WriteAttributeString("Plugin", function.parent.Name);
			stream.WriteAttributeString("Function", function.name);
		}

		public override string Name
		{
			get
			{
				return "FunctionNode";// function.name;
			}
		}

		public override int NumControlsRequired()
		{
			return parameters.Count - function.graphInputs;
		}

		public override Graphic CreateGraphic()
		{
			object[] parms;
			int i = 0;
			parms = new object[parameters.Count];

			// collect the non-Pin inputs
			foreach (Parameter p in parameters)
			{
				if (!p.IsGraphInput)
				{
					parms[i] = p.value;
				}
				i++;
			}

			// collect the Pin input parameters
			foreach (InputSocket s in Inputs)
			{
				Pin p = s.Pin;
				if (p != null)
				{
					Connection c = p.Connection;
					if (c != null)
					{
						Pin other = c.SendingPin;
						Node parent = other.ParentNode;
						if (parent != null)
						{
							Graphic r = parent.GetGraphic();	// by traversing back up the graph
							if (r != null && s.parameter != null)
							{
								parms[s.parameter.index] = r;
							}
							else
							{
								return null;	// graph is not properly linked, give back nothing
							}
						}
						else
						{
							return null;
						}
					}
				}
				else
				{
					return null;
				}
			}

			Function func = function;
			foreach (object o in parms)
			{
				if (o == null)
				{
					return null;
				}
				if (o is Graphic)
				{
					Graphic g = o as Graphic;
					if (g.bmp == null)
					{
						cache = new Graphic(g);	// if any input graphics have no bmp, return an empty Graphic
						return cache;
					}
				}
			}

			if (parms.Length == func.method.GetParameters().Length)
			{
				//Debug.WriteLine("Invoke " + func.name);
				cache = (Graphic)func.method.Invoke(null, parms);
				return cache;
			}
			else
			{
				return null;
			}
		}
	}
}
