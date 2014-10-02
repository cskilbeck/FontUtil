using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FontUtil
{
	public class FontNode : Node
	{
		public BitmapFont bitmapFont;

		public static char createChar;

		public static List<FontNode> allFontNodes = new List<FontNode>();

		public static TTFont theFont;

		public FontNode(FontDescriptor fontDescriptor = null, TTFont font = null) : base()
		{
			RenderTypeParameter bp = (RenderTypeParameter)Parameter.CreateParameter("Render Type", typeof(RenderType), 0, null);
			TTFontParameter fp = (TTFontParameter)Parameter.CreateParameter("Font", typeof(TTFont), 0, null);
			FontDescriptorParameter fdp = (FontDescriptorParameter)Parameter.CreateParameter("Options", typeof(FontDescriptor), 1, null);

			Init(bp, fp, fdp);

			if (font != null)
			{
				parameters[1].value = font;
			}

			if (fontDescriptor != null)
			{
				parameters[2].value = fontDescriptor;
			}

			CreateFont();
			CreateChar();
			Dirty = false;
		}

		public FontNode(RenderTypeParameter renderTypeParameter, TTFontParameter fontParameter, FontDescriptorParameter fontDescriptorParameter)
			: base()
		{
			Init(renderTypeParameter, fontParameter, fontDescriptorParameter);
		}

		void Init(RenderTypeParameter rtp, TTFontParameter fp, FontDescriptorParameter fdp)
		{
			allFontNodes.Add(this);

			theFont = fp.value as TTFont;

			if (rtp == null)
			{
				rtp = (RenderTypeParameter)Parameter.CreateParameter("Render Type", typeof(RenderType), 0, null);	// back compat hack
			}

			if (fdp == null)
			{
				fdp = (FontDescriptorParameter)Parameter.CreateParameter("Options", typeof(FontDescriptor), 2, null);
			}

			rtp.ParameterChanged += new EventHandler<EventArgs>(bp_ParameterChanged);
			fp.ParameterChanged += new EventHandler<EventArgs>(fp_OnChanged);
			fdp.ParameterChanged += new EventHandler<EventArgs>(fdp_OnChanged);

			parameters = new List<Parameter>();

			parameters.Add(rtp);
			parameters.Add(fp);
			parameters.Add(fdp);
		}

		void bp_ParameterChanged(object sender, EventArgs e)
		{
			// Tell all FontDialogs that it needs to repopulate its combo boxes based on the new RenderType value

			Debug.WriteLine("bp_ParameterChanged!");

			parameters[0].UpdateValue();
			parameters[1].UpdateValue();
			parameters[2].UpdateValue();

			foreach (MyFontDialog f in MyFontDialog.fontDialogs)
			{
				f.Repopulate((RenderType)parameters[0].value);
			}

			FontNode_OnChanged();
		}

		void CreateFont()
		{
			parameters[1].UpdateValue();
			parameters[2].UpdateValue();
			FontDescriptor fd = (FontDescriptor)parameters[2].value;
			theFont = (TTFont)parameters[1].value;
			bitmapFont = new BitmapFont(theFont.face, theFont.size, 1024, 1024, fd.antiAlias);
		}

		void CreateChar()
		{
			parameters[2].UpdateValue();
			FontDescriptor d = (FontDescriptor)parameters[2].value;

			bitmapFont.glyph.Remove(createChar);

			//Debug.WriteLine("Create " + createChar);
			bitmapFont.Create(new char[] { createChar }, d.outline, d.rounded);

			if (bitmapFont.glyph.ContainsKey(createChar))
			{
				if (bitmapFont.glyph[createChar].imageTable.Count > 0 && bitmapFont.glyph[createChar].imageTable[0].bmp != null)
				{
					//Debug.WriteLine("Creating Char: " + createChar);
					cache = bitmapFont.glyph[createChar].imageTable[0];
				}
				else
				{
					cache = new Graphic(bitmapFont.glyph[createChar], null, PointF.Empty);
				}
			}
		}

		public override bool Dirty
		{
			get
			{
				return base.Dirty;
			}
			set
			{
				base.Dirty = value;
			}
		}

		public override Graphic CreateGraphic()
		{
			CreateFont();
			CreateChar();
			return cache;
		}

		void fdp_OnChanged(object sender, EventArgs e)
		{
			FontNode_OnChanged();
		}

		void fp_OnChanged(object sender, EventArgs e)
		{
			FontNode_OnChanged();
		}

		void FontNode_OnChanged()
		{
			theFont = (TTFont)parameters[1].value;

			foreach (MyFontDialog d in MyFontDialog.fontDialogs)
			{
				if (!d.Value.Equals(theFont))
				{
					d.frozen = true;
					d.Value = theFont;
					d.frozen = false;
				}
			}
			foreach (Node n in Node.allNodes)
			{
				n.Dirty = true;
			}
			foreach (LayerNode l in LayerNode.allLayers)
			{
				l.GetGraphic();
			}
			RaiseChanged();
		}

		public override string Description
		{
			get
			{
				return theFont.face.fontFamily.name + " " + theFont.face.name + " " + theFont.size;
			}
		}

		public override string Name
		{
			get
			{
				return "FontNode";
			}
		}

		public override bool AcceptsInput
		{
			get
			{
				return false;
			}
		}

		public override void Destroy()
		{
			foreach (Parameter p in parameters)
			{
				if (p is TTFontParameter)
				{
					MyFontDialog f = (MyFontDialog)p.Control;
					MyFontDialog.fontDialogs.Remove(f);
				}
			}
			base.Destroy();
			allFontNodes.Remove(this);
		}

		public override void Delete()
		{
			base.Delete();
		}

		public override int NumControlsRequired()
		{
			return 2;
		}

		public override void DrawBody(Graphics g)
		{
			SmoothingMode o = g.SmoothingMode;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			Util.DrawRoundRect(g, Brushes.Black, Pens.White, Position.X, Position.Y, Size.Width, Size.Height, Size.Width / 4);
			StringFormat f = new StringFormat();
			f.Alignment = StringAlignment.Near;
			f.LineAlignment = StringAlignment.Center;
			f.FormatFlags = StringFormatFlags.FitBlackBox;
			SizeF textSize = g.MeasureString(Description, CaptionFont, Width - 4, f);
			g.DrawString(Description, CaptionFont, Selected ? Brushes.White : Brushes.Cyan, new RectangleF(Position.X + 4, Position.Y + Height / 2 - textSize.Height / 2, Width - 4, textSize.Height), f);
			g.SmoothingMode = o;
		}

		public static void SetAllDirty()
		{
			foreach (FontNode sourceNode in allFontNodes)
			{
				sourceNode.SetDirty();
			}
		}
	}
}
