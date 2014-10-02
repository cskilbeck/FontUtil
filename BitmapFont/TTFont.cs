using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FontUtil
{
	// a font face and size - the face points to the family

	public class TTFont : IEquatable<TTFont>
	{
		public TTFontFace face;
		public int size;

		public TTFont(TTFontFace face, int size)
		{
			this.face = face;
			this.size = size;
		}

		public bool Equals(TTFont other)
		{
			return face.Equals(other.face) && size == other.size;
		}
	}
}
