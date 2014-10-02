using System;
using System.Windows.Forms;
using System.Drawing;
using FontUtil;
using System.Xml;

namespace FontUtil
{
	public class PointParameter : Parameter
	{
		public override Type ParameterType { get { return typeof(Point); } }

		public override string ParameterName { get { return "Point"; } }

		public override void WriteAttributes(XmlTextWriter stream)
		{
			Point p = (Point)value;
			stream.WriteAttributeString("x", p.X.ToString());
			stream.WriteAttributeString("y", p.Y.ToString());
		}

		public override void Read(xml r)
		{
			value = new Point(r.Int("x"), r.Int("y"));
		}

		public override object GetDefault()
		{
			return new Point(0, 0);
		}

		public override void UpdateValue()
		{
			value = ((PointDialog)Control).Value;
		}

		public override void UpdateControl()
		{
			((PointDialog)Control).Value = (Point)value;
		}

		public override Control CreateControl()
		{
			PointDialog p = new PointDialog();
			p.Value = (Point)GetDefault();
			p.OnChanged += new EventHandler<EventArgs>(p_OnChanged);
			return p;
		}

		void p_OnChanged(object sender, EventArgs e)
		{
			RaiseParameterChanged();
		}
	}
}
