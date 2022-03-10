namespace MapProjectorLib.Plotters
{
    internal class LongPlotter : TransformPlotter
    {
        public double Phi;

        public LongPlotter(
            DestinationImage image, TransformParams tParams, Transform transform)
            : base(image, tParams, transform)
        {
            Phi = 0;
        }

        public override (bool inBounds, PointD mappedPoint) GetXY(double progressAlongPlot)
        {
            return _transform.MapXY(_image, _tParams, Phi, progressAlongPlot);
        }
    }
}