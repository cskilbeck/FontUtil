namespace FontUtil
{
	partial class ChannelMaskDialog
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
			this.checkBoxR = new System.Windows.Forms.CheckBox();
			this.checkBoxG = new System.Windows.Forms.CheckBox();
			this.checkBoxB = new System.Windows.Forms.CheckBox();
			this.checkBoxA = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// checkBoxR
			// 
			this.checkBoxR.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkBoxR.AutoSize = true;
			this.checkBoxR.BackColor = System.Drawing.Color.OrangeRed;
			this.checkBoxR.Location = new System.Drawing.Point(0, 0);
			this.checkBoxR.Name = "checkBoxR";
			this.checkBoxR.Size = new System.Drawing.Size(25, 23);
			this.checkBoxR.TabIndex = 1;
			this.checkBoxR.Tag = "";
			this.checkBoxR.Text = "R";
			this.checkBoxR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkBoxR.UseVisualStyleBackColor = false;
			this.checkBoxR.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
			// 
			// checkBoxG
			// 
			this.checkBoxG.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkBoxG.AutoSize = true;
			this.checkBoxG.BackColor = System.Drawing.Color.LimeGreen;
			this.checkBoxG.Location = new System.Drawing.Point(27, 0);
			this.checkBoxG.Name = "checkBoxG";
			this.checkBoxG.Size = new System.Drawing.Size(25, 23);
			this.checkBoxG.TabIndex = 2;
			this.checkBoxG.Text = "G";
			this.checkBoxG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkBoxG.UseVisualStyleBackColor = false;
			this.checkBoxG.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
			// 
			// checkBoxB
			// 
			this.checkBoxB.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkBoxB.AutoSize = true;
			this.checkBoxB.BackColor = System.Drawing.Color.CornflowerBlue;
			this.checkBoxB.Location = new System.Drawing.Point(54, 0);
			this.checkBoxB.Name = "checkBoxB";
			this.checkBoxB.Size = new System.Drawing.Size(24, 23);
			this.checkBoxB.TabIndex = 3;
			this.checkBoxB.Text = "B";
			this.checkBoxB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkBoxB.UseVisualStyleBackColor = false;
			this.checkBoxB.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
			// 
			// checkBoxA
			// 
			this.checkBoxA.Appearance = System.Windows.Forms.Appearance.Button;
			this.checkBoxA.AutoSize = true;
			this.checkBoxA.BackColor = System.Drawing.Color.White;
			this.checkBoxA.Location = new System.Drawing.Point(81, 0);
			this.checkBoxA.Name = "checkBoxA";
			this.checkBoxA.Size = new System.Drawing.Size(24, 23);
			this.checkBoxA.TabIndex = 4;
			this.checkBoxA.Text = "A";
			this.checkBoxA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.checkBoxA.UseVisualStyleBackColor = false;
			this.checkBoxA.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
			// 
			// Mask4
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.checkBoxA);
			this.Controls.Add(this.checkBoxB);
			this.Controls.Add(this.checkBoxG);
			this.Controls.Add(this.checkBoxR);
			this.Name = "Mask4";
			this.Size = new System.Drawing.Size(105, 23);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox checkBoxR;
		private System.Windows.Forms.CheckBox checkBoxG;
		private System.Windows.Forms.CheckBox checkBoxB;
		private System.Windows.Forms.CheckBox checkBoxA;
	}
}
