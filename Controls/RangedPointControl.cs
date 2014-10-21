using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FontUtil
{
	public partial class RangedPointControl : UserControl
	{
		bool dragging;
		PointF dragOffset;
		PointF halfSize;

		private RangedPointF _value;

		public event EventHandler<EventArgs> OnChanged;

		public RangedPointControl()
		{
			InitializeComponent();

			XTextBox.TextChanged += new EventHandler(TextBox_TextChanged);
			YTextBox.TextChanged += new EventHandler(TextBox_TextChanged);

			int w = pictureBox1.Width;
			int h = pictureBox1.Height;

			halfSize = new Point(w / 2, h / 2);

			pictureBox1.Image = new Bitmap(w, h);

			pictureBox1.MouseDown += new MouseEventHandler(pictureBox1_MouseDown);
			pictureBox1.MouseMove += new MouseEventHandler(pictureBox1_MouseMove);
			pictureBox1.MouseUp += new MouseEventHandler(pictureBox1_MouseUp);
		}

		void pictureBox1_MouseUp(object sender, MouseEventArgs e)
		{
			dragging = false;
		}

		void pictureBox1_MouseMove(object sender, MouseEventArgs e)
		{
			if (dragging)
			{
				PointF loc = e.Location;
				_value.Value = loc.Add(dragOffset).Subtract(halfSize);
				Value = _value;
			}
		}

		void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			PointF pos = e.Location;
			PointF offset = pos.Subtract(Value.Value.Add(halfSize));
			if (offset.Length() < 4)
			{
				dragging = true;
				dragOffset = offset;
			}
		}

		void TextBox_TextChanged(object sender, EventArgs e)
		{
			PlotPoint();
			if (OnChanged != null)
			{
				OnChanged(this, new EventArgs());
			}
		}

		void PlotPoint()
		{
			int w = pictureBox1.Width;
			int h = pictureBox1.Height;
			using (Graphics g = Graphics.FromImage(pictureBox1.Image))
			{
				g.FillRectangle(SystemBrushes.Control, (pictureBox1.Image as Bitmap).Dimensions());
				g.DrawLine(Pens.Black, w / 2, 0, w / 2, h);
				g.DrawLine(Pens.Black, 0, h / 2, w, h / 2);
				PointF v = _value.Value.Add(new Point(w / 2, h / 2));
				g.FillEllipse(Brushes.Magenta, new Rectangle(new Point((int)v.X, (int)v.Y).Subtract(new Point(4, 4)), new Size(8, 8)));
			}
			pictureBox1.Refresh();
		}

		public RangedPointF Value
		{
			get
			{
				float x = 0, y = 0;
				float.TryParse(XTextBox.Text, out x);
				float.TryParse(YTextBox.Text, out y);
				return new RangedPointF(_value.Min, _value.Max, new PointF(x, y));
			}
			set
			{
				_value = value;
				XTextBox.Text = _value.Value.X.ToString();
				YTextBox.Text = _value.Value.Y.ToString();
				PlotPoint();
			}
		}
	}
}
