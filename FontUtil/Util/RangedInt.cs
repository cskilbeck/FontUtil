using System;
using System.Drawing;
using System.Collections.Generic;

namespace FontUtil
{
	public interface IMinMax<T>
	{
		T Min(T a, T b);
		T Max(T a, T b);
	}

	public class Ranged<T>
	{
		private T _value;
		private IMinMax<T> MinMax;

		public T Min { get; private set; }
		public T Max { get; private set; }

		public T Value
		{
			get { return _value; }
			set
			{
				_value = MinMax.Min(Max, MinMax.Max(Min, value));
			}
		}

		public static implicit operator T(Ranged<T> r)
		{
			return r.Value;
		}

		public Ranged(T min, T max, T val, IMinMax<T> minMax)
		{
			Min = min;
			Max = max;
			MinMax = minMax;
			_value = val;
		}
	}

	public class IntMinMax : IMinMax<int>
	{
		public int Min(int a, int b) { return Math.Min(a, b); }
		public int Max(int a, int b) { return Math.Max(a, b); }
	}

	public class FloatMinMax : IMinMax<float>
	{
		public float Min(float a, float b) { return Math.Min(a, b); }
		public float Max(float a, float b) { return Math.Max(a, b); }
	}

	public class PointMinMax : IMinMax<Point>
	{
		public Point Min(Point a, Point b) { return new Point(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y)); }
		public Point Max(Point a, Point b) { return new Point(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y)); }
	}

	public class PointFMinMax : IMinMax<PointF>
	{
		public PointF Min(PointF a, PointF b) { return new PointF(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y)); }
		public PointF Max(PointF a, PointF b) { return new PointF(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y)); }
	}

	public class RangedInt : Ranged<int>
	{
		public RangedInt(int min, int max, int val) : base(min, max, val, new IntMinMax())
		{
		}
	}

	public class RangedFloat : Ranged<float>
	{
		public RangedFloat(float min, float max, float val) : base(min, max, val, new FloatMinMax())
		{
		}
	}

	public class RangedPointF : Ranged<PointF>
	{
		public RangedPointF(PointF min, PointF max, PointF val) : base(min, max, val, new PointFMinMax())
		{
		}
	}
}
