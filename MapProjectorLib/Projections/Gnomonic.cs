using System;

namespace MapProjectorLib.Projections
{
    class Gnomonic : PolarBase
    {
        protected override void GetPhi(double r, ref double phi)
        {
            phi = Math.Atan(1 / (2 * r));
        }

        protected override bool GetR(double phi, ref double r)
        {
            if (phi > 0)
            {
                r = 1 / (2 * Math.Tan(phi));
                return true;
            }
            return false;
        }
    }
}
