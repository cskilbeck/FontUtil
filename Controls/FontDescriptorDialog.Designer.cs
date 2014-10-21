namespace FontUtil
{
	partial class FontDescriptorDialog
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.roundedCheckBox = new System.Windows.Forms.CheckBox();
			this.antialiasCheckBox = new System.Windows.Forms.CheckBox();
			this.outlineNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.outlineTrackBar = new System.Windows.Forms.TrackBar();
			this.outlineCheckBox = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.outlineNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.outlineTrackBar)).BeginInit();
			this.SuspendLayout();
			// 
			// roundedCheckBox
			// 
			this.roundedCheckBox.AutoSize = true;
			this.roundedCheckBox.Location = new System.Drawing.Point(68, 26);
			this.roundedCheckBox.Name = "roundedCheckBox";
			this.roundedCheckBox.Size = new System.Drawing.Size(70, 17);
			this.roundedCheckBox.TabIndex = 14;
			this.roundedCheckBox.Text = "Rounded";
			this.roundedCheckBox.UseVisualStyleBackColor = true;
			this.roundedCheckBox.CheckedChanged += new System.EventHandler(this.roundedCheckBox_CheckedChanged);
			// 
			// antialiasCheckBox
			// 
			this.antialiasCheckBox.AutoSize = true;
			this.antialiasCheckBox.Location = new System.Drawing.Point(3, 3);
			this.antialiasCheckBox.Name = "antialiasCheckBox";
			this.antialiasCheckBox.Size = new System.Drawing.Size(65, 17);
			this.antialiasCheckBox.TabIndex = 13;
			this.antialiasCheckBox.Text = "Antialias";
			this.antialiasCheckBox.UseVisualStyleBackColor = true;
			this.antialiasCheckBox.CheckedChanged += new System.EventHandler(this.antialiasCheckBox_CheckedChanged);
			// 
			// outlineNumericUpDown
			// 
			this.outlineNumericUpDown.Location = new System.Drawing.Point(3, 49);
			this.outlineNumericUpDown.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
			this.outlineNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.outlineNumericUpDown.Name = "outlineNumericUpDown";
			this.outlineNumericUpDown.Size = new System.Drawing.Size(34, 20);
			this.outlineNumericUpDown.TabIndex = 12;
			this.outlineNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.outlineNumericUpDown.ValueChanged += new System.EventHandler(this.outlineNumericUpDown_ValueChanged);
			// 
			// outlineTrackBar
			// 
			this.outlineTrackBar.AutoSize = false;
			this.outlineTrackBar.Location = new System.Drawing.Point(44, 49);
			this.outlineTrackBar.Maximum = 32;
			this.outlineTrackBar.Minimum = 1;
			this.outlineTrackBar.Name = "outlineTrackBar";
			this.outlineTrackBar.Size = new System.Drawing.Size(91, 21);
			this.outlineTrackBar.TabIndex = 11;
			this.outlineTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
			this.outlineTrackBar.Value = 1;
			this.outlineTrackBar.ValueChanged += new System.EventHandler(this.outlineTrackBar_ValueChanged);
			// 
			// outlineCheckBox
			// 
			this.outlineCheckBox.AutoSize = true;
			this.outlineCheckBox.Location = new System.Drawing.Point(3, 26);
			this.outlineCheckBox.Name = "outlineCheckBox";
			this.outlineCheckBox.Size = new System.Drawing.Size(59, 17);
			this.outlineCheckBox.TabIndex = 10;
			this.outlineCheckBox.Text = "Outline";
			this.outlineCheckBox.UseVisualStyleBackColor = true;
			this.outlineCheckBox.CheckedChanged += new System.EventHandler(this.outlineCheckBox_CheckedChanged);
			// 
			// FontDescriptorDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.roundedCheckBox);
			this.Controls.Add(this.antialiasCheckBox);
			this.Controls.Add(this.outlineNumericUpDown);
			this.Controls.Add(this.outlineTrackBar);
			this.Controls.Add(this.outlineCheckBox);
			this.Name = "FontDescriptorDialog";
			this.Size = new System.Drawing.Size(140, 72);
			((System.ComponentModel.ISupportInitialize)(this.outlineNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.outlineTrackBar)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox roundedCheckBox;
		private System.Windows.Forms.CheckBox antialiasCheckBox;
		private System.Windows.Forms.NumericUpDown outlineNumericUpDown;
		private System.Windows.Forms.TrackBar outlineTrackBar;
		private System.Windows.Forms.CheckBox outlineCheckBox;
	}
}
