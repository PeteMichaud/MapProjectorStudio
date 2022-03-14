using System;


namespace MapProjectorLib
{
    public struct PointD
    {
        public double X;
        public double Y;
        public static PointD None = new PointD(double.MinValue, double.MinValue);

        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double ManhattanDistance(PointD p2)
        {
            return ManhattanDistance(this, p2);
        }

        public static double ManhattanDistance(PointD p1, PointD p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
        }

    }
}
