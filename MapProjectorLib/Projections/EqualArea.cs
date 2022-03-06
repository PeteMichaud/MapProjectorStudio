using System;

namespace MapProjectorLib.Projections
{
    internal class EqualArea : CylindricalBase
    {
        double _k;

        // f(phi) =  Math.Sin(phi)/k, g(phi) = 1, where k = sqr(cos(p))
        protected override double GetMaxHeight(TransformParams tParams)
        {
            return 1 / (Math.Cos(tParams.p) * Math.Cos(tParams.p));
        }

        public override void Init(TransformParams tParams)
        {
            base.Init(tParams);
            _k = Math.Cos(tParams.p) * Math.Cos(tParams.p);
        }

        protected override double GetLat(double y)
        {
            return Math.Asin(y * _k);
        }

        protected override double GetLong(double x)
        {
            return x;
        }

        protected override (bool inBounds, PointD mappedPoint) GetXY(
            double phi, double lambda)
        {
            return (true, new PointD(
                lambda,
                Math.Sin(phi) / _k
                ));
        }
    }
}