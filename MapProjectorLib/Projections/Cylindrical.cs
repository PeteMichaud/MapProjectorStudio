using System;

namespace MapProjectorLib.Projections
{
    internal class Cylindrical : CylindricalBase
    {
        protected override double GetLat(double _, double y)
        {
            return Math.Atan(y);
        }

        protected override double GetLong(double x, double _)
        {
            return x;
        }

        protected override (bool inBounds, PointD mappedPoint) GetXY(
            double phi, double lambda)
        {
            return (true, new PointD(
                lambda,
                Math.Tan(phi)
                ));
        }
    }
}