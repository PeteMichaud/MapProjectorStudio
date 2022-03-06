using System;

namespace MapProjectorLib.Projections
{
    internal class Hammer : Simple
    {
        public override double BasicScale(int width, int height)
        {
            return 2.0 / width;
        }

        protected override bool ProjectSimple(
            double x, double y,
            ref double phi, ref double lambda)
        {

            var z2 = 2 - ProjMath.Sqr(x) - ProjMath.Sqr(2 * y);
            var z = Math.Sqrt(z2);
            var t1 = 2 * y * z;
            if (t1 >= -1.0 && t1 <= 1.0)
            {
                phi = Math.Asin(t1);
                lambda = 2 * Math.Atan2(x * z, z2 - 1);
                if (lambda >= -Math.PI && lambda <= Math.PI) 
                    return true;
            }

            return false;
        }

        protected override (bool inBounds, PointD mappedPoint) ProjectInvSimple(
            double phi, double lambda)
        {
            var z = Math.Sqrt(1 + Math.Cos(phi) * Math.Cos(lambda / 2));
            var x = Math.Cos(phi) * Math.Sin(lambda / 2) / z;
            var y = Math.Sin(phi) / (2 * z);

            return (true, new PointD(x,y));
        }
    }
}