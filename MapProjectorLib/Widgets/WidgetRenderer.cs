using System;
using System.Collections.Generic;

using MapProjectorLib.PlotCalculators;
using MapProjectorLib.Extensions;

namespace MapProjectorLib
{
    static public class WidgetRenderer
    {
        static public WidgetRender Render(DestinationImage image, TransformParams tParams, Transform transform)
        {
            var wRender = DrawWidgets(image, tParams, transform);
            return wRender;
        }

        static WidgetRender DrawWidgets(DestinationImage image, TransformParams tParams, Transform transform)
        {
            var wRender = new WidgetRender(image.ImageData);
                        
            foreach(var flag in tParams.Widgets.GetEnabledFlags())
            {
                WidgetDrawActions[flag](wRender, image, tParams, transform);
            }

            return wRender;
        }

        static readonly Dictionary<MapWidget, Action<WidgetRender, DestinationImage, TransformParams, Transform>> 
            WidgetDrawActions = new Dictionary<MapWidget, Action<WidgetRender, DestinationImage, TransformParams, Transform>>
            {
                { MapWidget.None, (wR,dI,tP,t) => { /*noop*/ } },
                { MapWidget.Grid, DrawGrid },
                { MapWidget.TemporaryHours, DrawTemporaryHours },
                { MapWidget.LocalHours, DrawLocalHours },
                { MapWidget.Altitudes, DrawAltitudes },
                { MapWidget.Analemma, DrawAnalemma },
                { MapWidget.Tropics, DrawTropics },
                { MapWidget.Dateline, DrawDateline },
                { MapWidget.Datetime, DrawDatetime },
                { MapWidget.Indicatrix, DrawIndicatrix }
            };


        static void DrawLocalHours(
            WidgetRender wRender,
            DestinationImage image, TransformParams tParams, Transform transform)
        {
            var latCalc = new LatPlotCalculator(image, tParams, transform);
            const int nx = 24;
            for (var i = 0; i < nx; i++)
            {
                var lambda = (i - nx / 2) * 2 * Math.PI / nx +
                             tParams.widgetLon;
                latCalc.Lambda = lambda;
                // Omit the last section of the lines of latitude.
                wRender.PlotLine(
                    -ProjMath.Inclination, +ProjMath.Inclination, latCalc,
                    tParams.WidgetColor, 4);
            }
        }

        static void DrawGrid(
            WidgetRender wRender,
            DestinationImage image, TransformParams tParams, Transform transform)
        {
            var nx = 360 / tParams.gridX;
            var ny = 180 / tParams.gridY;

            var latCalc = new LatPlotCalculator(image, tParams, transform);
            for (var i = 0; i < nx; i++)
            {
                latCalc.Lambda = (i - nx / 2) * ProjMath.TwoPi / nx;
                // Omit the last section of the lines of latitude.
                wRender.PlotLine(
                    -Math.PI / 2, Math.PI / 2, latCalc, tParams.GridColor, 16);
            }

            var longCalc = new LongPlotCalculator(image, tParams, transform);
            for (var i = 0; i <= ny; i++)
            {
                longCalc.Phi = (i - ny / 2) * Math.PI / ny;
                wRender.PlotLine(
                    -Math.PI, Math.PI, longCalc, tParams.GridColor, 16);
            }
        }

        static void DrawAnalemma(WidgetRender wRender,
            DestinationImage image, TransformParams tParams, Transform transform)
        {
            var gridx = tParams.gridX;
            var analemmaCalc = new AnalemmaPlotCalculator(image, tParams, transform);
            var nx = 360 / gridx;
            for (var i = 0; i < nx; i++)
            {
                var time = 2 * Math.PI * (i - nx / 2) / nx;
                analemmaCalc.Time = time;
                wRender.PlotLine(
                    0, ProjMath.TwoPi, analemmaCalc, tParams.WidgetColor,
                    16);
            }
        }

        static void DrawTemporaryHours(
            WidgetRender wRender,
            DestinationImage image, TransformParams tParams, Transform transform)
        {
            var tempCalc = new TempPlotCalculator(image, tParams, transform)
            {
                Lambda = tParams.widgetLon,
                Phi = tParams.widgetLat
            };

            for (var i = 6; i <= 18; i++)
            {
                tempCalc.Time = i;
                wRender.PlotLine(
                    -ProjMath.Inclination, +ProjMath.Inclination, tempCalc,
                    tParams.WidgetColor, 4);
            }
        }

        static void DrawTropics(
            WidgetRender wRender,
            DestinationImage image, TransformParams tParams, Transform transform)
        {
            var longCalc = new LongPlotCalculator(image, tParams, transform);

            longCalc.Phi = ProjMath.Inclination;
            wRender.PlotLine(
                -Math.PI, Math.PI, longCalc, tParams.WidgetColor, 16);

            longCalc.Phi = -ProjMath.Inclination;
            wRender.PlotLine(
                -Math.PI, Math.PI, longCalc, tParams.WidgetColor, 16);
        }

        static void DrawDateline(
            WidgetRender wRender,
            DestinationImage image, TransformParams tParams, Transform transform)
        {
            var day = tParams.widgetDay;
            var longCalc = new LongPlotCalculator(image, tParams, transform);

            // Spring equinox is date 0, and is day 80 of a normal year.
            day -= 80;
            while (day < 0) day += 365;
            while (day >= 365) day -= 365;

            var date = 2 * Math.PI * day / 365;
            longCalc.Phi = ProjMath.SunDec(date);

            wRender.PlotLine(
                -Math.PI, Math.PI, longCalc, tParams.WidgetColor, 16);
        }

        static void DrawDatetime(
            WidgetRender wRender,
            DestinationImage image, TransformParams tParams, Transform transform)
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

            if (inBounds) wRender.PlotPoint(mappedPoint.X, mappedPoint.Y, 1, tParams.WidgetColor);
        }

        static void DrawAltitudes(
            WidgetRender wRender,
            DestinationImage image, TransformParams tParams, Transform transform)
        {
            var altitudesCalc = new AltitudesPlotCalculator(image, tParams, transform)
            {
                Lambda = tParams.widgetLon,
                Phi = tParams.widgetLat
            };

            for (var i = 10; i <= 80; i += 10)
            {
                altitudesCalc.Theta = ProjMath.ToRadians(i);
                wRender.PlotLine(
                    0, ProjMath.TwoPi, altitudesCalc, tParams.WidgetColor, 16);
            }
        }

        static void DrawIndicatrix(
        WidgetRender wRender,
        DestinationImage image, TransformParams tParams, Transform transform)
        {
            var circleCalc = new CirclePlotCalculator(image, tParams, transform)
            {
                Theta = ProjMath.ToRadians(100)
            };

            var nx = 360 / tParams.gridX + 1;
            var ny = 180 / tParams.gridY + 1;
            var skip = 0;
            for (var y = 0; y < ny; y++)
            {
                circleCalc.Phi = (y - ny / 2) * Math.PI / ny;

                if (tParams.widgetSmartSpacing)
                {
                    //as the center of a particular indicatrix gets nearer to the poles, skip
                    //some in that row to stop them overlapping so much
                    skip = (int)Math.Abs(circleCalc.Phi / ProjMath.OneOverPi);
                }

                for (var x = 0; x < nx; x+=(1+skip))
                {
                    circleCalc.Lambda = (x - nx / 2) * ProjMath.TwoPi / nx;

                    wRender.PlotLine(
                       0, ProjMath.TwoPi, circleCalc, tParams.WidgetColor, 16);
                }
            }
        }
    }
}
