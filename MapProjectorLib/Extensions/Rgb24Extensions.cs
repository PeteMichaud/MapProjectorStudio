using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorLib.Extensions
{
    public static class Rgb24Extensions
    {
        public static Rgb24 Dim(this Rgb24 color, double dimAmount)
        {
            return new Rgb24(
                (byte) (color.R * dimAmount),
                (byte) (color.G * dimAmount),
                (byte) (color.B * dimAmount));
        }
    }
}