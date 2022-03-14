using System;
using System.Collections.Generic;

using MapProjectorLib.Extensions;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using VectSharp;
using VectSharp.Raster.ImageSharp;

namespace MapProjectorLib.PlotCalculators
{
    public class WidgetRender
    {
        Image<RgbaVector> ImageData;
        Graphics Canvas => _page.Graphics;
        Page _page;

        readonly double _tooLongThreshhold = 10d;

        public WidgetRender(Image<RgbaVector> imageData)
        {
            ImageData = imageData;
            Page page = new Page(ImageData.Width, ImageData.Height)
            {
                Background = Colour.FromRgba(0, 0, 0, 0)
            };
            _page = page;
            _tooLongThreshhold = Math.Max(ImageData.Width, ImageData.Height) / 100d;
        }

        public void PlotLine(
            double t0, double t1,
            LinePlotCalculator linePlotter,
            RgbaVector color,
            int naiveLineResolution,
            int maxRecursiveDetailPerSegment = 10)
        {
            var linePoints = new List<VectSharp.Point>();
            //if a segment is too distorted PlotLineSegment will try to
            //split it up, which is why the initial line resolution is "naive"
            var incr = (t1 - t0) / naiveLineResolution;
            for (var i = 0; i < naiveLineResolution; i++)
            {
                linePoints.AddRange(
                    PlotLineSegment(
                        t0 + i * incr, t0 + (i + 1) * incr,
                        linePlotter, color, maxRecursiveDetailPerSegment)
                );
            }

            // It would be better to design the PlotLineSegment code to return only the needed
            // points instead of calling RemoveAdjacentDuplicates here, but I'm keeping it this 
            // way for now so I can flip back to pixel mode if I need to
            linePoints.RemoveAdjacentDuplicates(new PointComparer());

            // This is also fundamentally wrong. What I should be getting from PlotLineSegment is
            // a collection of continguous line segments defined by a series of points that
            // represent the next point on the line. What I actually get is pairs of points that
            // repeats the previous end point as the new start point, which may have a big enough
            // gap between two segments that we can infer they must be discontiguous. Revisit to
            // make right at some point.   
            foreach(var contiguousLine in SplitByDiscontinuity(linePoints))
            {
                Canvas.StrokePath(
                    new GraphicsPath().AddSmoothSpline(contiguousLine), 
                    color.ToVectColor(),
                    1
                );
            }

            IEnumerable<VectSharp.Point[]> SplitByDiscontinuity(IEnumerable<VectSharp.Point> points)
            {
                var current = new List<VectSharp.Point>();

                foreach (var pt in points)
                {
                    //the multiplier is an arbitrary number. The goal is to find places that
                    //really shouldn't be contiguous by finding points on the line that are 
                    //farther apart than the line algo is intended to make them
                    if (current.Count > 0 && pt.ManhattanDistance(current[current.Count - 1]) > _tooLongThreshhold * 5)
                    {
                        yield return current.ToArray();

                        current.Clear();
                    }

                    current.Add(pt);
                }

                if (current.Count > 0)
                    yield return current.ToArray();
            }
        }

        public Page ToSvg()
        {
            return _page;
        }

        public Image<Rgba32> ToRaster()
        {
            return ImageSharpContextInterpreter.SaveAsImage(_page);
        }

        public void PlotPoint(double x, double y, int r, RgbaVector color)
        {
            Canvas.StrokePath(
                new GraphicsPath().Arc(x, y, r, 0, ProjMath.TwoPi),
                color.ToVectColor(),
                1
            );
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

        List<VectSharp.Point> PlotLineSegment(
            double progressAlongPlotStart, double progressAlongPlotEnd,
            LinePlotCalculator linePlotter,
            RgbaVector color,
            int recursionLimit)
        {
            DebugCheckRecursionLimit(recursionLimit);

            var points = new List<VectSharp.Point>();

            List<VectSharp.Point> SplitSegment()
            {
                var progressAlongPlotMid = (progressAlongPlotStart + progressAlongPlotEnd) / 2;
                var pts = PlotLineSegment(progressAlongPlotStart, progressAlongPlotMid, linePlotter, color, recursionLimit - 1);
                pts.AddRange(PlotLineSegment(progressAlongPlotMid, progressAlongPlotEnd, linePlotter, color, recursionLimit - 1));
                return pts;
            }

            (var startInBounds, var mappedStart) = linePlotter.GetXY(progressAlongPlotStart);
            (var endInBounds, var mappedEnd) = linePlotter.GetXY(progressAlongPlotEnd);

            if (startInBounds && endInBounds)
            {
                //if the line is too long it'll make a big, ugly straight line where a curve should be
                var isTooLong = mappedStart.ManhattanDistance(mappedEnd) >= _tooLongThreshhold;
                if (isTooLong && recursionLimit > 0)
                {
                    points.AddRange(SplitSegment());
                }
                else //the length is fine, or it's too long but we're at the recursion limit, so just draw the line
                {
                    points.Add(new VectSharp.Point(mappedStart.X, mappedStart.Y));
                    points.Add(new VectSharp.Point(mappedEnd.X, mappedEnd.Y));
                }
            }
            else // one or both points are outside the map or the range in which the line is allowed
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
                        points.AddRange(PlotLineSegment(progressAlongPlotStart, progressAlongPlotMid, linePlotter, color, truncationLimit));
                    }
                    //else we can't split any more, so give up
                }
                else if (endInBounds)
                {
                    if (recursionLimit > 0)
                    {
                        var progressAlongPlotMid = (progressAlongPlotStart + progressAlongPlotEnd) / 2;
                        points.AddRange(PlotLineSegment(progressAlongPlotMid, progressAlongPlotEnd, linePlotter, color, truncationLimit));
                    }
                    //else we can't split any more, so give up
                }

            }
            return points;
        }

        //void SafeSetPixel(int x, int y, RgbaVector color)
        //{
        //    if (x < 0 || x >= ImageData.Width) return;
        //    if (y < 0 || y >= ImageData.Height) return;

        //    ImageData[x, y] = color;
        //}
    }
}
