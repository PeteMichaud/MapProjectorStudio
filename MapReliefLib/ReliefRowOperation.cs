
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Memory;

using MapReliefLib.Extensions;

namespace MapReliefLib
{
    readonly struct ReliefRowOperation : IRowOperation
    {
        private readonly Buffer2D<L8> _sourceBuffer;
        private readonly Buffer2D<L8> _destinationBuffer;
        public readonly Configuration configuration;

        readonly ReliefMapParams _rmParams;
        const int _kernelSize = 9;
        readonly double _nintyDeg = 90d.ToRadians();
        readonly double _divisor = 8 * _kernelSize;
        public ReliefRowOperation(
            Image<L8> heightMap,
            Image<L8> reliefMap,
            ReliefMapParams rmParams)
        {
            configuration = Configuration.Default;

            _rmParams = rmParams;

            _sourceBuffer = heightMap.Frames[0].PixelBuffer;
            _destinationBuffer = reliefMap.Frames[0].PixelBuffer;

        }

        public void Invoke(int outY)
        {
            if (outY == 0) return;
            if(outY == _sourceBuffer.Height - 1) return;
            var destinationRowSpan = _destinationBuffer.DangerousGetRowSpan(outY);

            for (int outX = 1; outX < destinationRowSpan.Length-1; outX++)
            {
                var a = _sourceBuffer[outX - 1, outY - 1].PackedValue;
                var b = _sourceBuffer[outX - 1, outY    ].PackedValue;
                var c = _sourceBuffer[outX - 1, outY + 1].PackedValue;

                var d = _sourceBuffer[outX    , outY - 1].PackedValue;
                //var e = _sourceBuffer[outX    , outY    ].PackedValue;
                var f = _sourceBuffer[outX    , outY + 1].PackedValue;

                var g = _sourceBuffer[outX + 1, outY - 1].PackedValue;
                var h = _sourceBuffer[outX + 1, outY    ].PackedValue;
                var i = _sourceBuffer[outX + 1, outY + 1].PackedValue;

                var dzdx = ((c + 2 * f + i) - (a + 2 * d + g)) / _divisor;
                var dzdy = ((g + 2 * h + i) - (a + 2 * b + c)) / _divisor;

                var slope = Math.Atan(Math.Sqrt(dzdx * dzdx + dzdy * dzdy));
                var aspect = Math.Atan(dzdy - dzdx); //should be atan2?

                var reliefValue = (
                    (Math.Cos(_nintyDeg - _rmParams.AltitudeAngle) * Math.Cos(slope))
                    + (Math.Sin(_nintyDeg - _rmParams.AltitudeAngle) 
                        * Math.Sin(slope) 
                        * Math.Cos(_rmParams.AzimuthAngle - aspect)
                      )
                );

                var pixelValue = (byte)(byte.MaxValue * reliefValue);

                destinationRowSpan[outX] = new L8(pixelValue);

            }
        }
    }
}
