using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace FontUtil
{
	public class Connection : UIObject
	{
		private OutputPin sendingPin;		// data from
		private InputPin receivingPin;		// data to

		public static List<Connection> allConnections = new List<Connection>();

		private static Pen selectedPen;
		private static Pen highlightedPen;
		private static Pen inactivePen;
		private static Pen widePen;

		private GraphicsPath path;

		public override int SelectionOrder
		{
			get { return 4; }
		}

		public override string Name
		{
			get
			{
				return "Connection";
			}
		}

		public OutputPin SendingPin
		{
			get
			{
				return sendingPin;
			}
			set
			{
				if (sendingPin != null)
				{
					sendingPin.Connection = null;
					sendingPin.PropertyChanged -= sendingPin_Changed;
					sendingPin.Moving -= sendingPin_Moving;
					sendingPin.Deleting -= sendingPin_Deleting;
				}
				sendingPin = value;
				if (sendingPin != null)
				{
					sendingPin.Connection = this;
					sendingPin.PropertyChanged += new EventHandler<EventArgs>(sendingPin_Changed);
					sendingPin.Moving += new EventHandler<MovingEventArgs>(sendingPin_Moving);
					sendingPin.Deleting += new EventHandler<EventArgs>(sendingPin_Deleting);
				}
			}
		}

		void sendingPin_Changed(object sender, EventArgs e)
		{
			RaiseChanged();
		}

		public InputPin ReceivingPin
		{
			get
			{
				return receivingPin;
			}
			set
			{
				if (receivingPin != null)
				{
					receivingPin.Connection = null;
					receivingPin.Moving -= receivingPin_Moving;
					receivingPin.Deleting -= receivingPin_Deleting;
				}
				receivingPin = value;
				if (receivingPin != null)
				{
					receivingPin.Connection = this;
					receivingPin.Moving += new EventHandler<MovingEventArgs>(receivingPin_Moving);
					receivingPin.Deleting += new EventHandler<EventArgs>(receivingPin_Deleting);
				}
			}
		}

		void sendingPin_Moving(object sender, MovingEventArgs e)
		{
		}

		void receivingPin_Moving(object sender, MovingEventArgs e)
		{
		}

		public static Pen SelectedPen
		{
			get
			{
				if (selectedPen == null)
				{
					selectedPen = new Pen(Color.FromKnownColor(KnownColor.ControlDarkDark), 2);
				}
				return selectedPen;
			}
		}

		public static Pen HighlightedPen
		{
			get
			{
				if (highlightedPen == null)
				{
					highlightedPen = new Pen(Color.FromKnownColor(KnownColor.MenuHighlight), 2);
				}
				return highlightedPen;
			}
		}

		public static Pen InactivePen
		{
			get
			{
				if (inactivePen == null)
				{
					inactivePen = new Pen(Color.FromKnownColor(KnownColor.ControlDarkDark), 1);
				}
				return inactivePen;
			}
		}

		public static Pen WidePen
		{
			get
			{
				if (widePen == null)
				{
					widePen = new Pen(Color.White, 12);
				}
				return widePen;
			}
		}

		public override bool IsDropTarget
		{
			get { return false; }
		}

		public override bool CanDrag
		{
			get { return false; }
		}

		public override bool HitTest(Point p)
		{
			if (sendingPin != null && receivingPin != null && path != null)
			{
				return path.IsVisible(p);
			}
			return false;
		}

		public Connection()
		{
			allConnections.Add(this);
		}

		public Connection(OutputPin sendingPin, InputPin receivingPin)
		{
			allConnections.Add(this);
			SendingPin = sendingPin;
			ReceivingPin = receivingPin;
			sendingPin.Deleting += new EventHandler<EventArgs>(sendingPin_Deleting);
			receivingPin.Deleting += new EventHandler<EventArgs>(receivingPin_Deleting);
		}

		void receivingPin_Deleting(object sender, EventArgs e)
		{
			ReceivingPin = null;
			Delete();
		}

		void sendingPin_Deleting(object sender, EventArgs e)
		{
			SendingPin = null;
			Delete();
		}

		public Node InputNode
		{
			get
			{
				return sendingPin != null ? sendingPin.ParentNode : null;
			}
		}

		public Node OutputNode
		{
			get
			{
				return receivingPin != null ? receivingPin.ParentNode : null;
			}
		}

		private Point[] DrawPoints()
		{
			Point[] rc = new Point[4];
			rc[0] = sendingPin.Position.Add(new Point(Pin.width - 1, Pin.height / 2));
			rc[3] = receivingPin.Position.Add(new Point(0, Pin.height / 2));
			Point mid = new Point(Math.Abs(rc[3].X - rc[0].X) / 2, 0);
			rc[1] = rc[0].Add(mid);
			rc[2] = rc[3].Subtract(mid);
			return rc;
		}

		public void Draw(Graphics g)
		{
			Pen p = SelectedPen;
			if (sendingPin != null && receivingPin != null)
			{
				if (sendingPin.ParentNode == null || receivingPin.ParentNode == null)
				{
					p = InactivePen;
				}
				if (highlight)
				{
					p = HighlightedPen;
				}
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				Point[] b = DrawPoints();
				if (path != null)
				{
					path.Dispose();
				}
				path = new GraphicsPath();
				path.AddBezier(b[0], b[1], b[2], b[3]);
				g.DrawPath(p, path);
				path.Widen(WidePen);
			}
			else
			{
				Debugger.Break();
			}
		}

		public override void Destroy()
		{
			base.Destroy();
			allConnections.Remove(this);
		}

		public override void Delete()
		{
			if(sendingPin != null)
			{
				sendingPin.Delete();
			}
			if(receivingPin != null)
			{
				receivingPin.Delete();
			}
			base.Delete();
		}

		public override void OnRightClick(ContextMenu m)
		{
			if (InputNode != null && OutputNode != null)
			{
				m.MenuItems.Add("Break", new EventHandler(BreakConnection));
			}
			PluginManager.AppendPluginsToContextMenu(m, new EventHandler(InsertFunction));
		}

		public void InsertFunction(object sender, EventArgs e)
		{
			Function func = (Function)(sender as MenuItem).Tag;

			if (func.graphInputs > 0)
			{
				Point pos = GraphManager.mouseClickPosition;

				Socket fromSocket = sendingPin.Socket;	// either of these can be null
				Socket toSocket = receivingPin.Socket;

				Point leftPos = SendingPin.Position;
				Point rightPos = ReceivingPin.Position;

				if (fromSocket != null)
				{
					if (fromSocket.Pin != null)
					{
						fromSocket.Pin.Socket = null;
					}
					fromSocket.Pin = null;
				}

				if (toSocket != null)
				{
					if (toSocket.Pin != null)
					{
						toSocket.Pin.Socket = null;
					}
					toSocket.Pin = null;
				}

				if (SendingPin != null)
				{
					SendingPin.Destroy();
					SendingPin = null;
				}
				if (ReceivingPin != null)
				{
					ReceivingPin.Destroy();
					ReceivingPin = null;
				}
				Destroy();

				OutputPin outputRightPin = new OutputPin(null);

				Node newNode = GraphManager.CreateFunctionNode(func, pos, outputRightPin);

				InputPin inputRightPin = new InputPin(null);

				OutputPin outputLeftPin = new OutputPin(null);
				InputPin inputLeftPin = new InputPin(newNode.Inputs[0]);

				Connection newFromConnection = new Connection(outputLeftPin, inputLeftPin);
				Connection newToConnection = new Connection(outputRightPin, inputRightPin);

				if (fromSocket != null)
				{
					fromSocket.Pin = outputLeftPin;
				}
				else
				{
					outputLeftPin.Position = leftPos;
				}

				if (toSocket != null)
				{
					toSocket.Pin = inputRightPin;
				}
				else
				{
					inputRightPin.Position = rightPos;
				}

				newNode.PropertyChanged += new EventHandler<EventArgs>(GraphManager.fnode_OnChanged);

				GraphManager.SelectedThing = newNode;

				newNode.Dirty = true;
				newNode.GetGraphic();
				newNode.RaiseChanged();
			}
		}

		void BreakConnection(object sender, EventArgs e)
		{
			if (InputNode != null && OutputNode != null)
			{
				OutputPin newSender = new OutputPin(null);
				InputPin newReceiver = new InputPin(null);
				Connection n = new Connection(newSender, ReceivingPin);
				ReceivingPin = newReceiver;
				newSender.Position = GraphManager.mouseClickPosition.Add(new Point(Pin.width, 0));
				newReceiver.Position = GraphManager.mouseClickPosition.Add(new Point(-Pin.width, 0));
			}
		}

		public void Save(XmlTextWriter stream)
		{
			int sendingNodeID = -1;
			int receivingNodeID = -1;
			int sendingSocketID = -1;
			int receivingSocketID = -1;
			Node sendingNode = null;
			Node receivingNode = null;
			if (SendingPin.Socket != null)
			{
				sendingNode = SendingPin.Socket.ParentNode;
				sendingSocketID = SendingPin.Socket.id;
			}
			if (ReceivingPin.Socket != null)
			{
				receivingNode = ReceivingPin.Socket.ParentNode;
				receivingSocketID = ReceivingPin.Socket.id;
			}
			if (sendingNode != null)
			{
				sendingNodeID = sendingNode.id;
			}
			if (receivingNode != null)
			{
				receivingNodeID = receivingNode.id;
			}
			stream.WriteStartElement("Connection");

			stream.WriteAttributeString("FromNode", sendingNodeID.ToString());
			stream.WriteAttributeString("FromSocket", sendingSocketID.ToString());
			stream.WriteAttributeString("ToNode", receivingNodeID.ToString());
			stream.WriteAttributeString("ToSocket", receivingSocketID.ToString());
			stream.WriteAttributeString("FromX", SendingPin.Position.X.ToString());
			stream.WriteAttributeString("FromY", SendingPin.Position.Y.ToString());
			stream.WriteAttributeString("ToX", ReceivingPin.Position.X.ToString());
			stream.WriteAttributeString("ToY", ReceivingPin.Position.Y.ToString());

			stream.WriteEndElement(); // Connection
		}
	}
}
