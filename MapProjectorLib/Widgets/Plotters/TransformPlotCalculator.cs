namespace MapProjectorLib.PlotCalculators
{
    internal abstract class TransformPlotCalculator : LinePlotCalculator
    {
        protected readonly DestinationImage _image;
        protected readonly TransformParams _tParams;
        protected readonly Transform _transform;

        public TransformPlotCalculator(
            DestinationImage image, TransformParams tParams, Transform transform)
        {
            _image = image;
            _tParams = tParams;
            _transform = transform;
        }
    }
}