using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapProjectorLib.Projections
{
    class EqualArea : CylindricalBase
    {
        private double k;

        // f(phi) =  Math.Sin(phi)/k, g(phi) = 1, where k = sqr(cos(p))
        protected override double GetMaxHeight(TransformParams tParams)
        {
            return 1 / (Math.Cos(tParams.p)* Math.Cos(tParams.p));
        }

        public override void Init(TransformParams tParams)
        {
            base.Init(tParams);
            k = Math.Cos(tParams.p) * Math.Cos(tParams.p);
        }

        protected override double GetLat(double y)
        {
            return Math.Asin(y * k);
        }

        protected override double GetLong(double x)
        {
            return x;
        }

        protected override bool GetXY(double phi, double lambda, ref double x, ref double y)
        {
            x = lambda;
            y = Math.Sin(phi) / k;
            return true;
        }
    }
}
