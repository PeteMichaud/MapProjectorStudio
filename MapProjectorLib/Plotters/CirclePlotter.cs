using System;

namespace MapProjectorLib.Plotters
{
    internal class CirclePlotter : TransformPlotter
    {
        public double phi;
        public double theta;

        public CirclePlotter(
            Image image, TransformParams tParams, Transform transform)
            : base(image, tParams, transform)
        {
            theta = 0;
            phi = 0;
        }

        public override bool GetXY(double t, ref double x, ref double y)
        {
            var x0 = Math.Cos(t);
            var y0 = Math.Sin(t);
            double z0 = 0;

            RotateX(phi, ref x0, ref y0, ref z0);
            RotateY(phi, ref x0, ref y0, ref z0);
            RotateZ(theta, ref x0, ref y0, ref z0);

            var phi0 = Math.Asin(z0);
            var lambda0 = Math.Atan2(y0, x0);
            return _transform.MapXY(
                _image, _tParams, phi0, lambda0, ref x, ref y);
        }

        void RotateX(double theta, ref double x, ref double y, ref double z)
        {
            var x0 = x;
            var y0 = Math.Cos(theta) * y - Math.Sin(theta) * z;
            var z0 = Math.Sin(theta) * y + Math.Cos(theta) * z;

            x = x0;
            y = y0;
            z = z0;
        }

        void RotateY(double theta, ref double x, ref double y, ref double z)
        {
            var x0 = Math.Cos(theta) * x - Math.Sin(theta) * z;
            var y0 = y;
            var z0 = Math.Sin(theta) * x + Math.Cos(theta) * z;

            x = x0;
            y = y0;
            z = z0;
        }

        void RotateZ(double theta, ref double x, ref double y, ref double z)
        {
            var x0 = Math.Cos(theta) * x - Math.Sin(theta) * y;
            var y0 = Math.Sin(theta) * x + Math.Cos(theta) * y;
            var z0 = z;

            x = x0;
            y = y0;
            z = z0;
        }
    }
}