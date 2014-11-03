//////////////////////////////////////////////////////////////////////
///TODO: Migrate to Plugin system so other renderers can be added

using System;
using System.Drawing;

//////////////////////////////////////////////////////////////////////

namespace FontUtil
{
    //////////////////////////////////////////////////////////////////////

    public class IRenderer
    {
        private TTFontFace font;

        void SetFont(TTFontFace font)
        {
            this.font = font;
        }

        protected Rectangle ScanBitmap(Bitmap b)
        {
            // scan for 
            return new Rectangle();
        }

        public virtual Bitmap Create(char c)
        {
            throw new NotImplementedException();
        }
    }

    //////////////////////////////////////////////////////////////////////
    // Bitmapped renderer

    public class BitmappedFontRenderer : IRenderer
    {
        public override Bitmap Create(char c)
        {
            return null;
        }
    }

    //////////////////////////////////////////////////////////////////////
    // GDI

    public class GDIRenderer : IRenderer
    {
        public override Bitmap Create(char c)
        {
            return null;
        }
    }

    //////////////////////////////////////////////////////////////////////
    // WPF Normal (no outline/geometry mode)

    public class WPFRenderer : IRenderer
    {
        public override Bitmap Create(char c)
        {
            return null;
        }
    }

    //////////////////////////////////////////////////////////////////////
    // WPF Geometry (can do outlining etc)

    public class WPFGeometryRenderer : IRenderer
    {
        public override Bitmap Create(char c)
        {
            return null;
        }
    }
}
