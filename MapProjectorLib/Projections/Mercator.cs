﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapProjectorLib.Projections
{
    class Mercator : CylindricalBase
    {
        protected override double GetLat(double y)
        {
            double k = Math.Exp(Math.Abs(y));
            double phi = Math.Acos(2 * k / (k * k + 1));
            if (y < 0) phi = -phi;
            return phi;
        }

        protected override double GetLong(double x)
        {
            return x;
        }

        protected override bool GetXY(double phi, double lambda, ref double x, ref double y)
        {
            x = lambda;
            y = Math.Log(((1 +  Math.Sin(Math.Abs(phi))) / Math.Cos(Math.Abs(phi))));
            if (phi < 0)
            {
                y = -y;
            }
            return true;
        }

    }
}
