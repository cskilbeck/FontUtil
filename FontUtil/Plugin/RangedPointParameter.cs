using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace FontUtil
{
	class RangedPointParameter : Parameter
	{
		public override Type ParameterType { get { return typeof(RangedPointF); } }

		public override string ParameterName { get { return "RangedPointF"; } }

		public override void WriteAttributes(XmlTextWriter stream)
		{
			RangedPointF p = (RangedPointF)value;
			stream.WriteAttributeString("MinX", p.Min.X.ToString());
			stream.WriteAttributeString("MaxX", p.Max.X.ToString());
			stream.WriteAttributeString("MinY", p.Min.Y.ToString());
			stream.WriteAttributeString("MaxY", p.Max.Y.ToString());
			stream.WriteAttributeString("X", p.Value.X.ToString());
			stream.WriteAttributeString("Y", p.Value.Y.ToString());
		}

		//MinX="-50" MaxX="50" MinY="-50" MaxY="50" X="3" Y="3"
		public override void Read(xml r)
		{
			PointF min = new PointF();
			PointF max = new PointF();
			PointF val = new PointF();
			min.X = r.Float("MinX");
			max.X = r.Float("MaxX");
			min.Y = r.Float("MinY");
			max.Y = r.Float("MaxY");
			val.X = r.Float("X");
			val.Y = r.Float("Y");
			value = new RangedPointF(min, max, val);
		}

		public override Control CreateControl()
		{
			RangedPointControl c = new RangedPointControl();
			c.Value = (RangedPointF)this.value;
			c.OnChanged += new EventHandler<EventArgs>(c_OnChanged);
			return c;
		}

		void c_OnChanged(object sender, EventArgs e)
		{
			RaiseParameterChanged();
		}

		public override object GetDefault()
		{
			return new RangedPointF(new Point(-50, -50), new Point(50, 50), new Point(0, 0));
		}

		public override bool IsGraphInput
		{
			get
			{
				return false;
			}
		}

		public override void UpdateControl()
		{
			(Control as RangedPointControl).Value = (RangedPointF)value;
		}

		public override void UpdateValue()
		{
			value = (Control as RangedPointControl).Value;
		}
	}
}
