using System;

namespace MapProjectorLib.Projections
{
    internal class Azimuthal : PolarBase
    {
        public override (int w, int h) AdjustSize(
            int w, int h, TransformParams tParams)
        {
            var w1 = (int) (tParams.scale * h);
            if (w1 < w) w = w1;
            return (w, h);
        }

        protected override double GetPhi(double r)
        {
            return Math.PI * (0.5 - r);
        }

        protected override (bool useR, double r) GetR(double phi)
        {
            return (true, 0.5 - phi / Math.PI);
        }
    }
}