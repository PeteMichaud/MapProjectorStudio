using System;

namespace MapProjectorLib.Projections
{
    internal class Sinusoidal2 : CylindricalBase
    {
        double _a;
        double _b;
        double _k;

        // f(phi) = bt, g(phi) = (cos(t) + a)/(a+1),
        // where  Math.Sin(t) + at = k(a+1)sin(phi)
        // and b + (a * pi)/2 = k*(a+1)

        public override void Init(TransformParams tParams)
        {
            base.Init(tParams);
            _a = tParams.a;
            (_k, _b) = CalcParams(tParams.a, tParams.p);
        }

        static (double k, double b) CalcParams(double a, double phi)
        {
            // Find the t value for phi
            double t1 = 0;
            var t2 = ProjMath.PiOverTwo;
            const double epsilon = 1e-6;
            double k0 = 0.0, b0 = 0.0;

            while (Math.Abs(t2 - t1) > epsilon)
            {
                var t0 = (t1 + t2) / 2.0;
                k0 = ProjMath.Sqr(
                    (Math.Cos(t0) + a) / ((a + 1) * Math.Cos(phi)));
                b0 = k0 * (a + 1) / (1 + a * Math.PI / 2.0);
                var p0 = b0 * Math.Sin(t0) + a * t0 -
                         k0 * (a + 1) * Math.Sin(phi);
                if (p0 <= 0.0)
                    t1 = t0;
                else
                    t2 = t0;
            }

            return (k0, b0);
        }

        protected override double GetMaxHeight(TransformParams tParams)
        {
            (double _, double b0) = CalcParams(tParams.a, tParams.p);
            return b0 * ProjMath.PiOverTwo;
        }

        protected override double GetLat(double x, double y)
        {
            var t = y / _b;
            var phi = Math.Asin(_b * (Math.Sin(t) + _a * t) / (_k * (_a + 1)));
            return phi;
        }

        protected override double GetLong(double x, double y)
        {
            var t = y / _b;
            var m = (_a + 1) / (Math.Cos(t) + _a);
            return m * x;
        }

        protected override (bool inBounds, PointD mappedPoint) GetXY(
            double phi, double lambda)
        {
            var t = 0.0;

            double ProjectionEquation(double tProjEq)
            {
                return _b * (Math.Sin(tProjEq) + _a * tProjEq) / (_k * (_a + 1)) -
                       Math.Sin(phi);
            }

            if (ProjMath.FindRoot(
                -Math.PI / 2, Math.PI / 2, 1e-5, ref t, ProjectionEquation))
            {
                var y = _b * t;
                var x = lambda * (Math.Cos(t) + _a) / (_a + 1);

                return (true, new PointD(x,y));
            }

            return (false, PointD.None);
        }
    }
}