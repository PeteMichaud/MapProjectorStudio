using System;

namespace MapProjectorLib.Projections
{
    internal abstract class PolarBase : Transform
    {
        protected double k;

        protected abstract double GetPhi(double r);
        protected abstract (bool useR, double r) GetR(double phi);

        public override double BasicScale(int width, int height)
        {
            return 2.0 / height;
        }

        public override void Init(TransformParams tParams)
        {
            base.Init(tParams);

            // Polar transforms have the north pole as the centre.
            // For consistency with other transforms, we rotate to set lat = 0
            transformMatrix = new Matrix3(RotationAxis.Y, -ProjMath.PiOverTwo) *
                              transformMatrix;
            transformMatrixInv *= new Matrix3(
                RotationAxis.Y, ProjMath.PiOverTwo);

            k = tParams.conic;
        }

        public override (bool inProjectionBounds, double x1, double y1, double z1, double phi, double lambda) 
        Project(
            TransformParams tParams,
            double x, double y,
            double x0, double y0, double z0,
            double phi, double lambda)
        {
            var r = Math.Sqrt(x * x + y * y) - tParams.conicr;
            lambda = tParams.conic * Math.Atan2(x, -y);

            if (r > 0.0 && lambda >= -Math.PI && lambda <= Math.PI)
            {
                phi = GetPhi(r);
                if (phi >= -ProjMath.PiOverTwo && phi <= ProjMath.PiOverTwo)
                {
                    (phi, lambda) = ConvertLatLong(
                        phi, lambda, transformMatrix);

                    return (true, x0, y0, z0, phi, lambda);
                }
            }

            return (false, x0, y0, z0, phi, lambda);
        }

        public override (bool inBounds, PointD mappedPoint) ProjectInv(
            TransformParams tParams,
            double phi, double lambda)
        {
            // Set x and y to where phi and lambda are mapped to
            // x, y are in image coordinates
            // Get projection coordinates for x and y
            (phi, lambda) = ConvertLatLong(phi, lambda, transformMatrixInv);

            //var r = 0.0;
            (var useR, var r) = GetR(phi);
            if (useR)
            {
                var x = (r + tParams.conicr) * Math.Sin(lambda / tParams.conic);
                var y = -(r + tParams.conicr) * Math.Cos(lambda / tParams.conic);

                return (true, new PointD(x,y));
            }

            return (false, PointD.None);
        }
    }
}