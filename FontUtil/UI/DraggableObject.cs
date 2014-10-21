using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace FontUtil
{
	public class MovingEventArgs : EventArgs
	{
	}

	public abstract class DraggableObject : UIObject
	{
		public event EventHandler<MovingEventArgs> Moving;

		private Point position;
		private Size size;
		private Point dragOffset;

		public override bool HitTest(Point p)
		{
			return Rectangle.Contains(p);
		}

		public sealed override bool CanDrag
		{
			get
			{
				return true;
			}
		}

		public override void OnLeftClick(Point pos)
		{
			dragOffset = pos.Subtract(Position);
		}

		public override bool DropTest(UIObject other)
		{
			DraggableObject d = other as DraggableObject;
			if (d != null)
			{
				return Rectangle.IntersectsWith(d.Rectangle);
			}
			return false;
		}

		public override bool IsDropTarget
		{
			get { return true; }
		}

		public virtual bool CanBeDropped
		{
			get { return true; }
		}

		public virtual Point Position
		{
			get
			{
				return position;
			}
			set
			{
				Point o = position;
				position = GraphManager.ConstrainPoint(value, Size);
				if (o.X != position.X || o.Y != position.Y)
				{
					RaiseMoving();
				}
			}
		}

		protected void RaiseMoving()
		{
			if (Moving != null)
			{
				Moving(this, new MovingEventArgs());
			}
		}

		public virtual Size Size
		{
			get
			{
				return size;
			}
			set
			{
				size = value;
			}
		}

		public Rectangle Rectangle
		{
			get
			{
				return new Rectangle(Position, Size);
			}
		}

		public virtual DraggableObject BeginDrag()
		{
			return this;
		}

		public virtual void EndDrag()
		{
		}

		public override void DoDrag(Point pos)
		{
			Position = pos.Subtract(dragOffset);
		}

		// please draw yourself into this graphics context
		public virtual void Draw(Graphics graphics)
		{
		}

		public DraggableObject(Size size)
			: base()
		{
			Size = size;
		}
	}
}
