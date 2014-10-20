//! This does not work, do not use...

using System;
using System.Drawing;

namespace FontUtil
{
	public class SimpleRectanglePacker : RectanglePacker
	{
		public class Node
		{
			Node left;
			Node right;
			Rectangle rect;
			bool used;

			public Point Position
			{
				get
				{
					return rect.Location;
				}
			}

			public Size Dimensions
			{
				get
				{
					return rect.Size;
				}
			}

			public static Node root;

			private Node()
			{
				used = false;
				left = right = null;
			}

			public Node(Size pageSize) : this()
			{
				rect = new Rectangle(Point.Empty, pageSize);
			}

			public Node(Node other) : this()
			{
				rect = other.rect;
			}

			internal Node Insert(Size s)
			{
				// if we're not a leaf then
				if (left != null)
				{
					// try inserting into first then second child
					return left.Insert(s) ?? right.Insert(s);
				}
				else
				{
					// if there's already something here, return
					if (used)
					{
						return null;
					}

					// if it can't fit, return
					if (s.Width > rect.Width || s.Height > rect.Height)
					{
						return null;
					}

					// if it fits exactly, accept
					if (s.Width == rect.Width && s.Height == rect.Height)
					{
						used = true;
						return this;
					}

					// otherwise, split this node and create some kids
					left = new Node(this);
					right = new Node(this);

					// decide which way to split
					int widthRemainder = rect.Width - s.Width;
					int heightRemainder = rect.Height - s.Height;

					if (widthRemainder > heightRemainder)
					{
						left.rect.Width = widthRemainder;
						right.rect.X = rect.X + s.Width;
						right.rect.Width = rect.Width - s.Width;
					}
					else
					{
						left.rect.Height = heightRemainder;
						right.rect.Y = rect.Y + s.Height;
						right.rect.Height = rect.Height - s.Height;
					}

					// insert into first child we created
					return left.Insert(s);
				}
			}
		}

		Node root;

		public SimpleRectanglePacker(int width, int height) : base(width, height)
		{
			root = new Node(new Size { Width = width, Height = height });
		}

		public Node AddRectangle(int width, int height)
		{
			return root.Insert(new Size { Width = width, Height = height });
		}

		public override bool TryPack(int rectangleWidth, int rectangleHeight, out Point placement)
		{
			Node n = root.Insert(new Size { Width = rectangleWidth, Height = rectangleHeight });
			if (n != null)
			{
				placement = n.Position;
				return true;
			}
			else
			{
				placement = new Point { X = 0, Y = 0 };
				return false;
			}
		}
	}
}