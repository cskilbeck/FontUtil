using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Diagnostics;

namespace FontUtil
{
	public class LayerNode : Node
	{
		public int layer;

		public static List<LayerNode> allLayers = new List<LayerNode>();

		public static void AddLayer(LayerNode l)
		{
			allLayers.Add(l);
			allLayers = allLayers.OrderBy(x => x.layer).ToList();
		}

		public LayerNode(int layerIndex = -1, Parameter color = null, Parameter offset = null, Parameter measure = null)
			: base()
		{
			layer = (layerIndex >= 0) ? layerIndex : allLayers.Count;
			AddLayer(this);
			parameters = new List<Parameter>();
			GraphicParameter graphic = (GraphicParameter)Parameter.CreateParameter("Graphic", typeof(Graphic), 0, null);
			parameters.Add(graphic);
			if (color == null)
			{
				color = Parameter.CreateParameter("Color", typeof(Color), 1, Color.White);
			}
			if (offset == null)
			{
				offset = Parameter.CreateParameter("Offset", typeof(RangedPointF), 2, Point.Empty);
			}
			if (measure == null)
			{
				measure = Parameter.CreateParameter("Measure", typeof(bool), 3, false);
			}
			color.ParameterChanged += new EventHandler<EventArgs>(color_OnChanged);
			offset.ParameterChanged += new EventHandler<EventArgs>(offset_OnChanged);
			measure.ParameterChanged += new EventHandler<EventArgs>(measure_ParameterChanged);
			parameters.Add(color);
			parameters.Add(offset);
			parameters.Add(measure);
			InputSocket s = AddInput();
			s.parameter = graphic;
		}

		void measure_ParameterChanged(object sender, EventArgs e)
		{
			RaiseChanged();
		}

		void offset_OnChanged(object sender, EventArgs e)
		{
			RaiseChanged();
		}

		void color_OnChanged(object sender, EventArgs e)
		{
			RaiseChanged();
		}

		public override void WriteExtraAttributes(System.Xml.XmlTextWriter stream)
		{
			stream.WriteAttributeString("Index", layer.ToString());
		}

		public override void Delete()
		{
			base.Delete();
			int index = 0;
			foreach (LayerNode l in allLayers)
			{
				l.layer = index++;
			}
		}

		public override void Destroy()
		{
			base.Destroy();
			allLayers.Remove(this);
		}

		public override bool CanOutput
		{
			get
			{
				return false;
			}
		}

		public override string Name
		{
			get
			{
				return "LayerNode";
			}
		}

		public override string Description
		{
			get
			{
				return "Layer " + layer.ToString();
			}
		}

		public override int NumControlsRequired()
		{
			return 3;
		}

		public override void DrawBody(Graphics g)
		{
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			//g.FillEllipse(Brushes.Black, Rectangle);
			Rectangle r = new Rectangle(Rectangle.Left - Width, Rectangle.Top, Width * 2, Height);
			g.FillPie(Brushes.Black, r, 270, 180);
			Pen pen = Selected ? Pens.White : highlight ? Pens.Yellow : Pens.Gray;
			g.DrawPie(pen, r, 270, 180);
			StringFormat f = new StringFormat();
			f.Alignment = StringAlignment.Center;
			f.LineAlignment = StringAlignment.Center;
			g.DrawString(Description, CaptionFont, Selected ? Brushes.White : Brushes.Cyan, new PointF(Position.X + Width / 2, Position.Y + Height / 2), f);
		}

		public override Graphic CreateGraphic()
		{
			if (Inputs.Count > 0)
			{
				Socket s = Inputs[0];
				Pin p = s.Pin;
				if (p != null)
				{
					Connection c = p.Connection;
					if (c != null)
					{
						Pin other = c.SendingPin;
						if (other != null)
						{
							Node parent = other.ParentNode;
							if (parent != null)
							{
								//Debug.WriteLine("Layer {0} getting input", this.Description);
								cache = parent.GetGraphic();
							}
						}
					}
				}
			}
			return cache;
		}

		public bool Measure
		{
			get
			{
				return (bool)parameters[3].value;
			}
		}
		
		public override Color RenderColor
		{
			get
			{
				return (Color)parameters[1].value;
			}
		}

		public override PointF RenderOffset
		{
			get
			{
				return ((RangedPointF)parameters[2].value).Value;
			}
		}
	}
}
