using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace FontUtil
{
	public class xml : XmlTextReader, IDisposable
	{
		public xml(string filename)
			: base(filename)
		{
		}

		public void XmlDeclaration()
		{
			Read();
			if (NodeType != XmlNodeType.XmlDeclaration)
			{
				Err("Missing XML Declaration");
			}
		}

		public void DocumentType(string name)
		{
			Read();
			if (NodeType != XmlNodeType.DocumentType || Name != name)
			{
				Err("Missing BitmapFont DocumentType");
			}
		}

		public void Element(string name = null)
		{
			Read();
			if (NodeType != XmlNodeType.Element || (name != null && Name != name))
			{
				if (name == null)
				{
					name = "";
				}
				Err("Missing <" + name + "> element");
			}
		}

		public void EndElement(string name = null)
		{
			Read();
			if (NodeType != XmlNodeType.EndElement || (name != null && Name != name))
			{
				if(name == null)
				{
					name = "";
				}
				Err("Missing </" + name + "> EndElement");
			}
		}

		public void EndElement(XmlTextReader r, string name)
		{
			Read();
			if (NodeType != XmlNodeType.EndElement || Name != name)
			{
				Err("Missing </" + name + "> end element");
			}
		}

		public void Err(string msg)
		{
			throw new XmlException(msg, null, LineNumber, LinePosition);
		}

		public string String(string name)
		{
			string s = GetAttribute(name);
			if (s == null)
			{
				Err("Missing attribute: " + name);
			}
			return s;
		}

		public UInt32 Hex(string name)
		{
			string s = String(name);
			UInt32 v;
			if (!UInt32.TryParse(s, System.Globalization.NumberStyles.HexNumber, null, out v))
			{
				Err("Bad attribute: " + name);
			}
			return v;
		}

		public int Int(string name)
		{
			string s = String(name);
			int v;
			if (!Int32.TryParse(s, out v))
			{
				Err("Bad attribute: " + name);
			}
			return v;
		}

		public float Float(string name)
		{
			string s = String(name);
			float v;
			if (!float.TryParse(s, out v))
			{
				Err("Bad attribute: " + name);
			}
			return v;
		}
	}
}
