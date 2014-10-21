using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime;
using FontUtil;
using System.Xml;
using System.IO;

namespace FontUtil
{
	public class BitmapParameter : Parameter
	{
		public static string WorkingFolder { get; set; }

		public override Type ParameterType { get { return typeof(NamedBitmap); } }

		public override string ParameterName { get { return "Bitmap"; } }

		public override void WriteAttributes(XmlTextWriter stream)
		{
			stream.WriteAttributeString("File", new Uri(WorkingFolder + "\\").MakeRelativeUri(new Uri(((NamedBitmap)value).Name)).ToString().Replace('/', '\\') + "'");
		}

		public override void Read(xml r)
		{
			value = new Bitmap(Path.Combine(WorkingFolder, r.String("File")));
		}

		public override object GetDefault()
		{
			bool xb = false;
			bool yb = false;
			Bitmap b = new Bitmap(16, 16);
			b.Fill(Color.White);
			for (int y = 0; y < 16; ++y)
			{
				yb = !yb;
				xb = yb;
				for (int x = 0; x < 16; ++x)
				{
					if (xb)
					{
						b.SetPixel(x, y, Color.Magenta);
					}
					xb = !xb;
				}
			}
			return new NamedBitmap("Default", b);
		}

		public override void UpdateControl()
		{
			(Control as BitmapDialog).Value = (NamedBitmap)value;
		}

		public override void UpdateValue()
		{
			value = (Control as BitmapDialog).Value;
		}

		public override Control CreateControl()
		{
			BitmapDialog b = new BitmapDialog();
			b.OnChanged += new EventHandler<EventArgs>(b_OnChanged);
			return b;
		}

		void b_OnChanged(object sender, EventArgs e)
		{
			RaiseParameterChanged();
		}
	}
}
