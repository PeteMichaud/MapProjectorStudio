using System;
using MapProjectorLib.Extensions;
using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorLib.Projections
{
    internal class Perspective : Transform
    {
        const double nightdim = 0.5;

        // Cached oblateness factors
        double a;

        // And their squares
        double a2;
        double b;
        double b2;
        double c;

        double c2;

        // And their inverse squares
        double ia2;
        double ib2;
        double ic2;
        double iscalefactor;

        // Rotated viewpoint position
        double rx;
        double ry;
        double rz;
        double scalefactor;

        //

        protected double sunx;
        protected double suny;
        protected double sunz;

        // Viewpoint position
        double vx;
        double vy;
        double vz;

        public override double BasicScale(int width, int height)
        {
            return 2.0 / height;
        }

        public static (double sunX, double sunY, double sunZ)
            CalculateSunCoordinates(TransformParams tParams)
        {
            // Construct the sun parameters
            if (!tParams.sun) return (0.0, 0.0, 0.0);

            // We will use the astronomical day, starting at midnight (at Greenwich)
            // And start the year at the spring equinox
            // Need the vector to the sun.
            var date = tParams.date - 80;
            while (date < 0) date += 365;
            while (date >= 365) date -= 365;
            date = 2 * Math.PI * date / 365;

            var sunHeight = ProjMath.SunHeight(date);
            var q = Math.Sqrt(1 - ProjMath.Sqr(sunHeight));
            // tParams.time is mean time, convert to apparent time
            var eot = ProjMath.EquationOfTime(date) * 2.0 * Math.PI /
                      (60.0 * 60.0 * 24.0);
            // AT = MT + EOT
            var atime = tParams.time + eot;
            var sunX = -Math.Cos(atime) * q;
            var sunY = Math.Sin(atime) * q;
            var sunZ = sunHeight;

            return (sunX, sunY, sunZ);
        }

        public override void Init(TransformParams tParams)
        {
            base.Init(tParams);

            vx = tParams.x;
            vy = tParams.y;
            vz = tParams.z;

            rx = vx;
            ry = vy;
            rz = vz;

            transformMatrix.Apply(ref rx, ref ry, ref rz);

            scalefactor = (vx + 1) * Math.Tan(tParams.aw / 2);
            iscalefactor = 1 / scalefactor;

            a = tParams.ox;
            b = tParams.oy;
            c = tParams.oz;

            a2 = ProjMath.Sqr(a);
            b2 = ProjMath.Sqr(b);
            c2 = ProjMath.Sqr(c);

            ia2 = 1 / a2;
            ib2 = 1 / b2;
            ic2 = 1 / c2;

            (sunx, suny, sunz) = CalculateSunCoordinates(tParams);
        }

        public override bool Project(
            TransformParams tParams,
            double x0, double y0,
            ref double x, ref double y, ref double z,
            ref double phi, ref double lambda)
        {
            // Apply our rotation to the point we are projecting to
            double x1 = -1;
            var y1 = scalefactor * x0 + vy;
            var z1 = scalefactor * y0 + vz;

            transformMatrix.Apply(ref x1, ref y1, ref z1);

            // Projecting from (rx, ry, rz) to point (x1, y1, z1)
            // Solve a quadratic obtained from equating line equation with r = 1
            var qa = a2 * ProjMath.Sqr(rx - x1) + b2 * ProjMath.Sqr(ry - y1) +
                     c2 * ProjMath.Sqr(rz - z1);
            var qb = 2 * (a2 * x1 * (rx - x1) + b2 * y1 * (ry - y1) +
                          c2 * z1 * (rz - z1));
            var qc = a2 * ProjMath.Sqr(x1) + b2 * ProjMath.Sqr(y1) +
                c2 * ProjMath.Sqr(z1) - 1;
            var qm = qb * qb - 4 * qa * qc;

            if (qm >= 0)
            {
                // Since qa is always positive, the + solution is nearest to the point
                // of view, so we always use that one.
                var k = (-qb + Math.Sqrt(qm)) / (2 * qa);
                x = k * rx + (1 - k) * x1;
                y = k * ry + (1 - k) * y1;
                z = k * rz + (1 - k) * z1;

                if (a == 1.0 && b == 1.0 && c == 1.0)
                {
                    phi = Math.Asin(z);
                    lambda = Math.Atan2(y, x);
                } else
                {
                    // This is a point on the ellipsoid, so convert to lat long
                    var r = Math.Sqrt(
                        ProjMath.Sqr(a2 * x) + ProjMath.Sqr(b2 * y));
                    phi = Math.Atan(c2 * z / r);
                    lambda = Math.Atan2(b2 * y, a2 * x);
                    // Now return the spherical x, y, z corresponding to phi, lambda
                    x = Math.Cos(lambda) * Math.Cos(phi);
                    y = Math.Sin(lambda) * Math.Cos(phi);
                    z = Math.Sin(phi);
                }

                return true;
            }

            return false;
        }

        public override bool ProjectInv(
            TransformParams tParams,
            double phi, double lambda, ref double x, ref double y)
        {
            // Find where phi, lambda project to on the ellipsoid
            double x0, y0, z0;
            double nx, ny, nz;
            if (a == 1.0 && b == 1.0 && c == 1.0)
            {
                // Just a sphere
                x0 = Math.Cos(lambda) * Math.Cos(phi);
                y0 = Math.Sin(lambda) * Math.Cos(phi);
                z0 = Math.Sin(phi);
                transformMatrixInv.Apply(ref x0, ref y0, ref z0);
                nx = x0;
                ny = y0;
                nz = z0;
            } else
            {
                // The normal vector
                nx = Math.Cos(lambda);
                ny = Math.Sin(lambda);
                nz = Math.Tan(phi);

                // The actual vector
                var r = Math.Sqrt(
                    1 / (ia2 * ProjMath.Sqr(nx) + ib2 * ProjMath.Sqr(ny) +
                         ic2 * ProjMath.Sqr(nz)));
                x0 = nx * r * ia2;
                y0 = ny * r * ib2;
                z0 = nz * r * ic2;
                // And apply rotations
                transformMatrixInv.Apply(ref x0, ref y0, ref z0);
                transformMatrixInv.Apply(ref nx, ref ny, ref nz);
            }

            // Test for visibility - dot product of nx,ny,nz and the line
            // towards to viewpoint must be positive
            var p = (vx - x0) * nx + (vy - y0) * ny + (vz - z0) * nz;
            if (p >= 0)
            {
                // Project from (vx, vy, vz) through (x0,y0,z0) to (-1, y, z)
                var t = (1 + x0) / (x0 - vx);
                x = t * vy + (1 - t) * y0; // Note change of axes
                y = t * vz + (1 - t) * z0;
                x = (x - vy) * iscalefactor;
                y = (y - vz) * iscalefactor;

                return true;
            }

            return false;
        }

        protected override Rgb24 AdjustOutputColor(
            Rgb24 inputcolor,
            double xProjected, double yProjected, double zProjected,
            TransformParams tParams)
        {
            if (!tParams.sun) return inputcolor;

            // Is where we are sunny?
            // Compute pi/2 - angle of the x-axis with the normal at the relevant point.
            // This should be Math.Asin(...) but we are only interested in small angles
            // so assume Math.Sin x = x
            var sunAltitude = sunx * xProjected + suny * yProjected +
                              sunz * zProjected;

            return sunAltitude < ProjMath.SunriseAngle
                ? inputcolor.Dim(nightdim)
                : inputcolor.Dim(0.8 + 0.2 * sunAltitude);
        }
    }
}