//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.PixelFormats;
//using SixLabors.ImageSharp.Advanced;
//using SixLabors.ImageSharp.Memory;

//using MapProjectorLib.ColorSamplers;

//namespace MapProjectorLib
//{
//    readonly struct RowOperation<TBuffer> : IRowOperation<TBuffer> 
//        where TBuffer : unmanaged, IPixel<TBuffer>
//    {
//        private readonly Buffer2D<TBuffer> source;
//        private readonly Buffer2D<TBuffer> destination;
//        public readonly Configuration configuration;

//        //readonly Image<TBuffer> sourceImage;
//        //readonly Image<TBuffer> destinationImage;

//        readonly Transform _transform;
//        readonly TransformParams _tParams;
//        readonly ColorSampler _sourceSampler;

//        readonly double xOrigin;
//        readonly double yOrigin;
//        readonly double _halfWidth;
//        readonly double _halfHeight;
//        readonly double _scaledWidth;
//        readonly double _scaledHeight;
//        readonly double scaleFactor;

//        public RowOperation(
//            Image<TBuffer> inImage,
//            Image<TBuffer> outImage,
//            TransformParams tParams,
//            Transform transform,
//            ColorSampleMode sampleMode
//            )
//        {
//            configuration = Configuration.Default;

//            _transform = transform;
//            this._tParams = tParams;

//            source = inImage.Frames[0].PixelBuffer;
//            destination = outImage.Frames[0].PixelBuffer;

//            _sourceSampler = ColorSampler.FromMode(sampleMode, source);

//            // precomputed values for the hot loop
//            xOrigin = 0.5d * outImage.Width;
//            yOrigin = 0.5d * outImage.Height;
//            _halfWidth = inImage.Width * 0.5;
//            _halfHeight = inImage.Height * 0.5;
//            _scaledWidth = inImage.Width * ProjMath.OneOverTwoPi;
//            _scaledHeight = inImage.Height * ProjMath.OneOverPi;

//            // Scale so that half width is 1
//            scaleFactor = _transform.BasicScale(outImage.Width, outImage.Height) / tParams.scale;

//        }

//        public void Invoke(int outY, Span<TBuffer> span)
//        {
//            var y = scaleFactor * (yOrigin - outY - 0.5) + _tParams.yOffset;
//            if (_tParams.rotate == 0) _transform.SetY(y);

//            Span<TBuffer> destinationRowSpan = this.destination.DangerousGetRowSpan(outY);
//            for (int outX = 0; outX < destinationRowSpan.Length; outX++)
//            {
//                var x1 = scaleFactor * (outX + 0.5 - xOrigin) +
//                        _tParams.xOffset;
//                var y1 = y;

//                if (_tParams.rotate != 0)
//                {
//                    (x1, y1) = ProjMath.ApplyRotation(-_tParams.rotate, x1, y1);
//                    _transform.SetY(y1);
//                }

//                double x0 = 0.0, y0 = 0.0, z0 = 0.0;
//                double phi = 0.0, lambda = 0.0;

//                bool inProjectionBounds = _transform.Project(_tParams, x1, y1,
//                    ref x0, ref y0, ref z0,
//                    ref phi, ref lambda);

//                if (inProjectionBounds && ProjMath.IsPointWithinRadius(_tParams, phi, lambda))
//                {
//                    var scaledX = _halfWidth + lambda * _scaledWidth;
//                    var scaledY = _halfHeight - phi * _scaledHeight;

//                    var outColor = _transform.AdjustOutputColor(
//                        _sourceSampler.Sample(scaledX, scaledY), x0, y0, z0, _tParams);

//                    destinationRowSpan[outX] = outColor;
//                }
//            }
//        }
//    }
//}
