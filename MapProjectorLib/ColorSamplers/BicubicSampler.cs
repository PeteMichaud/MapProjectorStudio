using System;
using System.Runtime.CompilerServices;
using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorLib.ColorSamplers
{
    public class BicubicSampler : ColorSampler
    {
     
        public BicubicSampler(Image sourceImage) : base(sourceImage)
        {
            Mode = ColorSampleMode.Bicubic;
        }

        public override Rgb24 Sample(double fractionalPixelX, double fractionalPixelY, Image image)
        {

            int intPixelX = Math.Min((int)fractionalPixelX, MaxWidth);
            int intPixelY = Math.Min((int)fractionalPixelY, MaxHeight);

            double dx = fractionalPixelX - intPixelX;
            double dy = fractionalPixelY - intPixelY;

            // Destination color components
            double r = 0;
            double g = 0;
            double b = 0;

            double kernelY, kernelX;
            int sampleY, sampleX;
            Rgb24 sampleColor;

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

                    sampleColor = image[sampleX, sampleY];

                    r += kernelX * sampleColor.R;
                    g += kernelX * sampleColor.G;
                    b += kernelX * sampleColor.B;
                }
            }

            return new Rgb24(ToByte(r), ToByte(g), ToByte(b));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ToByte(double n)
        {
            if (n < 0d) return 0;
            if (n > 255d) return 255;
            return (byte)n;
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

        ////https://stackoverflow.com/questions/20923956/bicubic-interpolation
        ////Bicubic convolution algorithm, cubic Hermite spline
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //static double CubicPolateConv
        //        (double vm1, double v0, double vp1, double vp2, double frac)
        //{
        //    //The polynomial of degree 4 where P(x)=f(x) for x in {0,1}
        //    //and P'(1) in one cell matches P'(0) in the next, gives a continuous smooth curve.
        //    //And we also wants the it to reduce nicely to a line, if that matches the data
        //    //With high order quotient weight of your choice....
        //    //P(x)=Ex⁴+Dx³+Cx²+Bx+A=((((Ex+D)x+C)x+B)x+A
        //    //P'(x)=4Ex³+3Dx²+2Cx+B
        //    //P(0)=A          =v0
        //    //P(1)=E+D+C+B+A  =Vp1
        //    //P'(0)=B         =(vp1-vm1)/2
        //    //P'(1)=4E+3D+2C+B=(vp2-v0 )/2
        //    //Subtracting Expressions for A, B and E from the E+D+C+B+A 
        //    //D+C =vp1-B-A -E = (vp1+vm1)/2 - v0 -E
        //    //Subtracting that twice, a B and 4E from the P'(1)
        //    //D=(vp2-v0)/2 - 2(D+C) -B -4E =(vp2-v0)/2 - (Vp1+vm1-2v0-2E) - (vp1-vm1)/2 -4E
        //    // = 3(v0-vp1)/2 + (vp2-vm1)/2 -2E
        //    //C=(D+C)-(D) = (vp1+vm1)/2 - v0 -E - (3(v0-vp1)/2 + (vp2-vm1)/2 -2E)
        //    // = vm1 + 2vp1 - (5v0+vp2)/2 +E;
            
        //    double m_BicubicSharpness = 0; //0 makes it the same as a standard bicubic interpolation, gt 0 is sharper, lt0 is fuzzier

        //    double E = (v0 - vm1 + vp1 - vp2) * m_BicubicSharpness;
        //    //double A = v0;
        //    double B = (vp1 - vm1) / 2;
        //    double DpC = (vp1 - v0) - B - E; //E+D+C+B+A - A - B -E
        //    double D = (vp2 - v0) / 2 - 2 * DpC - B - 4 * E;
        //    double C = DpC - D;

        //    return (((E * frac + D) * frac + C) * frac + B) * frac + v0;
        //}

    }
}
