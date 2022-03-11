using System;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Memory;

namespace MapProjectorLib.ColorSamplers
{
    public abstract class ColorSampler
    {
        public ColorSampleMode Mode { get; protected set; } 
        protected int MaxWidth;
        protected int MaxHeight;
        readonly protected Buffer2D<RgbaVector> SourceBuffer;
        public ColorSampler(Buffer2D<RgbaVector> sourceBuffer)
        {
            SourceBuffer = sourceBuffer;
            MaxWidth = sourceBuffer.Width - 1;
            MaxHeight = sourceBuffer.Height - 1;
        }
        public abstract RgbaVector Sample(double x, double y);

        public static ColorSampler GetColorSampler(Buffer2D<RgbaVector> source, ColorSampleMode mode)
        {
            switch (mode)
            {
                case ColorSampleMode.Fast:
                case ColorSampleMode.NearestNeighbor:
                    return new NearestNeighborSampler(source);
                case ColorSampleMode.Good:
                case ColorSampleMode.Bilinear:
                    return new BilinearSampler(source);
                case ColorSampleMode.Best:
                case ColorSampleMode.Bicubic:
                    return new BicubicSampler(source);
                default:
                    throw new ArgumentException($"Color Sample Mode not supported: {mode}", nameof(mode));
            }
        }
    }
}
