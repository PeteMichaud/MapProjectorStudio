using System;

namespace MapProjectorLib.Projections
{
    internal abstract class PolarBase : Transform
    {
        protected double k;

        protected abstract void GetPhi(double r, ref double phi);
        protected abstract bool GetR(double phi, ref double r);

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

        public override bool Project(
            TransformParams tParams,
            double x0, double y0,
            ref double x, ref double y, ref double z,
            ref double phi, ref double lambda)
        {
            var r = Math.Sqrt(ProjMath.Sqr(x0) + ProjMath.Sqr(y0)) -
                    tParams.conicr;
            lambda = tParams.conic * Math.Atan2(x0, -y0);

            if (r > 0.0 && lambda >= -Math.PI && lambda <= Math.PI)
            {
                GetPhi(r, ref phi);
                if (phi >= -ProjMath.PiOverTwo && phi <= ProjMath.PiOverTwo)
                {
                    ConvertLatLong(
                        ref phi, ref lambda, transformMatrix);

                    return true;
                }
            }

            return false;
        }

        protected override bool ProjectInv(
            TransformParams tParams,
            double phi, double lambda,
            ref double x, ref double y)
        {
            // Set x and y to where phi and lambda are mapped to
            // x, y are in image coordinates
            // Get projection coordinates for x and y
            ConvertLatLong(ref phi, ref lambda, transformMatrixInv);

            var r = 0.0;
            if (GetR(phi, ref r))
            {
                x = (r + tParams.conicr) * Math.Sin(lambda / tParams.conic);
                y = -(r + tParams.conicr) * Math.Cos(lambda / tParams.conic);

                return true;
            }

            return false;
        }
    }
}