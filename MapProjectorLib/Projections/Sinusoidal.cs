using System;

namespace MapProjectorLib.Projections
{
    internal class Sinusoidal : CylindricalBase
    {
        // f(phi) = phi, g(phi) = Math.Cos(phi)
        protected override double GetMaxHeight(TransformParams tParams)
        {
            return ProjMath.PiOverTwo;
        }

        protected override double GetLat(double _, double y)
        {
            return y;
        }

        protected override double GetLong(double x, double y)
        {
            var m = 1 / Math.Cos(y); 
            return x * m;
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