namespace FontUtil
{
	partial class BitmapDialog
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
			this.bitmapPictureBox = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.bitmapPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// bitmapPictureBox
			// 
			this.bitmapPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.bitmapPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.bitmapPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.bitmapPictureBox.ImageLocation = "";
			this.bitmapPictureBox.Location = new System.Drawing.Point(0, 0);
			this.bitmapPictureBox.Name = "bitmapPictureBox";
			this.bitmapPictureBox.Size = new System.Drawing.Size(68, 68);
			this.bitmapPictureBox.TabIndex = 0;
			this.bitmapPictureBox.TabStop = false;
			this.bitmapPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.bitmapPictureBox_MouseClick);
			// 
			// BitmapDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.bitmapPictureBox);
			this.Name = "BitmapDialog";
			this.Size = new System.Drawing.Size(68, 68);
			((System.ComponentModel.ISupportInitialize)(this.bitmapPictureBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox bitmapPictureBox;
	}
}
