using System.Collections.Generic;

namespace MapProjectorLib
{
    public class PointComparer : IComparer<VectSharp.Point>
    {
        int IComparer<VectSharp.Point>.Compare(VectSharp.Point p1, VectSharp.Point p2)
        {
            var res = p1.X.CompareTo(p2.X);
            if (res == 0)
            {
                res = p1.Y.CompareTo(p2.Y);
            }
            return res;
        }
    }
}
