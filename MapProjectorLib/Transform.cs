using System;
using MapProjectorLib.Plotters;
using MapProjectorLib.Projections;
using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorLib
{
    public abstract class Transform
    {
        protected Matrix3 transformMatrix;
        protected Matrix3 transformMatrixInv;

        protected Transform()
        {
            transformMatrix = new Matrix3();
            transformMatrixInv = new Matrix3();
        }

        public static Transform GetTransform(MapProjection projection)
        {
            switch (projection)
            {
                case MapProjection.LatLong:
                case MapProjection.Equirect:
                case MapProjection.Equirectangular:
                    return new LatLong();
                case MapProjection.EqualArea:
                    return new EqualArea();
                case MapProjection.Sinusoidal:
                    return new Sinusoidal();
                case MapProjection.Sinusoidal2:
                    return new Sinusoidal2();
                case MapProjection.Mollweide:
                    return new Mollweide();
                case MapProjection.Mercator:
                    return new Mercator();
                case MapProjection.Cylindrical:
                    return new Cylindrical();
                case MapProjection.Azimuthal:
                    return new Azimuthal();
                case MapProjection.Orthographic:
                case MapProjection.Rectilinear:
                    return new Rectilinear();
                case MapProjection.Stereographic:
                    return new Stereographic();
                case MapProjection.Gnomonic:
                    return new Gnomonic();
                case MapProjection.Perspective:
                    return new Perspective();
                case MapProjection.Bonne:
                    return new Bonne();
                case MapProjection.Hammer:
                    return new Hammer();
                default:
                    throw new ArgumentException(
                        "Projection Not Supported", nameof(projection));
            }
        }

        // Abstract Methods

        public abstract bool Project(
            TransformParams tParams,
            double x, double y,
            ref double x1, ref double y1, ref double z1,
            ref double phi, ref double lambda);

        protected abstract bool ProjectInv(
            TransformParams tParams,
            double phi, double lambda,
            ref double x, ref double y);

        // Virtual Methods

        public virtual void Init(TransformParams tParams)
        {
            transformMatrix.Identity();
            transformMatrix *= new Matrix3(RotationAxis.Z, tParams.turn);
            transformMatrix *= new Matrix3(RotationAxis.X, tParams.tilt);
            transformMatrix *= new Matrix3(RotationAxis.Y, tParams.lat);
            transformMatrix *= new Matrix3(RotationAxis.Z, tParams.lon);

            // Construct the inverse matrix
            transformMatrixInv.Identity();
            transformMatrixInv *= new Matrix3(RotationAxis.Z, -tParams.lon);
            transformMatrixInv *= new Matrix3(RotationAxis.Y, -tParams.lat);
            transformMatrixInv *= new Matrix3(RotationAxis.X, -tParams.tilt);
            transformMatrixInv *= new Matrix3(RotationAxis.Z, -tParams.turn);
        }

        public virtual (int w, int h) AdjustSize(
            int w, int h, TransformParams tParams)
        {
            return (w, h);
        }

        public virtual void SetY(double y)
        {
            /* noop */
        }

        protected virtual Rgb24 AdjustOutputColor(
            Rgb24 inputColor, double x, double y, double z,
            TransformParams tParams)
        {
            return inputColor;
        }

        public abstract double BasicScale(int width, int height);

        //
        static bool IsPointWithinRadius(
            TransformParams tParams, double phi, double lambda)
        {
            if (tParams.radius == 0) return true;

            return Distance(phi, lambda, tParams.lat, tParams.lon) <=
                   tParams.radius;
        }

        static (double x, double y) ApplyRotation(double r, double x, double y)
        {
            return (
                x: x * Math.Cos(r) + y * Math.Sin(r), 
                y: -x * Math.Sin(r) + y * Math.Cos(r)
            );
        }

        public void TransformImage(
            Image inImage, Image outImage,
            TransformParams tParams)
        {
            // precomputed values for the hot loop
            var imgWidth = inImage.Width;
            var imgHeight = inImage.Height;
            var outWidth = outImage.Width;
            var outHeight = outImage.Height;
            var xOrigin = 0.5 * outWidth;
            var yOrigin = 0.5 * outHeight;
            var halfWidth = imgWidth * 0.5;
            var halfHeight = imgHeight * 0.5;
            var scaledWidth = imgWidth * ProjMath.OneOverTwoPi;
            var scaledHeight = imgHeight * ProjMath.OneOverPi;
            // Scale so that half width is 1
            var scaleFactor = BasicScale(outWidth, outHeight) / tParams.scale;
            //

            outImage.ProcessPixelRows(outAccessor =>
            {
                for (int outY = 0; outY < outAccessor.Height; outY++)
                {
                    var y = scaleFactor * (yOrigin - outY - 0.5) + tParams.yOffset;
                    if (tParams.rotate == 0) SetY(y);

                    Span<Rgb24> outPixelRow = outAccessor.GetRowSpan(outY);

                    // pixelRow.Length has the same value as accessor.Width,
                    // but using pixelRow.Length allows the JIT to optimize away bounds checks:
                    for (int outX = 0; outX < outPixelRow.Length; outX++)
                    {
                        ref Rgb24 outPixel = ref outPixelRow[outX];

                        var x1 = scaleFactor * (outX + 0.5 - xOrigin) +
                            tParams.xOffset;
                        var y1 = y;

                        if (tParams.rotate != 0)
                        {
                            (x1, y1) = ApplyRotation(-tParams.rotate, x1, y1);
                            SetY(y1);
                        }

                        double x0 = 0.0, y0 = 0.0, z0 = 0.0;
                        double phi = 0.0, lambda = 0.0;

                        bool inProjectionBounds = Project(tParams, x1, y1,
                            ref x0, ref y0, ref z0,
                            ref phi, ref lambda);

                        if (inProjectionBounds && IsPointWithinRadius(tParams, phi, lambda))
                        {
                            // Use unsigned so we don't have to test for negative indices
                            var scaledX = (int)Math.Floor(halfWidth + lambda * scaledWidth);
                            // Clamp in case of rounding errors
                            if (scaledX >= imgWidth) scaledX = 0;

                            // Use unsigned so we don't have to test for negative indices

                            var scaledY = (int)Math.Floor(halfHeight - phi * scaledHeight);
                            if (scaledY >= imgHeight) scaledY = imgHeight - 1;

                            var outColor = AdjustOutputColor(
                                inImage[scaledX, scaledY], x0, y0, z0, tParams);

                            outPixel = outColor;
                        }
                    }
                }
            });

        }

        public void TransformImageInv(
            Image inImage, Image outImage,
            TransformParams tParams)
        {
            var outWidth = outImage.Width;
            var outHeight = outImage.Height;
            var xOrigin = 0.5 * outWidth;
            var yOrigin = 0.5 * outHeight;

            // Now scan across the output image
            for (var oy = 0; oy < outHeight; oy++)
            for (var ox = 0; ox < outWidth; ox++)
            {
                // Compute lat and long
                var phi = (yOrigin - oy - 0.5) * Math.PI / outHeight;
                var lambda = (ox + 0.5 - xOrigin) * ProjMath.TwoPi / outWidth;

                // Compute the scaled x,y coordinates for <phi,lambda>
                double x = 0.0, y = 0.0;
                if (!IsPointWithinRadius(tParams, phi, lambda) ||
                    !ProjectInv(tParams, phi, lambda, ref x, ref y) ||
                    !SetDataInv(inImage, outImage, tParams, ox, oy, x, y))
                {
                }
            }
        }

        bool SetDataInv(
            Image inImage, Image outImage,
            TransformParams tParams,
            double outX, double outY, // Coordinates in output image
            double x, double y // scaled coordinates in input image
        )
        {
            var imgWidth = inImage.Width;
            var imgHeight = inImage.Height;
            var xOrigin = 0.5 * imgWidth;
            var yOrigin = 0.5 * imgHeight;

            // Scale so that half width is 1
            var scaleFactor = BasicScale(imgWidth, imgHeight) / tParams.scale;

            // This is what we are inverting
            //double x = scaleFactor * (ix - orgx) + tParams.xoff;
            //double y = scaleFactor * (orgy - iy) + tParams.yoff;
            //applyRotation(-tParams.rotate,x,y)

            if (tParams.rotate != 0)
            {
                (x, y) = ApplyRotation(tParams.rotate, x, y);
            }

            var ix = (int) Math.Floor(
                xOrigin + (x - tParams.xOffset) / scaleFactor);
            var iy = (int) Math.Floor(
                yOrigin - (y - tParams.yOffset) / scaleFactor);

            if (ix < 0 || ix >= imgWidth || iy < 0 || iy >= imgHeight)
                return false;

            outImage[(int) outX, (int) outY] = inImage[ix, iy];
            return true;
        }

        public bool MapXY(
            Image outImage,
            TransformParams tParams,
            double phi, double lambda,
            ref double x, ref double y)
        {
            // Set x and y to where phi and lambda are mapped to
            // x, y are in image coordinates
            // Get projection coordinates for x and y
            if (IsPointWithinRadius(tParams, phi, lambda) &&
                ProjectInv(tParams, phi, lambda, ref x, ref y))
            {
                // Now x and y are in 2pi scale
                var outImgWidth = outImage.Width;
                var outImgHeight = outImage.Height;

                var xOrigin = 0.5 * outImgWidth;
                var yOrigin = 0.5 * outImgHeight;
                var scaleFactor = BasicScale(outImgWidth, outImgHeight) /
                                  tParams.scale;

                if (tParams.rotate != 0)
                {
                       (x, y) = ApplyRotation(tParams.rotate, x, y);
                }
                x = xOrigin + (x - tParams.xOffset) / scaleFactor;
                y = yOrigin + (y - tParams.yOffset) / -scaleFactor;

                return true;
            }

            return false;
        }

        // Apply matrix to phi, lambda, and put the resulting
        // cartesian coords in x,y,z.
        protected static void ConvertLatLong(
            ref double phi, ref double lambda,
            Matrix3 m)
        {
            if (m.isIdentity) return;

            var x = Math.Cos(lambda) * Math.Cos(phi);
            var y = Math.Sin(lambda) * Math.Cos(phi);
            var z = Math.Sin(phi);

            m.Apply(ref x, ref y, ref z);
            
            phi = Math.Asin(z);
            lambda = Math.Atan2(y, x);
        }

        static double Distance(
            double phi0, double lambda0, double phi1, double lambda1)
        {
            var x0 = Math.Cos(lambda0) * Math.Cos(phi0);
            var y0 = Math.Sin(lambda0) * Math.Cos(phi0);
            var z0 = Math.Sin(phi0);
            var x1 = Math.Cos(lambda1) * Math.Cos(phi1);
            var y1 = Math.Sin(lambda1) * Math.Cos(phi1);
            var z1 = Math.Sin(phi1);
            var d = Math.Acos(x0 * x1 + y0 * y1 + z0 * z1);

            return d;
        }

        // Widgets


    }
}