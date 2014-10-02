using System;
using System.Windows.Forms;
using System.Drawing;
using FontUtil;
using System.Xml;

namespace FontUtil
{
	public class ChannelMaskParameter : Parameter
	{
		public override Type ParameterType { get { return typeof(ChannelMask); } }

		public override string ParameterName { get { return "ChannelMask"; } }

		public override void WriteAttributes(XmlTextWriter stream)
		{
			stream.WriteAttributeString("Mask", ((int)(ChannelMask)value).ToString("X8"));
		}

		public override void Read(xml r)
		{
			value = (ChannelMask)r.Hex("Mask");
		}

		public override object GetDefault()
		{
			return (ChannelMask)0xFFFFFFFF;
		}

		public override void UpdateValue()
		{
			value = ((ChannelMaskDialog)Control).Value;
		}

		public override void UpdateControl()
		{
			((ChannelMaskDialog)Control).Value = (ChannelMask)value;
		}

		public override Control CreateControl()
		{
			ChannelMaskDialog mask = new ChannelMaskDialog();
			mask.Value = (ChannelMask)GetDefault();
			mask.OnChanged += new EventHandler<EventArgs>(mask_OnChanged);
			return mask;
		}

		void mask_OnChanged(object sender, EventArgs e)
		{
			RaiseParameterChanged();
		}
	}
}
