using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapProjectorLib.Projections
{
    class Hammer : Simple
    {
        public override double BasicScale(int width, int height) 
        { 
            return 2.0 / width; 
        }

        public override bool ProjectSimple(double x, double y,
                       ref double phi, ref double lambda)
        {
            bool result = false;
            double z2 = 2 - ProjMath.Sqr(x) - ProjMath.Sqr(2 * y);
            double z = Math.Sqrt(z2);
            double t1 = 2 * y * z;
            if (t1 >= -1.0 && t1 <= 1.0)
            {
                phi = Math.Asin(t1);
                lambda = 2 * Math.Atan2(x* z, z2 - 1);
                if (lambda >= -Math.PI && lambda <= Math.PI)
                {
                    result = true;
                }
            }

            return result;
        }

        public override bool ProjectInvSimple(double phi, double lambda, ref double x, ref double y)
        {
            double z = Math.Sqrt(1 + Math.Cos(phi) * Math.Cos(lambda / 2));
            x = Math.Cos(phi) * Math.Sin(lambda / 2) / z;
            y = Math.Sin(phi) / (2 * z);
            return true;
        }

    }
}
