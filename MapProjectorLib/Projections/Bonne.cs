using System;

namespace MapProjectorLib.Projections
{
    internal class Bonne : Simple
    {
        double cotp;
        double p;

        public override void Init(TransformParams tParams)
        {
            base.Init(tParams);
            p = tParams.p;
            cotp = ProjMath.Cot(p);
        }

        public override double BasicScale(int width, int height)
        {
            return 2.0 * Math.PI / width;
        }

        public override bool ProjectSimple(
            double x, double y,
            ref double phi, ref double lambda)
        {
            var rho = Math.Sqrt(ProjMath.Sqr(x) + ProjMath.Sqr(cotp - y));

            if (p > 0)
            {
                phi = cotp + p - rho;
                lambda = rho * Math.Atan2(x, cotp - y) / Math.Cos(phi);
            } else if (p < 0)
            {
                phi = cotp + p + rho;
                lambda = rho * Math.Atan2(x, y - cotp) / Math.Cos(phi);
            } else
            {
                // Degenerate case - the  Math.Sinusoidal projection
                phi = y;
                lambda = x / Math.Cos(phi);
            }

            if (phi >= -ProjMath.PiOverTwo && phi <= +ProjMath.PiOverTwo &&
                lambda >= -Math.PI && lambda <= Math.PI)
                return true;

            return false;
        }

        public override bool ProjectInvSimple(
            double phi, double lambda, ref double x, ref double y)
        {
            if (p == 0.0)
            {
                x = lambda * Math.Cos(phi);
                y = phi;
            } else
            {
                var rho = cotp + p - phi;
                var e = lambda * Math.Cos(phi) / rho;
                x = rho * Math.Sin(e);
                y = cotp - rho * Math.Cos(e);
            }

            return true;
        }
    }
}