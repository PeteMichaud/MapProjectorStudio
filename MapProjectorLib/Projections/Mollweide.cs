using System;

namespace MapProjectorLib.Projections
{
    internal class Mollweide : CylindricalBase
    {
        double _m;
        double _a;

        public override void Init(TransformParams tParams)
        {
            base.Init(tParams);
            var phi = tParams.p;
            CalcParams(phi, ref _a);
        }

        // f(phi) = 1/a  Math.Sin t, g(phi) = Math.Cos t, where t + 0.5  Math.Sin(2t) = 0.5 pi  Math.Sin(phi)
        protected override double GetMaxHeight(TransformParams tParams)
        {
            var a0 = 0.0;
            CalcParams(tParams.p, ref a0);
            return 1.0 / a0;
        }

        static void CalcParams(double phi, ref double a)
        {
            // Now we need to find a suitable t
            // Good old binary search - note we cavalierly fail to check if there's
            // a solution in the interval. Hell, we're going to terminate with something.
            double t = 0; // phi = 0;
            var t1 = Math.PI / 2; // phi = pi/2;
            var b = 0.5 * Math.PI * Math.Sin(phi);
            var epsilon = 1e-6;

            while (Math.Abs(t1 - t) > epsilon)
            {
                var t2 = (t + t1) / 2.0;
                var p = t2 + 0.5 * Math.Sin(2 * t2) - b;
                if (p <= 0.0) t = t2;
                else t1 = t2;
            }

            // t is our selected value
            a = Math.PI / (4 * ProjMath.Sqr(Math.Cos(t) / Math.Cos(phi)));
        }

        protected override double GetLat(double y)
        {
            // find t;
            var t = Math.Asin(y * _a);
            _m = 1 / Math.Cos(t);
            return Math.Asin((t + 0.5 * Math.Sin(2 * t)) * ProjMath.TwoOverPi);
        }

        protected override double GetLong(double x)
        {
            return _m * x;
        }

        protected override (bool inBounds, PointD mappedPoint) GetXY(
            double phi, double lambda)
        {
            double ProjectionEquation(double tProjEq)
            {
                return (tProjEq + 0.5 * Math.Sin(2 * tProjEq)) *
                    ProjMath.TwoOverPi - Math.Sin(phi);
            }

            var t = 0.0;
            if (ProjMath.FindRoot(
                -Math.PI / 2, Math.PI / 2, 1e-5, ref t, ProjectionEquation))
            {
                var y = Math.Sin(t) / _a;
                var x = lambda * Math.Cos(t);
                return (true, new PointD(x,y));
            }

            return (false, PointD.None);
        }
    }
}