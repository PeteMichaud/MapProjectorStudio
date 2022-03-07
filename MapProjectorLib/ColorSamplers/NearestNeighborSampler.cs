
using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorLib.ColorSamplers
{
    public class NearestNeighborSampler : ColorSampler
    {
        public NearestNeighborSampler(Image sourceImage) : base(sourceImage)
        {
            Mode = ColorSampleMode.NearestNeighbor;
        }

        public override Rgb24 Sample(double x, double y, Image image)
        {
            return image[(int)x, (int)y];
        }
    }
}
