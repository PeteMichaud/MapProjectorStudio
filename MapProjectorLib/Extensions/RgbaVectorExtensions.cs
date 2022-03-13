using System;
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
        
        public static VectSharp.Colour ToVectColor(this RgbaVector color)
        {
            return VectSharp.Colour.FromRgba(
                color.R / 255.0d,
                color.G / 255.0d,
                color.B / 255.0d,
                color.A
            );
        }
    }
}