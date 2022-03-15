using System;
using MapProjectorLib.Extensions;
using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorLib.Projections
{
    internal class Perspective : Transform
    {
        //todo: make param
        const float nightDim = 0.5f;

        // Cached oblateness factors
        double a;
        double b;
        double c;

        // And their squares
        double a2;
        double b2;
        double c2;

        // And their inverse squares
        double ia2;
        double ib2;
        double ic2;

        // Viewpoint position
        double viewX;
        double viewY;
        double viewZ;

        // Rotated viewpoint position
        double rotatedViewX;
        double rotatedViewY;
        double rotatedViewZ;

        //

        double scaleFactor;
        double invScaleFactor;

        //

        double sunX;
        double sunY;
        double sunZ;

        public override double BasicScale(int width, int height)
        {
            return 2.0 / height;
        }

        static (double sunX, double sunY, double sunZ)
        CalculateSunCoordinates(TransformParams tParams)
        {
            // Construct the sun parameters
            if (!tParams.sun) return (0.0, 0.0, 0.0);

            // We will use the astronomical day, starting at midnight (at Greenwich)
            // And start the year at the spring equinox
            // Need the vector to the sun.
            var date = tParams.Date - 80;
            while (date < 0) date += 365;
            while (date >= 365) date -= 365;
            date = 2 * Math.PI * date / 365;

            var sunHeight = ProjMath.SunHeight(date);
            var q = Math.Sqrt(1 - ProjMath.Sqr(sunHeight));
            // tParams.time is mean time, convert to apparent time
            var eot = ProjMath.EquationOfTime(date) * 2.0 * Math.PI /
                      (60.0 * 60.0 * 24.0);
            // AT = MT + EOT
            var apparentTime = tParams.Time + eot;
            var sunX = -Math.Cos(apparentTime) * q;
            var sunY = Math.Sin(apparentTime) * q;
            var sunZ = sunHeight;

            return (sunX, sunY, sunZ);
        }

        public override void Init(TransformParams tParams)
        {
            base.Init(tParams);

            viewX = tParams.X;
            viewY = tParams.Y;
            viewZ = tParams.Z;

            rotatedViewX = viewX;
            rotatedViewY = viewY;
            rotatedViewZ = viewZ;

            (rotatedViewX, rotatedViewY, rotatedViewZ) = 
                transformMatrix.Apply(rotatedViewX, rotatedViewY, rotatedViewZ);

            scaleFactor = (viewX + 1) * Math.Tan(tParams.aw / 2);
            invScaleFactor = 1 / scaleFactor;

            a = tParams.ox;
            b = tParams.oy;
            c = tParams.oz;

            a2 = ProjMath.Sqr(a);
            b2 = ProjMath.Sqr(b);
            c2 = ProjMath.Sqr(c);

            ia2 = 1 / a2;
            ib2 = 1 / b2;
            ic2 = 1 / c2;

            (sunX, sunY, sunZ) = CalculateSunCoordinates(tParams);
        }

        public override (bool inProjectionBounds, double x1, double y1, double z1, double phi, double lambda) 
        Project(
            TransformParams tParams,
            double x0, double y0,
            double x, double y, double z,
            double phi, double lambda)
        {

            double x1 = -1;
            var y1 = scaleFactor * x0 + viewY;
            var z1 = scaleFactor * y0 + viewZ;

            (x1, y1, z1) = transformMatrix.Apply(x1, y1, z1);

            // Projecting from (rx, ry, rz) to point (x1, y1, z1)
            // Solve a quadratic obtained from equating line equation with r = 1
            var qa = a2 * ProjMath.Sqr(rotatedViewX - x1) + b2 * 
                ProjMath.Sqr(rotatedViewY - y1) +
                c2 * ProjMath.Sqr(rotatedViewZ - z1);
            var qb = 2 * (a2 * x1 * (rotatedViewX - x1) + b2 * 
                y1 * (rotatedViewY - y1) +
                c2 * z1 * (rotatedViewZ - z1));
            var qc = a2 * ProjMath.Sqr(x1) + b2 * 
                ProjMath.Sqr(y1) +
                c2 * ProjMath.Sqr(z1) - 1;
            var qm = qb * qb - 4 * qa * qc;

            if (!(qm >= 0)) return (false, x, y, z, phi, lambda);

            // Since qa is always positive, the + solution is nearest to the point
            // of view, so we always use that one.
            var k = (-qb + Math.Sqrt(qm)) / (2 * qa);
            x = k * rotatedViewX + (1 - k) * x1;
            y = k * rotatedViewY + (1 - k) * y1;
            z = k * rotatedViewZ + (1 - k) * z1;

            if (a == 1.0 && b == 1.0 && c == 1.0)
            {
                phi = Math.Asin(z);
                lambda = Math.Atan2(y, x);
            } 
            else
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
            
            return (true, x, y, z, phi, lambda);

        }

        public override (bool inBounds, PointD mappedPoint) ProjectInv(
            TransformParams tParams,
            double phi, double lambda)
        {
            // Find where phi, lambda project to on the ellipsoid
            double x0, y0, z0;
            double normalX, normalY, normalZ;
            if (a == 1.0 && b == 1.0 && c == 1.0)
            {
                // Just a sphere
                x0 = Math.Cos(lambda) * Math.Cos(phi);
                y0 = Math.Sin(lambda) * Math.Cos(phi);
                z0 = Math.Sin(phi);
                (x0, y0, z0) = transformMatrixInv.Apply(x0, y0, z0);
                normalX = x0;
                normalY = y0;
                normalZ = z0;
            } 
            else
            {
                // The normal vector
                normalX = Math.Cos(lambda);
                normalY = Math.Sin(lambda);
                normalZ = Math.Tan(phi);

                // The actual vector
                var r = Math.Sqrt(
                    1 / (ia2 * ProjMath.Sqr(normalX) + ib2 * ProjMath.Sqr(normalY) +
                         ic2 * ProjMath.Sqr(normalZ)));
                x0 = normalX * r * ia2;
                y0 = normalY * r * ib2;
                z0 = normalZ * r * ic2;
                // And apply rotations
                (x0, y0, z0) = transformMatrixInv.Apply(x0, y0, z0);
                (normalX, normalY, normalZ) = transformMatrixInv.Apply(normalX, normalY, normalZ);
            }

            // Test for visibility - dot product of nx,ny,nz and the line
            // towards to viewpoint must be positive
            var p = (viewX - x0) * normalX + (viewY - y0) * normalY + (viewZ - z0) * normalZ;
            if (p >= 0)
            {
                // Project from (vx, vy, vz) through (x0,y0,z0) to (-1, y, z)
                var t = (1 + x0) / (x0 - viewX);
                var x = t * viewY + (1 - t) * y0; // Note change of axes
                var y = t * viewZ + (1 - t) * z0;
                x = (x - viewY) * invScaleFactor;
                y = (y - viewZ) * invScaleFactor;

                return (true, new PointD(x,y));
            }

            //point faces away from the viewpoint, so is invisible
            return (false, PointD.None);
        }

        public override RgbaVector AdjustOutputColor(
            RgbaVector inputColor,
            double xProjected, double yProjected, double zProjected,
            TransformParams tParams)
        {
            if (!tParams.sun) return inputColor;

            // Is where we are sunny?
            // Compute pi/2 - angle of the x-axis with the normal at the relevant point.
            // This should be Math.Asin(...) but we are only interested in small angles
            // so assume Math.Sin x = x
            var sunAltitude = sunX * xProjected + sunY * yProjected +
                              sunZ * zProjected;

            return sunAltitude < ProjMath.SunriseAngle
                ? inputColor.Dim(nightDim)
                : inputColor.Dim((float)(0.8 + 0.2 * sunAltitude));
        }
    }
}