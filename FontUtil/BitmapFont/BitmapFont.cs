//////////////////////////////////////////////////////////////////////
// Bitmapped
// GDI
// WPF/Geometry
// WPF/Normal

//////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Drawing2D;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Windows.Markup;
using System.Globalization;
using System.Xml;

// mixing and matching Windows Forms and PresentationCore is a pain...

using Color = System.Drawing.Color;
using Matrix = System.Drawing.Drawing2D.Matrix;
using Pen = System.Drawing.Pen;
using Brushes = System.Drawing.Brushes;

using WPFPen = System.Windows.Media.Pen;
using WPFBrushes = System.Windows.Media.Brushes;
using WPFPixelFormats = System.Windows.Media.PixelFormats;

using WinRect = System.Windows.Rect;
using System.Text;
using System.Drawing.Imaging;

//////////////////////////////////////////////////////////////////////

namespace FontUtil
{
    //////////////////////////////////////////////////////////////////////

    public enum RenderType : int
    {
        GDIFixed = 0,
        GDITrueType = 1,
        WPF = 2
    }

    //////////////////////////////////////////////////////////////////////

    public enum PackingType
    {
        Cygon = 0,
        Arevalo = 1
    }

    //////////////////////////////////////////////////////////////////////

    public class FontLayer
    {
        public Color color;
        public PointF offset;
        public bool measureThisLayer;

        public FontLayer(Color color, PointF offset, bool measure)
        {
            this.color = color;
            this.offset = offset;
            this.measureThisLayer = measure;
        }
    }

    //////////////////////////////////////////////////////////////////////

    public unsafe class BitmapFont
    {
        public int pageWidth;
        public int pageHeight;
        public List<Page> pages;
        public SortedDictionary<char, Glyph> glyph;
        public bool useKerning;
        public int numLayers;

        public List<Graphic> globalImageList;
        public List<FontLayer> layers;

        private float baseLine;
        public int height;
        public int internalLeading;	// height at the top of each cell where accents go - useful for aligning chars vertically sometimes

        //private IntPtr font;
        //private GDI.KERNINGPAIR[] kerningPairs;

        public TTFontFace FontFace;
        public TTFontFamily Font;

        IntPtr HFont;

        Bitmap bitmap;

        private bool antiAlias;

        public static int glyphSize;	// total glyph pixels
        public static int pageSize;	// total page pixels

        public static PackingType FontPackingType = PackingType.Cygon;

        public delegate void ProgressCallbackDelegate(int percent);

        public ProgressCallbackDelegate ProgressCallback;

        //////////////////////////////////////////////////////////////////////

        private void GetBaseline()
        {
            baseLine = FontFace.Baseline(height);
        }

        //////////////////////////////////////////////////////////////////////

        public string Name
        {
            get
            {
                return Font.name;
            }
        }

        //////////////////////////////////////////////////////////////////////

        public BitmapFont(TTFontFace fontFace, int height, int pageWidth, int pageHeight, bool antiAlias)
        {
            this.internalLeading = fontFace.textMetric.tmInternalLeading;
            this.height = height;
            this.Font = fontFace.fontFamily;
            this.FontFace = fontFace;
            this.pageWidth = pageWidth;
            this.pageHeight = pageHeight;
            this.antiAlias = antiAlias;

            GetBaseline();

            layers = new List<FontLayer>();

            globalImageList = new List<Graphic>();
            glyph = new SortedDictionary<char, Glyph>();
            useKerning = true;
            pages = new List<Page>();
        }

        //////////////////////////////////////////////////////////////////////

        public void CreateAllLayers(char[] chars, bool singlePixelBorder, ProgressCallbackDelegate progress = null)
        {
            layers.Clear();

            foreach (LayerNode l in LayerNode.allLayers)
            {
                layers.Add(new FontLayer(l.RenderColor, l.RenderOffset, l.Measure));
            }

            for(int i=0; i<chars.Length; ++i)
            {
                char c = chars[i];

                FontNode.createChar = c;

                if (!glyph.ContainsKey(c))
                {
                    Glyph g = new Glyph(this, chars[i]);

                    foreach(FontNode fontNode in FontNode.allFontNodes)
                    {
                        fontNode.SetDirty();
                        fontNode.GetGraphic();
                    }

                    foreach (LayerNode l in LayerNode.allLayers)
                    {
                        Graphic result = l.GetGraphic();
                        if (result != null)
                        {
                            g.advance = result.glyph.advance;
                            g.character = result.glyph.character;
                            if (result.bmp != null)
                            {
                                result.glyph = g;	// erk, nab it
                                g.originalBitmap = result.glyph.originalBitmap;
                                g.imageTable.Add(result);
                                globalImageList.Add(result);
                            }
                        }
                    }
                    glyph.Add(chars[i], g);
                }
                if (progress != null)
                {
                    progress(i * 100 / chars.Length);
                }
            }
            
            Bake(singlePixelBorder);
        }

