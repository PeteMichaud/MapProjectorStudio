using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorLib.ColorSamplers
{
    public abstract class ColorSampler
    {
        public ColorSampleMode Mode { get; protected set; } 
        protected int MaxWidth;
        protected int MaxHeight;
        readonly protected SamplableImage Image;
        public ColorSampler(SamplableImage sourceImage)
        {
            Image = sourceImage;
            MaxWidth = sourceImage.Width - 1;
            MaxHeight = sourceImage.Height - 1;
        }
        public abstract RgbaVector Sample(double x, double y);
    }
}
