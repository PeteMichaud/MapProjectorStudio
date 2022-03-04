

namespace MapProjectorLib.Plotters
{
    class LongPlotter : TransformPlotter
    {
        public double phi;
        public LongPlotter(Image image, TransformParams tParams, Transform transform)
            : base(image, tParams, transform)
        {
            phi = 0;
        }

        public override bool GetXY(double t, ref double x, ref double y)
        {
            return _transform.MapXY(_image, _tParams, phi, t, ref x, ref y);
        }
    }
}
