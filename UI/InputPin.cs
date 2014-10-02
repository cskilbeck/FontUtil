using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace FontUtil
{
	public class InputPin : Pin
	{
		public InputPin(Socket socket)
			: base(socket)
		{
			if (socket != null)
			{
				socket.Pin = this;
			}
		}

		public override Connection Connection
		{
			get
			{
				return base.Connection;
			}
			set
			{
				if (Connection != value)
				{
					if (Connection != null)
					{
						Connection.PropertyChanged -= connection_Changed;
					}

					base.Connection = value;

					if (Connection != null)
					{
						// only an input pin cares about the connection raising a changed event...
						Connection.PropertyChanged += new EventHandler<EventArgs>(connection_Changed);
					}
				}
			}
		}

		void connection_Changed(object sender, EventArgs e)
		{
			RaiseChanged();
		}

		public override void Detach(Node node)
		{
			Debug.Assert(ParentNode == node);
			node.RemoveInput(this);
		}

		public override void Detach(Connection connection)
		{
			Connection.ReceivingPin = null;
			Connection = null;
		}

		public override void Detach(Socket socket)
		{
			Socket.Pin = null;
			Socket = null;
		}

		public override Pin.Direction direction
		{
			get
			{
				return Pin.Direction.Input;
			}
		}

		public override Pin OtherPin
		{
			get
			{
				return Connection == null ? null : Connection.SendingPin;
			}
		}

		public override Pin AttachTo(Node node)
		{
			Debug.Assert(node.CanCurrentlyAcceptInput);
			Debug.Assert(ParentNode == null);

			Connection c = Connection;
			InputSocket n = node.FirstFreeInputSocket();

			if (c != null && n != null)
			{
				n.Pin = this;
			}

			return this;
		}

		public override void Drop(UIObject what)
		{
			if (what is Node)
			{
				AttachTo(what as Node);
			}
			else if (what is Pin)
			{
				if (what is Socket)
				{
					Socket = what as Socket;
				}
				else if (what is Pin)
				{
					Pin dragPin = what as Pin;

					if (dragPin.ParentNode == null && dragPin.direction != direction)
					{
						OutputPin o = Connection.SendingPin;
						InputPin i = dragPin.Connection.ReceivingPin;

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
		}

		public override string Name
		{
			get
			{
				return "Input";
			}
		}

		public override bool ShowDeleteMenuItem
		{
			get
			{
				return false;
			}
		}

		public override DraggableObject BeginDrag()
		{
			if (ParentNode != null && OtherNode != null)
			{
				Detach(Socket);
				Socket = null;
				return this;
			}
			else if (ParentNode == null)
			{
				return this;
			}
			return null;
		}

		public override void OnRightClick(ContextMenu m)
		{
			if (ParentNode == null)
			{
				m.MenuItems.Add("New Layer", new EventHandler(newLayer_Click));
			}
			base.OnRightClick(m);
		}

		void newLayer_Click(object sender, EventArgs e)
		{
			GraphManager.NewLayer(this);
		}
	}
}
