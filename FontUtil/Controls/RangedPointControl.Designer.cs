namespace FontUtil
{
	partial class RangedPointControl
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
			this.XLabel = new System.Windows.Forms.Label();
			this.YLabel = new System.Windows.Forms.Label();
			this.XTextBox = new System.Windows.Forms.TextBox();
			this.YTextBox = new System.Windows.Forms.TextBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// XLabel
			// 
			this.XLabel.Location = new System.Drawing.Point(6, 5);
			this.XLabel.Name = "XLabel";
			this.XLabel.Size = new System.Drawing.Size(15, 14);
			this.XLabel.TabIndex = 0;
			this.XLabel.Text = "X";
			// 
			// YLabel
			// 
			this.YLabel.Location = new System.Drawing.Point(86, 5);
			this.YLabel.Name = "YLabel";
			this.YLabel.Size = new System.Drawing.Size(15, 14);
			this.YLabel.TabIndex = 1;
			this.YLabel.Text = "Y";
			// 
			// XTextBox
			// 
			this.XTextBox.Location = new System.Drawing.Point(27, 2);
			this.XTextBox.Name = "XTextBox";
			this.XTextBox.Size = new System.Drawing.Size(50, 20);
			this.XTextBox.TabIndex = 2;
			// 
			// YTextBox
			// 
			this.YTextBox.Location = new System.Drawing.Point(107, 2);
			this.YTextBox.Name = "YTextBox";
			this.YTextBox.Size = new System.Drawing.Size(50, 20);
			this.YTextBox.TabIndex = 3;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(9, 28);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(200, 200);
			this.pictureBox1.TabIndex = 4;
			this.pictureBox1.TabStop = false;
			// 
			// RangedPointControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.YTextBox);
			this.Controls.Add(this.XTextBox);
			this.Controls.Add(this.YLabel);
			this.Controls.Add(this.XLabel);
			this.Name = "RangedPointControl";
			this.Padding = new System.Windows.Forms.Padding(2);
			this.Size = new System.Drawing.Size(223, 236);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label XLabel;
		private System.Windows.Forms.Label YLabel;
		private System.Windows.Forms.TextBox XTextBox;
		private System.Windows.Forms.TextBox YTextBox;
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}
