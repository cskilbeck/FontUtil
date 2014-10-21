using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FontUtil
{
	public abstract class Socket : DraggableObject
	{
		const int width = Pin.width;
		const int height = Pin.width;

		private Pin pin;
		public readonly Node ParentNode;
		public Point Offset;
		public int id;

		public static List<Socket> allSockets = new List<Socket>();

		public override int SelectionOrder
		{
			get { return 2; }
		}

		public virtual Pin Pin
		{
			get
			{
				return pin;
			}
			set
			{
				if(value != pin)
				{
					if(pin != null)
					{
						pin.Deleting -= pin_Deleting;
					}

					pin = value;

					if (pin != null)
					{
						pin.Deleting += new EventHandler<EventArgs>(pin_Deleting);
						pin.Socket = this;
					}
				}
			}
		}

		public Connection Connection
		{
			get
			{
				return pin != null ? pin.Connection : null;
			}
		}

		public override bool CanBeDropped
		{
			get { return false; }
		}

		void pin_Deleting(object sender, EventArgs e)
		{
			Pin = null;
		}

		public Socket(Point offset, Node parent)
			: base(new Size(Pin.width, Pin.height))
		{
			allSockets.Add(this);
			Offset = offset;
			ParentNode = parent;
			ParentNode.Moving += new EventHandler<MovingEventArgs>(ParentNode_Moving);
		}

		public override void Destroy()
		{
			base.Destroy();
			allSockets.Remove(this);
		}

		public override void Drop(UIObject what)
		{
			if (what is Pin && Pin == null)
			{
				Pin = what as Pin;
			}
		}

		void ParentNode_Moving(object sender, MovingEventArgs e)
		{
			Node node = sender as Node;

			Debug.Assert(node != null);
			if(node != null)
			{
				Position = node.Position.Add(Offset);
			}
		}

		private static Pen highlightedPen;
		private static Pen selectedPen;
		private static Pen inactivePen;

		private static Pen HighlightedPen
		{
			get
			{
				if (highlightedPen == null)
				{
					highlightedPen = new Pen(Color.Yellow, 2);
				}
				return highlightedPen;
			}
		}

		private static Pen SelectedPen
		{
			get
			{
				if (selectedPen == null)
				{
					selectedPen = new Pen(Color.BlueViolet, 2);
				}
				return selectedPen;
			}
		}

		private static Pen InactivePen
		{
			get
			{
				if (inactivePen == null)
				{
					inactivePen = new Pen(Color.Green, 2);
				}
				return inactivePen;
			}
		}

		public override void Draw(Graphics graphics)
		{
			if (Pin == null)
			{
				graphics.SmoothingMode = SmoothingMode.AntiAlias;
				Pen p = highlight ? HighlightedPen : Selected ? SelectedPen : InactivePen;
				graphics.DrawPolygon(p, Pin.GetDrawPoints(Position));
			}
		}
	}
}
