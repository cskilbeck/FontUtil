using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Diagnostics;

namespace FontUtil
{
	public partial class MyFontDialog : UserControl
	{
		public event EventHandler<EventArgs> OnChanged;

		public bool frozen = false;

		bool internalUpdate;

		int currentSize;

		static int[] sizes = new int[] { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72, 144, 220 };

		public static List<MyFontDialog> fontDialogs = new List<MyFontDialog>();

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

		public void Repopulate(RenderType renderType)
		{
			// try to maintain existing Font and Size choice...

			TTFontFamily family = null;
			int size = -1;

			if (fontsComboBox.Items.Count > 0)
			{
				family = Value.face.fontFamily;
				size = Value.size;
			}

			List<TTFontFamily> families = GetFontList(renderType);

			// families is the list of TTFontFamily
			// populate the combo boxes :)

			frozen = true;
			internalUpdate = true;
			{
				fontsComboBox.Items.Clear();
				foreach (TTFontFamily f in families)
				{
					fontsComboBox.Items.Add(f);
				}

				fontsComboBox.SelectedIndex = 0;
			}

			if (family != null)
			{
				foreach (object o in fontsComboBox.Items)
				{
					TTFontFamily f = o as TTFontFamily;

					if (f.name == family.name)
					{
						fontsComboBox.SelectedItem = o;
						break;
					}
				}
			}

			internalUpdate = false;
			frozen = false;
		}

		public MyFontDialog()
		{
			internalUpdate = true;

			InitializeComponent();

			Repopulate(RenderType.WPF);

			internalUpdate = false;
			fontDialogs.Add(this);
		}

		int GetSize()
		{
			int size;

			if (sizesComboBox.DropDownStyle == ComboBoxStyle.DropDown)
			{
				if (!Int32.TryParse(sizesComboBox.Text, out size))
				{
					Debug.WriteLine("Using default size!");
					size = 44;
				}

				if (size > 300)
				{
					size = 300;
				}
			}
			else
			{
				if (sizesComboBox.SelectedItem == null)
				{
					size = ((FontSizeDescriptor)sizesComboBox.Items[0]).size;
				}
				else
				{
					size = ((FontSizeDescriptor)sizesComboBox.SelectedItem).size;
				}
			}
			return size;
		}

		public TTFont Value
		{
			get
			{
				TTFontFace face = stylesComboBox.SelectedItem as TTFontFace;

				if (face == null)
				{
					face = stylesComboBox.Items[0] as TTFontFace;
				}

				if (face != null)
				{
					return new TTFont(face, GetSize());
				}
				return null;
			}
			set
			{
				internalUpdate = true;

				foreach (TTFontFamily f in fontsComboBox.Items)
				{
					if (f.name == value.face.fontFamily.name)
					{
						fontsComboBox.SelectedItem = f;
						break;
					}
				}

				currentSize = value.size;
	
				foreach (FontSizeDescriptor s in sizesComboBox.Items)
				{
					if (s.size == value.size)
					{
						sizesComboBox.SelectedItem = s;
						break;
					}
				}

				stylesComboBox.Items.Clear();
				foreach (TTFontFace f in value.face.fontFamily.faces)
				{
					stylesComboBox.Items.Add(f);
					if (f.name == value.face.name)
					{
						stylesComboBox.SelectedItem = f;
					}
				}

				if (stylesComboBox.SelectedItem == null)
				{
					stylesComboBox.SelectedItem = stylesComboBox.Items[0];
				}

				if (sizesComboBox.SelectedItem == null && (value as TTFont).face.fontFamily.sizesAreRestricted)
				{
					sizesComboBox.SelectedIndex = 0;
				}

				sizesComboBox.Text = currentSize.ToString();

				internalUpdate = false;
			}
		}

		private void fontsComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			internalUpdate = true;
			currentSize = GetSize();

			TTFontFamily f = (TTFontFamily)fontsComboBox.SelectedItem;
			stylesComboBox.Items.Clear();
			sizesComboBox.Items.Clear();

			if (f.sizesAreRestricted)
			{
				sizesComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
				foreach (int i in f.sizes)
				{
					sizesComboBox.Items.Add(new FontSizeDescriptor(i));
				}
				sizesComboBox.SelectedIndex = 0;
			}
			else
			{
				sizesComboBox.DropDownStyle = ComboBoxStyle.DropDown;
				foreach (int i in sizes)
				{
					sizesComboBox.Items.Add(new FontSizeDescriptor(i));
				}
				sizesComboBox.Text = currentSize.ToString();
			}

			if (f.faces != null)
			{
				foreach (TTFontFace face in f.faces)
				{
					stylesComboBox.Items.Add(face);
				}
				stylesComboBox.SelectedIndex = 0;
				stylesComboBox.Enabled = true;
			}
			else
			{
				stylesComboBox.Enabled = false;
			}

			foreach (FontSizeDescriptor s in sizesComboBox.Items)
			{
				if (s.size == currentSize)
				{
					sizesComboBox.SelectedItem = s;
					break;
				}
			}

			if (sizesComboBox.SelectedItem == null && f.sizesAreRestricted)
			{
				sizesComboBox.SelectedIndex = 0;
			}

			internalUpdate = false;
			RaiseOnChanged();
		}

		private void RaiseOnChanged()
		{
			if (!frozen && !internalUpdate && OnChanged != null)
			{
				OnChanged(this, new EventArgs());
			}
		}

		private void sizesComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Don't RaiseOnChanged() as the text change will trigger also
		}

		private void sizesComboBox_TextChanged(object sender, EventArgs e)
		{
			RaiseOnChanged();
		}

		private void stylesComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			RaiseOnChanged();
		}

		private void fontsComboBox_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index >= 0 && e.Index < fontsComboBox.Items.Count)
			{
				TTFontFamily f = (TTFontFamily)fontsComboBox.Items[e.Index];
				Color c = e.ForeColor;
				if (e.State == DrawItemState.Selected)
				{
					c = SystemColors.HotTrack;
				}
				else if (e.State == DrawItemState.Inactive)
				{
					c = SystemColors.WindowText;
				}
				e.DrawBackground();
				TextRenderer.DrawText(
					e.Graphics,
					f.ToString(),
					f.font,
					new Point(e.Bounds.X, e.Bounds.Y + 1),
					e.ForeColor,
					TextFormatFlags.NoPrefix);
			}
		}

		private void fontsComboBox_MeasureItem(object sender, MeasureItemEventArgs e)
		{
			if (e.Index >= 0 && e.Index < fontsComboBox.Items.Count)
			{
				TTFontFamily f = (TTFontFamily)fontsComboBox.Items[e.Index];
				Size s = TextRenderer.MeasureText(f.ToString(), f.font);
				e.ItemHeight = s.Height;
			}
		}
	}
}