        //////////////////////////////////////////////////////////////////////

        void CreateHFont()
        {
            GDI.LOGFONT logFont = FontFace.logFont;

            if (antiAlias)
            {
                logFont.lfQuality = (byte)GDI.FontQuality.ANTIALIASED_QUALITY;
            }
            else
            {
                logFont.lfQuality = (byte)GDI.FontQuality.NONANTIALIASED_QUALITY;
            }

            //logFont.lfOutPrecision = 4;// OUT_TT_PRECIS;

            bitmap = new Bitmap(Font.maxWidth, Font.maxHeight);

            //Debug.WriteLine("{0},{1}", Font.maxWidth, Font.maxHeight);

            GDI.TEXTMETRIC tm = new GDI.TEXTMETRIC();

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                IntPtr hdc = g.GetHdc();
                if (hdc != IntPtr.Zero)
                {
                    if (HFont != IntPtr.Zero)
                    {
                        GDI.DeleteObject(HFont);
                    }

                    logFont.lfHeight = height;
                    
                    HFont = GDI.CreateFont(logFont.lfHeight,
                                        0,//logFont.lfWidth,
                                        logFont.lfEscapement,
                                        logFont.lfOrientation,
                                        logFont.lfWeight,
                                        logFont.lfItalic,
                                        logFont.lfUnderline,
                                        logFont.lfStrikeOut,
                                        logFont.lfCharSet,
                                        logFont.lfOutPrecision,
                                        logFont.lfClipPrecision,
                                        logFont.lfQuality,
                                        logFont.lfPitchAndFamily,
                                        logFont.lfFaceName);

                    IntPtr oldFont = GDI.SelectObject(hdc, HFont);
                    GDI.GetTextMetrics(hdc, out tm);
                    GDI.SelectObject(hdc, oldFont);
                }
            }
            int maxWidth = tm.tmMaxCharWidth;
            if (bitmap.Width < maxWidth || bitmap.Height < height)
            {
                bitmap = new Bitmap(maxWidth, height);
            }
        }

        //////////////////////////////////////////////////////////////////////

        public void Create(char[] charSet, int outline, bool roundedOutline)
        {
            GetBaseline();

            // Create GDI HFONT if necessary here...
            if (FontFace.logFont != null)
            {
                CreateHFont();
            }

            foreach (char ch in charSet)
            {
                if (!glyph.ContainsKey(ch))
                {
                    if(CreateChar(ch, outline, roundedOutline))
                    {
                        Glyph g = glyph[ch];
                        if (g != null)
                        {
                            g.Process(this);
                            foreach (Graphic gr in g.imageTable)
                            {
                                gr.Bake();
                                globalImageList.Add(gr);
                            }
                        }
                    }
                }
            }
        }

        //////////////////////////////////////////////////////////////////////

        public void Bake(bool singlePixelBorder)
        {
            IEnumerable<Graphic> e = from i in globalImageList orderby i ascending select i;

            glyphSize = 0;

            pages = new List<Page>();

            foreach (Graphic i in e)
            {
                i.Bake();
            }

            // place all the images (there might be more than one per glyph now...)
            foreach (Graphic i in e)
            {
                i.Place(this, singlePixelBorder);
            }

            foreach (Graphic i in e)
            {
                i.bmp = null;
            }

            // fill in kerning entries
            //if (kerningPairs != null)
            //{
            //    foreach (GDI.KERNINGPAIR k in kerningPairs)
            //    {
            //        char second = (char)k.wSecond;
            //        if (glyph.ContainsKey(second) && k.iKernAmount != 0)
            //        {
            //            glyph[second].AddKerningEntry((char)k.wFirst, k.iKernAmount);
            //        }
            //    }
            //}

            foreach (Page p in this.pages)
            {
                p.ShrinkToFit();
                //p.bitmap.Save("C:\\users\\loft\\desktop\\page" + p.index + ".png");
            }
        }

        //////////////////////////////////////////////////////////////////////

        public bool CreateChar(char c, int penWidth, bool rounding)
        {
            if (FontFace.glyphTypeFace != null)
            {
                return WPFCreateChar(c, penWidth, rounding);
            }
            else
            {
                return GDICreateChar(c, penWidth, rounding);
            }
        }

