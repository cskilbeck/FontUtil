using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Globalization;
using System.Windows.Markup;
using System.Windows.Media;
using System.Drawing;

namespace FontUtil
{
	public class TTFontFamily : IComparable<string>, IEquatable<TTFontFamily>
	{
		private static List<TTFontFamily> allFonts = null;

		public string name;
		public List<TTFontFace> faces;
		public List<int> sizes;
		public bool sizesAreRestricted;
		public Font font;

		public int maxWidth;				// these used only in the GDI rendering pipe
		public int maxHeight;

		public override string ToString()
		{
			return name;
		}

		public int CompareTo(string other)
		{
			return name.CompareTo(other);
		}

		public bool Equals(TTFontFamily other)
		{
			return name == other.name;
		}

		public static List<TTFontFamily> GetFontList(RenderType renderType)
		{
			switch (renderType)
			{
				case RenderType.GDIFixed: return new TTFontCollection_GDIBitmapped().Fonts;
				case RenderType.GDITrueType: return new TTFontCollection_GDITrueType().Fonts;
				case RenderType.WPF: return new TTFontCollection_PresentationCore().Fonts;
				default: throw new ArgumentException("Bad RenderType");
			}
		}

		public static TTFontFamily FindFont(string familyname, RenderType renderType)
		{
			List<TTFontFamily> families = GetFontList(renderType);

			foreach (TTFontFamily f in families)
			{
				if (f.name == familyname)
				{
					return f;
				}
			}
			return null;
		}

		public TTFontFace FindFace(string name)
		{
			foreach(TTFontFace face in faces)
			{
				if (name.Equals(face.name))
				{
					return face;
				}
			}
			return null;
		}

		public static Font CreateFont(string name)
		{
			const int size = 12;
			Font font = null;
			try
			{
				font = new Font(name, size);
			}
			catch (ArgumentException)
			{
				try
				{
					font = new Font(name, size, FontStyle.Bold);
				}
				catch (ArgumentException)
				{
					try
					{
						font = new Font(name, size, FontStyle.Italic);
					}
					catch (ArgumentException)
					{
						try
						{
							font = new Font(name, size, FontStyle.Underline);
						}
						catch (ArgumentException)
						{
						}
					}
				}
			}
			return font;
		}

		public static List<TTFontFamily> AllFonts
		{
			get
			{
				if (allFonts == null)
				{
					allFonts = new List<TTFontFamily>();
					string fontFolder = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);

					RegistryKey fontsKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Fonts");

					string currentUICultureName = CultureInfo.CurrentUICulture.IetfLanguageTag;

					XmlLanguage userLanguage = XmlLanguage.GetLanguage(currentUICultureName);

					foreach (string s in fontsKey.GetValueNames())
					{
						string filename = fontsKey.GetValue(s) as string;

						int f = s.IndexOf(" (TrueType)");
						string n = s;
						if (f != -1)
						{
							n = n.Remove(f); 
						}

						try
						{
							Debug.WriteLine("Try: " + filename);

							var families = Fonts.GetFontFamilies(fontFolder + "\\" + filename);

							foreach (System.Windows.Media.FontFamily family in families)
							{
								TTFontFamily fnt = null;
								foreach (TTFontFamily srch in allFonts)
								{
									if (srch.name == family.FamilyNames[userLanguage])
									{
										fnt = srch;
										break;
									}
								}
								if (fnt == null)
								{
									fnt = new TTFontFamily();
									fnt.name = family.FamilyNames[userLanguage];
									fnt.faces = new List<TTFontFace>();
									fnt.font = CreateFont(fnt.name);
									allFonts.Add(fnt);
								}

								var typefaces = family.GetTypefaces();
								foreach (Typeface typeface in typefaces)
								{
									GlyphTypeface glph;
									typeface.TryGetGlyphTypeface(out glph);

									bool found = false;
									foreach (TTFontFace fac in fnt.faces)
									{
										if (fac.name == typeface.FaceNames[userLanguage])
										{
											found = true;
											break;
										}
									}
									if (!found)
									{
										fnt.faces.Add(new TTFontFace(typeface.FaceNames[userLanguage], fnt, glph));
										Debug.WriteLine("  Face: " + typeface.FaceNames[userLanguage]);
									}
									else
									{
										// ditch duplicates I guess...
									}
								}
							}
						}
						catch (System.NotSupportedException)
						{
						}
					}

					// What about the crappy bitmapped ones?

					allFonts = (from TTFontFamily f in allFonts orderby f.name select f).ToList();
				}
				return allFonts;
			}
		}
	}
}
