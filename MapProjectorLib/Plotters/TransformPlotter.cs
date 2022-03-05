namespace MapProjectorLib.Plotters
{
    internal abstract class TransformPlotter : LinePlotter
    {
        protected readonly Image _image;
        protected readonly TransformParams _tParams;
        protected readonly Transform _transform;

        public TransformPlotter(
            Image image, TransformParams tParams, Transform transform)
        {
            _image = image;
            _tParams = tParams;
            _transform = transform;
        }
    }
}