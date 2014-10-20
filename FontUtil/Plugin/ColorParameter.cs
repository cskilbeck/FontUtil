using System;
using System.Windows.Forms;
using System.Drawing;
using FontUtil;
using System.Xml;
using System.Diagnostics;

namespace FontUtil
{
	public class ColorParameter : Parameter
	{
		public override Type ParameterType { get { return typeof(Color); } }

		public override string ParameterName { get { return "Color"; } }

		public override void WriteAttributes(XmlTextWriter stream)
		{
			stream.WriteAttributeString("ARGB", ((Color)value).ToArgb().ToString("X8"));
		}

		public override void Read(xml r)
		{
			value = Color.FromArgb((int)r.Hex("ARGB"));
		}

		public override object GetDefault()
		{
			return Color.White;
		}

		public override void UpdateValue()
		{
			value = ((MyColorDialog)Control).Value;
		}

		public override void UpdateControl()
		{
			((MyColorDialog)Control).Value = (Color)value;
		}

		public override Control CreateControl()
		{
			MyColorDialog d = new MyColorDialog();
			d.Value = (System.Drawing.Color)value;
			d.OnChanged += new EventHandler<EventArgs>(d_OnChanged);
			return d;
		}

		void d_OnChanged(object sender, EventArgs e)
		{
			RaiseParameterChanged();
		}
	}
}
