using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorLib.ColorSamplers
{
    public abstract class ColorSampler
    {
        public ColorSampleMode Mode { get; protected set; } 
        readonly protected int MaxWidth;
        readonly protected int MaxHeight;
        public ColorSampler(Image sourceImage)
        {
            MaxWidth = sourceImage.Width - 1;
            MaxHeight = sourceImage.Height - 1;
        }
        public abstract Rgb24 Sample(double x, double y, Image image);
    }
}
