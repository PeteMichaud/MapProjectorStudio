using System;

namespace MapProjectorLib.Extensions
{
    public static class ArrayExtensions
    {
        public static T[] Slice<T>(this T[] array, int startIdx, int endInx)
        {
            var length = endInx - startIdx;
            var sliced = new T[length];
            Array.Copy(array, startIdx, sliced, 0, length);
            return sliced;
        }
    }
}