        //////////////////////////////////////////////////////////////////////

        private static Rectangle ScanForContent(Bitmap bitmap, Color bgcol)
        {
            int w = bitmap.Width;
            int h = bitmap.Height;
            int xstart = w + 1;
            int ystart = h + 1;
            int xend = -1;
            int yend = -1;
            int bgColARGB = bgcol.ToArgb();
            for (int y = 0; y < h; ++y)
            {
                for(int x=0; x<w; ++x)
                {
                    int p = bitmap.GetPixel(x, y).ToArgb();
                    if(p != bgColARGB)
                    {
                        xstart = Math.Min(xstart, x);
                        ystart = Math.Min(ystart, y);
                        xend = Math.Max(xend, x);
                        yend = Math.Max(yend, y);
                    }
                }
            }
            Rectangle r = Rectangle.Empty;
            if (xend >= 0)
            {
                int bw = Math.Max(0, xend - xstart);
                int bh = Math.Max(0, yend - ystart);
                r = new Rectangle(xstart, ystart, bw + 1, bh + 1);
            }
            return r;
        }

        //////////////////////////////////////////////////////////////////////

        public bool GDICreateChar(char c, int penWidth, bool rounding)
        {
            if (glyph.ContainsKey(c))
            {
                return true;
            }

            float advance;

            int AOffset = 0;

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.FillRectangle(new SolidBrush(Color.Magenta), bitmap.Dimensions());

                int b;
                int r;

                int xorg;	// where the char was drawn

                Size textSize = new Size();

                IntPtr hdc = g.GetHdc();
                {
                    IntPtr oldFont = GDI.SelectObject(hdc, HFont);
                    {
                        if (Font.sizesAreRestricted)
                        {
                            advance = GDI.GetCharWidth(hdc, c);
                            xorg = 0;
                        }
                        else
                        {
                            GDI.ABC a = GDI.GetCharABCWidth(hdc, c);	// this is only for TrueType fonts.... 
                            advance = (a.abcA + a.abcB + a.abcC);
                            xorg = -a.abcA;
                            AOffset = a.abcA;
                        }

                        b = height;
                        r = (int)advance;
                        string str = new string(c, 1);
                        GDI.GetTextExtentPoint(hdc, str, 1, ref textSize);
                        GDI.SetTextColor(hdc, 0xffffff);
                        GDI.SetBkColor(hdc, 0);
                        GDI.TextOut(hdc, xorg, 0, str, 1);
                        GDI.SelectObject(hdc, oldFont);
                    }
                    g.ReleaseHdc();
                }
            }

            Color magenta = Color.FromArgb(0xff, 0xff, 0x00, 0xff);
            Rectangle bounds = ScanForContent(bitmap, magenta);

            glyph[c] = new Glyph(this, c);
            glyph[c].advance = advance;

            if(!bounds.IsEmpty)
            {
                glyph[c].originalBitmap = new Bitmap(bounds.Width, bounds.Height);
                glyph[c].originalBitmap.CopyRect(bitmap, bounds.Left, bounds.Top, 0, 0, bounds.Width, bounds.Height);
                glyph[c].AddImage(this, c, glyph[c].originalBitmap, new Point(bounds.Left + AOffset, bounds.Top));
            }

