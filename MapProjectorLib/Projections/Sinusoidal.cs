using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapProjectorLib.Projections
{
    class Sinusoidal : CylindricalBase
    {
        private double _m;
        // f(phi) = phi, g(phi) = Math.Cos(phi)
        protected override double GetMaxHeight(TransformParams tParams)
        {
            return ProjMath.PiOverTwo;
        }

        protected override double GetLat(double y)
        {
            // Latitude is just y
            _m = 1 / Math.Cos(y); // Cache the Math.Cos
            return y;
        }

        protected override double GetLong(double x)
        {
          return x * _m;
        }

        protected override bool GetXY(double phi, double lambda, ref double x, ref double y)
        {
            x = lambda * Math.Cos(phi);
            y = phi;
            return true;
        }
    }
}
