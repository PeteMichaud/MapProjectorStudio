using System;
using System.Collections.Generic;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorLib.Plotters
{
    class WidgetPlotter
    {
        Image<RgbaVector> ImageData;

        public WidgetPlotter(Image<RgbaVector> imageData)
        {
            ImageData = imageData;
        }

        public void PlotLine(
            double t0, double t1,
            LinePlotCalculator linePlotter,
            RgbaVector color,
            int naiveLineResolution,
            int maxRecursiveDetailPerSegment = 10)
        {
            //if a segment is too distorted PlotLineSegment will try to
            //split it up, which is why the initial line resolution is "naive"
            var incr = (t1 - t0) / naiveLineResolution;
            for (var i = 0; i < naiveLineResolution; i++)
                PlotLineSegment(
                    t0 + i * incr, t0 + (i + 1) * incr,
                    linePlotter, color, maxRecursiveDetailPerSegment);
        }

        public void PlotPoint(double x, double y, int r, RgbaVector color)
        {
            for (var j = -r; j <= r; j++)
                for (var i = -r; i <= r; i++)
                    SafeSetPixel((int)(x + i), (int)(y + j), color);
        }

        void DrawLine(int xStart, int yStart, int xEnd, int yEnd, RgbaVector color)
        {
            foreach (var pt in GetPointsOnLine(xStart, yStart, xEnd, yEnd))
            {
                SafeSetPixel(pt.X, pt.Y, color);
            }
        }

        // Bresenham's Line Algorithm
        internal static IEnumerable<Point> GetPointsOnLine(int x0, int y0, int x1, int y1)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                int t;
                t = x0; // swap x0 and y0
                x0 = y0;
                y0 = t;
                t = x1; // swap x1 and y1
                x1 = y1;
                y1 = t;
            }

            if (x0 > x1)
            {
                int t;
                t = x0; // swap x0 and x1
                x0 = x1;
                x1 = t;
                t = y0; // swap y0 and y1
                y0 = y1;
                y1 = t;
            }

            int dx = x1 - x0;
            int dy = Math.Abs(y1 - y0);
            int error = dx / 2;
            int ystep = (y0 < y1) ? 1 : -1;
            int y = y0;
            for (int x = x0; x <= x1; x++)
            {
                yield return new Point((steep ? y : x), (steep ? x : y));
                error -= dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
            yield break;
        }

        // Sanity check for debugging purposes.
        // PlotLineSegment checks the limit before recursing so when the program runs in
        // production it never passes the recursion limit. This check is ensure a future
        // bug can't be introduced, but it also compiles the check out of the hot loop
        // for performance reasons
        [System.Diagnostics.Conditional("DEBUG")]
        void DebugCheckRecursionLimit(int recursionLimit)
        {
            if (recursionLimit < 0) throw new Exception("Recursion too deep, check recursionLimit before calling");
        }

        void PlotLineSegment(
            double progressAlongPlotStart, double progressAlongPlotEnd,
            LinePlotCalculator linePlotter,
            RgbaVector color,
            int recursionLimit)
        {
            DebugCheckRecursionLimit(recursionLimit);

            void SplitSegment()
            {
                var progressAlongPlotMid = (progressAlongPlotStart + progressAlongPlotEnd) / 2;
                PlotLineSegment(progressAlongPlotStart, progressAlongPlotMid, linePlotter, color, recursionLimit - 1);
                PlotLineSegment(progressAlongPlotMid, progressAlongPlotEnd, linePlotter, color, recursionLimit - 1);
            }

            (var startInBounds, var mappedStart) = linePlotter.GetXY(progressAlongPlotStart);
            (var endInBounds, var mappedEnd) = linePlotter.GetXY(progressAlongPlotEnd);

            if (startInBounds && endInBounds)
            {
                //todo: make parameter or vary by resolution
                var tooLongThreshhold = 10;
                //if the line is too long it'll make a big, ugly straight line where a curve should be
                var isTooLong = Math.Abs(mappedStart.X - mappedEnd.X) + Math.Abs(mappedStart.Y - mappedEnd.Y) >= tooLongThreshhold;
                if (isTooLong && recursionLimit > 0)
                {
                    SplitSegment();
                }
                else //the length is fine, or it's too long but we're at the recursion limit, so just draw the line
                {
                    DrawLine((int)mappedStart.X, (int)mappedStart.Y, (int)mappedEnd.X, (int)mappedEnd.Y, color);
                }
            }
            else // one or both points are outside the map
            {
                //we have one point in bounds, so we're going to keep trying to split this segment in half
                //until we find a segment that's fully contained in the image

                //I treat this recursion differently here because the recursion limit is about curvature detail,
                //but this is a different issue. I still can't leave it unbound because there's a chance
                //that no line will be legal

                var truncationLimit = 10;

                if (startInBounds)
                {
                    if (recursionLimit > 0)
                    {
                        var progressAlongPlotMid = (progressAlongPlotStart + progressAlongPlotEnd) / 2;
                        PlotLineSegment(progressAlongPlotStart, progressAlongPlotMid, linePlotter, color, truncationLimit);
                    }
                    //else we can't split any more, so give up
                }
                else if (endInBounds)
                {
                    if (recursionLimit > 0)
                    {
                        var progressAlongPlotMid = (progressAlongPlotStart + progressAlongPlotEnd) / 2;
                        PlotLineSegment(progressAlongPlotMid, progressAlongPlotEnd, linePlotter, color, truncationLimit);
                    }
                    //else we can't split any more, so give up
                }

            }
        }

        void SafeSetPixel(int x, int y, RgbaVector color)
        {
            if (x < 0 || x >= ImageData.Width) return;
            if (y < 0 || y >= ImageData.Height) return;

            ImageData[x, y] = color;
        }
    }
}
