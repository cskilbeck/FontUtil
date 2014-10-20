using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace FontUtil
{
	/// <summary>
	/// Abstract Base Class for all UIObjects in the system
	/// </summary>
	/// <remarks>
	/// Provides functionality for being selected, drag, drop etc etc
	/// </remarks>
	public abstract class UIObject
	{
		private bool selected;
		private bool deleted;

		/// <summary>
		/// This <see cref="UIObject"/> is being Deleted by the user
		/// </summary>
		public event EventHandler<EventArgs> Deleting;

		/// <summary>
		/// PropertyChanged is raised when this UIObject requires a rebuild of it's descendents in the graph
		/// </summary>
		public event EventHandler<EventArgs> PropertyChanged;

		/// <summary>
		/// Set to true when this UIObject is being highlighted in the <see cref="GraphManager"/>
		/// </summary>
		public bool highlight;

		public static List<UIObject> allUIObjects = new List<UIObject>();

		public abstract bool HitTest(Point p);

		/// <summary>
		/// Determine whether this <see cref="UIObject"/> can be dragged with the mouse
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// <value>
		/// bool: the UIObject can be dragged if true
		/// </value>
		public abstract bool CanDrag
		{
			get;
		}

		public virtual bool ShowDeleteMenuItem
		{
			get
			{
				return true;
			}
		}

		public bool Selected
		{
			get
			{
				return selected;
			}
			set
			{
				selected = value;
			}
		}

		public void Select()
		{
			selected = true;
			OnSelect();
		}

		protected virtual void OnSelect()
		{
		}

		public virtual int SelectionOrder
		{
			get
			{
				return 0;
			}
		}

		public void DeSelect()
		{
			selected = false;
			OnDeSelect();
		}

		protected virtual void OnDeSelect()
		{
		}

		public UIObject()
		{
			allUIObjects.Add(this);
			allUIObjects = (allUIObjects.OrderBy(x => x.SelectionOrder)).ToList();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pos"></param>
		public virtual void MouseEnter(Point pos)
		{
		}

		// mouse has stopped hovering over it
		public virtual void MouseLeave(Point pos)
		{
		}

		// left mouse has been clicked on it
		public virtual void OnLeftClick(Point pos)
		{
		}

		public virtual void DoDrag(Point pos)
		{
		}

		public virtual void EndDrag(Point pos)
		{
		}

		/// <summary>
		/// right mouse has been clicked in your rectangle, a ContextMenu is required
		/// </summary>
		/// <param name="ClickPoint">Mouse click position</param>
		/// <returns>A ContextMenu which will be displayed immediately</returns>
		public virtual void OnRightClick(ContextMenu m)
		{
		}

		void RaiseDeleting()
		{
			if (Deleting != null)
			{
				Deleting(this, null);
			}
		}

		public virtual void Destroy()
		{
			allUIObjects.Remove(this);
			allUIObjects = allUIObjects.OrderBy(x => x.SelectionOrder).ToList();
		}

		public virtual void Delete()
		{
			if (!deleted)
			{
				deleted = true;
				RaiseDeleting();
			}
			Destroy();
		}

		public abstract bool IsDropTarget
		{
			get;
		}

		public virtual void Drop(UIObject what)
		{
		}

		public virtual bool DropTest(UIObject other)
		{
			return false;
		}

		public UIObject FindDropTarget()
		{
			UIObject found = null;

			foreach (UIObject t in allUIObjects)
			{
				if (t != this)
				{
					if (t.IsDropTarget)
					{
						if (!(t is Pin && (t as Pin).ParentNode == this))
						{
							if (t.DropTest(this))
							{
								found = t;
							}
						}
					}
				}
			}
			return found;
		}

		public static UIObject FindThing(Point pos)
		{
			UIObject found = null;
			foreach (UIObject t in allUIObjects)
			{
				if (t.HitTest(pos))
				{
					found = t;
				}
			}
			return found;
		}

		public virtual void RaiseChanged()
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, null);
			}
			else
			{
			}
		}

		public virtual string Description
		{
			get
			{
				return "UI Object";
			}
		}

		public virtual string Caption
		{
			get
			{
				return Description;
			}
		}

		public virtual string Name
		{
			get
			{
				return "UI Object";
			}
		}

	}
}
