using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace FontUtil
{
	public abstract class Node : DraggableObject
	{
		/// <summary>
		/// All the nodes in the system
		/// </summary>
		public static readonly List<Node> allNodes = new List<Node>();

		/// <summary>
		/// Width in pixels of a Node
		/// </summary>
		public const int Width = 68;

		/// <summary>
		/// Height in pixels of a Node
		/// </summary>
		public const int Height = 68;

		/// <summary>
		/// Unique ID, used only when loading and saving the Graph
		/// </summary>
		public int id;

		/// <summary>
		/// All the input <see cref="Parameter"/>s to this Node
		/// </summary>
		protected List<Parameter> parameters;

		/// <summary>
		/// Most recently calculated result of this Node
		/// </summary>
		protected Graphic cache;

		/// <summary>
		/// InputSockets - other Nodes feed their results in through these
		/// </summary>
		private List<InputSocket> inputs;

		/// <summary>
		/// OutputSockets - this Node sends its result out through these
		/// </summary>
		private List<OutputSocket> outputs;

		/// <summary>
		/// Non-Graph inputs (Parameters) will use this FlowLayoutPanel to hold their Controls
		/// </summary>
		private FlowLayoutPanel parametersPanel;

		/// <summary>
		/// A Simple text Label to show the Node description at the top of the <see cref="parametersPanel"/>
		/// </summary>
		private Label descriptionLabel;

		/// <summary>
		/// is this Node Dirty? i.e. does the <see cref="cache"/> need to be recalculated
		/// </summary>
		private bool isDirty;

		/// <summary>
		/// Panel in which the <see cref="parametersPanel"/> will sit
		/// </summary>
		private static Panel parentPanel;

		/// <summary>
		/// small graphic to denote a collapsed <see cref="Parameter"/>
		/// </summary>
		private static Bitmap rightArrow;

		/// <summary>
		/// small graphic to denote an expanded <see cref="Parameter"/>
		/// </summary>
		private static Bitmap downArrow;

		/// <summary>
		/// <see cref="Font"/> which is used to draw the text inside the Node
		/// </summary>
		private static Font captionFont;

		public List<InputSocket> Inputs
		{
			get
			{
				return inputs;
			}
		}

		public void SetParameter(string name, object value)
		{
			Parameter p = parameters.Single(x => x.name == name);
			if (p != null)
			{
				p.value = value;
				p.RaiseParameterChanged();
			}
		}

		public object GetParameterValue(string name)
		{
			Parameter p = parameters.Single(x => x.name == name);
			return (p != null) ? p.value : null;
		}

		public List<OutputSocket> Outputs
		{
			get
			{
				return outputs;
			}
		}

		public static void SetParentPanel(Panel panel)
		{
			parentPanel = panel;
		}

		public void AddInputSocket(InputSocket socket)
		{
			socket.PropertyChanged += new EventHandler<EventArgs>(socket_Changed);	// when the input socket changes, I want to know...
			inputs.Add(socket);
			Dirty = true;
		}

		public void RemoveInputSocket(InputSocket socket)
		{
			Debug.Assert(inputs.Contains(socket));
			if (inputs.Contains(socket))
			{
				socket.PropertyChanged -= socket_Changed;
				inputs.Remove(socket);
				Dirty = true;
			}
		}

		public void AddOutputSocket(OutputSocket socket)
		{
			outputs.Add(socket);
			RefreshPinPositions();
		}

		public void RemoveOutputSocket(OutputSocket socket)
		{
			Debug.Assert(outputs.Contains(socket));
			if (outputs.Contains(socket))
			{
				outputs.Remove(socket);
				socket.Destroy();
			}
		}

		void socket_Changed(object sender, EventArgs e)
		{
			Dirty = true;
			RaiseChanged();
		}

		public override int SelectionOrder
		{
			get { return 1; }
		}

		public static Point HalfSize
		{
			get
			{
				return new Point(Width / 2, Height / 2);
			}
		}

		public static Size FullSize
		{
			get
			{
				return new Size(Width, Height);
			}
		}


		public override void OnLeftClick(Point pos)
		{
			base.OnLeftClick(pos);
			//Debug.WriteLine("Node getting graphic due to left click");
			cache = GetGraphic();
		}

		protected override void OnSelect()
		{
			ParametersPanel.Visible = true;
			parentPanel.Refresh();
		}

		protected override void OnDeSelect()
		{
			UpdateValues();
			ParametersPanel.Visible = false;
		}

		public virtual bool Dirty
		{
			get
			{
				return isDirty;
			}
			set
			{
				isDirty = value;
				if (isDirty)
				{
					cache = null;
				}
			}
		}

		public override void Drop(UIObject what)
		{
			if (what is Pin)
			{
				(what as Pin).AttachTo(this);
			}
		}

		protected static Font CaptionFont
		{
			get
			{
				if (captionFont == null)
				{
					captionFont = new Font("Consolas", 8, FontStyle.Regular);
				}
				return captionFont;
			}
		}

		public virtual void DrawPreview(Graphics graphics, Point pos)
		{
			Graphic graphic = GetGraphic();
			if (graphic != null)
			{
				Glyph glyph = graphic.glyph;
				if (glyph != null)
				{
					PointF offset = RenderOffset;
					Color color = RenderColor;

					glyph.DrawPreview(graphic, graphics, new PointF(pos.X + offset.X, pos.Y + offset.Y), color);
				}
			}
		}

		public virtual void WriteExtraAttributes(XmlTextWriter stream)
		{
		}

		public void Save(XmlTextWriter stream)
		{
			int sid = 0;
			foreach (InputSocket i in Inputs)
			{
				i.id = sid++;
			}
			sid = 0;
			foreach (OutputSocket o in Outputs)
			{
				o.id = sid++;
			}
			stream.WriteStartElement(Name);
			{
				stream.WriteAttributeString("id", id.ToString());
				stream.WriteAttributeString("inputs", Inputs.Count.ToString());
				stream.WriteAttributeString("outputs", Outputs.Count.ToString());
				stream.WriteAttributeString("X", Position.X.ToString());
				stream.WriteAttributeString("Y", Position.Y.ToString());

				WriteExtraAttributes(stream);

				foreach (Parameter p in parameters)
				{
					if (!p.IsGraphInput)
					{
						p.Save(stream);
					}
				}
			}
			stream.WriteEndElement(); // Name
		}

		public virtual PointF RenderOffset
		{
			get
			{
				return PointF.Empty;
			}
		}

		public virtual Color RenderColor
		{
			get
			{
				return Color.White;
			}
		}

		public bool CanCurrentlyAcceptInput
		{
			get
			{
				if (AcceptsInput)
				{
					foreach (Socket s in inputs)
					{
						if (s.Pin == null)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		public virtual Graphic GetGraphic()
		{
			if (Dirty)
			{
				CreateGraphic();
				Dirty = false;
			}
			return cache;
		}

		public abstract Graphic CreateGraphic();

		public virtual bool AcceptsInput
		{
			get
			{
				return true;
			}
		}

		public virtual bool CanOutput
		{
			get
			{
				return true;
			}
		}

		public override bool DropTest(UIObject other)
		{
			if(other is Pin)
			{
				return base.DropTest(other) && ((other as Pin).direction == Pin.Direction.Output || CanCurrentlyAcceptInput);
			}
			else
			{
				return false;
			}
		}

		public virtual int NumControlsRequired()
		{
			return 0;
		}

		public static void SetDoubleBuffered(System.Windows.Forms.Control c)
		{
			typeof(Control).GetProperty("DoubleBuffered",System.Reflection.BindingFlags.NonPublic |System.Reflection.BindingFlags.Instance).SetValue(c, true, null);
		}

		private static Bitmap RightArrow
		{
			get
			{
				if (rightArrow == null)
				{
					rightArrow = new Bitmap(11, 11);
					rightArrow.Fill(Color.Transparent);
					using (Graphics g = Graphics.FromImage(rightArrow))
					{
						g.FillPolygon(Brushes.Black, new Point[] { new Point(4, 0), new Point(9, 5), new Point(4, 10) });
					}
				}
				return rightArrow;
			}
		}

		private static Bitmap DownArrow
		{
			get
			{
				if (downArrow == null)
				{
					downArrow = new Bitmap(11, 11);
					downArrow.Fill(Color.Transparent);
					using (Graphics g = Graphics.FromImage(downArrow))
					{
						g.FillPolygon(Brushes.Black, new Point[] { new Point(0, 4), new Point(10, 4), new Point(5,9) });
					}
				}
				return downArrow;
			}
		}

		protected Panel ParametersPanel
		{
			get
			{
				if (parametersPanel == null)
				{
					parametersPanel = new FlowLayoutPanel();
					parametersPanel.FlowDirection = FlowDirection.TopDown;
					parametersPanel.AutoSize = true;
					parametersPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
					parametersPanel.Location = Point.Empty;
					parametersPanel.Size = new Size(parentPanel.Size.Width - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth - 2, parentPanel.Size.Height - 2);
					parametersPanel.Visible = false;
					parentPanel.Controls.Add(parametersPanel);

					SetDoubleBuffered(parametersPanel);
					SetDoubleBuffered(parentPanel);

					FlowLayoutPanel title = new FlowLayoutPanel();
					title.FlowDirection = FlowDirection.LeftToRight;
					title.AutoSize = true;
					title.AutoSizeMode = AutoSizeMode.GrowAndShrink;
					title.Dock = DockStyle.Top;

					descriptionLabel = new Label();
					descriptionLabel.Text = Description;
					descriptionLabel.AutoSize = true;

					Label nameLabel = new Label();
					nameLabel.Text = Name;
					nameLabel.AutoSize = true;

					//title.Controls.Add(nameLabel);
					title.Controls.Add(descriptionLabel);

					parametersPanel.Controls.Add(title);
					
					foreach (Parameter p in parameters)
					{
						if (!p.IsGraphInput)
						{
							FlowLayoutPanel panel = new FlowLayoutPanel();
							panel.FlowDirection = FlowDirection.LeftToRight;
							panel.AutoSize = true;
							panel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

							Panel controlContainer = new Panel();

							Label name = new Label();
							name.Text = p.name;
							name.TextAlign = ContentAlignment.MiddleLeft;
							name.Click += new EventHandler(name_Click);
							name.Tag = controlContainer;
							name.Image = DownArrow;
							name.Margin = new Padding(0, 0, 0, 0);
							name.ImageAlign = ContentAlignment.MiddleRight;
							int w = name.GetPreferredSize(new Size(Int32.MaxValue, 24)).Width;
							name.AutoSize = false;
							name.Width = w + RightArrow.Width + 4;
							name.Height = 24;
							name.Top = 8;

							controlContainer.Controls.Add(p.Control);
							controlContainer.Height = p.Control.Height + 2;
							controlContainer.Width = p.Control.Width + 2;

							panel.Controls.Add(name);
							panel.Controls.Add(controlContainer);

							p.Control.Top = 0;

							p.UpdateControl();
							//p.ParameterChanged += new EventHandler<EventArgs>(p_OnChanged);
							parametersPanel.Controls.Add(panel);
						}
					}
					parametersPanel.Visible = true;
				}
				return parametersPanel;
			}
		}

		void name_Click(object sender, EventArgs e)
		{
			Label t = sender as Label;
			Panel p = t.Tag as Panel;
			if (p.Visible)
			{
				p.Visible = false;
				t.Image = RightArrow;
			}
			else
			{
				p.Visible = true;
				t.Image = DownArrow;
			}
		}

		void p_OnChanged(object sender, EventArgs e)
		{
			//Debug.WriteLine("Node ({0}) creating graphic due to parameter changed", Description);
			cache = GetGraphic();
			RaiseChanged();
		}

		public InputSocket FirstFreeInputSocket()
		{
			foreach (InputSocket s in inputs)
			{
				if (s.Pin == null)
				{
					return s;
				}
			}
			return null;
		}

		public void SetDirty()
		{
			Dirty = true;
			foreach (Socket s in Outputs)
			{
				if (s.Pin != null && s.Pin.OtherNode != null)
				{
					s.Pin.OtherNode.SetDirty();
				}
			}
		}
		
		internal Node()
			: base(new Size(Width, Height))
		{
			allNodes.Add(this);
			inputs = new List<InputSocket>();
			outputs = new List<OutputSocket>();
			Dirty = true;
		}

		public virtual void UpdateValues()
		{
			foreach (Parameter p in parameters)
			{
				p.UpdateValue();
			}
		}

		public virtual void UpdateControls()
		{
			foreach (Parameter p in parameters)
			{
				p.UpdateControl();
			}
		}

		public void DeHighlight()
		{
			highlight = false;
			foreach (Socket i in inputs)
			{
				i.highlight = false;
				if (i.Pin != null)
				{
					i.Pin.highlight = false;
				}
			}
			foreach (Socket o in outputs)
			{
				o.highlight = false;
				if (o.Connection != null)
				{
					Pin dst = o.Connection.ReceivingPin;
					if (dst != null)
					{
						dst.highlight = false;
					}
				}
			}
		}

		public void RefreshPinPositions()
		{
			if (inputs != null)
			{
				for (int i = 1; i <= inputs.Count; ++i)
				{
					int y = i * Height / (inputs.Count + 1) - Pin.height / 2;
					inputs[i - 1].Offset = new Point(1, y);
				}
			}

			if (outputs != null)
			{
				for (int i = 1; i <= outputs.Count; ++i)
				{
					int y = i * Height / (outputs.Count + 1) - Pin.height / 2;
					outputs[i - 1].Offset = new Point(Width + 1, y);
				}
			}
			RaiseMoving();
		}

		public void AttachOutput(OutputPin pin)
		{
			OutputSocket s = new OutputSocket(Point.Empty, this);
			s.Pin = pin;
		}

		public OutputPin AddOutput()
		{
			OutputSocket s = new OutputSocket(Point.Empty, this);
			RefreshPinPositions();
			OutputPin o = new OutputPin(s);
			InputPin i = new InputPin(null);
			s.Pin = o;
			i.Position = Position.Add(new Point(Width + 50, Height / 2 - Pin.height / 2));
			Connection c = new Connection(o, i);
			return o;
		}

		public void DetachPin(Pin p)
		{
			p.Detach(this);
		}

		public void RemoveInput(InputPin p)
		{
			Debug.Assert(inputs.Contains(p.Socket));
			p.Socket = null;
		}

		public void RemoveOutput(OutputPin p)
		{
			Debug.Assert(outputs.Contains(p.Socket));
			p.Socket = null;
		}

		public override void OnRightClick(ContextMenu m)
		{
			if (CanOutput)
			{
				m.MenuItems.Add("Add output", new EventHandler(m1_NewOutputClick));
			}
		}

		void m1_NewOutputClick(object sender, EventArgs e)
		{
			AddOutput();
		}

		public InputSocket AddInput()
		{
			InputSocket s = new InputSocket(Point.Empty, this);
			s.PropertyChanged += new EventHandler<EventArgs>(input_Changed);
			SetDirty();
			RefreshPinPositions();
			return s;
		}

		void input_Changed(object sender, EventArgs e)
		{
			SetDirty();
			GetGraphic();
		}

		public override void Delete()
		{
			// try to fix up any existing connections

			int inputIndex = 0;
			int outputIndex = 0;

			while (inputIndex < inputs.Count && outputIndex < outputs.Count)
			{
				while (inputIndex < inputs.Count && inputs[inputIndex].Connection == null)
				{
					++inputIndex;
				}
				while (outputIndex < outputs.Count && outputs[outputIndex].Connection == null)
				{
					++outputIndex;
				}

				if(inputIndex < inputs.Count && outputIndex < outputs.Count)
				{
					Connection left = inputs[inputIndex].Connection;
					Connection right = outputs[outputIndex].Connection;

					if (right.SendingPin != null)
					{
						right.SendingPin.Destroy();
					}
					right.SendingPin = null;

					if (left.ReceivingPin != null)
					{
						left.ReceivingPin.Destroy();
					}
					left.ReceivingPin = right.ReceivingPin;

					right.Destroy();

					++inputIndex;
					++outputIndex;
				}
			}

			foreach (Socket s in inputs)
			{
				if (s.Pin != null)
				{
					if (s.Pin.Connection != null)
					{
						if (s.Pin.OtherNode == null)
						{
							s.Pin.Connection.Destroy();
						}
						else
						{
							s.Pin.Socket = null;
						}
					}

					if (s.Pin.Socket != null)
					{
						s.Pin.Destroy();
					}
				}
				s.Pin = null;
				s.Destroy();
			}

			foreach (Socket s in outputs)
			{
				if (s.Pin != null)
				{
					if (s.Pin.Connection != null)
					{
						if (s.Pin.OtherPin != null)
						{
							if (s.Pin.OtherPin.Socket != null)
							{
								s.Pin.OtherPin.Socket.Pin = null;
							}
							s.Pin.OtherPin.Destroy();
						}
						s.Pin.Connection.Destroy();
					}
					s.Pin.Destroy();
				}
				s.Pin = null;
				s.Destroy();
			}

			base.Delete();
		}

		public override void Destroy()
		{
			base.Destroy();
			allNodes.Remove(this);
		}

		public virtual void DrawBody(Graphics g)
		{
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
			g.FillRectangle(Brushes.Black, Position.X, Position.Y, Width, Height);
			Pen pen = Selected ? Pens.White : highlight ? Pens.Yellow : Pens.Gray;
			g.DrawRectangle(pen, Position.X, Position.Y, Width, Height);
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			g.DrawString(Caption, CaptionFont, Selected ? Brushes.White : Brushes.Cyan, new RectangleF(Position.X + 2, Position.Y + 2, Width - 4, Height - 4));
		}

		public override void Draw(Graphics graphics)
		{
			DrawBody(graphics);

			foreach (Socket s in outputs)
			{
				s.Draw(graphics);
			}

			foreach(Socket s in outputs)
			{
				if (s.Pin != null)
				{
					s.Pin.Draw(graphics);

					if (s.Pin.Connection != null)
					{
						s.Pin.Connection.Draw(graphics);

						if (s.Pin.Connection.ReceivingPin != null && s.Pin.Connection.ReceivingPin.ParentNode == null)
						{
							s.Pin.Connection.ReceivingPin.Draw(graphics);
						}
					}
				}
			}

			foreach (Socket s in inputs)
			{
				s.Draw(graphics);

				if (s.Pin != null)
				{
					s.Pin.Draw(graphics);
				}
			}
		}
	}
}
