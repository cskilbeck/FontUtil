using System;
using System.Windows.Forms;
using System.Drawing;
using FontUtil;
using System.Xml;

namespace FontUtil
{
	public class TTFontParameter : Parameter
	{
		string mFamily;
		string mFace;
		int mSize;

		public override Type ParameterType { get { return typeof(TTFont); } }

		public override string ParameterName { get { return "TTFont"; } }

		public override void WriteAttributes(XmlTextWriter stream)
		{
			TTFont font = (TTFont)value;
			stream.WriteAttributeString("family", font.face.fontFamily.name);
			stream.WriteAttributeString("face", font.face.name);
			stream.WriteAttributeString("size",font.size.ToString());
		}

		public RenderType mRenderType;

		public override void Read(xml r)
		{
			mFamily = r.String("family");
			mFace = r.String("face");
			mSize = r.Int("size");
		}

		public void Create()
		{
			TTFontFamily fontFamily = TTFontFamily.FindFont(mFamily, mRenderType);
			if (fontFamily != null)
			{
				TTFontFace fontFace = fontFamily.FindFace(mFace);

				if (fontFace != null)
				{
					value = new TTFont(fontFace, mSize);
				}
			}
		}

		public override object GetDefault()
		{
			return new TTFont(TTFontFamily.FindFont("Cooper", RenderType.WPF).faces[0], 44);
		}

		public override void UpdateControl()
		{
			suppressChangedEvents = true;
			((MyFontDialog)Control).Value = (TTFont)value;
			suppressChangedEvents = false;
		}

		public override void UpdateValue()
		{
			value = ((MyFontDialog)Control).Value;
		}

		public override Control CreateControl()
		{
			MyFontDialog f = new MyFontDialog();
			f.Value = (TTFont)GetDefault();
			f.OnChanged += new EventHandler<EventArgs>(f_OnChanged);
			return f;
		}

		void f_OnChanged(object sender, EventArgs e)
		{
			UpdateValue();
			RaiseParameterChanged();
		}
	}
}
