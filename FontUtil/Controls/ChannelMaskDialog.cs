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
	public partial class ChannelMaskDialog : UserControl
	{
		public event EventHandler<EventArgs> OnChanged;

		public ChannelMaskDialog()
		{
			InitializeComponent();
			checkBoxR.Tag = checkBoxR.BackColor;
			checkBoxG.Tag = checkBoxG.BackColor;
			checkBoxB.Tag = checkBoxB.BackColor;
			checkBoxA.Tag = checkBoxA.BackColor;
		}

		public ChannelMask Value
		{
			get
			{
				uint r = (uint)(checkBoxR.Checked ? 0xff : 00);
				uint g = (uint)(checkBoxG.Checked ? 0xff : 00);
				uint b = (uint)(checkBoxB.Checked ? 0xff : 00);
				uint a = (uint)(checkBoxA.Checked ? 0xff : 00);
				return (ChannelMask)(a << 24 | r << 16 | g << 8 | b);
			}
			set
			{
				UInt32 v = (UInt32)(value);
				uint a = v >> 24 & 0xff;
				uint r = v >> 16 & 0xff;
				uint g = v >> 8 & 0xff;
				uint b = v & 0xff;
				checkBoxR.Checked = r != 0;
				checkBoxG.Checked = g != 0;
				checkBoxB.Checked = b != 0;
				checkBoxA.Checked = a != 0;
				SetCheckboxColor(checkBoxR);
				SetCheckboxColor(checkBoxG);
				SetCheckboxColor(checkBoxB);
				SetCheckboxColor(checkBoxA);
			}
		}

		private static void SetCheckboxColor(CheckBox c)
		{
			bool IsChecked = c.Checked;
			c.BackColor = (Color)(IsChecked ? c.Tag : Color.LightGray);
		}

		private void checkBox_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox c = sender as CheckBox;
			SetCheckboxColor(c);
			if (OnChanged != null)
			{
				OnChanged(this, new EventArgs());
			}
		}
	}
}
