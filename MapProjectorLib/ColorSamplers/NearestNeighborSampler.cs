using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorLib.ColorSamplers
{
    public class NearestNeighborSampler : ColorSampler
    {
        public NearestNeighborSampler(SamplableImage sourceImage) : base(sourceImage)
        {
            Mode = ColorSampleMode.NearestNeighbor;
        }

        public override RgbaVector Sample(double x, double y)
        {
            return Image[(int)x, (int)y];
        }
    }
}
