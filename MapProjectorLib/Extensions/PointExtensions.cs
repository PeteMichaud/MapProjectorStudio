using System;
using VectSharp;

namespace MapProjectorLib.Extensions
{
    public static class PointExtensions
    {

        public static double ManhattanDistance(this VectSharp.Point p1, VectSharp.Point p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
        }
    }
}
