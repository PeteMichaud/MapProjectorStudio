using System;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Memory;

using MapProjectorLib.ColorSamplers;

namespace MapProjectorLib
{
    readonly struct TransformRowOperation : IRowOperation//<RgbaVector>
    {
        private readonly Buffer2D<RgbaVector> _sourceBuffer;
        private readonly Buffer2D<RgbaVector> _destinationBuffer;
        public readonly Configuration configuration;

        readonly Transform _transform;
        readonly TransformParams _tParams;
        readonly ColorSampler _sourceSampler;

        readonly double _xDestOrigin;
        readonly double _yDestOrigin;
        readonly double _halfSourceWidth;
        readonly double _halfSourceHeight;
        readonly double _scaledSourceWidth;
        readonly double _scaledSourceHeight;
        readonly double _scaleFactor;

        public TransformRowOperation(
            Image<RgbaVector> srcImage,
            Image<RgbaVector> outImage,
            TransformParams tParams,
            Transform transform,
            ColorSampleMode sampleMode)
        {
            configuration = Configuration.Default;

            _transform = transform;
            _tParams = tParams;

            _sourceBuffer = srcImage.Frames[0].PixelBuffer;
            _destinationBuffer = outImage.Frames[0].PixelBuffer;

            _sourceSampler = ColorSampler.GetColorSampler(_sourceBuffer, sampleMode);

            // precomputed values for the hot loop
            _xDestOrigin = 0.5d * outImage.Width;
            _yDestOrigin = 0.5d * outImage.Height;
            _halfSourceWidth = srcImage.Width * 0.5;
            _halfSourceHeight = srcImage.Height * 0.5;
            _scaledSourceWidth = srcImage.Width * ProjMath.OneOverTwoPi;
            _scaledSourceHeight = srcImage.Height * ProjMath.OneOverPi;

            // Scale so that half width is 1
            _scaleFactor = _transform.BasicScale(outImage.Width, outImage.Height) / tParams.Scale;

        }

        public void Invoke(int outY)
        {
            var y = _scaleFactor * (_yDestOrigin - outY - 0.5d) + _tParams.yOffset;
            
            var destinationRowSpan = _destinationBuffer.DangerousGetRowSpan(outY);

            for (int outX = 0; outX < destinationRowSpan.Length; outX++)
            {
                var x1 = _scaleFactor * (outX + 0.5d - _xDestOrigin) + _tParams.xOffset;
                var y1 = y;
               
                if (_tParams.Rotate != 0)
                {
                    (x1, y1) = ProjMath.ApplyRotation(-_tParams.Rotate, x1, y1);
                }

                (bool inProjectionBounds, double x0, double y0, double z0, double phi, double lambda) = 
                    _transform.Project(_tParams, x1, y1);

                if (inProjectionBounds && ProjMath.IsPointWithinRadius(_tParams, phi, lambda))
                {
                    var scaledX = _halfSourceWidth + lambda * _scaledSourceWidth;
                    var scaledY = _halfSourceHeight - phi * _scaledSourceHeight;
                    
                    var outColor = _transform.AdjustOutputColor(
                        _sourceSampler.Sample(scaledX, scaledY), x0, y0, z0, _tParams);
                    destinationRowSpan[outX] = outColor;
                }
                else
                {
                    _tParams.SomeDestinationPixelsAreBlank = true;
                }
            }
        }
    }
}
