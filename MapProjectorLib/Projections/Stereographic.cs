using System;

namespace MapProjectorLib.Projections
{
    internal class Stereographic : PolarBase
    {
        protected override void GetPhi(double r, ref double phi)
        {
            var sr = Math.Pow(r, k);
            phi = Math.Asin((1 - ProjMath.Sqr(sr)) / (1 + ProjMath.Sqr(sr)));
        }

        protected override bool GetR(double phi, ref double r)
        {
            r = Math.Cos(phi) / (1 + Math.Sin(phi));
            r = Math.Pow(r, 1 / k);
            return true;
        }
    }
}