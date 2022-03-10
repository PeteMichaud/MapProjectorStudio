using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorLib.Extensions
{
    public static class RgbaVector4Extensions
    {
        public static RgbaVector Dim(this RgbaVector color, float dimAmount)
        {
            return new RgbaVector(
                color.R * dimAmount,
                color.G * dimAmount,
                color.B * dimAmount,
                color.A
            );
        }
    }
}