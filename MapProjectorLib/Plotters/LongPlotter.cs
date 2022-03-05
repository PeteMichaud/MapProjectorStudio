namespace MapProjectorLib.Plotters
{
    internal class LongPlotter : TransformPlotter
    {
        public double Phi;

        public LongPlotter(
            Image image, TransformParams tParams, Transform transform)
            : base(image, tParams, transform)
        {
            Phi = 0;
        }

        public override bool GetXY(double t, ref double x, ref double y)
        {
            return _transform.MapXY(_image, _tParams, Phi, t, ref x, ref y);
        }
    }
}