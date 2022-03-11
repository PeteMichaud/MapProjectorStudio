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
    }
}