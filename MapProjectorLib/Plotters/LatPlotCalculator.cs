namespace MapProjectorLib.Plotters
{
    internal class LatPlotCalculator : TransformPlotCalculator
    {
        public double Lambda;

        public LatPlotCalculator(
            DestinationImage image, TransformParams tParams, Transform transform)
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