namespace FontUtil
{
	partial class MyColorDialog
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
			this.redNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.greenNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.blueNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.alphaNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.redTrackBar = new System.Windows.Forms.TrackBar();
			this.greenTrackBar = new System.Windows.Forms.TrackBar();
			this.blueTrackBar = new System.Windows.Forms.TrackBar();
			this.alphaTrackBar = new System.Windows.Forms.TrackBar();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.colorPictureBox = new System.Windows.Forms.PictureBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			this.button7 = new System.Windows.Forms.Button();
			this.button8 = new System.Windows.Forms.Button();
			this.button12 = new System.Windows.Forms.Button();
			this.button13 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.redNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.greenNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.blueNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.alphaNumericUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.redTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.greenTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.blueTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.alphaTrackBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.colorPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 2);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(13, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "R";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// redNumericUpDown
			// 
			this.redNumericUpDown.Location = new System.Drawing.Point(18, 4);
			this.redNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.redNumericUpDown.Name = "redNumericUpDown";
			this.redNumericUpDown.Size = new System.Drawing.Size(40, 20);
			this.redNumericUpDown.TabIndex = 4;
			this.redNumericUpDown.ValueChanged += new System.EventHandler(this.redNumericUpDown_ValueChanged);
			// 
			// greenNumericUpDown
			// 
			this.greenNumericUpDown.Location = new System.Drawing.Point(18, 24);
			this.greenNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.greenNumericUpDown.Name = "greenNumericUpDown";
			this.greenNumericUpDown.Size = new System.Drawing.Size(40, 20);
			this.greenNumericUpDown.TabIndex = 5;
			this.greenNumericUpDown.ValueChanged += new System.EventHandler(this.greenNumericUpDown_ValueChanged);
			// 
			// blueNumericUpDown
			// 
			this.blueNumericUpDown.Location = new System.Drawing.Point(18, 44);
			this.blueNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.blueNumericUpDown.Name = "blueNumericUpDown";
			this.blueNumericUpDown.Size = new System.Drawing.Size(40, 20);
			this.blueNumericUpDown.TabIndex = 6;
			this.blueNumericUpDown.ValueChanged += new System.EventHandler(this.blueNumericUpDown_ValueChanged);
			// 
			// alphaNumericUpDown
			// 
			this.alphaNumericUpDown.Location = new System.Drawing.Point(18, 64);
			this.alphaNumericUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.alphaNumericUpDown.Name = "alphaNumericUpDown";
			this.alphaNumericUpDown.Size = new System.Drawing.Size(40, 20);
			this.alphaNumericUpDown.TabIndex = 7;
			this.alphaNumericUpDown.ValueChanged += new System.EventHandler(this.alphaNumericUpDown_ValueChanged);
			// 
			// redTrackBar
			// 
			this.redTrackBar.AutoSize = false;
			this.redTrackBar.Location = new System.Drawing.Point(62, 4);
			this.redTrackBar.Maximum = 255;
			this.redTrackBar.Name = "redTrackBar";
			this.redTrackBar.Size = new System.Drawing.Size(111, 20);
			this.redTrackBar.TabIndex = 11;
			this.redTrackBar.TickFrequency = 16;
			this.redTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
			this.redTrackBar.Scroll += new System.EventHandler(this.redTrackBar_Scroll);
			// 
			// greenTrackBar
			// 
			this.greenTrackBar.AutoSize = false;
			this.greenTrackBar.Location = new System.Drawing.Point(62, 24);
			this.greenTrackBar.Maximum = 255;
			this.greenTrackBar.Name = "greenTrackBar";
			this.greenTrackBar.Size = new System.Drawing.Size(111, 20);
			this.greenTrackBar.TabIndex = 12;
			this.greenTrackBar.TickFrequency = 16;
			this.greenTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
			this.greenTrackBar.Scroll += new System.EventHandler(this.greenTrackBar_Scroll);
			// 
			// blueTrackBar
			// 
			this.blueTrackBar.AutoSize = false;
			this.blueTrackBar.Location = new System.Drawing.Point(62, 44);
			this.blueTrackBar.Maximum = 255;
			this.blueTrackBar.Name = "blueTrackBar";
			this.blueTrackBar.Size = new System.Drawing.Size(111, 20);
			this.blueTrackBar.TabIndex = 13;
			this.blueTrackBar.TickFrequency = 16;
			this.blueTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
			this.blueTrackBar.Scroll += new System.EventHandler(this.blueTrackBar_Scroll);
			// 
			// alphaTrackBar
			// 
			this.alphaTrackBar.AutoSize = false;
			this.alphaTrackBar.Location = new System.Drawing.Point(62, 64);
			this.alphaTrackBar.Maximum = 255;
			this.alphaTrackBar.Name = "alphaTrackBar";
			this.alphaTrackBar.Size = new System.Drawing.Size(111, 20);
			this.alphaTrackBar.TabIndex = 14;
			this.alphaTrackBar.TickFrequency = 16;
			this.alphaTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
			this.alphaTrackBar.Scroll += new System.EventHandler(this.alphaTrackBar_Scroll);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(0, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(13, 20);
			this.label2.TabIndex = 15;
			this.label2.Text = "G";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(0, 44);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(13, 20);
			this.label3.TabIndex = 16;
			this.label3.Text = "B";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(0, 64);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(13, 20);
			this.label4.TabIndex = 17;
			this.label4.Text = "A";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// colorPictureBox
			// 
			this.colorPictureBox.Location = new System.Drawing.Point(173, 4);
			this.colorPictureBox.Name = "colorPictureBox";
			this.colorPictureBox.Size = new System.Drawing.Size(43, 20);
			this.colorPictureBox.TabIndex = 18;
			this.colorPictureBox.TabStop = false;
			// 
			// button1
			// 
			this.button1.BackColor = System.Drawing.Color.Lime;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Location = new System.Drawing.Point(188, 42);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(12, 12);
			this.button1.TabIndex = 19;
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new System.EventHandler(this.presetButton_Click);
			// 
			// button2
			// 
			this.button2.BackColor = System.Drawing.Color.Blue;
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button2.Location = new System.Drawing.Point(203, 42);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(12, 12);
			this.button2.TabIndex = 20;
			this.button2.UseVisualStyleBackColor = false;
			this.button2.Click += new System.EventHandler(this.presetButton_Click);
			// 
			// button3
			// 
			this.button3.BackColor = System.Drawing.Color.Fuchsia;
			this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button3.Location = new System.Drawing.Point(203, 57);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(12, 12);
			this.button3.TabIndex = 22;
			this.button3.UseVisualStyleBackColor = false;
			this.button3.Click += new System.EventHandler(this.presetButton_Click);
			// 
			// button4
			// 
			this.button4.BackColor = System.Drawing.Color.Yellow;
			this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button4.Location = new System.Drawing.Point(173, 57);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(12, 12);
			this.button4.TabIndex = 21;
			this.button4.UseVisualStyleBackColor = false;
			this.button4.Click += new System.EventHandler(this.presetButton_Click);
			// 
			// button6
			// 
			this.button6.BackColor = System.Drawing.Color.Cyan;
			this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button6.Location = new System.Drawing.Point(188, 57);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(12, 12);
			this.button6.TabIndex = 25;
			this.button6.UseVisualStyleBackColor = false;
			this.button6.Click += new System.EventHandler(this.presetButton_Click);
			// 
			// button7
			// 
			this.button7.BackColor = System.Drawing.Color.Black;
			this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button7.Location = new System.Drawing.Point(188, 27);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(12, 12);
			this.button7.TabIndex = 24;
			this.button7.UseVisualStyleBackColor = false;
			this.button7.Click += new System.EventHandler(this.presetButton_Click);
			// 
			// button8
			// 
			this.button8.BackColor = System.Drawing.Color.White;
			this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button8.Location = new System.Drawing.Point(173, 27);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(12, 12);
			this.button8.TabIndex = 23;
			this.button8.UseVisualStyleBackColor = false;
			this.button8.Click += new System.EventHandler(this.presetButton_Click);
			// 
			// button12
			// 
			this.button12.BackColor = System.Drawing.Color.Silver;
			this.button12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button12.Location = new System.Drawing.Point(203, 27);
			this.button12.Name = "button12";
			this.button12.Size = new System.Drawing.Size(12, 12);
			this.button12.TabIndex = 27;
			this.button12.UseVisualStyleBackColor = false;
			this.button12.Click += new System.EventHandler(this.presetButton_Click);
			// 
			// button13
			// 
			this.button13.BackColor = System.Drawing.Color.Red;
			this.button13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button13.Location = new System.Drawing.Point(173, 42);
			this.button13.Name = "button13";
			this.button13.Size = new System.Drawing.Size(12, 12);
			this.button13.TabIndex = 31;
			this.button13.UseVisualStyleBackColor = false;
			this.button13.Click += new System.EventHandler(this.presetButton_Click);
			// 
			// MyColorDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.button13);
			this.Controls.Add(this.button12);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.button7);
			this.Controls.Add(this.button8);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.colorPictureBox);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.alphaTrackBar);
			this.Controls.Add(this.blueTrackBar);
			this.Controls.Add(this.greenTrackBar);
			this.Controls.Add(this.redTrackBar);
			this.Controls.Add(this.alphaNumericUpDown);
			this.Controls.Add(this.blueNumericUpDown);
			this.Controls.Add(this.greenNumericUpDown);
			this.Controls.Add(this.redNumericUpDown);
			this.Controls.Add(this.label1);
			this.Name = "MyColorDialog";
			this.Size = new System.Drawing.Size(224, 84);
			((System.ComponentModel.ISupportInitialize)(this.redNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.greenNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.blueNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.alphaNumericUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.redTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.greenTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.blueTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.alphaTrackBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.colorPictureBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown redNumericUpDown;
		private System.Windows.Forms.NumericUpDown greenNumericUpDown;
		private System.Windows.Forms.NumericUpDown blueNumericUpDown;
		private System.Windows.Forms.NumericUpDown alphaNumericUpDown;
		private System.Windows.Forms.TrackBar redTrackBar;
		private System.Windows.Forms.TrackBar greenTrackBar;
		private System.Windows.Forms.TrackBar blueTrackBar;
		private System.Windows.Forms.TrackBar alphaTrackBar;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.PictureBox colorPictureBox;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.Button button12;
		private System.Windows.Forms.Button button13;
	}
}
