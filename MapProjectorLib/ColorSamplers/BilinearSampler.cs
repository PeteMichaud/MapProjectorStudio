using System;
using System.Runtime.CompilerServices;
using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorLib.ColorSamplers
{
    class BilinearSampler : ColorSampler
    {

        public BilinearSampler(Image sourceImage) : base(sourceImage)
        {
            Mode = ColorSampleMode.Bilinear;
        }

        public override Rgb24 Sample(double fractionalPixelX, double fractionalPixelY, Image image)
        {

            int intPixelX = Math.Min((int)fractionalPixelX,image.Width - 2);
            int intPixelY = Math.Min((int)fractionalPixelY, image.Height - 2);

            Rgb24 c00 = image[intPixelX, intPixelY];
            Rgb24 c10 = image[intPixelX + 1, intPixelY];
            Rgb24 c01 = image[intPixelX, intPixelY + 1];
            Rgb24 c11 = image[intPixelX + 1, intPixelY + 1];

            return new Rgb24(
                (byte)BilinearLerp(c00.R, c10.R, c01.R, c11.R, fractionalPixelX - intPixelX, fractionalPixelY - intPixelY),
                (byte)BilinearLerp(c00.G, c10.G, c01.G, c11.G, fractionalPixelX - intPixelX, fractionalPixelY - intPixelY),
                (byte)BilinearLerp(c00.B, c10.B, c01.B, c11.B, fractionalPixelX - intPixelX, fractionalPixelY - intPixelY)
            );

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double BilinearLerp(double start1, double end1, double start2, double end2, double tx, double ty)
        {
            return ProjMath.Lerp(ProjMath.Lerp(start1, end1, tx), ProjMath.Lerp(start2, end2, tx), ty);
        }
    }
}
