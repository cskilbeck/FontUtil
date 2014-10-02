using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Globalization;
using System.Windows.Markup;
using System.Windows.Media;

namespace FontUtil
{
	public abstract class TTFontCollection
	{
		protected List<TTFontFamily> allFonts;

		public abstract List<TTFontFamily> Fonts
		{
			get;
		}
	}

	public class TTFontCollection_GDIBitmapped : TTFontCollection
	{
		public override List<TTFontFamily> Fonts
		{
			get
			{
				if (allFonts == null)
				{
					GDI.InitializeFonts();

					allFonts = GDI.allFixedSizeFonts;
				}
				return allFonts;
			}
		}
	}

	public class TTFontCollection_GDITrueType : TTFontCollection
	{
		public override List<TTFontFamily> Fonts
		{
			get
			{
				if (allFonts == null)
				{
					GDI.InitializeFonts();

					allFonts = GDI.allVariableSizeFonts;
				}
				return allFonts;
			}
		}
	}

	public class TTFontCollection_PresentationCore : TTFontCollection
	{
		public override List<TTFontFamily> Fonts
		{
			get
			{
				if (allFonts == null)
				{
					allFonts = new List<TTFontFamily>();
					string fontFolder = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);

					RegistryKey fontsKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Fonts");

					string currentUICultureName = CultureInfo.CurrentUICulture.IetfLanguageTag;

                    XmlLanguage userLanguage = XmlLanguage.GetLanguage("en-US");
                    //XmlLanguage userLanguage = XmlLanguage.GetLanguage(currentUICultureName);

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
							var families = System.Windows.Media.Fonts.GetFontFamilies(fontFolder + "\\" + filename);

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
									fnt.font = TTFontFamily.CreateFont(fnt.name);
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
					allFonts = (from TTFontFamily f in allFonts orderby f.name select f).ToList();
				}
				return allFonts;
			}
		}
	}
}
