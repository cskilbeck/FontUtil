using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace FontUtil
{
	public abstract class Pin : DraggableObject
	{
		public enum Direction
		{
			Input,
			Output
		}

		public const int width = 13;
		public const int height = 15;

		private Connection _connection;
		private Socket _socket;

		public abstract Pin AttachTo(Node node);
		public abstract void Detach(Node node);
		public abstract void Detach(Connection connection);
		public abstract void Detach(Socket socket);

		public override int SelectionOrder
		{
			get { return 3; }
		}

		public abstract Direction direction
		{
			get;
		}

		public static List<Pin> allPins = new List<Pin>();

		public static Point HalfSize
		{
			get
			{
				return new Point(width / 2, height / 2);
			}
		}

		public Node ParentNode
		{
			get
			{
				return Socket != null ? Socket.ParentNode : null;
			}
		}

		public virtual Socket Socket
		{
			get
			{
				return _socket;
			}
			set
			{
				if (_socket != value)
				{
					if (_socket != null)
					{
						_socket.Moving -= OnSocketMoving;
						_socket.Deleting -= OnSocketDelete;
					}

					_socket = value;

					if (_socket != null)
					{
						Position = _socket.Position;
						_socket.Moving += new EventHandler<MovingEventArgs>(OnSocketMoving);
						_socket.Deleting += new EventHandler<EventArgs>(OnSocketDelete);
					}
				}
			}
		}

		void OnSocketMoving(object sender, MovingEventArgs e)
		{
			Position = (sender as Socket).Position;
		}

		void OnSocketDelete(object sender, EventArgs e)
		{
			Delete();
			Socket = null;
		}

		public override bool DropTest(UIObject other)
		{
			if (base.DropTest(other))
			{
				if (ParentNode == null && other is Pin)
				{
					Pin dragPin = other as Pin;
					if (dragPin.ParentNode == null && dragPin.direction != direction)
					{
						return true;
					}
				}
			}
			return false;
		}

		public override void OnRightClick(ContextMenu m)
		{
			if (ParentNode == null)
			{
				PluginManager.AppendPluginsToContextMenu(m, NewNode);
			}
		}

		void NewNode(object sender, EventArgs e)
		{
			GraphManager.NewFunction((Function)(sender as MenuItem).Tag, this);
		}

		public virtual Connection Connection
		{
			get
			{
				return _connection;
			}
			set
			{
				if (_connection != value)
				{
					if (_connection != null)
					{
						_connection.Deleting -= OnConnectionDelete;
					}

					_connection = value;

					if (_connection != null)
					{
						// only an input pin cares about the connection raising a changed event...
						_connection.Deleting += new EventHandler<EventArgs>(OnConnectionDelete);
					}
				}
			}
		}

		void OnConnectionDelete(object sender, EventArgs e)
		{
			if (direction == Direction.Input && ParentNode != null)
			{
				Connection = null;
			}
			else
			{
				Delete();
			}
		}

		public bool Floating
		{
			get
			{
				return ParentNode == null;
			}
		}

		public override void Drop(UIObject what)
		{
			if (what is Node)
			{
				AttachTo(what as Node);
			}
			else if (what is Pin)
			{
				Pin dragPin = what as Pin;

				if (ParentNode != null && Connection == null)
				{
					Connection = dragPin.Connection;
					dragPin.Connection = null;
					dragPin.Delete();
					if (Connection != null)
					{
						switch (direction)
						{
							case Direction.Input:
								Connection.ReceivingPin = (InputPin)this;
								break;

							case Direction.Output:
								Connection.SendingPin = (OutputPin)this;
								break;
						}
					}
				}
			}
		}

		public void Drag(Point pos)
		{
			Position = pos;
		}

		public Pin(Socket socket)
			: base(new Size(width, height))
		{
			Socket = socket;
			allPins.Add(this);
		}

		public abstract Pin OtherPin
		{
			get;
		}

		public Node OtherNode
		{
			get
			{
				Pin o = OtherPin;
				return o != null ? o.ParentNode : null;
			}
		}

		// Never delete an InputPin which has a parentNode
		public override void Destroy()
		{
			base.Destroy();
			allPins.Remove(this);
		}

		// what sort of Pin am I? a transform or a Graphic?

		public static Point[] GetDrawPoints(Point position)
		{
			int x = position.X;
			int y = position.Y;
			Point[] points = new Point[3];
			points[0] = new Point(x, y);
			points[1] = new Point(x + width, y + height / 2);
			points[2] = new Point(x, y + height);
			return points;
		}

		public override void Draw(Graphics graphics)
		{
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			graphics.FillPolygon(highlight ? Brushes.Yellow : Brushes.Red, GetDrawPoints(Position));
		}
	}
}
