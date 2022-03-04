using System;

namespace MapProjectorLib.Plotters
{
    class CirclePlotter : TransformPlotter
    {
        public double theta;
        public double phi;
        public CirclePlotter(Image image, TransformParams tParams, Transform transform)
            : base(image, tParams, transform)
        {
            theta = 0;
            phi = 0;
        }

        public override bool GetXY(double t, ref double x, ref double y)
        {
            double x0 = Math.Cos(t);
            double y0 = Math.Sin(t);
            double z0 = 0;

            RotateX(phi, ref x0, ref y0, ref z0);
            RotateY(phi, ref x0, ref y0, ref z0);
            RotateZ(theta, ref x0, ref y0, ref z0);

            double phi0 = Math.Asin(z0);
            double lambda0 = Math.Atan2(y0, x0);
            return _transform.MapXY(_image, _tParams, phi0, lambda0, ref x, ref y);
        }

        void RotateX(double theta, ref double x, ref double y, ref double z)
        {
            double x0 = x;
            double y0 = Math.Cos(theta) * y - Math.Sin(theta) * z;
            double z0 = Math.Sin(theta) * y + Math.Cos(theta) * z;
            
            x = x0; 
            y = y0; 
            z = z0;
        }

        void RotateY(double theta, ref double x, ref double y, ref double z)
        {
            double x0 = Math.Cos(theta) * x - Math.Sin(theta) * z;
            double y0 = y;
            double z0 = Math.Sin(theta) * x + Math.Cos(theta) * z;
            
            x = x0; 
            y = y0; 
            z = z0;
        }

        void RotateZ(double theta, ref double x, ref double y, ref double z)
        {
            double x0 = Math.Cos(theta) * x - Math.Sin(theta) * y;
            double y0 = Math.Sin(theta) * x + Math.Cos(theta) * y;
            double z0 = z;

            x = x0; 
            y = y0; 
            z = z0;
        }
    }
}
