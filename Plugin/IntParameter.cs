using System;
using System.Windows.Forms;
using System.Drawing;
using FontUtil;
using System.Xml;

namespace FontUtil
{
	public class IntParameter : Parameter
	{
		public override Type ParameterType { get { return typeof(int); } }

		public override string ParameterName { get { return "int"; } }

		public override void WriteAttributes(XmlTextWriter stream)
		{
			stream.WriteAttributeString("value", ((int)value).ToString());
		}

		public override void Read(xml r)
		{
			value = r.Int("value");
		}

		public override object GetDefault()
		{
			return 0;
		}

		public override void UpdateValue()
		{
			int v;
			if (int.TryParse(((TextBox)Control).Text, out v))
			{
				value = v;
			}
		}

		public override void UpdateControl()
		{
			((TextBox)Control).Text = ((int)value).ToString();
		}

		public override Control CreateControl()
		{
			TextBox it = new TextBox();
			it.Width = 100;
			it.Text = GetDefault().ToString();
			it.TextChanged += new EventHandler(it_TextChanged);
			return it;
		}

		void it_TextChanged(object sender, EventArgs e)
		{
			RaiseParameterChanged();
		}
	}
}
