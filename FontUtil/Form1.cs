// Different kinds of anti aliasing on GDI TrueType renderer
// RangedInt Control (slider)
// Set character ranges for preview and export/save
// Undo?
// Enforce acyclicity on the graph
// Tranform node and Transform on the layer
// Convolution plugin
// Other plugins?
// Fix zooming in tpages which are < 512x512
// Xbox & WP7 runtimes
// WP7 pipeline
// Fix glyph caching
// Glyph renderer plugin system

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace FontUtil
{
    public partial class Form1 : Form
    {
        // Used for the tpage size combo datasource
        private class PowerOf2
        {
            public string text { get; set; }
            public int value { get; set; }

            public PowerOf2(int p)
            {
                this.value = 1 << p;
                this.text = value.ToString();
            }

            public override string ToString()
            {
                return text;
            }
        }

        // Used for the packer type combo datasource
        private class PackerType
        {
            public string name { get; set; }
            public PackingType packingType;

            public PackerType(PackingType type)
            {
                packingType = type;
                foreach (PackingType p in Enum.GetValues(typeof(PackingType)))
                {
                    if (p == type)
                    {
                        name = Enum.GetName(typeof(PackingType), type);
                        break;
                    }
                }
            }

            public override string ToString()
            {
                return name;
            }

            public static implicit operator PackingType(PackerType packerType)
            {
                return packerType.packingType;
            }
        }

        BitmapFont bitmapFont;
        Bitmap texturePageBitmap;
        Bitmap previewNodeBitmap;
        Point mouseGrabOffset;
        PointF texturePageDisplayOffset;
        float zoomLevel;
        bool mouseGrabbed;
        int currentTPage;
        int fontPageWidth;
        int fontPageHeight;
        bool changed;
        BitmapFont currentFont;

        Color glyphPreviewColor = Color.FromKnownColor(KnownColor.ControlDark);

        Dictionary<char, int> chars = new Dictionary<char, int>();

        char previewChar;

        static List<PackerType> packerTypes = new List<PackerType>();
        static List<PowerOf2> powersWidth = new List<PowerOf2>();
        static List<PowerOf2> powersHeight = new List<PowerOf2>();

        delegate void FontCompleteCallback(BitmapFont font);
        delegate void SetProgressBarCallback(int percent);

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ThreadManager.DeleteAllThreads();
        }

        public Form1()
        {
            InitializeComponent();

            TTFParser p = new TTFParser();

            Node.SetParentPanel(parametersPanel);

            GraphManager.PictureBox = pictureBoxGraph;

            GraphManager.RedrawRequired += new EventHandler<EventArgs>(GraphManager_RedrawRequired);
            GraphManager.RefreshRequired += new EventHandler<EventArgs>(GraphManager_RefreshRequired);

            previewChar = 'y';
            textBoxGlyph.Text = "y";

            pictureBoxTexturePage.MouseWheel += new MouseEventHandler(pictureBoxPage_MouseWheel);

            pictureBoxTexturePage.Select();

            for (int i = 7; i <= 12; ++i)	// 128 to 4096 tpage dimensions
            {
                powersWidth.Add(new PowerOf2(i));
                powersHeight.Add(new PowerOf2(i));
            }

            comboBoxWidth.ValueMember = "value";
            comboBoxWidth.DataSource = powersWidth;
            comboBoxWidth.SelectedIndex = 2;

            comboBoxHeight.ValueMember = "value";
            comboBoxHeight.DataSource = powersHeight;
            comboBoxHeight.SelectedIndex = 3;

            packerTypes.Add(new PackerType(PackingType.Arevalo));
            packerTypes.Add(new PackerType(PackingType.Cygon));

            comboBoxPacker.ValueMember = "value";
            comboBoxPacker.DataSource = packerTypes;
            comboBoxPacker.SelectedIndex = 0;

            mouseGrabOffset = new Point(0, 0);
            texturePageDisplayOffset = new PointF(0, 0);
            mouseGrabbed = false;

            pictureBoxTexturePage.Width = 512;
            pictureBoxTexturePage.Height = 512;

            texturePageBitmap = new Bitmap(fontPageWidth, fontPageHeight);
            pictureBoxTexturePage.Image = texturePageBitmap;

            GraphManager.UpdateGraph();

            ScanPlugins();

            zoomLevel = 1;

            FontNode f = GraphManager.CreateSourceNode(new Point(100, 100));
            LayerNode l = GraphManager.NewLayer(f.Outputs[0].Pin.Connection.ReceivingPin);
            l.Position = new Point(400, 200);
            l.Dirty = true;
            l.GetGraphic();
            l.SetParameter("Measure", true);

            GraphManager.SelectedThing = f;
            GraphManager.UpdateGraph();

            for (char c = ' '; c < (int)128; ++c)
            {
                AddChar(c);
            }
            RefreshChars();

        }

        void GraphManager_RedrawRequired(object sender, EventArgs e)
        {
            pictureBoxGraph.Refresh();
        }

        void GraphManager_RefreshRequired(object sender, EventArgs e)
        {
            RefreshPreview(GraphManager.SelectedNode);
            pictureBoxNodePreview.Refresh();
        }

        private void ShowPage(int page)
        {
            if (bitmapFont.pages.Count > page && page != -1)
            {
                currentTPage = page;
                using (Graphics g = Graphics.FromImage(texturePageBitmap))
                {
                    g.FillRectangle(Brushes.Black, texturePageBitmap.Dimensions());
                }
                texturePageDisplayOffset.X = 0;
                texturePageDisplayOffset.Y = 0;
                RedrawPicBmp();
                Bitmap bmp = CurrentPage.bitmap;
                labelDimensions.Text = string.Format("page {0} of {1}\r\n{2} x {3}", currentTPage + 1, bitmapFont.pages.Count, bmp.Width, bmp.Height);
            }
        }

        void RedrawPicBmp()
        {
            if (bitmapFont != null && bitmapFont.pages.Count > 0)
            {
                texturePageDisplayOffset.X = Math.Max(0, Math.Min(texturePageDisplayOffset.X, CurrentPage.bitmap.Width - pictureBoxTexturePage.Width / zoomLevel));
                texturePageDisplayOffset.Y = Math.Max(0, Math.Min(texturePageDisplayOffset.Y, CurrentPage.bitmap.Height - pictureBoxTexturePage.Height / zoomLevel));

                int w = CurrentPage.nonAlphaBitmap.Width;
                int h = CurrentPage.nonAlphaBitmap.Height;

                using (Graphics g = Graphics.FromImage(texturePageBitmap))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                    float sourceWidth = pictureBoxTexturePage.Width / zoomLevel;
                    float sourceHeight = pictureBoxTexturePage.Height / zoomLevel;
                    float sourceX = texturePageDisplayOffset.X - 0.5f;
                    float sourceY = texturePageDisplayOffset.Y - 0.5f;
                    g.FillRectangle(Brushes.Green, texturePageBitmap.Dimensions());
                    Rectangle d = pictureBoxTexturePage.DisplayRectangle;
                    g.DrawImage(CurrentPage.nonAlphaBitmap, d, sourceX, sourceY, sourceWidth, sourceHeight, GraphicsUnit.Pixel);
                }
                pictureBoxTexturePage.Refresh();
            }
        }

        private void pageUpDown_ValueChanged(object sender, System.EventArgs e)
        {
            ShowPage((int)pageUpDown.Value - 1);
        }

        private void textBoxGlyph_TextChanged(object sender, System.EventArgs e)
        {
            if (textBoxGlyph.Text.Length > 0)
            {
                previewChar = textBoxGlyph.Text[0];
                textBoxGlyph.Text = new string(new char[] { previewChar });
                textBoxGlyph.SelectAll();
                RefreshPreview(GraphManager.SelectedNode);
            }
        }

        private void pictureBoxPage_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.LightGoldenrodYellow, 0, 0, pictureBoxTexturePage.Width, pictureBoxTexturePage.Height);
            e.Graphics.DrawImage(texturePageBitmap, texturePageBitmap.Dimensions());
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            ShowSample(textBoxSample.Text);
            pictureBoxNodePreview.Refresh();
        }

        private void checkBoxKerning_CheckedChanged(object sender, System.EventArgs e)
        {
            //ShowSample(textBoxSample.Text);
        }

        public static void ScanPlugins()
        {
            string path = Assembly.GetExecutingAssembly().GetLoadedModules(false)[0].FullyQualifiedName;
            path = Path.GetDirectoryName(path);
            path = Path.Combine(path, "Plugins");
            PluginManager.ScanDirectory(path);
        }

        public void Run()
        {
            // root is always a BitmapFont
            // iterate the Graphics
            // for each Graphic, run the plugin graph
            // it's a DAG, but with tree-like limitations
        }

        private void buttonSave_Click(object sender, System.EventArgs e)
        {
            SaveAs(false);
        }

        Page CurrentPage
        {
            get
            {
                return bitmapFont.pages[currentTPage];
            }
        }

        void pictureBoxPage_MouseWheel(object sender, MouseEventArgs e)
        {
            float oz = zoomLevel;
            zoomLevel = Math.Min(20, Math.Max(1, zoomLevel + Math.Sign(e.Delta)));
            texturePageDisplayOffset.X += e.X / oz - e.X / zoomLevel;
            texturePageDisplayOffset.Y += e.Y / oz - e.Y / zoomLevel;
            RedrawPicBmp();
        }

        private void pictureBoxPage_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBoxTexturePage.Select();
            if (bitmapFont != null)
            {
                mouseGrabbed = true;
                mouseGrabOffset.X = e.X;
                mouseGrabOffset.Y = e.Y;
            }
        }

        private void pictureBoxPage_MouseUp(object sender, MouseEventArgs e)
        {
            mouseGrabbed = false;
        }

        private void pictureBoxPage_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseGrabbed)
            {
                PointF o = texturePageDisplayOffset;

                float z = 1.0f / zoomLevel;

                texturePageDisplayOffset.X += (mouseGrabOffset.X - e.X) * z;
                texturePageDisplayOffset.Y += (mouseGrabOffset.Y - e.Y) * z;

                mouseGrabOffset = e.Location;

                if (texturePageDisplayOffset != o)
                {
                    RedrawPicBmp();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void comboBoxWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            fontPageWidth = (int)comboBoxWidth.SelectedValue;
        }

        private void comboBoxHeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            fontPageHeight = (int)comboBoxHeight.SelectedValue;
        }

        private void comboBoxPacker_SelectedIndexChanged(object sender, EventArgs e)
        {
            BitmapFont.FontPackingType = ((PackerType)comboBoxPacker.SelectedItem).packingType;
        }

        void UpdatePreviewChar()
        {
            if (FontNode.createChar != previewChar)
            {
                FontNode.createChar = previewChar;
                FontNode.SetAllDirty();
            }
        }

        private void ShowSample(string sample)
        {
            if (FontNode.allFontNodes.Count > 0)
            {
                bitmapFont = new BitmapFont(FontNode.theFont.face, FontNode.theFont.size, fontPageWidth, fontPageHeight, false);

                char[] c = textBoxSample.Text.ToCharArray();
                if (c.Length > 0)
                {
                    FontNode.createChar = (char)0;
                    bitmapFont.CreateAllLayers(c, checkBoxSinglePixelBorder.Checked);
                    bitmapFont.useKerning = checkBoxKerning.Checked;
                    bitmapFont.numLayers = LayerNode.allLayers.Count;

                    using (Graphics gr = Graphics.FromImage(previewNodeBitmap))
                    {
                        gr.FillRectangle(new SolidBrush(glyphPreviewColor), previewNodeBitmap.Dimensions());
                        PointF pos = new PointF(16, 16);
                        bitmapFont.PlotString(sample, previewNodeBitmap, ref pos);
                    }
                    pictureBoxNodePreview.Refresh();
                }
            }
        }

        void RefreshPreview(Node selectedNode = null)
        {
            //Debug.WriteLine("RefreshPreview!");
            if (previewNodeBitmap == null)
            {
                previewNodeBitmap = new Bitmap(pictureBoxNodePreview.Width, pictureBoxNodePreview.Height);
                pictureBoxNodePreview.Image = previewNodeBitmap;
            }

            UpdatePreviewChar();

            using (Graphics gr = Graphics.FromImage(previewNodeBitmap))
            {
                gr.FillRectangle(new SolidBrush(glyphPreviewColor), previewNodeBitmap.Dimensions());
                gr.DrawLine(Pens.Black, new Point(previewNodeBitmap.Width / 2, 0), new Point(previewNodeBitmap.Width / 2, previewNodeBitmap.Height));

                if (selectedNode != null)
                {
                    PreviewNode(selectedNode, gr);
                }

                if (LayerNode.allLayers.Count > 0)
                {
                    Graphic topGraphic = null;
                    int index = LayerNode.allLayers.Count - 1;

                    do
                    {
                        topGraphic = LayerNode.allLayers[index].GetGraphic();
                        index--;
                    }
                    while (topGraphic == null && index >= 0);

                    if (topGraphic != null && topGraphic.bmp != null)
                    {
                        PointF o = topGraphic.drawOffset;

                        int hw = topGraphic.bmp.Width / 2;
                        int hh = topGraphic.bmp.Height / 2;
                        hw = previewNodeBitmap.Width / 2 + previewNodeBitmap.Width / 4 - hw;
                        hh = previewNodeBitmap.Height / 2 - hh;
                        hw -= (int)o.X;
                        hh -= (int)o.Y;
                        Point offset = new Point(hw, hh);

                        foreach (LayerNode layer in LayerNode.allLayers)
                        {
                            Graphic g = layer.GetGraphic();
                            if (g != null)
                            {
                                gr.SetClip(new Rectangle(previewNodeBitmap.Width / 2, 0, previewNodeBitmap.Width / 2, previewNodeBitmap.Height));
                                layer.DrawPreview(gr, offset);
                                gr.SetClip(previewNodeBitmap.Dimensions());
                            }
                        }
                    }
                }
            }
            pictureBoxNodePreview.Refresh();
            currentFont = null;
        }

        void PreviewNode(Node node, Graphics gr)
        {
            UpdatePreviewChar();
            Graphic g = node.GetGraphic();
            if (g != null && g.bmp != null)
            {
                Point mid = new Point(previewNodeBitmap.Width / 4, previewNodeBitmap.Height / 2);
                gr.SetClip(new Rectangle(0, 0, previewNodeBitmap.Width / 2, previewNodeBitmap.Height));
                int hw = g.bmp.Width / 2;
                int hh = g.bmp.Height / 2;
                node.DrawPreview(gr, mid.Subtract(new Point((int)(g.drawOffset.X + hw), (int)g.drawOffset.Y + hh)));
                gr.SetClip(previewNodeBitmap.Dimensions());
            }
        }

        private void pictureBoxGlyphs_Click(object sender, EventArgs e)
        {
            ColorDialog d = new ColorDialog();
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                glyphPreviewColor = d.Color;
                RefreshPreview(GraphManager.SelectedNode);
            }
        }

        private void pictureBoxNodePreview_SizeChanged(object sender, EventArgs e)
        {
            previewNodeBitmap = new Bitmap(pictureBoxNodePreview.Width, pictureBoxNodePreview.Height);
            previewNodeBitmap.Fill(glyphPreviewColor);
            pictureBoxNodePreview.Image = previewNodeBitmap;
            RefreshPreview(GraphManager.SelectedNode);
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            splitContainer1.Parent.Select();
        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            splitContainer2.Parent.Select();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        BitmapFont CreateBitmapFont(bool export, bool force = false)
        {
            if (currentFont == null || force)
            {
                currentFont = new BitmapFont(FontNode.theFont.face, FontNode.theFont.size, fontPageWidth, fontPageHeight, false);

                char[] c = new char[textBoxCharacters.Text.Length];

                int i = 0;
                foreach (char ch in textBoxCharacters.Text)
                {
                    if (ch != 0)
                    {
                        c[i++] = ch;
                    }
                }

                FontNode.createChar = (char)0;

                // do this in an async! Use a copy of the graph...
                if (export)
                {
                    currentFont.CreateAllLayers(c, checkBoxSinglePixelBorder.Checked);
                }

                currentFont.useKerning = checkBoxKerning.Checked;
                currentFont.numLayers = LayerNode.allLayers.Count;

                bitmapFont = currentFont;

                pageUpDown.Minimum = 1;
                pageUpDown.Maximum = currentFont.pages.Count;
                zoomLevel = 1;
                ShowPage(0);
                pictureBoxTexturePage.Select();
                labelABC.Text = "Usage: " + currentFont.Utilization() * 100 + "%";
                changed = false;
            }
            return currentFont;
        }

        void SaveAs(bool export)
        {
            if (FontNode.allFontNodes.Count == 0)
            {
                MessageBox.Show("Can't save or export without at least 1 source node defined");
            }
            else if (LayerNode.allLayers.Count == 0 && export)
            {
                MessageBox.Show("Can't export with no layers defined...");
            }
            // many more checks need to be done here...
            else
            {
                BitmapFont saveFont = CreateBitmapFont(export, force: false);

                SaveFileDialog s = new SaveFileDialog();
                s.CheckPathExists = true;
                s.DefaultExt = ".bitmapfont";
                s.OverwritePrompt = true;
                s.Title = "Save font";
                s.ValidateNames = true;
                s.Filter = "Bitmapfont file (*.bitmapfont)|*.bitmapfont";

                TTFontFace fontFace = saveFont.FontFace;
                TTFontFamily fontFamily = saveFont.Font;

                s.FileName = fontFamily.ToString() + "_" + fontFace.name + "_" + saveFont.height.ToString() + ".bitmapfont";

                if (s.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    BitmapParameter.WorkingFolder = System.IO.Path.GetDirectoryName(s.FileName);
                    saveFont.Save(s.FileName, export);
                }
            }
        }

        void LoadFont()
        {
            OpenFileDialog s = new OpenFileDialog();
            s.Title = "Load font";
            s.DefaultExt = ".bitmapfont";
            s.ValidateNames = true;
            s.CheckFileExists = true;
            s.Filter = "Bitmapfont file (*.bitmapfont)|*.bitmapfont|All files (*.*)|*.*";

            if (s.ShowDialog() == System.Windows.Forms.DialogResult.OK && s.FileName != null && s.FileName.Length > 0)
            {
                BitmapParameter.WorkingFolder = System.IO.Path.GetDirectoryName(s.FileName);
                List<char> chars = GraphManager.Load(s.FileName);
                GraphManager.UpdateGraph();
                RefreshPreview();

                textBoxCharacters.Clear();
                foreach (char c in chars)
                {
                    textBoxCharacters.Text += c;
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs(false);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadFont();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs(true);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddChar(char c)
        {
            if (c >= ' ')
            {
                if (chars.ContainsKey(c))
                {
                    chars[c]++;
                }
                else
                {
                    chars[c] = 0;
                }
            }
        }

        private void RefreshChars()
        {
            List<char> chr = new List<char>();
            foreach (KeyValuePair<char, int> kvp in chars)
            {
                chr.Add(kvp.Key);
            }
            chr.Sort();
            string s = "";
            foreach (char c in chr)
            {
                s += c;
            }
            textBoxCharacters.Text = s;
            currentFont = null;
        }

        private void buttonClearCharacters_Click(object sender, EventArgs e)
        {
            textBoxCharacters.Text = "";
            chars.Clear();
            currentFont = null;
        }

        private void buttonCharsFromTextFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Title = "Choose a text file to scan";
            f.DefaultExt = ".txt";
            f.Filter = "Text files|*.txt";
            f.CheckFileExists = true;
            if (f.ShowDialog() == DialogResult.OK)
            {
                StreamReader s = new StreamReader(f.FileName);

                if (s.BaseStream.Length > 0)
                {
                    string chars = s.ReadToEnd();
                    foreach (char c in chars)
                    {
                        AddChar(c);
                    }
                }
            }
            RefreshChars();
        }

        private void buttonASCII_Click(object sender, EventArgs e)
        {
            for (char c = ' '; c < (int)128; ++c)
            {
                AddChar(c);
            }
            RefreshChars();
        }

        private void buttonASCII255_Click(object sender, EventArgs e)
        {
            for (char c = ' '; c < (int)255; ++c)
            {
                AddChar(c);
            }
            RefreshChars();
        }

        private void buttonCreateFont_Click(object sender, EventArgs e)
        {
            CreateBitmapFont(export:true, force:true);
        }
    }
}
