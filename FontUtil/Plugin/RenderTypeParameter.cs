using System;
using System.Windows.Forms;
using System.Drawing;
using FontUtil;
using System.Xml;

namespace FontUtil
{
	public class RenderTypeParameter : Parameter
	{
		public override Type ParameterType { get { return typeof(RenderType); } }

		public override string ParameterName { get { return "RenderType"; } }

		public override void WriteAttributes(XmlTextWriter stream)
		{
			stream.WriteAttributeString("value", ((int)value).ToString());
		}

		public override void Read(xml r)
		{
			value = (RenderType)r.Int("value");
		}

		public override object GetDefault()
		{
			return RenderType.WPF;
		}

		public override void UpdateValue()
		{
			value = (RenderType)((ComboBox)Control).SelectedIndex;
		}

		public override void UpdateControl()
		{
			suppressChangedEvents = true;
			((ComboBox)Control).SelectedIndex = (int)value;	// this causes an onchanged event to fire, which then erroneously fills the other parameters with the wrong (old) values from the font dialog.
			suppressChangedEvents = false;
		}

		public override Control CreateControl()
		{
			ComboBox b = new ComboBox();
			b.Items.Add("GDI Bitmapped");
			b.Items.Add("GDI TrueType");
			b.Items.Add("PresentationCore");
			b.DropDownStyle = ComboBoxStyle.DropDownList;
			value = GetDefault();
			b.SelectedIndexChanged += new EventHandler(it_SelectedIndexChanged);
			return b;
		}

		void it_SelectedIndexChanged(object sender, EventArgs e)
		{
			RaiseParameterChanged();
		}
	}
}
