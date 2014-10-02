using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FontUtil
{
	public partial class MyColorDialog : UserControl
	{
		private Bitmap previewBitmap;
		private bool updating;

		public Color color;

		public event EventHandler<EventArgs> OnChanged;

		public MyColorDialog()
		{
			InitializeComponent();
			int w = colorPictureBox.Width;
			int h = colorPictureBox.Height;
			previewBitmap = new Bitmap(w, h);
			colorPictureBox.Image = previewBitmap;
			RepaintPreview();
		}

		void RepaintPreview()
		{
			using(Graphics g = Graphics.FromImage(previewBitmap))
			{
				g.FillRectangle(new SolidBrush(color), previewBitmap.Dimensions());
			}
			colorPictureBox.Refresh();
		}

		public Color Value
		{
			get
			{
				return color;
			}
			set
			{
				color = value;
				UpdateControls(false);
			}
		}

		void UpdateControls(bool raiseChangedEvent = true)
		{
			if (!updating)
			{
				updating = true;
				int r = color.R;
				int g = color.G;
				int b = color.B;
				int a = color.A;

				redTrackBar.Value = r;
				redNumericUpDown.Value = r;

				greenTrackBar.Value = g;
				greenNumericUpDown.Value = g;

				blueTrackBar.Value = b;
				blueNumericUpDown.Value = b;
	
				alphaTrackBar.Value = a;
				alphaNumericUpDown.Value = a;

				RepaintPreview();

				if (raiseChangedEvent && OnChanged != null)
				{
					OnChanged(this, new EventArgs());
				}
				updating = false;
			}
		}

		private void redTrackBar_Scroll(object sender, EventArgs e)
		{
			color = Color.FromArgb(color.A, redTrackBar.Value, color.G, color.B);
			UpdateControls();
		}

		private void greenTrackBar_Scroll(object sender, EventArgs e)
		{
			color = Color.FromArgb(color.A, color.R, greenTrackBar.Value, color.B);
			UpdateControls();
		}

		private void blueTrackBar_Scroll(object sender, EventArgs e)
		{
			color = Color.FromArgb(color.A, color.R, color.G, blueTrackBar.Value);
			UpdateControls();
		}

		private void alphaTrackBar_Scroll(object sender, EventArgs e)
		{
			color = Color.FromArgb(alphaTrackBar.Value, color.R, color.G, color.B);
			UpdateControls();
		}

		private void redNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			color = Color.FromArgb(color.A, (int)redNumericUpDown.Value, color.G, color.B);
			UpdateControls();
		}

		private void greenNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			color = Color.FromArgb(color.A, color.R, (int)greenNumericUpDown.Value, color.B);
			UpdateControls();
		}

		private void blueNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			color = Color.FromArgb(color.A, color.R, color.G, (int)blueNumericUpDown.Value);
			UpdateControls();
		}

		private void alphaNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			color = Color.FromArgb((int)alphaNumericUpDown.Value, color.R, color.G, color.B);
			UpdateControls();
		}

		private void presetButton_Click(object sender, EventArgs e)
		{
			color = (sender as Button).BackColor;
			UpdateControls();
		}
	}
}
