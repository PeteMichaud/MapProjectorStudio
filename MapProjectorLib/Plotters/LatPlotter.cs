

namespace MapProjectorLib.Plotters
{
    class LatPlotter : TransformPlotter
    {
        public double lambda;
        public LatPlotter(Image image, TransformParams tParams, Transform transform) 
            : base(image, tParams, transform)
        {
            lambda = 0;
        }

        public override bool GetXY(double t, ref double x, ref double y)
        {
            return _transform.MapXY(_image, _tParams, t, lambda, ref x, ref y);
        }
    }
}
