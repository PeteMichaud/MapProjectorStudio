using System;

namespace MapProjectorLib.Projections
{
    internal class Mercator : CylindricalBase
    {
        protected override double GetLat(double _, double y)
        {
            var k = Math.Exp(Math.Abs(y));
            var phi = Math.Acos(2 * k / (k * k + 1));
            if (y < 0) phi = -phi;

            return phi;
        }

        protected override double GetLong(double x, double _)
        {
            return x;
        }

        protected override (bool inBounds, PointD mappedPoint) GetXY(
            double phi, double lambda)
        {
            var x = lambda;
            var y = Math.Log(
                (1 + Math.Sin(Math.Abs(phi))) / Math.Cos(Math.Abs(phi)));
            if (phi < 0) y = -y;

            return (true, new PointD(x,y));
        }
    }
}