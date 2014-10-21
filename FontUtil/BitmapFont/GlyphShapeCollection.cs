using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace FontUtil
{
	public class GlyphShapeCollection
	{
		public class GlyphShape
		{
			private float threshold;

			public List<PointF> points;
			public PointF lastPoint;

			public GlyphShape(float threshold)
			{
				this.threshold = Math.Max(threshold, 1);
				points = new List<PointF>();
			}

			public void AddPoint(PointF p)
			{
				if (points.Count == 0 || lastPoint.DistanceTo(p) > threshold)	// this not enough sometimes
				{
					lastPoint = p;
					points.Add(p);
				}
			}
		}

		private int penWidth;

		public List<GlyphShape> shapes;
		public Rectangle Rectangle;

		public GlyphShapeCollection(int penWidth)
		{
			this.penWidth = penWidth;
			Rectangle = new Rectangle();
			shapes = new List<GlyphShape>();
		}

		public GlyphShape AddShape()
		{
			GlyphShape shape = new GlyphShape(penWidth / 32.0f);
			shapes.Add(shape);
			return shape;
		}
	}

}
