using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace FontUtil
{
	public class TTFontFace : IEquatable<TTFontFace>
	{
		public string name;
		public TTFontFamily fontFamily;
		public GlyphTypeface glyphTypeFace;	// depending on which of these is NULL, render it differently...
		public GDI.LOGFONT logFont;
		public GDI.NEWTEXTMETRIC textMetric;

		public TTFontFace(string name, TTFontFamily fontFamily, GlyphTypeface glyphTypeFace)
		{
			this.name = name;
			this.fontFamily = fontFamily;
			this.glyphTypeFace = glyphTypeFace;
			this.logFont = null;
		}

		public TTFontFace(string name, TTFontFamily fontFamily, GDI.LOGFONT logFont, GDI.NEWTEXTMETRIC textMetric)
		{
			this.name = name;
			this.fontFamily = fontFamily;
			this.logFont = logFont;
			this.textMetric = textMetric;
			this.glyphTypeFace = null;
		}

		public float Baseline(int height)
		{
			if (glyphTypeFace != null)
			{
				return (float)glyphTypeFace.CapsHeight * height;
			}
			else
			{
				return (float)textMetric.tmAscent * height;
			}
		}

		public override string ToString()
		{
			return name;
		}

		public bool Equals(TTFontFace other)
		{
			return name == other.name && fontFamily.Equals(other.fontFamily);
		}
	}
}
