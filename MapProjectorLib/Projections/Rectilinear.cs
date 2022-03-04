using System;


namespace MapProjectorLib.Projections
{
    class Rectilinear : PolarBase
    {
        protected override void GetPhi(double r, ref double phi)
        {
            phi = Math.Acos(r);
        }

    protected override bool GetR(double phi, ref double r)
    {
        if (phi >= 0.0)
        {
            r = Math.Cos(phi);
            return true;
        }

        return false;
    }
    }
}
