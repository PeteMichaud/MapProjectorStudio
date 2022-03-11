using System;

namespace MapProjectorLib.Projections
{
    internal class Gnomonic : PolarBase
    {
        protected override double GetPhi(double r)
        {
            return Math.Atan(1 / (2 * r));
        }

        protected override (bool useR, double r) GetR(double phi)
        {
            if (!ProjMath.AboutEqual(phi,0) && phi > 0)
            {
                return (true, 1 / (2 * Math.Tan(phi)));
            }

            return (false, 0d);
        }
    }
}