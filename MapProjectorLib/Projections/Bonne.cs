using System;

namespace MapProjectorLib.Projections
{
    internal class Bonne : Simple
    {
        double _cotp;
        double _p;

        public override void Init(TransformParams tParams)
        {
            base.Init(tParams);
            _p = tParams.p;
            _cotp = ProjMath.Cot(_p);
        }

        public override double BasicScale(int width, int height)
        {
            return 2.0 * Math.PI / width;
        }

        protected override (bool result, double phi, double lambda) ProjectSimple(
            double x, double y,
            double phi, double lambda)
        {
            var rho = Math.Sqrt(ProjMath.Sqr(x) + ProjMath.Sqr(_cotp - y));

            if (_p > 0)
            {
                phi = _cotp + _p - rho;
                lambda = rho * Math.Atan2(x, _cotp - y) / Math.Cos(phi);
            } 
            else if (_p < 0)
            {
                phi = _cotp + _p + rho;
                lambda = rho * Math.Atan2(x, y - _cotp) / Math.Cos(phi);
            } 
            else
            {
                // Degenerate case - the  Math.Sinusoidal projection
                phi = y;
                lambda = x / Math.Cos(phi);
            }

            if (phi >= -ProjMath.PiOverTwo && phi <= +ProjMath.PiOverTwo &&
                lambda >= -Math.PI && lambda <= Math.PI)
                return (true, phi, lambda);

            return (false, phi, lambda);
        }

        protected override (bool inBounds, PointD mappedPoint) ProjectInvSimple(
            double phi, double lambda)
        {
            double x;
            double y;

            if (_p == 0.0)
            {
                x = lambda * Math.Cos(phi);
                y = phi;
            } else
            {
                var rho = _cotp + _p - phi;
                var e = lambda * Math.Cos(phi) / rho;
                x = rho * Math.Sin(e);
                y = _cotp - rho * Math.Cos(e);
            }

            return (true, new PointD(x,y));
        }
    }
}