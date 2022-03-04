using System;

namespace MapProjectorLib.Plotters
{

    // Draw a temporary hour line
    class TempPlotter : TransformPlotter
    {
        public double time;
        public double phi;
        public double lambda;

        public TempPlotter(Image image, TransformParams tParams, Transform transform)
            : base(image, tParams, transform)
        {
            time = 0;
            phi = 0;
            lambda = 0;
        }

        // t is the solar declination
        // phi,lambda is origin
        public override bool GetXY(double t, ref double x, ref double y)
        {
            double delta = t;
            // time/angle between sunrise and noon and noon and sunset
            double tau = Math.Acos(-Math.Tan(phi) * Math.Tan(delta));
            //fprintf(stderr,"%g %g %g %g\n", t, phi, lambda, tau);
            double h = tau * (time - 12) / 6.0 + lambda;
    
            return _transform.MapXY(_image, _tParams, delta, h, ref x, ref y);
        }
    }
}
