using System;

namespace MapProjectorLib.Projections
{
    class Cylindrical : CylindricalBase
    {
        protected override double GetLat(double y)
        {
            return Math.Atan(y);
        }

        protected override double GetLong(double x)
        {
            return x;
        }

        protected override bool GetXY(double phi, double lambda, ref double x, ref double y)
        {
            x = lambda;
            y = Math.Tan(phi);
            return true;
        }
    }
}
