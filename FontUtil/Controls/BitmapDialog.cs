using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FontUtil
{
	public partial class BitmapDialog : UserControl
	{
		NamedBitmap bmp;

		public event EventHandler<EventArgs> OnChanged;

		public BitmapDialog()
		{
			InitializeComponent();
		}

		public NamedBitmap Value
		{
			get
			{
				return bmp;
			}
			set
			{
				bmp = value;
				bitmapPictureBox.Image = bmp;
				if (bmp.Bitmap.Width > bitmapPictureBox.Width || bmp.Bitmap.Height > bitmapPictureBox.Height)
				{
					bitmapPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
				}
				else
				{
					bitmapPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
				}
			}
		}

		private void bitmapPictureBox_MouseClick(object sender, MouseEventArgs e)
		{
			OpenFileDialog f = new OpenFileDialog();
			f.CheckFileExists = true;
			f.CheckPathExists = true;
			f.Filter = "Bitmap Files|*.bmp;*.jpg;*.png;*.gif;*.tiff|All files (*.*)|*.*";
			if(f.ShowDialog() == DialogResult.OK)
			{
				Value = new NamedBitmap(f.FileName);
				bitmapPictureBox.Image = Value.Bitmap;
				bitmapPictureBox.Refresh();
				if (OnChanged != null)
				{
					OnChanged(this, new EventArgs());
				}
			}
		}
	}
}
