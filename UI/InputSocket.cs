using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FontUtil
{
	public class InputSocket : Socket
	{
		public Parameter parameter;

		public InputSocket(Point offset, Node parent)
			: base(offset, parent)
		{
			parent.AddInputSocket(this);
		}

		public override DraggableObject BeginDrag()
		{
			if (Pin == null)
			{
				InputPin i = new InputPin(this);
				OutputPin o = new OutputPin(null);
				Connection c = new Connection(o, i);
				i.Socket = this;
				return o;
			}
			else
			{
				return null;
			}
		}

		public override Pin Pin
		{
			get
			{
				return base.Pin;
			}
			set
			{
				if (value != base.Pin)
				{
					if (base.Pin != null)
					{
						base.Pin.PropertyChanged -= Pin_Changed;
					}
					base.Pin = value;
					if (base.Pin != null)
					{
						base.Pin.PropertyChanged += new EventHandler<EventArgs>(Pin_Changed);
					}
					if (ParentNode != null)
					{
						ParentNode.Dirty = true;
					}
				}
			}
		}

		void Pin_Changed(object sender, EventArgs e)
		{
			RaiseChanged();
		}

		void inputPin_Changed(object sender, EventArgs e)
		{
			RaiseChanged();	// pass it on...
		}

		public override string Name
		{
			get
			{
				return "Input";
			}
		}
	}

}
