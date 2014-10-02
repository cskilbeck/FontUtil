namespace FontUtil
{
	partial class Form1
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pictureBoxTexturePage = new System.Windows.Forms.PictureBox();
			this.pageUpDown = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.pictureBoxNodePreview = new System.Windows.Forms.PictureBox();
			this.textBoxGlyph = new System.Windows.Forms.TextBox();
			this.textBoxSample = new System.Windows.Forms.TextBox();
			this.buttonShowSample = new System.Windows.Forms.Button();
			this.checkBoxKerning = new System.Windows.Forms.CheckBox();
			this.labelABC = new System.Windows.Forms.Label();
			this.pictureBoxGraph = new System.Windows.Forms.PictureBox();
			this.parametersPanel = new System.Windows.Forms.Panel();
			this.buttonSave = new System.Windows.Forms.Button();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.comboBoxWidth = new System.Windows.Forms.ComboBox();
			this.comboBoxHeight = new System.Windows.Forms.ComboBox();
			this.labelWidth = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.labelDimensions = new System.Windows.Forms.Label();
			this.comboBoxPacker = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.designTabPage = new System.Windows.Forms.TabPage();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.splitContainer3 = new System.Windows.Forms.SplitContainer();
			this.outputTabPage = new System.Windows.Forms.TabPage();
			this.buttonCreateFont = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.buttonASCII255 = new System.Windows.Forms.Button();
			this.buttonASCII127 = new System.Windows.Forms.Button();
			this.buttonClearCharacters = new System.Windows.Forms.Button();
			this.textBoxCharacters = new System.Windows.Forms.TextBox();
			this.buttonCharsFromTextFile = new System.Windows.Forms.Button();
			this.checkBoxSinglePixelBorder = new System.Windows.Forms.CheckBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxTexturePage)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pageUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxNodePreview)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxGraph)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.designTabPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			this.outputTabPage.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBoxTexturePage
			// 
			this.pictureBoxTexturePage.Location = new System.Drawing.Point(3, 3);
			this.pictureBoxTexturePage.Margin = new System.Windows.Forms.Padding(0);
			this.pictureBoxTexturePage.Name = "pictureBoxTexturePage";
			this.pictureBoxTexturePage.Size = new System.Drawing.Size(512, 512);
			this.pictureBoxTexturePage.TabIndex = 0;
			this.pictureBoxTexturePage.TabStop = false;
			this.pictureBoxTexturePage.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxPage_Paint);
			this.pictureBoxTexturePage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxPage_MouseDown);
			this.pictureBoxTexturePage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxPage_MouseMove);
			this.pictureBoxTexturePage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxPage_MouseUp);
			// 
			// pageUpDown
			// 
			this.pageUpDown.Location = new System.Drawing.Point(664, 32);
			this.pageUpDown.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.pageUpDown.Name = "pageUpDown";
			this.pageUpDown.Size = new System.Drawing.Size(44, 20);
			this.pageUpDown.TabIndex = 1;
			this.pageUpDown.ValueChanged += new System.EventHandler(this.pageUpDown_ValueChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(676, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Page";
			// 
			// pictureBoxNodePreview
			// 
			this.pictureBoxNodePreview.BackColor = System.Drawing.SystemColors.MenuBar;
			this.pictureBoxNodePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBoxNodePreview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBoxNodePreview.Location = new System.Drawing.Point(0, 0);
			this.pictureBoxNodePreview.Margin = new System.Windows.Forms.Padding(0);
			this.pictureBoxNodePreview.Name = "pictureBoxNodePreview";
			this.pictureBoxNodePreview.Size = new System.Drawing.Size(738, 401);
			this.pictureBoxNodePreview.TabIndex = 5;
			this.pictureBoxNodePreview.TabStop = false;
			this.pictureBoxNodePreview.SizeChanged += new System.EventHandler(this.pictureBoxNodePreview_SizeChanged);
			this.pictureBoxNodePreview.Click += new System.EventHandler(this.pictureBoxGlyphs_Click);
			// 
			// textBoxGlyph
			// 
			this.textBoxGlyph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textBoxGlyph.Location = new System.Drawing.Point(273, 2);
			this.textBoxGlyph.Name = "textBoxGlyph";
			this.textBoxGlyph.Size = new System.Drawing.Size(21, 20);
			this.textBoxGlyph.TabIndex = 6;
			this.textBoxGlyph.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.textBoxGlyph.TextChanged += new System.EventHandler(this.textBoxGlyph_TextChanged);
			// 
			// textBoxSample
			// 
			this.textBoxSample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.textBoxSample.Location = new System.Drawing.Point(2, 2);
			this.textBoxSample.Name = "textBoxSample";
			this.textBoxSample.Size = new System.Drawing.Size(199, 20);
			this.textBoxSample.TabIndex = 7;
			this.textBoxSample.Text = "Hello, World! eyeseze";
			// 
			// buttonShowSample
			// 
			this.buttonShowSample.Location = new System.Drawing.Point(300, 0);
			this.buttonShowSample.Name = "buttonShowSample";
			this.buttonShowSample.Size = new System.Drawing.Size(51, 25);
			this.buttonShowSample.TabIndex = 8;
			this.buttonShowSample.Text = "Show";
			this.buttonShowSample.UseVisualStyleBackColor = true;
			this.buttonShowSample.Click += new System.EventHandler(this.button1_Click);
			// 
			// checkBoxKerning
			// 
			this.checkBoxKerning.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.checkBoxKerning.AutoSize = true;
			this.checkBoxKerning.Checked = true;
			this.checkBoxKerning.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxKerning.Location = new System.Drawing.Point(205, 4);
			this.checkBoxKerning.Margin = new System.Windows.Forms.Padding(0);
			this.checkBoxKerning.Name = "checkBoxKerning";
			this.checkBoxKerning.Size = new System.Drawing.Size(62, 17);
			this.checkBoxKerning.TabIndex = 9;
			this.checkBoxKerning.Text = "Kerning";
			this.checkBoxKerning.UseVisualStyleBackColor = true;
			this.checkBoxKerning.CheckedChanged += new System.EventHandler(this.checkBoxKerning_CheckedChanged);
			// 
			// labelABC
			// 
			this.labelABC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelABC.Location = new System.Drawing.Point(521, 130);
			this.labelABC.Name = "labelABC";
			this.labelABC.Size = new System.Drawing.Size(187, 17);
			this.labelABC.TabIndex = 10;
			this.labelABC.Text = "label2";
			// 
			// pictureBoxGraph
			// 
			this.pictureBoxGraph.BackColor = System.Drawing.SystemColors.MenuBar;
			this.pictureBoxGraph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBoxGraph.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBoxGraph.Location = new System.Drawing.Point(3, 0);
			this.pictureBoxGraph.Margin = new System.Windows.Forms.Padding(0);
			this.pictureBoxGraph.Name = "pictureBoxGraph";
			this.pictureBoxGraph.Size = new System.Drawing.Size(1093, 401);
			this.pictureBoxGraph.TabIndex = 12;
			this.pictureBoxGraph.TabStop = false;
			// 
			// parametersPanel
			// 
			this.parametersPanel.AutoScroll = true;
			this.parametersPanel.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.parametersPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.parametersPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.parametersPanel.Location = new System.Drawing.Point(0, 0);
			this.parametersPanel.Margin = new System.Windows.Forms.Padding(0);
			this.parametersPanel.Name = "parametersPanel";
			this.parametersPanel.Size = new System.Drawing.Size(351, 372);
			this.parametersPanel.TabIndex = 13;
			// 
			// buttonSave
			// 
			this.buttonSave.Location = new System.Drawing.Point(518, 3);
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.Size = new System.Drawing.Size(75, 23);
			this.buttonSave.TabIndex = 16;
			this.buttonSave.Text = "Save";
			this.buttonSave.UseVisualStyleBackColor = true;
			this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
			// 
			// progressBar1
			// 
			this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar1.Location = new System.Drawing.Point(7, 134);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(187, 23);
			this.progressBar1.TabIndex = 17;
			// 
			// comboBoxWidth
			// 
			this.comboBoxWidth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxWidth.DropDownWidth = 63;
			this.comboBoxWidth.FormattingEnabled = true;
			this.comboBoxWidth.Items.AddRange(new object[] {
            "128",
            "256",
            "512",
            "1024"});
			this.comboBoxWidth.Location = new System.Drawing.Point(540, 40);
			this.comboBoxWidth.Name = "comboBoxWidth";
			this.comboBoxWidth.Size = new System.Drawing.Size(63, 21);
			this.comboBoxWidth.TabIndex = 18;
			this.comboBoxWidth.SelectedIndexChanged += new System.EventHandler(this.comboBoxWidth_SelectedIndexChanged);
			// 
			// comboBoxHeight
			// 
			this.comboBoxHeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxHeight.DropDownWidth = 63;
			this.comboBoxHeight.FormattingEnabled = true;
			this.comboBoxHeight.Items.AddRange(new object[] {
            "128",
            "256",
            "512",
            "1024"});
			this.comboBoxHeight.Location = new System.Drawing.Point(540, 70);
			this.comboBoxHeight.Name = "comboBoxHeight";
			this.comboBoxHeight.Size = new System.Drawing.Size(63, 21);
			this.comboBoxHeight.TabIndex = 19;
			this.comboBoxHeight.SelectedIndexChanged += new System.EventHandler(this.comboBoxHeight_SelectedIndexChanged);
			// 
			// labelWidth
			// 
			this.labelWidth.Location = new System.Drawing.Point(518, 38);
			this.labelWidth.Name = "labelWidth";
			this.labelWidth.Size = new System.Drawing.Size(16, 23);
			this.labelWidth.TabIndex = 20;
			this.labelWidth.Text = "W";
			this.labelWidth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(518, 68);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(16, 23);
			this.label2.TabIndex = 21;
			this.label2.Text = "H";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelDimensions
			// 
			this.labelDimensions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelDimensions.Location = new System.Drawing.Point(634, 55);
			this.labelDimensions.Name = "labelDimensions";
			this.labelDimensions.Size = new System.Drawing.Size(74, 48);
			this.labelDimensions.TabIndex = 22;
			this.labelDimensions.Text = "X x Y";
			this.labelDimensions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// comboBoxPacker
			// 
			this.comboBoxPacker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxPacker.FormattingEnabled = true;
			this.comboBoxPacker.Location = new System.Drawing.Point(570, 106);
			this.comboBoxPacker.Name = "comboBoxPacker";
			this.comboBoxPacker.Size = new System.Drawing.Size(138, 21);
			this.comboBoxPacker.TabIndex = 23;
			this.comboBoxPacker.SelectedIndexChanged += new System.EventHandler(this.comboBoxPacker_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(522, 106);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(42, 21);
			this.label3.TabIndex = 24;
			this.label3.Text = "Packer";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.designTabPage);
			this.tabControl1.Controls.Add(this.outputTabPage);
			this.tabControl1.Location = new System.Drawing.Point(-3, 22);
			this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.Padding = new System.Drawing.Point(0, 0);
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(1107, 838);
			this.tabControl1.TabIndex = 28;
			// 
			// designTabPage
			// 
			this.designTabPage.Controls.Add(this.splitContainer1);
			this.designTabPage.Location = new System.Drawing.Point(4, 22);
			this.designTabPage.Margin = new System.Windows.Forms.Padding(0);
			this.designTabPage.Name = "designTabPage";
			this.designTabPage.Size = new System.Drawing.Size(1099, 812);
			this.designTabPage.TabIndex = 0;
			this.designTabPage.Text = "Design";
			this.designTabPage.UseVisualStyleBackColor = true;
			// 
			// splitContainer1
			// 
			this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
			this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
			this.splitContainer1.Panel1MinSize = 20;
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.pictureBoxGraph);
			this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.splitContainer1.Size = new System.Drawing.Size(1099, 812);
			this.splitContainer1.SplitterDistance = 404;
			this.splitContainer1.TabIndex = 27;
			this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer2.Location = new System.Drawing.Point(3, 3);
			this.splitContainer2.Margin = new System.Windows.Forms.Padding(0);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.pictureBoxNodePreview);
			this.splitContainer2.Size = new System.Drawing.Size(1093, 401);
			this.splitContainer2.SplitterDistance = 351;
			this.splitContainer2.TabIndex = 14;
			this.splitContainer2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer2_SplitterMoved);
			// 
			// splitContainer3
			// 
			this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.splitContainer3.IsSplitterFixed = true;
			this.splitContainer3.Location = new System.Drawing.Point(0, 0);
			this.splitContainer3.Margin = new System.Windows.Forms.Padding(0);
			this.splitContainer3.Name = "splitContainer3";
			this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer3.Panel1
			// 
			this.splitContainer3.Panel1.Controls.Add(this.parametersPanel);
			// 
			// splitContainer3.Panel2
			// 
			this.splitContainer3.Panel2.Controls.Add(this.buttonShowSample);
			this.splitContainer3.Panel2.Controls.Add(this.textBoxGlyph);
			this.splitContainer3.Panel2.Controls.Add(this.checkBoxKerning);
			this.splitContainer3.Panel2.Controls.Add(this.textBoxSample);
			this.splitContainer3.Size = new System.Drawing.Size(351, 401);
			this.splitContainer3.SplitterDistance = 372;
			this.splitContainer3.TabIndex = 14;
			// 
			// outputTabPage
			// 
			this.outputTabPage.Controls.Add(this.buttonCreateFont);
			this.outputTabPage.Controls.Add(this.groupBox1);
			this.outputTabPage.Controls.Add(this.checkBoxSinglePixelBorder);
			this.outputTabPage.Controls.Add(this.label2);
			this.outputTabPage.Controls.Add(this.labelWidth);
			this.outputTabPage.Controls.Add(this.pageUpDown);
			this.outputTabPage.Controls.Add(this.label3);
			this.outputTabPage.Controls.Add(this.pictureBoxTexturePage);
			this.outputTabPage.Controls.Add(this.label1);
			this.outputTabPage.Controls.Add(this.comboBoxPacker);
			this.outputTabPage.Controls.Add(this.labelDimensions);
			this.outputTabPage.Controls.Add(this.labelABC);
			this.outputTabPage.Controls.Add(this.comboBoxHeight);
			this.outputTabPage.Controls.Add(this.buttonSave);
			this.outputTabPage.Controls.Add(this.comboBoxWidth);
			this.outputTabPage.Location = new System.Drawing.Point(4, 22);
			this.outputTabPage.Name = "outputTabPage";
			this.outputTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.outputTabPage.Size = new System.Drawing.Size(717, 812);
			this.outputTabPage.TabIndex = 1;
			this.outputTabPage.Text = "Output";
			this.outputTabPage.UseVisualStyleBackColor = true;
			// 
			// buttonCreateFont
			// 
			this.buttonCreateFont.Location = new System.Drawing.Point(600, 4);
			this.buttonCreateFont.Name = "buttonCreateFont";
			this.buttonCreateFont.Size = new System.Drawing.Size(75, 23);
			this.buttonCreateFont.TabIndex = 27;
			this.buttonCreateFont.Text = "Create";
			this.buttonCreateFont.UseVisualStyleBackColor = true;
			this.buttonCreateFont.Click += new System.EventHandler(this.buttonCreateFont_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.buttonASCII255);
			this.groupBox1.Controls.Add(this.buttonASCII127);
			this.groupBox1.Controls.Add(this.buttonClearCharacters);
			this.groupBox1.Controls.Add(this.progressBar1);
			this.groupBox1.Controls.Add(this.textBoxCharacters);
			this.groupBox1.Controls.Add(this.buttonCharsFromTextFile);
			this.groupBox1.Location = new System.Drawing.Point(521, 173);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(200, 342);
			this.groupBox1.TabIndex = 26;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Characters";
			// 
			// buttonASCII255
			// 
			this.buttonASCII255.Location = new System.Drawing.Point(105, 79);
			this.buttonASCII255.Name = "buttonASCII255";
			this.buttonASCII255.Size = new System.Drawing.Size(89, 23);
			this.buttonASCII255.TabIndex = 4;
			this.buttonASCII255.Text = "ASCII 0-255";
			this.buttonASCII255.UseVisualStyleBackColor = true;
			this.buttonASCII255.Click += new System.EventHandler(this.buttonASCII255_Click);
			// 
			// buttonASCII127
			// 
			this.buttonASCII127.Location = new System.Drawing.Point(7, 79);
			this.buttonASCII127.Name = "buttonASCII127";
			this.buttonASCII127.Size = new System.Drawing.Size(91, 23);
			this.buttonASCII127.TabIndex = 3;
			this.buttonASCII127.Text = "ASCII 0-127";
			this.buttonASCII127.UseVisualStyleBackColor = true;
			this.buttonASCII127.Click += new System.EventHandler(this.buttonASCII_Click);
			// 
			// buttonClearCharacters
			// 
			this.buttonClearCharacters.Location = new System.Drawing.Point(7, 20);
			this.buttonClearCharacters.Name = "buttonClearCharacters";
			this.buttonClearCharacters.Size = new System.Drawing.Size(187, 23);
			this.buttonClearCharacters.TabIndex = 2;
			this.buttonClearCharacters.Text = "Clear";
			this.buttonClearCharacters.UseVisualStyleBackColor = true;
			this.buttonClearCharacters.Click += new System.EventHandler(this.buttonClearCharacters_Click);
			// 
			// textBoxCharacters
			// 
			this.textBoxCharacters.Location = new System.Drawing.Point(7, 108);
			this.textBoxCharacters.Name = "textBoxCharacters";
			this.textBoxCharacters.Size = new System.Drawing.Size(187, 20);
			this.textBoxCharacters.TabIndex = 1;
			this.textBoxCharacters.WordWrap = false;
			// 
			// buttonCharsFromTextFile
			// 
			this.buttonCharsFromTextFile.Location = new System.Drawing.Point(7, 49);
			this.buttonCharsFromTextFile.Name = "buttonCharsFromTextFile";
			this.buttonCharsFromTextFile.Size = new System.Drawing.Size(187, 23);
			this.buttonCharsFromTextFile.TabIndex = 0;
			this.buttonCharsFromTextFile.Text = "From Text File";
			this.buttonCharsFromTextFile.UseVisualStyleBackColor = true;
			this.buttonCharsFromTextFile.Click += new System.EventHandler(this.buttonCharsFromTextFile_Click);
			// 
			// checkBoxSinglePixelBorder
			// 
			this.checkBoxSinglePixelBorder.AutoSize = true;
			this.checkBoxSinglePixelBorder.Checked = true;
			this.checkBoxSinglePixelBorder.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxSinglePixelBorder.Location = new System.Drawing.Point(525, 150);
			this.checkBoxSinglePixelBorder.Name = "checkBoxSinglePixelBorder";
			this.checkBoxSinglePixelBorder.Size = new System.Drawing.Size(114, 17);
			this.checkBoxSinglePixelBorder.TabIndex = 25;
			this.checkBoxSinglePixelBorder.Text = "Single Pixel Border";
			this.checkBoxSinglePixelBorder.UseVisualStyleBackColor = true;
			// 
			// menuStrip1
			// 
			this.menuStrip1.BackColor = System.Drawing.SystemColors.MenuBar;
			this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(0);
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(4, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Padding = new System.Windows.Forms.Padding(0);
			this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.menuStrip1.Size = new System.Drawing.Size(1100, 24);
			this.menuStrip1.TabIndex = 29;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 24);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.newToolStripMenuItem.Text = "&New";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.openToolStripMenuItem.Text = "&Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.saveAsToolStripMenuItem.Text = "Save &As";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// exportToolStripMenuItem
			// 
			this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
			this.exportToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.exportToolStripMenuItem.Text = "&Export As";
			this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.MenuBar;
			this.ClientSize = new System.Drawing.Size(1108, 857);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.menuStrip1);
			this.DoubleBuffered = true;
			this.KeyPreview = true;
			this.MainMenuStrip = this.menuStrip1;
			this.MinimumSize = new System.Drawing.Size(200, 100);
			this.Name = "Form1";
			this.Padding = new System.Windows.Forms.Padding(4, 0, 4, 4);
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Form1";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxTexturePage)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pageUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxNodePreview)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxGraph)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.designTabPage.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			this.splitContainer3.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
			this.splitContainer3.ResumeLayout(false);
			this.outputTabPage.ResumeLayout(false);
			this.outputTabPage.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBoxTexturePage;
		private System.Windows.Forms.NumericUpDown pageUpDown;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pictureBoxNodePreview;
		private System.Windows.Forms.TextBox textBoxGlyph;
		private System.Windows.Forms.TextBox textBoxSample;
		private System.Windows.Forms.Button buttonShowSample;
		private System.Windows.Forms.CheckBox checkBoxKerning;
		private System.Windows.Forms.Label labelABC;
		private System.Windows.Forms.PictureBox pictureBoxGraph;
		private System.Windows.Forms.Panel parametersPanel;
		private System.Windows.Forms.Button buttonSave;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.ComboBox comboBoxWidth;
		private System.Windows.Forms.ComboBox comboBoxHeight;
		private System.Windows.Forms.Label labelWidth;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label labelDimensions;
		private System.Windows.Forms.ComboBox comboBoxPacker;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage designTabPage;
		private System.Windows.Forms.TabPage outputTabPage;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.SplitContainer splitContainer3;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
		private System.Windows.Forms.CheckBox checkBoxSinglePixelBorder;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button buttonCharsFromTextFile;
		private System.Windows.Forms.Button buttonClearCharacters;
		private System.Windows.Forms.TextBox textBoxCharacters;
		private System.Windows.Forms.Button buttonASCII127;
		private System.Windows.Forms.Button buttonASCII255;
		private System.Windows.Forms.Button buttonCreateFont;
	}
}

