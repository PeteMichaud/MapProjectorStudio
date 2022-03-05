namespace MapProjectorLib.Plotters
{
    internal class LatPlotter : TransformPlotter
    {
        public double Lambda;

        public LatPlotter(
            Image image, TransformParams tParams, Transform transform)
            : base(image, tParams, transform)
        {
            Lambda = 0;
        }

        public override bool GetXY(double t, ref double x, ref double y)
        {
            return _transform.MapXY(_image, _tParams, t, Lambda, ref x, ref y);
        }
    }
}