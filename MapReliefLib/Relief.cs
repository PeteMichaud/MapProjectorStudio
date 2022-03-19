using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Advanced;

namespace MapReliefLib
{
    public class Relief
    {
        public static Image<L8> Light(Image<L8> heightMap, ReliefMapParams rmParams)
        {
            var reliefMap = new Image<L8>(heightMap.Width, heightMap.Height);

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