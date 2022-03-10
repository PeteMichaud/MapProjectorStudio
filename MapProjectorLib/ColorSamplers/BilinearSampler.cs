using System;
using System.Runtime.CompilerServices;
using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorLib.ColorSamplers
{
    class BilinearSampler : ColorSampler
    {

        public BilinearSampler(SamplableImage sourceImage) : base(sourceImage)
        {
            Mode = ColorSampleMode.Bilinear;
            MaxWidth = Image.Width - 2;
            MaxHeight = Image.Height - 2;
        }

        public override RgbaVector Sample(double fractionalPixelX, double fractionalPixelY)
        {

            int intPixelX = Math.Min((int)fractionalPixelX, MaxWidth);
            int intPixelY = Math.Min((int)fractionalPixelY, MaxHeight);

            double dX = fractionalPixelX - intPixelX;
            double dY = fractionalPixelY - intPixelY;

            RgbaVector c00 = Image[intPixelX, intPixelY];
            RgbaVector c10 = Image[intPixelX + 1, intPixelY];
            RgbaVector c01 = Image[intPixelX, intPixelY + 1];
            RgbaVector c11 = Image[intPixelX + 1, intPixelY + 1];
            
            return new RgbaVector(
                BilinearLerp(c00.R, c10.R, c01.R, c11.R, dX, dY),
                BilinearLerp(c00.G, c10.G, c01.G, c11.G, dX, dY),
                BilinearLerp(c00.B, c10.B, c01.B, c11.B, dX, dY),
                BilinearLerp(c00.A, c10.A, c01.A, c11.A, dX, dY)
            );

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float BilinearLerp(double start1, double end1, double start2, double end2, double tx, double ty)
        {
            return (float)ProjMath.Lerp(ProjMath.Lerp(start1, end1, tx), ProjMath.Lerp(start2, end2, tx), ty);
        }
    }
}
