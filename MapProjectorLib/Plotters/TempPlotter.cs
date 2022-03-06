using System;

namespace MapProjectorLib.Plotters
{
    // Draw a temporary hour line
    internal class TempPlotter : TransformPlotter
    {
        public double Lambda;
        public double Phi;
        public double Time;

        public TempPlotter(
            Image image, TransformParams tParams, Transform transform)
            : base(image, tParams, transform)
        {
            Time = 0;
            Phi = 0;
            Lambda = 0;
        }

        // delta is the solar declination
        // phi,lambda is origin
        public override (bool inBounds, PointD mappedPoint) GetXY(double delta)
        {
            // time/angle between sunrise and noon and noon and sunset
            var tau = Math.Acos(-Math.Tan(Phi) * Math.Tan(delta));
            var h = tau * (Time - 12) / 6.0 + Lambda;

            return _transform.MapXY(_image, _tParams, delta, h);
        }
    }
}