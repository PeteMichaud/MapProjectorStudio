using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapProjectorLib.Projections
{
    class LatLong : CylindricalBase
    {
        //private double k;

        protected override double GetMaxHeight(TransformParams tParams)
        {
            return ProjMath.PiOverTwo;
        }

        protected override double GetLat(double y)
        {
            return y;
        }
        
        protected override double GetLong(double x)
        {
          return x;
        }

        protected override bool GetXY(double phi, double lambda, ref double x, ref double y)
        {
            x = lambda;
            y = phi;
            return true;
        }

    }
}
