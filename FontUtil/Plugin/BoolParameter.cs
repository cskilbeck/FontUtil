using System;
using System.Windows.Forms;
using System.Drawing;
using FontUtil;
using System.Xml;

namespace FontUtil
{
	public class BoolParameter : Parameter
	{
		public override Type ParameterType { get { return typeof(bool); } }

		public override string ParameterName { get { return "bool"; } }

		public override void WriteAttributes(XmlTextWriter stream)
		{
			stream.WriteAttributeString("value", ((bool)value).ToString());
		}

		public override void Read(xml r)
		{
			switch (r.String("value"))
			{
				case "True": value = true; break;
				case "False": value = false; break;
			}
		}

		public override object GetDefault()
		{
			return false;
		}

		public override void UpdateValue()
		{
			value = ((CheckBox)Control).Checked;
		}

		public override void UpdateControl()
		{
			((CheckBox)Control).Checked = (bool)value;
		}

		public override Control CreateControl()
		{
			CheckBox it = new CheckBox();
			it.CheckedChanged += new EventHandler(it_CheckedChanged);
			return it;
		}

		void it_CheckedChanged(object sender, EventArgs e)
		{
			RaiseParameterChanged();
		}
	}
}
