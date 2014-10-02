using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace FontUtil
{
	public unsafe class GDI
	{
		public static List<TTFontFamily> families = null;
		public static TTFontFamily thisFamily = null;

		#region imports

		[DllImport("gdi32.dll", SetLastError = true)]
		public static extern unsafe bool TextOut(IntPtr hdc, int x, int y, string text, int strlen);

		[DllImport("gdi32.dll")]
		public static extern bool GetTextExtentPoint(IntPtr hdc, string lpString, int cbString, ref Size lpSize);

		[DllImport("gdi32.dll", SetLastError = true)]
		public static extern unsafe bool GetCharABCWidths(IntPtr hdc, uint uFirstChar, uint uLastChar, IntPtr lpabc);

		[DllImport("gdi32.dll", SetLastError = true)]
		public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

		[DllImport("GDI32.dll", SetLastError = true)]
		public static extern bool DeleteObject(IntPtr objectHandle);

		[DllImport("gdi32.dll", SetLastError = true)]
		public static extern bool GetTextMetrics(IntPtr hdc, out TEXTMETRIC lptm);

		[DllImport("gdi32.dll", SetLastError = true)]
		public static extern uint GetOutlineTextMetrics(IntPtr hdc, uint cbData, IntPtr ptrZero);

		[DllImport("gdi32.dll", SetLastError = true)]
		public static extern uint SetBkColor(IntPtr hdc, int crColor);

		[DllImport("gdi32.dll", SetLastError = true)]
		public static extern uint SetTextColor(IntPtr hdc, uint crColor);

		[DllImport("gdi32.dll", SetLastError = true)]
		public static extern uint SetBkMode(IntPtr hdc, int nBkMode);

		[DllImport("gdi32.dll", SetLastError = true)]
		static extern uint GetKerningPairs(IntPtr hdc, uint nNumPairs, IntPtr lpkrnpair);

		public delegate int EnumFontExDelegate(ref GDI.ENUMLOGFONTEX lpelfe, ref GDI.NEWTEXTMETRICEX lpntme, int FontType, IntPtr lParam);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto)]
		public static extern int EnumFontFamiliesEx(IntPtr hdc,
						  [In] IntPtr pLogfont,
						  EnumFontExDelegate lpEnumFontFamExProc,
						  IntPtr lParam,
						  uint dwFlags);

		[DllImport("gdi32", SetLastError = true)]
		public static extern IntPtr CreateFont(
					[In] Int32 nHeight,
					[In] Int32 nWidth,
					[In] Int32 nEscapement,
					[In] Int32 nOrientation,
					[In] Int32 fnWeight,
					[In] UInt32 fdwItalic,
					[In] UInt32 fdwUnderline,
					[In] UInt32 fdwStrikeOut,
					[In] UInt32 fdwCharSet,
					[In] UInt32 fdwOutputPrecision,
					[In] UInt32 fdwClipPrecision,
					[In] UInt32 fdwQuality,
					[In] UInt32 fdwPitchAndFamily,
					[In] string lpszFace);

		[DllImport("gdi32.dll")]
		public static extern uint GetGlyphOutline(IntPtr hdc, uint uChar, uint uFormat, out GLYPHMETRICS lpgm, uint cbBuffer, IntPtr lpvBuffer, ref MAT2 lpmat2);

		#endregion

		#region foreign structs

		[StructLayout(LayoutKind.Sequential)]
		public struct TTPOLYGONHEADER
		{
			public int cb;
			public int dwType;
			[MarshalAs(UnmanagedType.Struct)]
			public POINTFX pfxStart;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct TTPOLYCURVEHEADER
		{
			public short wType;
			public short cpfx;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct FIXED
		{
			public short fract;
			public short value;

			public static implicit operator float(FIXED f)
			{
				return f.fract / 65536.0f + f.value;
			}

			public static explicit operator FIXED(float f)
			{
				FIXED n = new FIXED();
				n.value = (short)f;
				n.fract = (short)((f - n.value) * 65536.0f);
				return n;
			}

		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MAT2
		{
			[MarshalAs(UnmanagedType.Struct)]
			public FIXED eM11;
			[MarshalAs(UnmanagedType.Struct)]
			public FIXED eM12;
			[MarshalAs(UnmanagedType.Struct)]
			public FIXED eM21;
			[MarshalAs(UnmanagedType.Struct)]
			public FIXED eM22;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int x;
			public int y;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct POINTFX
		{
			[MarshalAs(UnmanagedType.Struct)]
			public FIXED x;
			[MarshalAs(UnmanagedType.Struct)]
			public FIXED y;

			public static implicit operator PointF(POINTFX p)
			{
				return new PointF(p.x, -p.y);
			}

			public static explicit operator POINTFX(PointF p)
			{
				POINTFX pfx = new POINTFX();
				pfx.x = (FIXED)p.X;
				pfx.y = (FIXED)p.Y;
				return pfx;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct GLYPHMETRICS
		{
			public int gmBlackBoxX;
			public int gmBlackBoxY;
			[MarshalAs(UnmanagedType.Struct)]
			public POINT gmptGlyphOrigin;
			public short gmCellIncX;
			public short gmCellIncY;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct NEWTEXTMETRIC
		{
			public int tmHeight;
			public int tmAscent;
			public int tmDescent;
			public int tmInternalLeading;
			public int tmExternalLeading;
			public int tmAveCharWidth;
			public int tmMaxCharWidth;
			public int tmWeight;
			public int tmOverhang;
			public int tmDigitizedAspectX;
			public int tmDigitizedAspectY;
			public char tmFirstChar;
			public char tmLastChar;
			public char tmDefaultChar;
			public char tmBreakChar;
			public byte tmItalic;
			public byte tmUnderlined;
			public byte tmStruckOut;
			public byte tmPitchAndFamily;
			public byte tmCharSet;
			public int ntmFlags;
			public int ntmSizeEM;
			public int ntmCellHeight;
			public int ntmAvgWidth;
		}

#pragma warning disable 169

		public struct FONTSIGNATURE
		{
			[MarshalAs(UnmanagedType.ByValArray)]
			public int[] fsUsb;

			[MarshalAs(UnmanagedType.ByValArray)]
			public int[] fsCsb;
		}
		
		public struct NEWTEXTMETRICEX
		{
			public NEWTEXTMETRIC ntmTm;
			public FONTSIGNATURE ntmFontSig;
		}

#pragma warning restore 169

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct ENUMLOGFONTEX
		{
			public LOGFONT elfLogFont;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
			public string elfFullName;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string elfStyle;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string elfScript;
		}
	  
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class LOGFONT
		{
			public int lfHeight;
			public int lfWidth;
			public int lfEscapement;
			public int lfOrientation;
			public int lfWeight;
			public byte lfItalic;
			public byte lfUnderline;
			public byte lfStrikeOut;
			public byte lfCharSet;
			public byte lfOutPrecision;
			public byte lfClipPrecision;
			public byte lfQuality;
			public byte lfPitchAndFamily;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string lfFaceName;
		}
		
		[StructLayout(LayoutKind.Sequential)]
		public struct OUTLINETEXTMETRIC
		{
			public uint otmSize;
			public TEXTMETRIC otmTextMetrics;
			public byte otmFiller;
			public PANOSE otmPanoseNumber;
			public uint otmfsSelection;
			public uint otmfsType;
			public int otmsCharSlopeRise;
			public int otmsCharSlopeRun;
			public int otmItalicAngle;
			public uint otmEMSquare;
			public int otmAscent;
			public int otmDescent;
			public uint otmLineGap;
			public uint otmsCapEmHeight;
			public uint otmsXHeight;
			public RECT otmrcFontBox;
			public int otmMacAscent;
			public int otmMacDescent;
			public uint otmMacLineGap;
			public uint otmusMinimumPPEM;
			public POINT otmptSubscriptSize;
			public POINT otmptSubscriptOffset;
			public POINT otmptSuperscriptSize;
			public POINT otmptSuperscriptOffset;
			public uint otmsStrikeoutSize;
			public int otmsStrikeoutPosition;
			public int otmsUnderscoreSize;
			public int otmsUnderscorePosition;
			public IntPtr otmpFamilyName;
			public IntPtr otmpFaceName;
			public IntPtr otmpStyleName;
			public IntPtr otmpFullName;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct ABC
		{
			public int abcA;
			public uint abcB;
			public int abcC;

			public int Width
			{
				get
				{
					return (int)(abcA + abcB + abcC);
				}
			}

			public override string ToString()
			{
				return string.Format("A={0}, B={1}, C={2}", abcA, abcB, abcC);
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct TEXTMETRIC
		{
			public int tmHeight;
			public int tmAscent;
			public int tmDescent;
			public int tmInternalLeading;
			public int tmExternalLeading;
			public int tmAveCharWidth;
			public int tmMaxCharWidth;
			public int tmWeight;
			public int tmOverhang;
			public int tmDigitizedAspectX;
			public int tmDigitizedAspectY;
			public char tmFirstChar;
			public char tmLastChar;
			public char tmDefaultChar;
			public char tmBreakChar;
			public byte tmItalic;
			public byte tmUnderlined;
			public byte tmStruckOut;
			public byte tmPitchAndFamily;
			public byte tmCharSet;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct PANOSE
		{
			public byte bFamilyType;
			public byte bSerifStyle;
			public byte bWeight;
			public byte bProportion;
			public byte bContrast;
			public byte bStrokeVariation;
			public byte bArmStyle;
			public byte bLetterform;
			public byte bMidline;
			public byte bXHeight;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			private int _Left;
			private int _Top;
			private int _Right;
			private int _Bottom;
		}

		public enum StockObjects
		{
			WHITE_BRUSH = 0,
			LTGRAY_BRUSH = 1,
			GRAY_BRUSH = 2,
			DKGRAY_BRUSH = 3,
			BLACK_BRUSH = 4,
			NULL_BRUSH = 5,
			HOLLOW_BRUSH = NULL_BRUSH,
			WHITE_PEN = 6,
			BLACK_PEN = 7,
			NULL_PEN = 8,
			OEM_FIXED_FONT = 10,
			ANSI_FIXED_FONT = 11,
			ANSI_VAR_FONT = 12,
			SYSTEM_FONT = 13,
			DEVICE_DEFAULT_FONT = 14,
			DEFAULT_PALETTE = 15,
			SYSTEM_FIXED_FONT = 16,
			DEFAULT_GUI_FONT = 17,
			DC_BRUSH = 18,
			DC_PEN = 19,
		}

		public enum FontQuality : int
		{
			DEFAULT_QUALITY = 0,
			DRAFT_QUALITY = 1,
			PROOF_QUALITY = 2,
			NONANTIALIASED_QUALITY = 3,
			ANTIALIASED_QUALITY = 4,
			CLEARTYPE_QUALITY = 5,
			CLEARTYPE_NATURAL_QUALITY = 6
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct KERNINGPAIR
		{
			public ushort wFirst;
			public ushort wSecond;
			public int iKernAmount;

			public override string ToString()
			{
				return string.Format("{0}{1} : {2}", (char)wFirst, (char)wSecond, iKernAmount);
			}
		}

		public const int GGO_METRICS = 0;
		public const int GGO_BITMAP = 1;
		public const int GGO_NATIVE = 2;
		public const int GGO_BEZIER = 3;
		public const int GGO_GRAY2_BITMAP = 4;
		public const int GGO_GRAY4_BITMAP = 5;
		public const int GGO_GRAY8_BITMAP = 6;
		public const int GGO_GLYPH_INDEX = 0x0080;
		public const int GGO_UNHINTED = 0x0100;

		public const int TT_POLYGON_TYPE = 24;

		public const int TT_PRIM_LINE = 1;
		public const int TT_PRIM_QSPLINE = 2;
		public const int TT_PRIM_CSPLINE = 3;

		#endregion	// foreign structs

		#region font support functions

		/// <summary>
		/// Get some vertical constants about the selected font
		/// </summary>
		/// <param name="hdc"></param>
		/// <param name="ascent"></param>
		/// <param name="descent"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static bool GetFontMetrics(IntPtr hdc, out int ascent, out int descent, out int height, out int externalLeading)
		{
			ascent = 0;
			descent = 0;
			height = 0;
			externalLeading = 0;

			uint cbBuffer = GetOutlineTextMetrics(hdc, 0, IntPtr.Zero);

			if (cbBuffer != 0)
			{
				IntPtr buffer = Marshal.AllocHGlobal((int)cbBuffer);
				try
				{
					if (GetOutlineTextMetrics(hdc, cbBuffer, buffer) != 0)
					{
						OUTLINETEXTMETRIC otm = new OUTLINETEXTMETRIC();
						otm = (OUTLINETEXTMETRIC)Marshal.PtrToStructure(buffer, typeof(OUTLINETEXTMETRIC));
						ascent = otm.otmAscent;
						descent = otm.otmDescent;
						height = (int)otm.otmTextMetrics.tmHeight;
						externalLeading = otm.otmTextMetrics.tmExternalLeading;
						return true;
					}
				}
				finally
				{
					Marshal.FreeHGlobal(buffer);
				}
			}
			else
			{
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			return false;
		}

		[DllImport("gdi32.dll")]
		static extern bool GetCharWidth32(IntPtr hdc, uint iFirstChar, uint iLastChar, [Out] int [] lpBuffer);


		public static int GetCharWidth(IntPtr hDC, char c)
		{
			int[] ints = new int[1];

			try
			{
				bool result = GetCharWidth32(hDC, (uint)c, (uint)c, ints);

				if (!result)
				{
					throw new Exception("Can't get Width");
				}
			}
			finally
			{
			}
			return ints[0];
		}

		/// <summary>
		/// Get some horizontal info about a character in the selected font
		/// </summary>
		/// <param name="hDC"></param>
		/// <param name="c"></param>
		/// <returns></returns>
		public static ABC GetCharABCWidth(IntPtr hDC, char c)
		{
			ABC[] abc_array = new ABC[1];
			GCHandle pin_abc_arr = GCHandle.Alloc(abc_array, GCHandleType.Pinned);
			IntPtr abcptr = Marshal.UnsafeAddrOfPinnedArrayElement(abc_array, 0);

			try
			{
				bool result = GetCharABCWidths(hDC, (uint)c, (uint)c, abcptr);

				if (!result)
				{
					throw new Exception("Can't get ABCWidths");
				}
			}
			finally
			{
				pin_abc_arr.Free();
			}

			return abc_array[0];
		}

		/// <summary>
		/// Get all the Kerning Pairs for the selected font
		/// </summary>
		/// <param name="hdc"></param>
		/// <returns></returns>
		public static KERNINGPAIR[] GetKerningTable(IntPtr hdc)
		{
			uint numPairs = GetKerningPairs(hdc, 0, IntPtr.Zero);
			if (numPairs != 0)
			{
				KERNINGPAIR[] kerningTable = new KERNINGPAIR[numPairs];
				GCHandle pinned = GCHandle.Alloc(kerningTable, GCHandleType.Pinned);
				IntPtr ktp = Marshal.UnsafeAddrOfPinnedArrayElement(kerningTable, 0);
				if (GetKerningPairs(hdc, numPairs, ktp) != 0)
				{
					pinned.Free();
					return kerningTable;
				}
				else
				{
					throw (new Exception("Error getting kerning pairs!"));
				}
			}
			return null;
		}

		#endregion

		public static GDI.LOGFONT CreateLogFont()
		{
			GDI.LOGFONT lf = new GDI.LOGFONT();
			lf.lfHeight = 0;
			lf.lfWidth = 0;
			lf.lfEscapement = 0;
			lf.lfOrientation = 0;
			lf.lfWeight = 0;
			lf.lfItalic = 0;
			lf.lfUnderline = 0;
			lf.lfStrikeOut = 0;
			lf.lfCharSet = 0; // FontCharSet.DEFAULT_CHARSET;
			lf.lfOutPrecision = 0;
			lf.lfClipPrecision = 0;
			lf.lfQuality = 0;
			lf.lfPitchAndFamily = 0; // FontPitchAndFamily.FF_DONTCARE;
			lf.lfFaceName = "";
			return lf;
		}

		[Flags]
		enum NTMFlags
		{
			NTM_ITALIC = 1 << 0,
			NTM_BOLD = 1 << 5,
			NTM_REGULAR = 1 << 8,
			NTM_NONNEGATIVE_AC = 1 << 16,
			NTM_PS_OPENTYPE = 1 << 17,
			NTM_TT_OPENTYPE = 1 << 18,
			NTM_MULTIPLEMASTER = 1 << 19,
			NTM_TYPE1 = 1 << 20,
			NTM_DSIG = 1 << 21
		}

		static string NTMFlagsDescription(int f)
		{
			StringBuilder s = new StringBuilder();

			if ((f & (int)NTMFlags.NTM_ITALIC) != 0) s.Append("NTM_ITALIC ");
			if ((f & (int)NTMFlags.NTM_BOLD) != 0) s.Append("NTM_BOLD ");
			if ((f & (int)NTMFlags.NTM_REGULAR) != 0) s.Append("NTM_REGULAR ");
			if ((f & (int)NTMFlags.NTM_NONNEGATIVE_AC) != 0) s.Append("NTM_NONNEGATIVE_AC ");
			if ((f & (int)NTMFlags.NTM_PS_OPENTYPE) != 0) s.Append("NTM_PS_OPENTYPE ");
			if ((f & (int)NTMFlags.NTM_TT_OPENTYPE) != 0) s.Append("NTM_TT_OPENTYPE ");
			if ((f & (int)NTMFlags.NTM_MULTIPLEMASTER) != 0) s.Append("NTM_MULTIPLEMASTER ");
			if ((f & (int)NTMFlags.NTM_TYPE1) != 0) s.Append("NTM_TYPE1 ");
			if ((f & (int)NTMFlags.NTM_DSIG) != 0) s.Append("NTM_DSIG ");

			return s.ToString();
		}

		private static List<TTFontFamily> filter(bool fixedSize)
		{
			return (from f in families where f.sizesAreRestricted == fixedSize select f).ToList();
		}

		public static List<TTFontFamily> allFixedSizeFonts
		{
			get
			{
				return filter(true);
			}
		}

		public static List<TTFontFamily> allVariableSizeFonts
		{
			get
			{
				return filter(false);
			}
		}

		public static int FontFacesCallback(ref ENUMLOGFONTEX lpelfe, ref NEWTEXTMETRICEX lpntme, int FontType, IntPtr lParam)
		{
			if (thisFamily.faces == null)
			{
				thisFamily.faces = new List<TTFontFace>();
			}

			if (lpelfe.elfStyle == "")
			{
				thisFamily.sizesAreRestricted = true;
				if (thisFamily.sizes == null)
				{
					thisFamily.sizes = new List<int>();
				}
				if (thisFamily.faces.Count == 0)
				{
					thisFamily.faces.Add(new TTFontFace("Regular", thisFamily, lpelfe.elfLogFont, lpntme.ntmTm));
				}
				thisFamily.sizes.Add(lpelfe.elfLogFont.lfHeight);
			}
			else
			{
				thisFamily.faces.Add(new TTFontFace(lpelfe.elfStyle, thisFamily, lpelfe.elfLogFont, lpntme.ntmTm));
			}
			return 1;
		}

		public static int FontFamiliesCallback(ref ENUMLOGFONTEX lpelfe, ref NEWTEXTMETRICEX lpntme, int FontType, IntPtr lParam)
		{
			IntPtr plogFont = Marshal.AllocHGlobal(Marshal.SizeOf(lpelfe.elfLogFont));
			Marshal.StructureToPtr(lpelfe.elfLogFont, plogFont, true);

			try
			{
				if (lpelfe.elfFullName[0] != '@')		// not dealing with vertical fonts yet...
				{
					thisFamily = new TTFontFamily();
					thisFamily.name = lpelfe.elfFullName;
					thisFamily.maxWidth = lpntme.ntmTm.tmMaxCharWidth;
					thisFamily.maxHeight = lpntme.ntmTm.tmHeight;
					thisFamily.font = TTFontFamily.CreateFont(thisFamily.name);
					families.Add(thisFamily);
					EnumFontFamiliesEx(lParam, plogFont, FontFacesCallback, IntPtr.Zero, 0);
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.ToString());
			}
			finally
			{
				Marshal.DestroyStructure(plogFont, typeof(LOGFONT));
			}
			return 1;
		}

		public static void InitializeFonts()
		{
			if (families == null)
			{
				families = new List<TTFontFamily>();

				LOGFONT lf = CreateLogFont();

				IntPtr plogFont = Marshal.AllocHGlobal(Marshal.SizeOf(lf));
				Marshal.StructureToPtr(lf, plogFont, true);

				try
				{
					Bitmap b = new Bitmap(32, 32);
					using (Graphics G = Graphics.FromImage(b))
					{
						IntPtr P = G.GetHdc();
						{
							int ret = EnumFontFamiliesEx(P, plogFont, new EnumFontExDelegate(FontFamiliesCallback), P, 0);
							families = families.OrderBy(x => x.name).ToList();
						}
						G.ReleaseHdc(P);
					}
				}
				catch (Exception e)
				{
					Debug.WriteLine(e.ToString());
				}
				finally
				{
					Marshal.DestroyStructure(plogFont, typeof(LOGFONT));
				}
			}
		}
	}
}
