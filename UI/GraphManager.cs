using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace FontUtil
{
	public static class GraphManager
	{
		static Size size;
		static PictureBox pictureBox;
		static Bitmap graphBitmap;
		static UIObject _hoverThing = null;
		static UIObject clickThing = null;
		static UIObject _selectedThing = null;
		static UIObject dragThing = null;

		public static Point mouseClickPosition;

		public static event EventHandler<EventArgs> RedrawRequired;
		public static event EventHandler<EventArgs> RefreshRequired;

		public static Node SelectedNode
		{
			get
			{
				return SelectedThing as Node;
			}
		}

		public static UIObject SelectedThing
		{
			get
			{
				return _selectedThing;
			}
			set
			{
				if (_selectedThing != value)
				{
					if (_selectedThing != null)
					{
						_selectedThing.DeSelect();
					}
					_selectedThing = value;
					if (_selectedThing != null)
					{
						_selectedThing.Select();
					}
				}
			}
		}

		static UIObject HoverThing
		{
			get
			{
				return _hoverThing;
			}
			set
			{
				if (_hoverThing != null)
				{
					_hoverThing.highlight = false;
				}
				_hoverThing = value;
				if (_hoverThing != null)
				{
					_hoverThing.highlight = true;
				}
			}
		}

		static void RaiseRedrawRequired()
		{
			if (RedrawRequired != null)
			{
				RedrawRequired(null, null);
			}
		}

		static void RaiseRefreshRequired()
		{
			if (RefreshRequired != null)
			{
				RefreshRequired(null, null);
			}
		}

		public static Size Size
		{
			get
			{
				return size;
			}
			set
			{
				// re-constrain all the nodes?
				size = value;
				graphBitmap = new Bitmap(size.Width, size.Height);
				if (pictureBox != null)
				{
					pictureBox.Image = graphBitmap;
				}
				UpdateGraph();
			}
		}

		public static PictureBox PictureBox
		{
			get
			{
				return pictureBox;
			}
			set
			{
				pictureBox = value;
				pictureBox.SizeChanged += new EventHandler(pictureBox_SizeChanged);
				pictureBox.MouseDown += new MouseEventHandler(pictureBox_MouseDown);
				pictureBox.MouseMove += new MouseEventHandler(pictureBox_MouseMove);
				pictureBox.MouseUp += new MouseEventHandler(pictureBox_MouseUp);
				pictureBox.PreviewKeyDown += new PreviewKeyDownEventHandler(pictureBox_PreviewKeyDown);
				Size = pictureBox.Size;
			}
		}

		static void pictureBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
		}

		private static void pictureBox_MouseDown(object sender, MouseEventArgs e)
		{
			mouseClickPosition = e.Location;

			switch(e.Button)
			{
				case MouseButtons.Left:
					MouseLeftClick(e.Location);
					break;

				case MouseButtons.Right:
					MouseRightClick(e.Location);
					break;
			}
		}

		private static void pictureBox_MouseMove(object sender, MouseEventArgs e)
		{
			MouseMovement(e.Location);
		}

		private static void pictureBox_MouseUp(object sender, MouseEventArgs e)
		{
			switch (e.Button)
			{
				case System.Windows.Forms.MouseButtons.Left:
					MouseLeftRelease(e.Location);
					break;

				case System.Windows.Forms.MouseButtons.Right:
					//MouseRightRelease(e.Location);
					break;
			}
		}

		static void MouseMovement(Point pos)
		{
			if (clickThing != null)
			{
				if (pos.Subtract(mouseClickPosition).Length() > 4)
				{
					if (clickThing.CanDrag)
					{
						DraggableObject d = clickThing as DraggableObject;

						dragThing = d.BeginDrag();
						clickThing = null;
					}
				}
			}
			if (dragThing != null)
			{
				dragThing.DoDrag(pos);
				UIObject dropTarget = dragThing.FindDropTarget();
				HoverThing = dropTarget;
				UpdateGraph();
			}
			else
			{
				UIObject t = DraggableObject.FindThing(pos);
				if (HoverThing != null)
				{
					HoverThing.MouseLeave(pos);
				}

				bool graphUpdatedRequired = HoverThing != t;

				HoverThing = t;

				if (HoverThing != null)
				{
					HoverThing.MouseEnter(pos);
				}
				if (graphUpdatedRequired)
				{
					UpdateGraph();
				}
			}
		}

		static void MouseLeftRelease(Point pos)
		{
			if (dragThing != null)
			{
				dragThing.EndDrag(pos);

				UIObject dropTarget = dragThing.FindDropTarget();

				if (dropTarget != null)
				{
					dropTarget.Drop(dragThing);	// dragThing or dropTarget or both may well get destroyed in there...
				}
			}
			dragThing = null;
			clickThing = null;
			UpdateGraph();
		}

		static void MouseLeftClick(Point pos)
		{
			bool graphUpdateRequired = false;

			UIObject t = UIObject.FindThing(pos);	// hmmm - would rather a pin than a socket, for example...

			clickThing = t;
			HoverThing = null;

			graphUpdateRequired = SelectThing(t);

			if (t != null)
			{
				t.OnLeftClick(pos);
			}

			if (graphUpdateRequired)
			{
				UpdateGraph();
			}
		}

		static void AddDelete(UIObject o, ContextMenu menu)
		{
			if (o.ShowDeleteMenuItem)
			{
				menu.MenuItems.Add("-");
				string name = o.Caption;
				menu.MenuItems.Add("Delete " + name, new EventHandler(MenuDelete)).Tag = o;
			}
		}

		static void MenuDelete(object sender, EventArgs e)
		{
			MenuItem m = sender as MenuItem;
			UIObject obj = (m.Tag) as UIObject;
			obj.Delete();
			UpdateGraph();
		}

		static void MouseRightClick(Point pos)
		{
			SelectedThing = null;
			bool graphUpdateRequired = false;

			UIObject o = UIObject.FindThing(pos);
			ContextMenu m;

			if (o != HoverThing)
			{
				graphUpdateRequired = true;
			}

			if (o != null)
			{
				graphUpdateRequired |= SelectThing(o);
				HoverThing = o;
				m = new ContextMenu();
				o.OnRightClick(m);
				AddDelete(o, m);
			}
			else
			{
				m = ContextMenu();
			}
			if (graphUpdateRequired)
			{
				UpdateGraph();
			}
			if (m != null)
			{
				m.Show(pictureBox, pos);
			}
			if (graphUpdateRequired)
			{
				UpdateGraph();
			}
		}

		public static FontNode CreateSourceNode(Point pos)
		{
			FontNode sourceNode = GraphManager.CreateFontNode(null, null, pos);
			SelectThing(sourceNode);
			sourceNode.PropertyChanged += new EventHandler<EventArgs>(fnode_OnChanged);
			UpdateGraph();
			return sourceNode;
		}

		static void newSource_Click(object sender, EventArgs e)
		{
			MenuItem m = sender as MenuItem;
			if (m != null)
			{
				CreateSourceNode(mouseClickPosition);
			}
		}

		public static void fnode_OnChanged(object sender, EventArgs e)
		{
			Node n = sender as Node;
			n.UpdateValues();
			if (n is FontNode)
			{
				UpdateGraph();
			}
			RaiseRefreshRequired();
		}

		static void SelectNode(Node node)
		{
			MoveNodeToTop(node);
			RaiseRefreshRequired();
		}

		static void MoveNodeToTop(Node n)
		{
			Node.allNodes.Remove(n);
			Node.allNodes.Add(n);
		}

		static void NewNodePlease(object sender, EventArgs e)
		{
			MenuItem m = sender as MenuItem;
			if (m != null)
			{
				Node newNode = CreateFunctionNode((Function)m.Tag, mouseClickPosition);
				SelectThing(newNode);
				UpdateGraph();
				newNode.PropertyChanged += new EventHandler<EventArgs>(fnode_OnChanged);
			}
		}

		public static LayerNode NewLayer(Pin p)
		{
			Node layer = CreateLayerNode(p.Position);
			p.AttachTo(layer);
			layer.PropertyChanged += new EventHandler<EventArgs>(fnode_OnChanged);
			GraphManager.SelectThing(layer);
			GraphManager.UpdateGraph();
			RaiseRefreshRequired();
			return (LayerNode)layer;
		}

		public static void NewFunction(Function func, Pin p)
		{
			FunctionNode f = (FunctionNode)CreateFunctionNode(func, p.Position, p);
			f.PropertyChanged += new EventHandler<EventArgs>(fnode_OnChanged);
			f.AddOutput();
			SelectedThing = f;
			f.Select();
			f.SetDirty();
			RaiseRefreshRequired();
		}

		public static void NewSource(OutputPin p)
		{
			Debug.Assert(p.Socket == null);

			FontNode sourceNode = CreateFontNode((FontDescriptor)null, null, mouseClickPosition);

			// delete the output
			Socket socket = sourceNode.Outputs[0];
			Pin oldPin = socket.Pin;
			oldPin.Socket = null;
			oldPin.Delete();
			socket.Delete();

			p.AttachTo(sourceNode);
			sourceNode.GetGraphic();
			SelectThing(sourceNode);
			sourceNode.PropertyChanged += new EventHandler<EventArgs>(fnode_OnChanged);
			UpdateGraph();
			RaiseRefreshRequired();
		}

		static void newLayer_Click(object sender, EventArgs e)
		{
			MenuItem m = sender as MenuItem;
			if (m != null)
			{
				Node layer = CreateLayerNode(mouseClickPosition);
				layer.PropertyChanged += new EventHandler<EventArgs>(fnode_OnChanged);
				SelectThing(layer);
				UpdateGraph();
			}
		}

		static bool SelectThing(UIObject t)
		{
			bool rc = t != SelectedThing;
			SelectedThing = t;
			Node n = SelectedThing as Node;
			if (n != null)
			{
				SelectNode(n);
			}
			return rc;
		}

		static void pictureBox_SizeChanged(object sender, EventArgs e)
		{
			Size = pictureBox.Size;
		}

		static ContextMenu ContextMenu()
		{
			ContextMenu m = new ContextMenu();
			m.MenuItems.Add("New Source", new EventHandler(newSource_Click));
			m.MenuItems.Add("New Layer", new EventHandler(newLayer_Click));
			PluginManager.AppendPluginsToContextMenu(m, new EventHandler(NewNodePlease));
			return m;
		}

		static void Clear()
		{
			SelectedThing = null;
			HoverThing = null;
			UIObject.allUIObjects.Clear();
			Node.allNodes.Clear();
			FontNode.allFontNodes.Clear();
			LayerNode.allLayers.Clear();
			Connection.allConnections.Clear();
			Pin.allPins.Clear();
			Socket.allSockets.Clear();
		}

		public static Point ConstrainPoint(Point n, Size size)
		{
			Point p = n;
			int w = 0;
			int h = 0;
			if (size != null)
			{
				w = size.Width;
				h = size.Height;
			}
			if (p.X < 10)
			{
				p.X = 10;
			}
			if (p.Y < 10)
			{
				p.Y = 10;
			}
			if (p.X > Size.Width - w - 10)
			{
				p.X = Size.Width - w - 10;
			}
			if (p.Y > Size.Height - h - 10)
			{
				p.Y = Size.Height - h - 10;
			}
			return p;
		}

		public static void UpdateGraph()
		{
			DrawGraph(graphBitmap);
			RaiseRedrawRequired();
		}

		public static void DrawGraph(Image image)
		{
			using (Graphics g = Graphics.FromImage(image))
			{
				g.FillRectangle(new SolidBrush(pictureBox.BackColor), 0, 0, image.Width, image.Height);
				foreach (Node n in Node.allNodes)
				{
					n.Draw(g);
				}

				// now draw all the sending pins which have no parentNode
				foreach (Pin p in Pin.allPins)
				{
					if (p.direction == Pin.Direction.Output && p.ParentNode == null)
					{
						p.Draw(g);
						if (p.Connection != null)
						{
							p.Connection.Draw(g);

							if (p.OtherPin != null)
							{
								p.OtherPin.Draw(g);
							}
						}
					}
				}


				foreach (Socket s in Socket.allSockets)
				{
					if (s.ParentNode == null || !s.ParentNode.Outputs.Contains(s))
					{
						s.Draw(g);
					}
				}
#if DEBUG
				g.DrawString(UIObject.allUIObjects.Count + " things, " +
								Node.allNodes.Count + " nodes, " +
								Pin.allPins.Count + " pins, " +
								Socket.allSockets.Count() + " sockets, " +
								Connection.allConnections.Count + " connections",
							new Font("Arial", 10), Brushes.Black, new PointF(0, 0));
#endif
			}
		}

		public static Node InitNode(Node n, Point pos, Pin attachPin = null)
		{
			Point subFromPos = Node.HalfSize;
			if (attachPin != null)
			{
				subFromPos.Y -= Pin.HalfSize.Y;
				subFromPos.X -= Node.HalfSize.X;
			}
			n.Position = GraphManager.ConstrainPoint(pos.Subtract(subFromPos), Node.FullSize);
			if (attachPin == null)
			{
				if (n.CanOutput)
				{
					n.AddOutput();
				}
			}
			else
			{
				attachPin.AttachTo(n);
			}
			return n;
		}

		public static Node CreateFunctionNode(Function function, Point pos, Pin pin = null)
		{
			return InitNode(new FunctionNode(function), pos, pin);
		}

		public static FontNode CreateFontNode(FontDescriptor fontDescriptor, TTFont font, Point pos, Pin pin = null)
		{
			return (FontNode)InitNode(new FontNode(fontDescriptor, font), pos, pin);
		}

		public static Node CreateLayerNode(Point pos, Pin pin = null)
		{
			return InitNode(new LayerNode(), pos, pin);
		}

		public static void Save(XmlTextWriter stream)
		{
			stream.WriteStartElement("Graph");
			{
				stream.WriteStartElement("Nodes");
				{
					stream.WriteAttributeString("Count", Node.allNodes.Count.ToString());

					int id = 0;
					foreach (Node n in Node.allNodes)
					{
						n.id = id++;
						n.Save(stream);
					}
				}
				stream.WriteEndElement(); // Nodes

				stream.WriteStartElement("Connections");
				{
					stream.WriteAttributeString("Count", Connection.allConnections.Count.ToString());

					foreach (Connection c in Connection.allConnections)
					{
						c.Save(stream);
					}
				}
				stream.WriteEndElement(); // Connections
			}
			stream.WriteEndElement(); // Graph
		}

		static List<Parameter> ReadParameters(xml r)
		{
			Parameter.Init();

			List<Parameter> list = new List<Parameter>();

			if (!r.IsEmptyElement)
			{
				int index = 0;

				while (true)
				{
					r.Read();

					if (r.NodeType == XmlNodeType.EndElement)
					{
						break;	// it eats the </Node> end element
					}
					else if (r.NodeType == XmlNodeType.Element)
					{
						if (r.Name != "Parameter")
						{
							r.Err("Expected <Parameter> node");
						}

						string name = r.String("Name");
						string type = r.String("Type");

						if (Parameter.allTypes.ContainsKey(type))
						{
							Type t = Parameter.allTypes[type];

							Parameter p = Parameter.CreateParameter(name, t, index++, null);
							p.Read(r);
							list.Add(p);
						}
					}
				}
			}
			return list;
		}

		static Node ReadFontNode(xml r)
		{
			List<Parameter> p = ReadParameters(r);
			if(p.Count == 2)
			{

				return new FontNode(null, p[0] as TTFontParameter, p[1] as FontDescriptorParameter);
			}
			else if (p.Count == 3)
			{
				// grotty hack to get around cross-parameter contamination issue - TTFontParameter relies on knowing RenderType to init correctly
				(p[1] as TTFontParameter).mRenderType = (RenderType)(p[0] as RenderTypeParameter).value;
				(p[1] as TTFontParameter).Create();
				return new FontNode(p[0] as RenderTypeParameter, p[1] as TTFontParameter, p[2] as FontDescriptorParameter);
			}
			else
			{
				return null;
			}
		}

		static Node ReadLayerNode(xml r)
		{
			int index = r.Int("Index");
			List<Parameter> p = ReadParameters(r);
			if (p.Count == 2)
			{
				return new LayerNode(index, p[0], p[1]);
			}
			else if(p.Count == 3)
			{
				return new LayerNode(index, p[0], p[1], p[2]);
			}
			return null;
		}

		static Node ReadFunctionNode(xml r)
		{
			string plugin = r.String("Plugin");
			string function = r.String("Function");
			List<Parameter> p = ReadParameters(r);			
			Function f = PluginManager.FindFunction(plugin, function);
			return f != null ? new FunctionNode(f, p) : null;
		}

		static void ReadNodes(xml r)
		{
			r.Element("Nodes");
			{
				int numNodes = r.Int("Count");

				if (numNodes > 0)
				{
					Clear();

					for (int i = 0; i < numNodes; ++i)
					{
						string nodeName = null;

						r.Element();
						{
							int id = r.Int("id");
							int inputs = r.Int("inputs");
							int outputs = r.Int("outputs");
							int X = r.Int("X");
							int Y = r.Int("Y");

							Node node = null;

							nodeName = r.Name;

							switch (r.Name)
							{
								case "FontNode":
									node = ReadFontNode(r);
									break;

								case "LayerNode":
									node = ReadLayerNode(r);
									node.PropertyChanged += new EventHandler<EventArgs>(node_Changed);
									break;

								case "FunctionNode":
									node = ReadFunctionNode(r);
									break;
							}

							if (node != null)
							{
								node.Position = new Point(X, Y);
								node.id = id;
								int oid = 0;
								while (node.Outputs.Count < outputs)
								{
									new OutputSocket(Point.Empty, node).id = oid++;
								}
								//node.Changed += new EventHandler<EventArgs>(node_Changed);
								node.RefreshPinPositions();
								node.UpdateControls();
							}
						}
					}
				}
			}
			r.EndElement("Nodes");
		}

		static void node_Changed(object sender, EventArgs e)
		{
			RaiseRefreshRequired();
		}

		static void ReadConnections(xml r)
		{
			r.Element("Connections");
			{
				int numConnections = r.Int("Count");
				for (int i = 0; i < numConnections; ++i)
				{
					r.Element("Connection");
					int fromNodeID = r.Int("FromNode");
					int fromSocketID = r.Int("FromSocket");
					int toNodeID = r.Int("ToNode");
					int toSocketID = r.Int("ToSocket");
					int FromX = r.Int("FromX");
					int FromY = r.Int("FromY");
					int ToX = r.Int("ToX");
					int ToY = r.Int("ToY");

					Node fromNode = null;
					Node toNode = null;

					foreach (Node n in Node.allNodes)
					{
						if (n.id == fromNodeID)
						{
							fromNode = n;
						}
						if (n.id == toNodeID)
						{
							toNode = n;
						}
					}
					OutputSocket fromSocket = null;
					InputSocket toSocket = null;

					if (fromNode != null && fromNode.Outputs.Count > fromSocketID)
					{
						fromSocket = fromNode.Outputs[fromSocketID];
					}
					if (toNode != null && toNode.Inputs.Count > toSocketID)
					{
						toSocket = toNode.Inputs[toSocketID];
					}
					OutputPin outputPin = new OutputPin(fromSocket);
					if (fromSocket == null)
					{
						outputPin.Position = new Point(FromX, FromY);
					}
					InputPin inputPin = new InputPin(toSocket);
					if (toSocket == null)
					{
						inputPin.Position = new Point(ToX, ToY);
					}
					Connection c = new Connection(outputPin, inputPin);
				}
			}
			r.EndElement("Connections");
		}

		static void ReadGraph(xml r)
		{
			r.Element("Graph");
			{
				ReadNodes(r);
				ReadConnections(r);
			}
			r.EndElement("Graph");
		}

		public static List<char> Load(string filename)
		{
			List<char> chars = new List<char>();
			try
			{
				using (xml r = new xml(filename))
				{
					r.WhitespaceHandling = WhitespaceHandling.None;

					r.XmlDeclaration();

					r.DocumentType("BitmapFont");

					r.Element("BitmapFont");
					{
						int height = r.Int("Height");
						float baseLine = r.Float("Baseline");

						ReadGraph(r);

						do
						{
							// might have layers, might not
							r.Read();
							if (r.NodeType == XmlNodeType.Element)
							{
								switch (r.Name)
								{
									case "Layers":
										int numLayers = r.Int("Count");
										for (int i = 0; i < numLayers; ++i)
										{
											r.Element("Layer");
										}
										if (!r.IsEmptyElement)
										{
											r.EndElement("Layers");
										}
										break;

									case "Glyphs":
										int numGlyphs = r.Int("Count");

										for (int g = 0; g < numGlyphs; ++g)
										{
											int numImages = 0;
											r.Element("Glyph");
											{
												char c = (char)r.Int("char");
												numImages = r.Int("images");
												for (int img = 0; img < numImages; ++img)
												{
													r.Element("Graphic");
												}
												chars.Add(c);
											}
											if (numImages > 0)
											{
												r.EndElement("Glyph");
											}
										}
										if (!r.IsEmptyElement)
										{
											r.EndElement("Glyphs");
										}
										break;

									default:
										r.Err("Unknown node <" + r.Name + ">");
										break;
								}
							}
						}
						while (!r.EOF);
					}
				}
			}
			catch (FileNotFoundException f)
			{
				MessageBox.Show(f.Message);
			}
			catch (XmlException e)
			{
				MessageBox.Show(e.Message);
			}
			return chars;
		}
	}
}
