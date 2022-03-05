using System;

namespace MapProjectorLib.Plotters
{
    internal class FooPlotter : TransformPlotter
    {
        public double lambda;
        public double phi;
        public double theta;

        public FooPlotter(
            Image image, TransformParams tParams, Transform transform)
            : base(image, tParams, transform)
        {
            theta = 0;
            lambda = 0;
            phi = 0;
        }

        public override bool GetXY(double t, ref double x, ref double y)
        {
            // Angle up from the pole
            var x0 = Math.Cos(t) * Math.Cos(theta);
            var y0 = Math.Sin(t) * Math.Cos(theta);
            var z0 = Math.Sin(theta);

            // Now rotate about y axis
            var x1 = Math.Sin(phi) * x0 + Math.Cos(phi) * z0;
            var y1 = y0;
            var z1 = -Math.Cos(phi) * x0 + Math.Sin(phi) * z0;
            var phi0 = Math.Asin(z1);

            if (phi0 < -ProjMath.Inclination || phi0 > ProjMath.Inclination)
                return false;

            var lambda0 = Math.Atan2(y1, x1);
            return _transform.MapXY(
                _image, _tParams, phi0, lambda0 + lambda, ref x, ref y);
        }
    }
}