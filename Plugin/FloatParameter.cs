using System;
using System.Windows.Forms;
using System.Drawing;
using FontUtil;
using System.Xml;

namespace FontUtil
{
	public class FloatParameter : Parameter
	{
		public override Type ParameterType { get { return typeof(float); } }

		public override string ParameterName { get { return "float"; } }

		public override void WriteAttributes(XmlTextWriter stream)
		{
			stream.WriteAttributeString("value", ((float)value).ToString());
		}

		public override void Read(xml r)
		{
			value = r.Float("value");
		}

		public override object GetDefault()
		{
			return 0;
		}

		public override void UpdateValue()
		{
			float f;
			if (float.TryParse(((TextBox)Control).Text, out f))
			{
				value = f;
			}
		}

		public override void UpdateControl()
		{
			((TextBox)Control).Text = ((float)value).ToString();
		}

		public override Control CreateControl()
		{
			TextBox ft = new TextBox();
			ft.Width = 100;
			ft.Text = GetDefault().ToString();
			ft.TextChanged += new EventHandler(ft_TextChanged);
			return ft;
		}

		void ft_TextChanged(object sender, EventArgs e)
		{
			RaiseParameterChanged();
		}
	}

}
