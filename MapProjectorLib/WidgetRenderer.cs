﻿using System;
using MapProjectorLib.Plotters;
namespace MapProjectorLib
{
    static public class WidgetRenderer
    {
        static public void Render(Image image, TransformParams tParams, Transform transform)
        {
            if (tParams.Widgets.HasFlag(MapWidget.Grid))
                DrawGrid(image, tParams, transform);

            if (tParams.Widgets.HasFlag(MapWidget.TemporaryHours))
                DrawTemporaryHours(image, tParams, transform);

            if (tParams.Widgets.HasFlag(MapWidget.LocalHours))
                DrawLocalHours(image, tParams, transform);

            if (tParams.Widgets.HasFlag(MapWidget.Altitudes))
                DrawAltitudes(image, tParams, transform);

            if (tParams.Widgets.HasFlag(MapWidget.Analemma))
                //uses gridx
                DrawAnalemma(image, tParams, transform);

            if (tParams.Widgets.HasFlag(MapWidget.Tropics))
                DrawTropics(image, tParams, transform);

            if (tParams.Widgets.HasFlag(MapWidget.Dateline))
                DrawDateline(image, tParams, transform);

            if (tParams.Widgets.HasFlag(MapWidget.Datetime))
                DrawDatetime(image, tParams, transform);

            if (tParams.Widgets.HasFlag(MapWidget.Indicatrix))
                DrawIndicatrix(image, tParams, transform);

        }

        static void DrawLocalHours(
            Image image, TransformParams tParams, Transform transform)
        {
            var latPlotter = new LatPlotter(image, tParams, transform);
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

        static void DrawGrid(
            Image image, TransformParams tParams, Transform transform)
        {
            var gridX = tParams.gridX;
            var gridY = tParams.gridY;
            var nx = 360 / gridX;
            var ny = 180 / gridY;

            for (var i = 0; i < nx; i++)
            {
                var latPlotter = new LatPlotter(image, tParams, transform);

                var lambda = (i - nx / 2) * ProjMath.TwoPi / nx;
                latPlotter.Lambda = lambda;
                // Omit the last section of the lines of latitude.
                //image.PlotLine(-pi/2+radians(gridy), pi/2-radians(gridy), latplotter, cmdtParams.color);
                image.PlotLine(
                    -Math.PI / 2, Math.PI / 2, latPlotter, tParams.gridColor,
                    16);
            }

            var longPlotter = new LongPlotter(image, tParams, transform);
            for (var i = 0; i <= ny; i++)
            {
                longPlotter.Phi = (i - ny / 2) * Math.PI / ny;
                image.PlotLine(
                    -Math.PI, Math.PI, longPlotter, tParams.gridColor, 16);
            }
        }

        static void DrawAnalemma(Image image, TransformParams tParams, Transform transform)
        {
            var gridx = tParams.gridX;
            var analemmaPlotter = new AnalemmaPlotter(image, tParams, transform);
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

        static void DrawTemporaryHours(
            Image image, TransformParams tParams, Transform transform)
        {
            var tempPlotter = new TempPlotter(image, tParams, transform)
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

        static void DrawTropics(
            Image image, TransformParams tParams, Transform transform)
        {
            var longPlotter = new LongPlotter(image, tParams, transform);

            longPlotter.Phi = ProjMath.Inclination;
            image.PlotLine(
                -Math.PI, Math.PI, longPlotter, tParams.widgetColor, 16);

            longPlotter.Phi = -ProjMath.Inclination;
            image.PlotLine(
                -Math.PI, Math.PI, longPlotter, tParams.widgetColor, 16);
        }

        static void DrawDateline(
            Image image, TransformParams tParams, Transform transform)
        {
            var day = tParams.widgetDay;
            var longPlotter = new LongPlotter(image, tParams, transform);

            // Spring equinox is date 0, and is day 80 of a normal year.
            day -= 80;
            while (day < 0) day += 365;
            while (day >= 365) day -= 365;

            var date = 2 * Math.PI * day / 365;
            longPlotter.Phi = ProjMath.SunDec(date);

            image.PlotLine(
                -Math.PI, Math.PI, longPlotter, tParams.widgetColor, 16);
        }

        static void DrawDatetime(
            Image image, TransformParams tParams, Transform transform)
        {
            var day = tParams.widgetDay;

            // Spring equinox is day 0
            day -= 80;
            while (day < 0) day += 365;
            while (day >= 365) day -= 365;

            var time = day % 1.0d - 0.5; // Time relative to noon
            var date = 2 * Math.PI * day / 365;
            var phi = ProjMath.SunDec(date);
            var eot = ProjMath.EquationOfTime(date) / ProjMath.SecondsPerDay;
            var apparentTime = time + eot; // AT = Mean Time + EOT
            var lambda = -(2 * Math.PI * apparentTime);
            (var inBounds, var mappedPoint) = transform.MapXY(image, tParams, phi, lambda);

            if (inBounds) image.PlotPoint(mappedPoint.X, mappedPoint.Y, 1, tParams.widgetColor);
        }

        static void DrawAltitudes(
            Image image, TransformParams tParams, Transform transform)
        {
            var altitudesPlotter = new AltitudesPlotter(image, tParams, transform)
            {
                Lambda = tParams.widgetLon,
                Phi = tParams.widgetLat
            };

            for (var i = 10; i <= 80; i += 10)
            {
                altitudesPlotter.Theta = ProjMath.ToRadians(i);
                image.PlotLine(
                    0, ProjMath.TwoPi, altitudesPlotter, tParams.widgetColor, 16);
            }
        }

        static void DrawIndicatrix(
        Image image, TransformParams tParams, Transform transform)
        {
            var circlePlotter = new CirclePlotter(image, tParams, transform)
            {
                Theta = ProjMath.ToRadians(100)
            };


            var nx = 360 / tParams.gridX + 1;
            var ny = 180 / tParams.gridY + 1;
            var skip = 0;
            for (var y = 0; y < ny; y++)
            {
                circlePlotter.Phi = (y - ny / 2) * Math.PI / ny;

                if (tParams.widgetSmartSpacing)
                {
                    //as the center of a particular indicatrix gets nearer to the poles, skip
                    //some in that row to stop them overlapping so much
                    skip = (int)Math.Abs(circlePlotter.Phi / ProjMath.OneOverPi);
                }

                for (var x = 0; x < nx; x+=(1+skip))
                {
                    circlePlotter.Lambda = (x - nx / 2) * ProjMath.TwoPi / nx;

                    image.PlotLine(
                       0, ProjMath.TwoPi, circlePlotter, tParams.widgetColor, 16);
                }
            }
        }
    }
}
