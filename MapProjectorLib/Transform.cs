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

        void SetData(
            Image inImage, Image outImage,
            TransformParams tParams,
            double x, double y, double z,
            double phi, double lambda,
            int ox, int oy)
        {
            var imgWidth = inImage.Width;
            var imgHeight = inImage.Height;

            var halfWidth = imgWidth * 0.5;
            var halfHeight = imgHeight * 0.5;
            var scaledWidth = imgWidth * ProjMath.OneOverTwoPi;
            var scaledHeight = imgHeight * ProjMath.OneOverPi;

            // Use unsigned so we don't have to test for negative indices
            var scaledX = (int) Math.Floor(halfWidth + lambda * scaledWidth);
            // Clamp in case of rounding errors
            if (scaledX >= imgWidth) scaledX = 0;

            // Use unsigned so we don't have to test for negative indices

            var scaledY = (int) Math.Floor(halfHeight - phi * scaledHeight);
            if (scaledY >= imgHeight) scaledY = imgHeight - 1;

            var outColor = AdjustOutputColor(
                inImage[scaledX, scaledY], x, y, z, tParams);

            outImage[ox, oy] = outColor;
        }

        bool IsPointWithinRadius(
            TransformParams tParams, double phi, double lambda)
        {
            if (tParams.radius == 0) return true;

            return Distance(phi, lambda, tParams.lat, tParams.lon) <=
                   tParams.radius;
        }

        static void ApplyRotation(double r, ref double x, ref double y)
        {
            var x1 = x * Math.Cos(r) + y * Math.Sin(r);
            var y1 = -x * Math.Sin(r) + y * Math.Cos(r);

            x = x1;
            y = y1;
        }

        public void TransformImage(
            Image inImage, Image outImage,
            TransformParams tParams)
        {
            var outWidth = outImage.Width;
            var outHeight = outImage.Height;
            var xOrigin = 0.5 * outWidth;
            var yOrigin = 0.5 * outHeight;

            // Scale so that half width is 1
            var scaleFactor = BasicScale(outWidth, outHeight) / tParams.scale;

            for (var outY = 0; outY < outHeight; outY++)
            {
                var y = scaleFactor * (yOrigin - outY - 0.5) + tParams.yOffset;
                if (tParams.rotate == 0) SetY(y);

                for (var outX = 0; outX < outWidth; outX++)
                {
                    // Really ought to just apply a 3x2 matrix to the point
                    var x = scaleFactor * (outX + 0.5 - xOrigin) +
                            tParams.xOffset;
                    var x1 = x;
                    var y1 = y;

                    if (tParams.rotate != 0)
                    {
                        ApplyRotation(-tParams.rotate, ref x1, ref y1);
                        SetY(y1);
                    }

                    double x0 = 0.0, y0 = 0.0, z0 = 0.0;
                    double phi = 0.0, lambda = 0.0;

                    var projSuccess = Project(
                        tParams, x1, y1, 
                        ref x0, ref y0, ref z0, 
                        ref phi, ref lambda);

                    var isWithinRadius = IsPointWithinRadius(tParams, phi, lambda);

                    if (projSuccess && isWithinRadius)
                    {
                        SetData(
                            inImage, outImage, tParams, 
                            x0, y0, z0, 
                            phi, lambda,
                            outX, outY);
                    }
                    else if (!tParams.noback)
                    {
                        outImage[outX, outY] = tParams.backgroundColor;
                    }
                }
            }
        }

        public void TransformImageInv(
            Image inImage, Image outImage,
            TransformParams tParams)
        {
            var ow = outImage.Width;
            var oh = outImage.Height;
            var xOrigin = 0.5 * ow;
            var yOrigin = 0.5 * oh;

            // Now scan across the output image
            for (var oy = 0; oy < oh; oy++)
            for (var ox = 0; ox < ow; ox++)
            {
                // Compute lat and long
                var phi = (yOrigin - oy - 0.5) * Math.PI / oh;
                var lambda = (ox + 0.5 - xOrigin) * ProjMath.TwoPi / ow;

                // Compute the scaled x,y coordinates for <phi,lambda>
                double x = 0.0, y = 0.0;
                if (!IsPointWithinRadius(tParams, phi, lambda) ||
                    !ProjectInv(tParams, phi, lambda, ref x, ref y) ||
                    !SetDataInv(inImage, outImage, tParams, ox, oy, x, y))
                    if (!tParams.noback)
                        outImage[ox, oy] = tParams.backgroundColor;
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
                ApplyRotation(tParams.rotate, ref x, ref y);

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
                    ApplyRotation(tParams.rotate, ref x, ref y);

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
            double x, double y, double z,
            Matrix3 m)
        {
            if (m.isIdentity) return;

            x = Math.Cos(lambda) * Math.Cos(phi);
            y = Math.Sin(lambda) * Math.Cos(phi);
            z = Math.Sin(phi);

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

        // Commands

        public void DrawWidgets(Image image, TransformParams tParams)
        {
            if (tParams.Widgets.HasFlag(MapWidget.Grid))
                DrawGrid(image, tParams);

            if (tParams.Widgets.HasFlag(MapWidget.TemporaryHours))
                DrawTemporaryHours(image, tParams);

            if (tParams.Widgets.HasFlag(MapWidget.LocalHours))
                DrawLocalHours(image, tParams);

            if (tParams.Widgets.HasFlag(MapWidget.Altitudes))
                DrawAltitudes(image, tParams);

            if (tParams.Widgets.HasFlag(MapWidget.Analemma))
                //uses gridx
                DrawAnalemma(image, tParams);

            if (tParams.Widgets.HasFlag(MapWidget.Tropics))
                DrawTropics(image, tParams);

            if (tParams.Widgets.HasFlag(MapWidget.Dateline))
                DrawDateline(image, tParams);

            if (tParams.Widgets.HasFlag(MapWidget.Datetime))
                DrawDatetime(image, tParams);
        }

        void DrawLocalHours(Image image, TransformParams tParams)
        {
            var latPlotter = new LatPlotter(image, tParams, this);
            const int nx = 24;
            for (var i = 0; i < nx; i++)
            {
                var lambda = (i - nx / 2) * 2 * Math.PI / nx +
                             tParams.widgetLon;
                latPlotter.Lambda = lambda;
                // Omit the last section of the lines of latitude.
                image.PlotLine(
                    -ProjMath.Inclination, +ProjMath.Inclination, latPlotter,
                    tParams.widgetColor, 4);
            }
        }

        void DrawGrid(
            Image image,
            TransformParams tParams)
        {
            var gridX = tParams.gridX;
            var gridY = tParams.gridY;
            var nx = 360 / gridX;
            var ny = 180 / gridY;

            for (var i = 0; i < nx; i++)
            {
                var latPlotter = new LatPlotter(image, tParams, this);

                var lambda = (i - nx / 2) * 2 * Math.PI / nx;
                latPlotter.Lambda = lambda;
                // Omit the last section of the lines of latitude.
                //image.PlotLine(-pi/2+radians(gridy), pi/2-radians(gridy), latplotter, cmdtParams.color);
                image.PlotLine(
                    -Math.PI / 2, Math.PI / 2, latPlotter, tParams.gridColor,
                    16);
            }

            var longPlotter = new LongPlotter(image, tParams, this);
            for (var i = 0; i <= ny; i++)
            {
                longPlotter.Phi = (i - ny / 2) * Math.PI / ny;
                image.PlotLine(
                    -Math.PI, Math.PI, longPlotter, tParams.gridColor, 16);
            }
        }

        void DrawAnalemma(Image image, TransformParams tParams)
        {
            var gridx = tParams.gridX;
            var analemmaPlotter = new AnalemmaPlotter(image, tParams, this);
            var nx = 360 / gridx;
            for (var i = 0; i < nx; i++)
            {
                var time = 2 * Math.PI * (i - nx / 2) / nx;
                analemmaPlotter.Time = time;
                image.PlotLine(
                    0, ProjMath.TwoPi, analemmaPlotter, tParams.widgetColor,
                    16);
            }
        }

        void DrawAltitudes(Image image, TransformParams tParams)
        {
            var fooPlotter = new FooPlotter(image, tParams, this)
            {
                Lambda = tParams.widgetLon,
                Phi = tParams.widgetLat
            };

            for (var i = 10; i <= 80; i += 10)
            {
                fooPlotter.Theta = ProjMath.ToRadians(i);
                image.PlotLine(
                    0, ProjMath.TwoPi, fooPlotter, tParams.widgetColor, 16);
            }
        }

        void DrawTemporaryHours(
            Image image,
            TransformParams tParams)
        {
            var tempPlotter = new TempPlotter(image, tParams, this)
            {
                Lambda = tParams.widgetLon,
                Phi = tParams.widgetLat
            };

            for (var i = 6; i <= 18; i++)
            {
                tempPlotter.Time = i;
                image.PlotLine(
                    -ProjMath.Inclination, +ProjMath.Inclination, tempPlotter,
                    tParams.widgetColor, 4);
            }
        }

        void DrawTropics(
            Image image,
            TransformParams tParams)
        {
            var longPlotter = new LongPlotter(image, tParams, this)
            {
                Phi = ProjMath.Inclination
            };

            image.PlotLine(
                -Math.PI, Math.PI, longPlotter, tParams.widgetColor, 16);
            longPlotter.Phi = -ProjMath.Inclination;
            image.PlotLine(
                -Math.PI, Math.PI, longPlotter, tParams.widgetColor, 16);
        }

        void DrawDateline(Image image, TransformParams tParams)
        {
            var day = tParams.widgetDay;
            var longPlotter = new LongPlotter(image, tParams, this);

            // Spring equinox is date 0, and is day 80 of a normal year.
            day -= 80;
            while (day < 0) day += 365;
            while (day >= 365) day -= 365;

            var date = 2 * Math.PI * day / 365;
            longPlotter.Phi = ProjMath.SunDec(date);

            image.PlotLine(
                -Math.PI, Math.PI, longPlotter, tParams.widgetColor, 16);
        }

        void DrawDatetime(Image image, TransformParams tParams)
        {
            var day = tParams.widgetDay;

            // Spring equinox is day 0
            day -= 80;
            while (day < 0) day += 365;
            while (day >= 365) day -= 365;

            var time = day % 1.0d - 0.5; // Time relative to noon
            var date = 2 * Math.PI * day / 365;
            var phi = ProjMath.SunDec(date);
            var eot = ProjMath.EquationOfTime(date) / (24 * 60 * 60);
            var apparentTime = time + eot; // AT = MT + EOT
            var lambda = -(2 * Math.PI * apparentTime);
            double x = 0.0, y = 0.0;
            MapXY(image, tParams, phi, lambda, ref x, ref y);

            image.PlotPoint(x, y, 1, tParams.widgetColor);
        }
    }
}