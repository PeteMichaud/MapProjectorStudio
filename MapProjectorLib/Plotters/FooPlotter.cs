using System;

namespace MapProjectorLib.Plotters
{
    internal class FooPlotter : TransformPlotter
    {
        public double Lambda;
        public double Phi;
        public double Theta;

        public FooPlotter(
            Image image, TransformParams tParams, Transform transform)
            : base(image, tParams, transform)
        {
            Theta = 0;
            Lambda = 0;
            Phi = 0;
        }

        public override bool GetXY(double t, ref double x, ref double y)
        {
            // Angle up from the pole
            var x0 = Math.Cos(t) * Math.Cos(Theta);
            var y0 = Math.Sin(t) * Math.Cos(Theta);
            var z0 = Math.Sin(Theta);

            // Now rotate about y axis
            var x1 = Math.Sin(Phi) * x0 + Math.Cos(Phi) * z0;
            var y1 = y0;
            var z1 = -Math.Cos(Phi) * x0 + Math.Sin(Phi) * z0;
            var phi0 = Math.Asin(z1);

            if (phi0 < -ProjMath.Inclination || phi0 > ProjMath.Inclination)
                return false;

            var lambda0 = Math.Atan2(y1, x1);
            return _transform.MapXY(
                _image, _tParams, phi0, lambda0 + Lambda, ref x, ref y);
        }
    }
}