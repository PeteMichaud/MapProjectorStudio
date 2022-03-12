using System;

namespace MapProjectorLib.Projections
{
    internal class Stereographic : PolarBase
    {
        protected override double GetPhi(double r)
        {
            var sr = Math.Pow(r, k);
            return Math.Asin((1 - ProjMath.Sqr(sr)) / (1 + ProjMath.Sqr(sr)));
        }

        protected override (bool useR, double r) GetR(double phi)
        {
            var r = Math.Cos(phi) / (1 + Math.Sin(phi));
            r = Math.Pow(r, 1 / k);
            return (true, r);
        }

        //this is a dirty hack. 
        //the problem is that for Azimuthal and Stereographic projections
        //when they are rotated precisely -45 or 45 degrees, they JUST
        //eek out of bounds and cause an error
        //this class is so that ONLY those projections get a value guard
        //when lambda = PI or -PI 
        const double epsilon = 1e-6;
        public override (bool inProjectionBounds, double x1, double y1, double z1, double phi, double lambda)
        Project(
           TransformParams tParams,
           double x, double y,
           double x0, double y0, double z0,
           double phi, double lambda)
        {
            (var inBounds, var x1, var y1, var z1, var phi0, var lambda0) = base.Project(tParams, x, y, x0, y0, z0, phi, lambda);

            return (inBounds, x1, y1, z1, phi0, ValueGuard(inBounds, lambda0));
        }

        double ValueGuard(bool doGuard, double lambda)
        {
            if (!doGuard) return lambda;

            if (ProjMath.AboutEqual(lambda, Math.PI)
                || ProjMath.AboutEqual(lambda, -Math.PI))
            {
                lambda += epsilon * -Math.Sign(lambda);
            }

            return lambda;
        }

    }
}