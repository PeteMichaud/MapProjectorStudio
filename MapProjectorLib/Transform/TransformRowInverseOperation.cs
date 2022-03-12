using System;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Memory;

using MapProjectorLib.ColorSamplers;

namespace MapProjectorLib
{
    readonly struct TransformRowInverseOperation : IRowOperation
    {
        private readonly Buffer2D<RgbaVector> _sourceBuffer;
        private readonly Buffer2D<RgbaVector> _destinationBuffer;
        public readonly Configuration configuration;

        readonly Transform _transform;
        readonly TransformParams _tParams;
        readonly ColorSampler _sourceSampler;

        readonly int _outWidth;
        readonly int _outHeight;
        readonly double _xOrigin;
        readonly double _yOrigin;

        readonly int _sourceWidth;
        readonly int _sourceHeight;
        readonly double _sourceXOrigin;
        readonly double _sourceYOrigin;

        public TransformRowInverseOperation(
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
            _outWidth = outImage.Width;
            _outHeight = outImage.Height;
            _xOrigin = 0.5 * _outWidth;
            _yOrigin = 0.5 * _outHeight;

            _sourceWidth = srcImage.Width;
            _sourceHeight = srcImage.Height;
            _sourceXOrigin = 0.5 * _sourceWidth;
            _sourceYOrigin = 0.5 * _sourceHeight;
        }

        public void Invoke(int outY)
        {
            var destinationRowSpan = _destinationBuffer.DangerousGetRowSpan(outY);

            for (var outX = 0; outX < destinationRowSpan.Length; outX++)
            {
                // Compute lat and long
                var phi = (_yOrigin - outY - 0.5) * Math.PI / _outHeight;
                var lambda = (outX + 0.5 - _xOrigin) * ProjMath.TwoPi / _outWidth;

                if (!ProjMath.IsPointWithinRadius(_tParams, phi, lambda)) continue;

                // Compute the scaled x,y coordinates for <phi,lambda>
                (var inverseProjectionInBounds, var projectedPoint) =
                    _transform.ProjectInv(_tParams, phi, lambda);

                if (inverseProjectionInBounds)
                {
                    SetDataInv(destinationRowSpan, outX, projectedPoint.X, projectedPoint.Y);
                }
            }
        }

        bool SetDataInv(
           Span<RgbaVector> destinationRowSpan,
           int outX, // Coordinates in output image
           double x, double y // scaled coordinates in input image
        )
        {
            // Scale so that half width is 1
            var scaleFactor = _transform.BasicScale(_sourceWidth, _sourceHeight) / _tParams.Scale;

            if (_tParams.Rotate != 0)
            {
                (x, y) = ProjMath.ApplyRotation(_tParams.Rotate, x, y);
            }
            
            var sourceX = Math.Floor(
                _sourceXOrigin + (x - _tParams.xOffset) / scaleFactor);
            var sourceY = Math.Floor(
                _sourceYOrigin - (y - _tParams.yOffset) / scaleFactor);

            if (sourceX < 0 || sourceX >= _sourceWidth || sourceY < 0 || sourceY >= _sourceHeight)
            {
                _tParams.SomeDestinationPixelsAreBlank = true;
                return false;
            }

            destinationRowSpan[outX] = _sourceSampler.Sample(sourceX, sourceY);
            return true;
        }
    }
}