            return true;
        }

        //////////////////////////////////////////////////////////////////////

        public bool WPFCreateChar(char c, int penWidth, bool rounding)
        {
            if (glyph.ContainsKey(c))
            {
                return true;
            }

            Debug.WriteLine("Creating " + c);

            Glyph g = new Glyph(this, c);

            GlyphTypeface glyphTypeface = FontFace.glyphTypeFace;

            // Bake the bmp

            try
            {
                ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[c];
                Geometry geom = glyphTypeface.GetGlyphOutline(glyphIndex, height, height);

                g.advance = (float)glyphTypeface.AdvanceWidths[glyphIndex] * this.height;

                if (!geom.Bounds.IsEmpty)
                {
                    if (penWidth > 0)
                    {
                        WPFPen pen = new WPFPen(WPFBrushes.White, penWidth);

                        if (rounding)
                        {
                            pen.LineJoin = PenLineJoin.Round;
                        }
                        geom = geom.GetWidenedPathGeometry(pen, 0, ToleranceType.Absolute);
                    }

                    DrawingVisual dv1 = new DrawingVisual();
                    using (DrawingContext dc = dv1.RenderOpen())
                    {
                        dc.DrawGeometry(WPFBrushes.White, null, geom);
                    }
                    DrawingVisual dv2 = new DrawingVisual();
                    WinRect rc = dv1.ContentBounds;
                    using (DrawingContext dc = dv2.RenderOpen())
                    {
                        VisualBrush vb = new VisualBrush(dv1);
                        dc.DrawRectangle(vb, null, new WinRect(0, 0, rc.Width, rc.Height));
                    }
                    rc = dv2.ContentBounds;

                    RenderTargetBitmap renderBmp = new RenderTargetBitmap((int)rc.Width + 1, (int)rc.Height + 1, 96, 96, WPFPixelFormats.Pbgra32);
                    renderBmp.Render(dv2);	// this can take a very very long time... thread it, I guess...
                    renderBmp.Freeze();

                    // OK, it's drawn into a shrunk bitmap

                    // work out where to draw it in the real glyph bmp
                    float left = (float)glyphTypeface.LeftSideBearings[glyphIndex] * height;
                    float top = (float)glyphTypeface.TopSideBearings[glyphIndex] * height;
                    float outline_offset = penWidth / 2.0f;
                    PointF origin = new PointF(left, top);                                  // where the glyph actually is
                    PointF border = new PointF(outline_offset, outline_offset);             // how much border to add because of fat outlining
                    PointF pos = origin.Add(border);                                        // where to draw the graphic in the big scannable bitmap

                    int bmpHeight = renderBmp.PixelHeight;
                    int bmpWidth = renderBmp.PixelWidth;

                    int nStride = (renderBmp.PixelWidth * renderBmp.Format.BitsPerPixel + 7) / 8;

                    UInt32[] pixelByteArray = new UInt32[renderBmp.PixelHeight * nStride / 4];

                    // get the small bitmap
                    renderBmp.CopyPixels(pixelByteArray, nStride, 0);
                    uint offset = 0;
                    Bitmap gdiBmp = new Bitmap(bmpWidth, bmpHeight);
                    using (RawBitmap raw = new RawBitmap(gdiBmp))
                    {
                        for (int y = 0; y < bmpHeight; ++y)
                        {
                            UInt32 *line = (UInt32 *)raw[0, y];

                            for (int x = 0; x < bmpWidth; ++x)
                            {
                                line[x] = pixelByteArray[offset + x];
                            }
                            offset += (uint)bmpWidth;
                        }
                    }

                    // now create a bitmap big enough to DrawImage renderBmp into at the right offset
                    Bitmap gbmp = new Bitmap((int)pos.X + 2 + bmpWidth + 2, (int)pos.Y + 2 + bmpHeight);
                    Graphics gr = Graphics.FromImage(gbmp);
                    gr.Clear(Color.FromArgb(0));
                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    gr.CompositingQuality = CompositingQuality.HighQuality;
                    gr.CompositingMode = CompositingMode.SourceCopy;
                    gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    gr.DrawImage(gdiBmp, origin.Add(border));

                    // ok then, now scan for the edges of the glyph to get the real (pixel based) draw offset
                    Rectangle bounds = ScanForContent(gbmp, Color.FromArgb(0));

                    // now snarf into a new bitmap with the right dimensions

                    Bitmap cropped = new Bitmap(bounds.Width, bounds.Height);
                    using(Graphics cr = Graphics.FromImage(cropped))
                    {
                        cr.SmoothingMode = SmoothingMode.None;
                        cr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        cr.CompositingMode = CompositingMode.SourceCopy;
                        cr.InterpolationMode = InterpolationMode.NearestNeighbor;
                        cr.CompositingQuality = CompositingQuality.HighQuality;
                        using (ImageAttributes ia = new ImageAttributes())
                        {
                            Point[] p = {
                                             new Point(0, 0),
                                             new Point(bounds.Width, 0),
                                             new Point(0, bounds.Height)
                                         };
                            ia.SetWrapMode(WrapMode.TileFlipXY);
                            cr.DrawImage(gbmp, p, bounds, GraphicsUnit.Pixel, ia);
                        }
                    }

                    // calc drawOffset
                    g.penWidth = penWidth;
                    g.originalBitmap = cropped;
                    g.AddImage(this, c, cropped, new PointF(bounds.Left, bounds.Top));
                }
                glyph.Add(c, g);

                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        //////////////////////////////////////////////////////////////////////

        private void PlotChar(char c, char previous, Bitmap dest, ref PointF pos, int index)
        {
            if (glyph.ContainsKey(c))
            {
                Glyph g = glyph[c];
                pos.X += g.Draw(this, dest, pos, previous, index);
            }
        }

        //////////////////////////////////////////////////////////////////////

        public void PlotString(string s, Bitmap dest, ref PointF pos, int index)
        {
            char p = (char)0;
            foreach (char c in s)
            {
                PlotChar(c, p, dest, ref pos, index);
                p = c;
            }
        }

        //////////////////////////////////////////////////////////////////////

        public void PlotString(string s, Bitmap dest, ref PointF pos)
        {
            // draw each layer in turn....
            for (int i = 0; i < numLayers; ++i)
            {
                PointF n = pos;
                PlotString(s, dest, ref n, i);
            }
        }

        //////////////////////////////////////////////////////////////////////

        public float Utilization()
        {
            pageSize = 0;
            foreach (Page p in pages)
            {
                int pw = p.bitmap.Width;
                int ph = p.bitmap.Height;
                pageSize += pw * ph;
            }
            return (float)glyphSize / pageSize;
        }

        //////////////////////////////////////////////////////////////////////

        public void Load(string filename)
        {
        }

        //////////////////////////////////////////////////////////////////////

        public void Save(string filename, bool export)
        {
            using (XmlTextWriter output = new XmlTextWriter(filename, System.Text.Encoding.UTF8))
            {
                output.Formatting = Formatting.Indented;

                output.WriteStartDocument();

                output.WriteDocType("BitmapFont", null, null, null);

                output.WriteStartElement("BitmapFont");
                {
                    output.WriteAttributeString("PageCount", pages.Count.ToString());
                    output.WriteAttributeString("Height", height.ToString());
                    output.WriteAttributeString("InternalLeading", internalLeading.ToString());
                    output.WriteAttributeString("Baseline", baseLine.ToString());

                    GraphManager.Save(output);

                    if (export)
                    {
                        output.WriteStartElement("Layers");
                        {
                            output.WriteAttributeString("Count", layers.Count.ToString());
                            foreach (FontLayer l in layers)
                            {
                                output.WriteStartElement("Layer");
                                {
                                    output.WriteAttributeString("offsetX", l.offset.X.ToString());
                                    output.WriteAttributeString("offsetY", l.offset.Y.ToString());
                                    output.WriteAttributeString("color", l.color.ToArgb().ToString("X8"));
                                }
                                output.WriteEndElement();	// layer
                            }
                        }
                        output.WriteEndElement(); // layers

                        output.WriteStartElement("Glyphs");
                        {
                            output.WriteAttributeString("Count", glyph.Keys.Count.ToString());

                            foreach (KeyValuePair<char, Glyph> kvp in glyph)
                            {
                                Glyph gl = kvp.Value;
                                output.WriteStartElement("Glyph");
                                {
                                    output.WriteAttributeString("char", ((int)(kvp.Key)).ToString());
                                    output.WriteAttributeString("images", gl.imageTable.Count.ToString());
                                    output.WriteAttributeString("advance", gl.advance.ToString());

                                    foreach (Graphic gr in gl.imageTable)
                                    {
                                        output.WriteStartElement("Graphic");
                                        {
                                            output.WriteAttributeString("offsetX", gr.drawOffset.X.ToString());
                                            output.WriteAttributeString("offsetY", gr.drawOffset.Y.ToString());
                                            output.WriteAttributeString("x", gr.glyphPosition.X.ToString());
                                            output.WriteAttributeString("y", gr.glyphPosition.Y.ToString());
                                            output.WriteAttributeString("w", gr.glyphPosition.Width.ToString());
                                            output.WriteAttributeString("h", gr.glyphPosition.Height.ToString());
                                            output.WriteAttributeString("page", gr.pageIndex.ToString());
                                        }
                                        output.WriteEndElement(); // Graphic
                                    }
                                }
                                output.WriteEndElement(); // Glyph
                            }
                        }
                        output.WriteEndElement(); // Glyphs
                    }
                }
                output.WriteEndElement(); // Bitmapfont
            }

            string textureBaseName = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename));

            foreach (Page p in pages)
            {
                string name = textureBaseName + p.index.ToString() + ".png";
                //Debug.WriteLine("Saving {0} ({1},{2})", name, p.bitmap.Width, p.bitmap.Height);
                try
                {
                    p.bitmap.Save(name, System.Drawing.Imaging.ImageFormat.Png);
                }
                catch (ExternalException e)
                {
                    System.Windows.MessageBox.Show("Error saving " + name + "\n" + e.ToString());
                }
            }
        }
    }
}
