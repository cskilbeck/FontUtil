using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace FontUtil
{
	public class OutputPin : Pin
	{
		public OutputPin(Socket socket)
			: base(socket)
		{
			if (socket != null)
			{
				socket.Pin = this;
			}
		}

		public override Pin.Direction direction
		{
			get
			{
				return Pin.Direction.Output;
			}
		}

		public override string Name
		{
			get
			{
				return "Output";
			}
		}

		public override string Caption
		{
			get
			{
				return "Output";
			}
		}

		public override Pin AttachTo(Node node)
		{
			node.AttachOutput(this);
			return this;
		}

		public override void Detach(Node node)
		{
			Debug.Assert(ParentNode == node);
			node.RemoveOutput(this);
		}

		public override void Detach(Connection connection)
		{
			Connection.SendingPin = null;
			Connection = null;
		}

		public override void Detach(Socket socket)
		{
			Socket.Pin = null;
			Socket = null;
		}

		public override DraggableObject BeginDrag()
		{
			if (Socket != null)
			{
				if (OtherNode != null)
				{
					Socket s = Socket;
					ParentNode.RemoveOutput(this);
					s.Delete();
				}
				else
				{
					return null;
				}
			}
			return this;
		}

		public override Socket Socket
		{
			get
			{
				return base.Socket;
			}
			set
			{
				if (base.Socket != value)
				{
					if (base.Socket != null)
					{
						base.Socket.PropertyChanged -= Socket_Changed;
					}

					base.Socket = value;

					if (base.Socket != null)
					{
						base.Socket.PropertyChanged += new EventHandler<EventArgs>(Socket_Changed);
					}
				}
			}
		}

		void Socket_Changed(object sender, EventArgs e)
		{
			RaiseChanged();
		}

		public override void Delete()
		{
			Node n = ParentNode;

			OutputSocket s = (OutputSocket)Socket;

			Pin otherPin = OtherPin;
			if (otherPin != null && otherPin.ParentNode == null)
			{
				Connection.Destroy();
				otherPin.Destroy();
			}

			if (s.ParentNode != null)
			{
				s.ParentNode.RemoveOutputSocket(s);
			}
			s.Destroy();

			base.Delete();
			Socket = null;

			if (n != null)
			{
				n.RefreshPinPositions();
			}
		}

		public override void Drop(UIObject what)
		{
			if (what is Pin)
			{
				Pin dragPin = what as Pin;

				if (dragPin.ParentNode == null && dragPin.direction != direction)
				{
					InputPin i = Connection.ReceivingPin;
					OutputPin o = dragPin.Connection.SendingPin;

					Connection oldConnection1 = Connection;
					Connection oldConnection2 = dragPin.Connection;

					dragPin.Connection = null;
					Connection = null;

					Connection c = new Connection(o, i);

					oldConnection1.Destroy();	// just fry them
					oldConnection2.Destroy();

					dragPin.Destroy();
					Destroy();
				}
			}
		}

		public override Pin OtherPin
		{
			get
			{
				return Connection == null ? null : Connection.ReceivingPin;
			}
		}

		public override bool ShowDeleteMenuItem
		{
			get
			{
				return ParentNode != null;
			}
		}

		public override void OnRightClick(ContextMenu m)
		{
			if (ParentNode == null)
			{
				m.MenuItems.Add("New Source", new EventHandler(newSource_Click));
			}
			base.OnRightClick(m);
		}

		void newSource_Click(object sender, EventArgs e)
		{
			GraphManager.NewSource(this);
		}
	}
}
