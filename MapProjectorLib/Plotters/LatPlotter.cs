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

        public override (bool inBounds, PointD mappedPoint) GetXY(double progressAlongPlot)
        {
            return _transform.MapXY(_image, _tParams, progressAlongPlot, Lambda);
        }
    }
}