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
	public partial class PointDialog : UserControl
	{
		public event EventHandler<EventArgs> OnChanged;

		public PointDialog()
		{
			InitializeComponent();
			XTextBox.TextChanged += new EventHandler(TextBox_TextChanged);
			YTextBox.TextChanged += new EventHandler(TextBox_TextChanged);
		}

		void TextBox_TextChanged(object sender, EventArgs e)
		{
			if (OnChanged != null)
			{
				OnChanged(this, new EventArgs());
			}
		}

		public Point Value
		{
			get
			{
				int x = 0, y = 0;
				Int32.TryParse(XTextBox.Text, out x);
				Int32.TryParse(YTextBox.Text, out y);
				return new Point(x, y);
			}
			set
			{
				XTextBox.Text = value.X.ToString();
				YTextBox.Text = value.Y.ToString();
			}
		}
	}
}
