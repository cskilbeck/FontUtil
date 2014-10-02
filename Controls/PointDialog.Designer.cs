namespace FontUtil
{
	partial class PointDialog
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.XTextBox = new System.Windows.Forms.TextBox();
			this.YTextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(14, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "X";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(63, 3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(14, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Y";
			// 
			// XTextBox
			// 
			this.XTextBox.Location = new System.Drawing.Point(23, 0);
			this.XTextBox.Name = "XTextBox";
			this.XTextBox.Size = new System.Drawing.Size(34, 20);
			this.XTextBox.TabIndex = 2;
			// 
			// YTextBox
			// 
			this.YTextBox.Location = new System.Drawing.Point(83, 0);
			this.YTextBox.Name = "YTextBox";
			this.YTextBox.Size = new System.Drawing.Size(34, 20);
			this.YTextBox.TabIndex = 3;
			// 
			// PointDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.YTextBox);
			this.Controls.Add(this.XTextBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "PointDialog";
			this.Size = new System.Drawing.Size(119, 20);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox XTextBox;
		private System.Windows.Forms.TextBox YTextBox;
	}
}
