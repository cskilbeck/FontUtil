using System;
using System.Windows.Forms;
using System.Drawing;
using FontUtil;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Diagnostics;
using System.Windows;
using System.IO;
using System.Xml;
using System.Collections;

namespace FontUtil
{
	public class ParameterTypeAttribute : Attribute
	{
		public ParameterTypeAttribute(Type t)
		{
			type = t;
		}

		public Type type { get; set; }
	}

	public abstract class Parameter
	{
		public string name;
		public Type type;
		public object value;
		public object defaultValue;
		public bool highlight;
		public int index;		// where in the parameter list it is
		private Control control;
		public bool suppressChangedEvents;

		public event EventHandler<EventArgs> ParameterChanged;

		public static Dictionary<string, Type> allTypes = new Dictionary<string, Type>();

		private static Dictionary<Type, ConstructorInfo> parameterConstructors = null;

		private static Dictionary<Type, ConstructorInfo> ParameterConstructors
		{
			get
			{
				if(parameterConstructors == null)
				{
					parameterConstructors = new Dictionary<Type, ConstructorInfo>();

					// get a list of constructors for all types derived from Parameter
					foreach (ConstructorInfo ctor in from t in Assembly.GetExecutingAssembly().GetTypes() where t.BaseType == typeof(Parameter) select (t.GetConstructor(System.Type.EmptyTypes)))
					{
						// create a dummy one
						Parameter p = (Parameter)ctor.Invoke(null);

						// for looking up later..
						allTypes.Add(p.ParameterType.Name, p.ParameterType);

						// so we can ask it what type it is a parameter for and add it to the dictionary of constructors

						//Debug.WriteLine("ParameterType for {0} is {1}", ctor.ReflectedType.Name, p.ParameterType.Name);

						parameterConstructors.Add(p.ParameterType, ctor);
					}
				}
				return parameterConstructors;
			}
		}

		public static void Init()
		{
			var a = ParameterConstructors;
		}

		public static Parameter CreateParameter(string name, Type type, int index, object defaultValue)
		{
			Parameter p = null;

			if(ParameterConstructors.ContainsKey(type))
			{
				p = (Parameter)ParameterConstructors[type].Invoke(null);
				p.defaultValue = defaultValue;
				p.name = name;
				p.type = type;
				p.value = p.GetDefault();
				p.index = index;
				p.suppressChangedEvents = false;
			}
			return p;
		}

		public abstract Type ParameterType
		{
			get;
		}

		public abstract string ParameterName
		{
			get;
		}

		public abstract void WriteAttributes(XmlTextWriter stream);

		public void Save(XmlTextWriter stream)
		{
			stream.WriteStartElement("Parameter");
			{
				stream.WriteAttributeString("Name", name);
				stream.WriteAttributeString("Type", type.Name);
				WriteAttributes(stream);
			}
			stream.WriteEndElement(); // ParameterName
		}

		public abstract void Read(xml r);

		public virtual Control Control
		{
			get
			{
				if (control == null)
				{
					control = CreateControl();
				}
				return control;
			}
		}

		public virtual bool IsGraphInput
		{
			get
			{
				return false;
			}
		}

		public override string ToString()
		{
			return name + "(" + type.Name + ")";
		}

		public virtual object GetDefault()
		{
			return null;
		}

		public virtual void UpdateValue()
		{
		}

		public virtual void UpdateControl()
		{
		}

		public abstract Control CreateControl();

		public void RaiseParameterChanged()
		{
			if (ParameterChanged != null && !suppressChangedEvents)
			{
				ParameterChanged(this, null);
			}
		}
	}
}
