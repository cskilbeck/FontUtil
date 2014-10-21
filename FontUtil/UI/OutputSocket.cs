using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FontUtil
{
	public class OutputSocket : Socket
	{
		public OutputSocket(Point offset, Node parent)
			: base(offset, parent)
		{
			ParentNode.AddOutputSocket(this);
			ParentNode.PropertyChanged += new EventHandler<EventArgs>(ParentNode_Changed);
		}

		void ParentNode_Changed(object sender, EventArgs e)
		{
			RaiseChanged();
		}

		public override void Destroy()
		{
			if (ParentNode != null)
			{
				ParentNode.PropertyChanged -= ParentNode_Changed;
			}
			base.Destroy();
		}

		public void parent_Changed(object sender, EventArgs e)
		{
			RaiseChanged();
		}

		public override string Name
		{
			get
			{
				return "Output";
			}
		}

		public override void Delete()
		{
			if (ParentNode != null)
			{
				ParentNode.RemoveOutputSocket(this);
			}
			Destroy();
		}

		public override DraggableObject BeginDrag()
		{
			if (Pin == null)
			{
				OutputPin o = new OutputPin(this);
				InputPin i = new InputPin(null);
				Connection c = new Connection(o, i);
				Pin = o;
				return i;
			}
			else
			{
				return null;
			}
		}
	}

}
