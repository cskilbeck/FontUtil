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
	public partial class FontDescriptorDialog : UserControl
	{
		public event EventHandler<EventArgs> OnChanged;

		private bool internalUpdate;

		public FontDescriptorDialog()
		{
			internalUpdate = true;
			InitializeComponent();
			internalUpdate = false;
		}

		private void RaiseOnChanged()
		{
			if (OnChanged != null && !internalUpdate)
			{
				OnChanged(this, new EventArgs());
			}
		}

		public FontDescriptor Value
		{
			get
			{
				return new FontDescriptor(outlineCheckBox.Checked ? (int)outlineNumericUpDown.Value : 0, antialiasCheckBox.Checked, roundedCheckBox.Checked);
			}
			set
			{
				internalUpdate = true;
				outlineCheckBox.Checked = value.outline != 0;
				if (value.outline > 0)
				{
					outlineNumericUpDown.Value = value.outline;
					outlineTrackBar.Value = value.outline;
				}
				antialiasCheckBox.Checked = value.antiAlias;
				roundedCheckBox.Checked = value.rounded;
				internalUpdate = false;
			}
		}

		private void antialiasCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			RaiseOnChanged();
		}

		private void outlineCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			bool en = outlineCheckBox.Checked;
			roundedCheckBox.Enabled = en;
			outlineNumericUpDown.Enabled = en;
			outlineTrackBar.Enabled = en;
			RaiseOnChanged();
		}

		private void roundedCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			RaiseOnChanged();
		}

		private void outlineNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			outlineTrackBar.Value = (int)outlineNumericUpDown.Value;
			RaiseOnChanged();
		}

		private void outlineTrackBar_ValueChanged(object sender, EventArgs e)
		{
			outlineNumericUpDown.Value = outlineTrackBar.Value;
		}
	}
}
