using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapProjectorLib.Projections
{
    class Sinusoidal2 : CylindricalBase
    {
        private double a;
        private double b;
        private double k;
        private double _m;


        // f(phi) = bt, g(phi) = (cos(t) + a)/(a+1), where  Math.Sin(t) + at = k(a+1)sin(phi)
        // and b + (a * pi)/2 = k*(a+1)

        protected override double GetMaxHeight(TransformParams tParams)
        {
            double k0 = 0.0, b0 = 0.0;
            CalcParams(tParams.a, tParams.p, ref k0, ref b0);
            return b0 * ProjMath.PiOverTwo;
        }

        private void CalcParams(double a, double phi, ref double k, ref double b)
        {
            // Find the t value for phi
            double t1 = 0;
            double t2 = ProjMath.PiOverTwo;
            double epsilon = 1e-6;
            double k0 = 0.0, b0 = 0.0;
            
            while (Math.Abs(t2 - t1) > epsilon)
            {
                double t0 = (t1 + t2) / 2.0;
                k0 = ProjMath.Sqr((Math.Cos(t0) + a) / ((a + 1) * Math.Cos(phi)));
                b0 = (k0 * (a + 1)) / (1 + a * Math.PI / 2.0);
                double p0 = b0 * Math.Sin(t0) + a * t0 - k0 * (a + 1) * Math.Sin(phi);
                if (p0 <= 0.0) 
                    t1 = t0;
                else 
                    t2 = t0;
            }

            k = k0; 
            b = b0;
        }

        public override void Init(TransformParams tParams)
        {
            base.Init(tParams);
            a = tParams.a;
            CalcParams(tParams.a, tParams.p, ref k, ref b);
        }

        protected override double GetLat(double y)
        {
            double t = y / b;
            double phi = Math.Asin((b * (Math.Sin(t) + a * t)) / (k * (a + 1)));
            _m = (a + 1) / (Math.Cos(t) + a);

            return phi;
        }

        protected override double GetLong(double x)
        {
            return _m * x;
        }

    protected override bool GetXY(double phi, double lambda, ref double x, ref double y)
    {
        double t = 0.0;

        double ProjectionEquation(double tProjEq)
        {
            return b * (Math.Sin(tProjEq) + a * tProjEq) / (k * (a + 1)) - Math.Sin(phi);
        }

        if (ProjMath.FindRoot(-Math.PI / 2, Math.PI / 2, 1e-5, ref t, ProjectionEquation))
        {
            y = b * t;
            x = lambda * (Math.Cos(t) + a) / (a + 1);

            return true;
        }
        return false;
    }
    }
}
