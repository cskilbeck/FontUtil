using System;
using System.Windows.Forms;
using System.Drawing;
using FontUtil;
using System.Xml;

namespace FontUtil
{
	public class ColorChannelParameter : Parameter
	{
		public override Type ParameterType { get { return typeof(ColorChannel); } }

		public override string ParameterName { get { return "ColorChannel"; } }

		public override void WriteAttributes(XmlTextWriter stream)
		{
			stream.WriteAttributeString("Channel", ((int)(ColorChannel)value).ToString());
		}

		public override void Read(xml r)
		{
			value = (ColorChannel)(r.Int("Channel"));
		}

		public override object GetDefault()
		{
			return ColorChannel.Alpha;
		}

		public override void UpdateValue()
		{
			value = ((ColorChannelHolder)((ComboBox)Control).SelectedItem).channel;
		}

		public override void UpdateControl()
		{
			if (value != null)
			{
				ColorChannel channel = (ColorChannel)value;
				ComboBox box = (ComboBox)Control;
				for (int i = 0; i < box.Items.Count; ++i)
				{
					ColorChannelHolder ch = (ColorChannelHolder)box.Items[i];
					if (ch.channel == channel)
					{
						box.SelectedIndex = i;
						break;
					}
				}
			}
		}

		public override Control CreateControl()
		{
			ComboBox b = new ComboBox();
			b.Sorted = false;
			b.DropDownStyle = ComboBoxStyle.DropDownList;
			for(int i=0; i<4; ++i)
			{
				b.Items.Add(new ColorChannelHolder(i));
			}
			b.SelectedIndex = 0;
			b.SelectedIndexChanged += new EventHandler(b_SelectedIndexChanged);
			return b;
		}

		void b_SelectedIndexChanged(object sender, EventArgs e)
		{
			RaiseParameterChanged();
		}
	}
}
