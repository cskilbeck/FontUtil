using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace FontUtil
{
	public class FontDescriptorParameter : Parameter
	{
		public override Type ParameterType { get { return typeof(FontDescriptor); } }

		public override string ParameterName { get { return "FontDescriptor"; } }

		public override void WriteAttributes(XmlTextWriter stream)
		{
			FontDescriptor f = (FontDescriptor)value;
			stream.WriteAttributeString("Outline", f.outline.ToString());
			stream.WriteAttributeString("AntiAlias", f.antiAlias ? "1" : "0");
			stream.WriteAttributeString("Rounded", f.rounded ? "1" : "0");
		}

		public override void Read(xml r)
		{
			int outline = r.Int("Outline");
			int antialias = r.Int("AntiAlias");
			int rounded = r.Int("Rounded");
			value = new FontDescriptor(outline, antialias != 0, rounded != 0);
		}

		public override object GetDefault()
		{
			return new FontDescriptor(0, true, false);
		}

		public override void UpdateValue()
		{
			value = ((FontDescriptorDialog)Control).Value;
		}

		public override void UpdateControl()
		{
			suppressChangedEvents = true;
			((FontDescriptorDialog)Control).Value = (FontDescriptor)value;
			suppressChangedEvents = false;
		}

		public override Control CreateControl()
		{
			FontDescriptorDialog p = new FontDescriptorDialog();
			p.Value = (FontDescriptor)GetDefault();
			p.OnChanged += new EventHandler<EventArgs>(p_OnChanged);
			return p;
		}

		public void p_OnChanged(object sender, EventArgs e)
		{
			RaiseParameterChanged();
		}
	}
}
