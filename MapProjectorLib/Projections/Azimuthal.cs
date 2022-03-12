using System;

namespace MapProjectorLib.Projections
{
    internal class Azimuthal : PolarBase
    {
        public override (int w, int h) AdjustSize(
            int w, int h, TransformParams tParams)
        {
            var w1 = (int) (tParams.Scale * h);
            if (w1 < w) w = w1;
            return (w, h);
        }

        protected override double GetPhi(double r)
        {
            return Math.PI * (0.5 - r);
        }

        protected override (bool useR, double r) GetR(double phi)
        {
            return (true, 0.5 - phi / Math.PI);
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