using System;
using System.Windows.Forms;
using System.Drawing;
using FontUtil;
using System.Xml;

namespace FontUtil
{
	public class GraphicParameter : Parameter
	{
		public override Type ParameterType { get { return typeof(Graphic); } }

		public override string ParameterName { get { return "Graphic"; } }

		public override void WriteAttributes(XmlTextWriter stream)
		{
		}

		public override void Read(xml r)
		{
			// do nothing silently...
		}

		public override bool IsGraphInput
		{
			get
			{
				return true;
			}
		}

		public override Control CreateControl()
		{
			throw new NotImplementedException();
		}
	}
}
