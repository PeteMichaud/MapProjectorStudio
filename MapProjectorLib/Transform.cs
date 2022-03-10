using System;
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

        protected abstract (bool inBounds, PointD mappedPoint) ProjectInv(
            TransformParams tParams,
            double phi, double lambda);

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

        public virtual RgbaVector AdjustOutputColor(
            RgbaVector inputColor, double x, double y, double z,
            TransformParams tParams)
        {
            return inputColor;
        }

        public abstract double BasicScale(int width, int height);

        //

        public void TransformImage(
            SamplableImage inImage, DestinationImage outImage,
            TransformParams tParams)
        {
            // precomputed values for the hot loop
            var xOrigin = 0.5 * outImage.Width;
            var yOrigin = 0.5 * outImage.Height;
            var halfWidth = inImage.Width * 0.5;
            var halfHeight = inImage.Height * 0.5;
            var scaledWidth = inImage.Width * ProjMath.OneOverTwoPi;
            var scaledHeight = inImage.Height * ProjMath.OneOverPi;

            // Scale so that half width is 1
            var scaleFactor = BasicScale(outImage.Width, outImage.Height) / tParams.scale;

            //var operation = new RowOperation<Rgb24>(
            //    inImage._image.Frames[0].PixelBuffer,
            //    outImage._image.Frames[0].PixelBuffer);
            
            //ParallelRowIterator.IterateRows<RowOperation<Rgb24>, Rgb24>(
            //                operation.configuration,
            //                outImage._image.Bounds(),
            //                in operation);

            //
            for (var outY = 0; outY < outImage.Height; outY++)
            //Parallel.For(0, outImage.Height, outY =>
            {
                var y = scaleFactor * (yOrigin - outY - 0.5) + tParams.yOffset;
                if (tParams.rotate == 0) SetY(y);

                //Parallel.For(0, outImage.Width, outX =>
                for (int outX = 0; outX < outImage.Width; outX++)
                {
                    var x1 = scaleFactor * (outX + 0.5 - xOrigin) + tParams.xOffset;
                    var y1 = y;

                    if(tParams.rotate != 0)
                    {
                        (x1, y1) = ProjMath.ApplyRotation(-tParams.rotate, x1, y1);
                        SetY(y1);
                    }

                    double x0 = 0.0, y0 = 0.0, z0 = 0.0;
                    double phi = 0.0, lambda = 0.0;

                    bool inProjectionBounds = Project(tParams, x1, y1,
                        ref x0, ref y0, ref z0,
                        ref phi, ref lambda
                    );

                    if (inProjectionBounds && ProjMath.IsPointWithinRadius(tParams, phi, lambda))
                    {
                        var scaledX = halfWidth + lambda * scaledWidth;
                        var scaledY = halfHeight - phi * scaledHeight;

                        var outColor = AdjustOutputColor(
                            inImage.Sample(scaledX, scaledY), x0, y0, z0, tParams);

                        outImage[outX, outY] = outColor;
                    }
                    else
                    {
                        tParams.SomeDestinationPixelsAreBlank = true;
                    }
                }
                //);
            }
            //);

        }

        public void TransformImageInv(
            SamplableImage inImage, DestinationImage outImage,
            TransformParams tParams)
        {
            var outWidth = outImage.Width;
            var outHeight = outImage.Height;
            var xOrigin = 0.5 * outWidth;
            var yOrigin = 0.5 * outHeight;

            // Now scan across the output image
            for (var oy = 0; oy < outHeight; oy++)
            {
                for (var ox = 0; ox < outWidth; ox++)
                {
                    // Compute lat and long
                    var phi = (yOrigin - oy - 0.5) * Math.PI / outHeight;
                    var lambda = (ox + 0.5 - xOrigin) * ProjMath.TwoPi / outWidth;

                    if (!ProjMath.IsPointWithinRadius(tParams, phi, lambda)) continue;

                    // Compute the scaled x,y coordinates for <phi,lambda>
                    (var inverseProjectionInBounds, var projectedPoint) =
                        ProjectInv(tParams, phi, lambda);

                    if (inverseProjectionInBounds)
                    {
                        SetDataInv(inImage, outImage, tParams, ox, oy, projectedPoint.X, projectedPoint.Y);
                    }
                }
            }
        }

        bool SetDataInv(
            SamplableImage srcImage, DestinationImage outImage,
            TransformParams tParams,
            double outX, double outY, // Coordinates in output image
            double x, double y // scaled coordinates in input image
        )
        {
            var imgWidth = srcImage.Width;
            var imgHeight = srcImage.Height;
            var xOrigin = 0.5 * imgWidth;
            var yOrigin = 0.5 * imgHeight;

            // Scale so that half width is 1
            var scaleFactor = BasicScale(imgWidth, imgHeight) / tParams.scale;

            if (tParams.rotate != 0)
            {
                (x, y) = ProjMath.ApplyRotation(tParams.rotate, x, y);
            }

            var inX = (xOrigin + (x - tParams.xOffset) / scaleFactor);
            var inY = (yOrigin - (y - tParams.yOffset) / scaleFactor);

            if (inX < 0 || inX >= imgWidth || inY < 0 || inY >= imgHeight)
                return false;

            outImage[(int) outX, (int) outY] = srcImage.Sample(inX, inY);
            return true;
        }

        //Map phi,lambda (lat,long) to x,y image coords
        internal (bool inBounds, PointD mappedPoint) MapXY(
            DestinationImage outImage,
            TransformParams tParams,
            double phi, double lambda)
        {
            
            if (!ProjMath.IsPointWithinRadius(tParams, phi, lambda)) return (false, PointD.None);

            // Get projection coordinates for x and y, if any exist
            (var mappingWithinImageBounds, var mappedPoint) = ProjectInv(tParams, phi, lambda);

            if (!mappingWithinImageBounds) return (false, PointD.None);

            var outImgWidth = outImage.Width;
            var outImgHeight = outImage.Height;

            var xCenter = 0.5 * outImgWidth;
            var yCenter = 0.5 * outImgHeight;
            var scaleFactor = BasicScale(outImgWidth, outImgHeight) /
                                tParams.scale;

            // Now x and y are in 2pi scale
            var x = mappedPoint.X;
            var y = mappedPoint.Y;

            if (tParams.rotate != 0)
            {
                (x, y) = ProjMath.ApplyRotation(tParams.rotate, x, y);
            }

            x = xCenter + (x - tParams.xOffset) / scaleFactor;
            y = yCenter + (y - tParams.yOffset) / -scaleFactor;

            return (true, new PointD(x,y));
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

    }
}