using System;

namespace MapProjectorLib.Projections
{
    internal abstract class CylindricalBase : Transform
    {
        // x is g(phi)*lambda
        // y is f(phi)
        // The drawing area is 2pi units across. The scale factor therefore
        // is w/2pi. The north pole is at w/2pi * f(pi/2)

        // Abstract
        protected abstract (bool inBounds, PointD mappedPoint) GetXY(
            double phi, double lambda);

        protected abstract double GetLat(double x, double y);
        protected abstract double GetLong(double x, double y);

        protected virtual double GetMaxHeight(TransformParams tParams)
        {
            return 0.0;
        }

        public override double BasicScale(int width, int height)
        {
            return 2.0 * Math.PI / width;
        }

        public override (bool inProjectionBounds, double x1, double y1, double z1, double phi, double lambda) 
        Project(
           TransformParams tParams,
           double x, double y,
           double x1, double y1, double z1,
           double phi, double lambda)
        {
            phi = GetLat(x, y);
            lambda = GetLong(x, y);

            if (lambda >= -Math.PI && lambda <= Math.PI &&
                phi >= -ProjMath.PiOverTwo && phi <= ProjMath.PiOverTwo)
            {
                // Transform to new lat and long.
                // cartesian coords from latlong
                (phi, lambda) = ConvertLatLong(phi, lambda, transformMatrix);
                return (true, x1, y1, z1, phi, lambda);
            }

            return (false, x1, y1, z1, phi, lambda);
        }

        public override (bool inBounds, PointD mappedPoint) ProjectInv(
            TransformParams tParams,
            double phi, double lambda)
        {
            transformMatrixInv.ApplyLatLong(ref phi, ref lambda);
            return GetXY(phi, lambda);
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