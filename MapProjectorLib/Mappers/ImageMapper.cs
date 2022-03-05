using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorLib.Mappers
{
    public abstract class ImageMapper
    {
        public abstract double Scale(int width, int height);
        public abstract void InitY(double y);
        public abstract Rgb24 Map(double x, double y);
    }
}