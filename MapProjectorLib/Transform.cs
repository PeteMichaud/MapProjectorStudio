using System;
using MapProjectorLib.Projections;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Advanced;

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

        public (bool inProjectionBounds, double x1, double y1, double z1, double phi, double lambda) 
        Project(TransformParams tParams, double x, double y)
        {
            return Project(tParams, x, y, 0d, 0d, 0d, 0d, 0d);
        }

        public abstract (bool inProjectionBounds, double x1, double y1, double z1, double phi, double lambda) Project(
            TransformParams tParams,
            double x, double y,
            double x1, double y1, double z1,
            double phi, double lambda);

        public abstract (bool inBounds, PointD mappedPoint) ProjectInv(
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
            var operation = new TransformRowOperation(
                inImage.ImageData, outImage.ImageData,
                tParams, this, inImage.ColorSampleMode);

            ParallelRowIterator.IterateRows(
                operation.configuration,
                outImage.ImageData.Bounds(),
                in operation);
        }

        public void TransformImageInv(
            SamplableImage inImage, DestinationImage outImage,
            TransformParams tParams)
        {
            var operation = new TransformRowInverseOperation(
              inImage.ImageData, outImage.ImageData,
              tParams, this, inImage.ColorSampleMode);

            ParallelRowIterator.IterateRows(
                operation.configuration,
                outImage.ImageData.Bounds(),
                in operation);
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
        protected static (double phi, double lambda) ConvertLatLong(
            double phi, double lambda,
            Matrix3 m)
        {
            if (m.isIdentity) return (phi, lambda);

            var x = Math.Cos(lambda) * Math.Cos(phi);
            var y = Math.Sin(lambda) * Math.Cos(phi);
            var z = Math.Sin(phi);

            m.Apply(ref x, ref y, ref z);
            
            phi = Math.Asin(z);
            lambda = Math.Atan2(y, x);
            return (phi, lambda);
        }

    }
}