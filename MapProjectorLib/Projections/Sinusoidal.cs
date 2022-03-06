using System;

namespace MapProjectorLib.Projections
{
    internal class Sinusoidal : CylindricalBase
    {
        double _m;

        // f(phi) = phi, g(phi) = Math.Cos(phi)
        protected override double GetMaxHeight(TransformParams tParams)
        {
            return ProjMath.PiOverTwo;
        }

        protected override double GetLat(double y)
        {
            // Latitude is just y
            _m = 1 / Math.Cos(y); // Cache the Math.Cos
            return y;
        }

        protected override double GetLong(double x)
        {
            return x * _m;
        }

        protected override (bool inBounds, PointD mappedPoint) GetXY(
            double phi, double lambda)
        {
            var x = lambda * Math.Cos(phi);
            var y = phi;

            return (true, new PointD(x,y));
        }
    }
}