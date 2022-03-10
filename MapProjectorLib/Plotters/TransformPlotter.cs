namespace MapProjectorLib.Plotters
{
    internal abstract class TransformPlotter : LinePlotter
    {
        protected readonly DestinationImage _image;
        protected readonly TransformParams _tParams;
        protected readonly Transform _transform;

        public TransformPlotter(
            DestinationImage image, TransformParams tParams, Transform transform)
        {
            _image = image;
            _tParams = tParams;
            _transform = transform;
        }
    }
}