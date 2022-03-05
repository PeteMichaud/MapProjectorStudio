using System;

namespace MapProjectorLib.Projections
{
    internal abstract class CylindricalBase : Transform
    {
        // x is g(phi)*lambda
        // y is f(phi)
        // The drawing area is 2pi units across. The scale factor therefore
        // is w/2pi. The north pole is at w/2pi * f(pi/2)

        double phi0;

        // Abstract
        protected abstract bool GetXY(
            double phi, double lambda, ref double x, ref double y);

        protected abstract double GetLat(double y);
        protected abstract double GetLong(double x);

        // Virtual
        public override void SetY(double y)
        {
            phi0 = GetLat(y);
        }

        protected virtual double GetMaxHeight(TransformParams tParams)
        {
            return 0.0;
        }

        public override double BasicScale(int width, int height)
        {
            return 2.0 * Math.PI / width;
        }

        public override void Init(TransformParams tParams)
        {
            base.Init(tParams);
        }

        public override bool Project(
            TransformParams tParams,
            double x0, double y0,
            ref double x, ref double y, ref double z,
            ref double phi, ref double lambda)
        {
            phi = phi0;
            lambda = GetLong(x0);

            if (lambda >= -Math.PI && lambda <= Math.PI &&
                phi >= -ProjMath.PiOverTwo && phi <= ProjMath.PiOverTwo)
            {
                // Transform to new lat and long.
                // cartesian coords from latlong
                ConvertLatLong(ref phi, ref lambda, x, y, z, transformMatrix);
                return true;
            }

            return false;
        }

        public override bool ProjectInv(
            TransformParams tParams,
            double phi, double lambda,
            ref double x, ref double y)
        {
            transformMatrixInv.ApplyLatLong(ref phi, ref lambda);
            return GetXY(phi, lambda, ref x, ref y);
        }

        public override (int w, int h) AdjustSize(
            int w, int h,
            TransformParams tParams)
        {
            if (tParams.scale < 1.0)
            {
                w = (int) (w * tParams.scale);
                tParams.scale = 1.0;
            }

            // If a parallel has been given, check it's valid
            if (tParams.p < 0 || tParams.p >= ProjMath.PiOverTwo)
                throw new ArgumentException("Invalid parallel given");

            var h0 = GetMaxHeight(tParams);

            if (h0 > 0.0)
            {
                // Return <= 0 for don't adjust
                var newh = (int) (h0 * w * tParams.scale / Math.PI);
                if (newh < h) h = newh;
            }

            return (w, h);
        }
    }
}