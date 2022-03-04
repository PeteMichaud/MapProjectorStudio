
namespace MapProjectorLib.Plotters
{
    abstract class TransformPlotter : LinePlotter
    {
        protected Image _image;
        protected TransformParams _tParams;
        protected Transform _transform;

        public TransformPlotter(Image image, TransformParams tParams, Transform transform)
        {
            _image = image;
            _tParams = tParams;
            _transform = transform;
        }
    }
}
