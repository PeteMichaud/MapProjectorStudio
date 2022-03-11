using System;
using System.Runtime.CompilerServices;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Memory;

namespace MapProjectorLib.ColorSamplers
{
    public class BicubicSampler : ColorSampler
    {
     
        public BicubicSampler(Buffer2D<RgbaVector> sourceBuffer) 
            : base(sourceBuffer)
        {
            Mode = ColorSampleMode.Bicubic;
        }

        public override RgbaVector Sample(double fractionalPixelX, double fractionalPixelY)
        {

            int intPixelX = Math.Min((int)fractionalPixelX, MaxWidth);
            int intPixelY = Math.Min((int)fractionalPixelY, MaxHeight);

            double dx = fractionalPixelX - intPixelX;
            double dy = fractionalPixelY - intPixelY;

            // Destination color components
            double r = 0;
            double g = 0;
            double b = 0;
            double a = 0;

            double kernelY, kernelX;
            int sampleY, sampleX;
            RgbaVector sampleColor;

            for (int n = -1; n < 3; n++)
            {
                kernelY = BicubicKernel(dy - n);
                sampleY = intPixelY + n;
                if (sampleY < 0) sampleY = 0;
                if (sampleY > MaxHeight) sampleY = MaxHeight;
                
                for (int m = -1; m < 3; m++)
                {
                    kernelX = kernelY * BicubicKernel(m - dx);
                    sampleX = intPixelX + m;
                    if (sampleX < 0) sampleX = 0;
                    if (sampleX > MaxWidth) sampleX = MaxWidth;

                    sampleColor = SourceBuffer[sampleX, sampleY];

                    r += kernelX * sampleColor.R;
                    g += kernelX * sampleColor.G;
                    b += kernelX * sampleColor.B;
                    a += kernelX * sampleColor.A;
                }
            }
            //todo: maybe I need to clamp before converting to float?
            return new RgbaVector((float)r, (float)g, (float)b, (float)a);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double BicubicKernel(double x)
        {
            if (x < 0)
            {
                x = -x;
            }

            //return bicubic Coefficient
            if (x <= 1)
            {
                return (1.5 * x - 2.5) * x * x + 1;
            }
            else if (x < 2)
            {
                return ((-0.5 * x + 2.5) * x - 4) * x + 2;
            }

            return 0;
        }
    }
}
