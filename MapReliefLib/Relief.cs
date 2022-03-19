using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Advanced;

namespace MapReliefLib
{
    public class Relief
    {
        public static Image<L16> Light(Image<L16> heightMap, ReliefMapParams rmParams)
        {
            var reliefMap = new Image<L16>(heightMap.Width, heightMap.Height);

            var operation = new ReliefRowOperation(
                heightMap, reliefMap,
                rmParams);

            ParallelRowIterator.IterateRows(
                operation.configuration,
                heightMap.Bounds(),
                in operation);

            return reliefMap;
        }
    }
}