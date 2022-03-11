using System;

namespace MapProjectorLib.Projections
{
    internal class Rectilinear : PolarBase
    {
        protected override double GetPhi(double r)
        {
            return Math.Acos(r);
        }

        protected override (bool useR, double r) GetR(double phi)
        {
            if (!(phi >= 0.0)) return (false, 0d);
            return (true, Math.Cos(phi));
        }
    }
}