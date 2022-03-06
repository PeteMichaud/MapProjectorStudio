using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapProjectorLib
{
    public struct PointD
    {
        public double X;
        public double Y;
        public static PointD None = new PointD(double.MinValue, double.MinValue);

        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
