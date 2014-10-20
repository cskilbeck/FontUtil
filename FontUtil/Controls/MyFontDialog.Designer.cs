namespace FontUtil
{
	partial class MyFontDialog
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
			this.fontsComboBox = new System.Windows.Forms.ComboBox();
			this.sizesComboBox = new System.Windows.Forms.ComboBox();
			this.stylesComboBox = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// fontsComboBox
			// 
			this.fontsComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.fontsComboBox.DropDownHeight = 300;
			this.fontsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.fontsComboBox.DropDownWidth = 200;
			this.fontsComboBox.FormattingEnabled = true;
			this.fontsComboBox.IntegralHeight = false;
			this.fontsComboBox.ItemHeight = 22;
			this.fontsComboBox.Location = new System.Drawing.Point(0, 0);
			this.fontsComboBox.Margin = new System.Windows.Forms.Padding(0);
			this.fontsComboBox.MaxDropDownItems = 32;
			this.fontsComboBox.Name = "fontsComboBox";
			this.fontsComboBox.Size = new System.Drawing.Size(137, 28);
			this.fontsComboBox.TabIndex = 1;
			this.fontsComboBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.fontsComboBox_DrawItem);
			this.fontsComboBox.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.fontsComboBox_MeasureItem);
			this.fontsComboBox.SelectedIndexChanged += new System.EventHandler(this.fontsComboBox_SelectedIndexChanged);
			// 
			// sizesComboBox
			// 
			this.sizesComboBox.FormattingEnabled = true;
			this.sizesComboBox.Location = new System.Drawing.Point(218, 0);
			this.sizesComboBox.Name = "sizesComboBox";
			this.sizesComboBox.Size = new System.Drawing.Size(67, 21);
			this.sizesComboBox.TabIndex = 3;
			this.sizesComboBox.SelectedIndexChanged += new System.EventHandler(this.sizesComboBox_SelectedIndexChanged);
			this.sizesComboBox.TextChanged += new System.EventHandler(this.sizesComboBox_TextChanged);
			// 
			// stylesComboBox
			// 
			this.stylesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.stylesComboBox.FormattingEnabled = true;
			this.stylesComboBox.Location = new System.Drawing.Point(140, 0);
			this.stylesComboBox.Name = "stylesComboBox";
			this.stylesComboBox.Size = new System.Drawing.Size(72, 21);
			this.stylesComboBox.TabIndex = 10;
			this.stylesComboBox.SelectedIndexChanged += new System.EventHandler(this.stylesComboBox_SelectedIndexChanged);
			// 
			// MyFontDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.stylesComboBox);
			this.Controls.Add(this.sizesComboBox);
			this.Controls.Add(this.fontsComboBox);
			this.Name = "MyFontDialog";
			this.Size = new System.Drawing.Size(286, 30);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox fontsComboBox;
		private System.Windows.Forms.ComboBox sizesComboBox;
		private System.Windows.Forms.ComboBox stylesComboBox;
	}
}
