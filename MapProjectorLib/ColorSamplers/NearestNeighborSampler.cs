using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Memory;

namespace MapProjectorLib.ColorSamplers
{
    public class NearestNeighborSampler : ColorSampler
    {
        public NearestNeighborSampler(Buffer2D<RgbaVector> sourceBuffer) 
            : base(sourceBuffer)
        {
            Mode = ColorSampleMode.NearestNeighbor;
        }

        public override RgbaVector Sample(double x, double y)
        {
            return SourceBuffer[(int)x, (int)y];
        }
    }
}
