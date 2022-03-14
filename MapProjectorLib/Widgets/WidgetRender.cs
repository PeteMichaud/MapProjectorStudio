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
        readonly Image<RgbaVector> ImageData;
        readonly Page _page;
        Graphics Canvas => _page.Graphics;

        readonly double _tooLongThreshhold;
        readonly double _discontinuityThreshold;

        public WidgetRender(Image<RgbaVector> imageData)
        {
            ImageData = imageData;
            Page page = new Page(ImageData.Width, ImageData.Height)
            {
                Background = Colour.FromRgba(0, 0, 0, 0)
            };
            _page = page;

            _tooLongThreshhold = Math.Max(ImageData.Width, ImageData.Height) / 100d;
            _discontinuityThreshold = _tooLongThreshhold * 5;
        }

        public void PlotLine(
            double t0, double t1,
            LinePlotCalculator linePlotCalculator,
            RgbaVector color,
            int naiveLineResolution,
            int maxRecursiveDetailPerSegment = 10,
            PlotRenderType renderType = PlotRenderType.Stroke
            )
        {
            var linePoints = new List<VectSharp.Point>();
            var col = color.ToVectColor();
            //if a segment is too distorted PlotLineSegment will try to
            //split it up, which is why the initial line resolution is "naive"
            var incr = (t1 - t0) / naiveLineResolution;            
            for (var i = 0; i < naiveLineResolution; i++)
            {
                PlotLineSegment(
                    linePoints, linePlotCalculator,
                    t0 + i * incr, t0 + (i + 1) * incr,
                    maxRecursiveDetailPerSegment);
            }

            // This is fundamentally wrong. What I should be getting from PlotLineSegment is
            // a collection of continguous line segments defined by a series of points that
            // represent the next point on the line. What I actually get is points that may
            // have a big enough gap between them that we can infer they must be
            // discontiguous. Revisit to make right at some point.   
            foreach(var contiguousLine in SplitByDiscontinuity(linePoints.ToArray()))
            {
                var path = new GraphicsPath().AddSmoothSpline(contiguousLine);
                if (renderType == PlotRenderType.Stroke)
                {
                    Canvas.StrokePath(path, col, lineWidth: 1);
                } 
                else //(renderType == PlotRenderType.Fill)
                {
                    Canvas.FillPath(path, col);
                }
            }

            IEnumerable<VectSharp.Point[]> SplitByDiscontinuity(VectSharp.Point[] points)
            {
                var contiguousLineStartIndex = 0;
                var contiguousLineEndIndex = 0;
                for (var i = 0; i < points.Length - 1; i++)
                {
                    if (points[i].ManhattanDistance(points[i+1]) > _discontinuityThreshold)
                    {
                        //Console.WriteLine($"Removing Discontinuity between: {points[i].X},{points[i].Y} and {points[i+1].X},{points[i+1].Y}");
                        yield return points.Slice(contiguousLineStartIndex, contiguousLineEndIndex);

                        contiguousLineStartIndex = contiguousLineEndIndex+1;
                    }

                    contiguousLineEndIndex = i+1;
                }

                if(contiguousLineStartIndex == 0)
                {
                    yield return points;
                }

                if(contiguousLineStartIndex < points.Length)
                {
                    yield return points.Slice(contiguousLineStartIndex, points.Length - 1);
                }
            }
        }

        void PlotLineSegment(
            List<VectSharp.Point> points,
            LinePlotCalculator linePlotCalculator,
            double progressAlongPlotStart,
            double progressAlongPlotEnd,
            int recursionLimit)
        {
#if DEBUG
            // PlotLineSegment checks the limit before recursing so when the program runs in
            // production it never passes the recursion limit. This check is ensure a future
            // bug can't be introduced, but it also compiles the check out of the hot loop
            // for performance reasons
            if (recursionLimit < 0) throw new Exception("Recursion too deep, check recursionLimit before calling");
#endif

            (var startInBounds, var mappedStart) = linePlotCalculator.GetXY(progressAlongPlotStart);
            (var endInBounds, var mappedEnd) = linePlotCalculator.GetXY(progressAlongPlotEnd);

            if (startInBounds && endInBounds)
            {
                //if the line is too long it'll make a big, ugly straight line where a curve should be
                var isTooLong = mappedStart.ManhattanDistance(mappedEnd) >= _tooLongThreshhold;
                if (isTooLong && recursionLimit > 0)
                {
                    //Split Too Long Segment in Half
                    var progressAlongPlotMid = (progressAlongPlotStart + progressAlongPlotEnd) / 2;
                    PlotLineSegment(points, linePlotCalculator, progressAlongPlotStart, progressAlongPlotMid, recursionLimit - 1);
                    PlotLineSegment(points, linePlotCalculator, progressAlongPlotMid, progressAlongPlotEnd, recursionLimit - 1);
                }
                else //the length is fine, or it's too long but we're at the recursion limit, so just draw the line
                {
                    if(points.Count == 0)
                    {
                        //if this is a new line, then track the beginning of it, otherwise the start point here is 
                        //just a duplicate of the previous end point, so we can skip it 
                        points.Add(new VectSharp.Point(mappedStart.X, mappedStart.Y));
                    }
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
                        PlotLineSegment(points, linePlotCalculator, progressAlongPlotStart, progressAlongPlotMid, truncationLimit);
                    }
                    //else we can't split any more, so give up
                }
                else if (endInBounds)
                {
                    if (recursionLimit > 0)
                    {
                        var progressAlongPlotMid = (progressAlongPlotStart + progressAlongPlotEnd) / 2;
                        PlotLineSegment(points, linePlotCalculator, progressAlongPlotMid, progressAlongPlotEnd, truncationLimit);
                    }
                    //else we can't split any more, so give up
                }
                //else
                //{
                //    var lastGoodPoint = points.Count > 0 ? points[points.Count - 1] : new VectSharp.Point(-69, -69);
                //    Console.WriteLine($"Found Discontinuity between: {lastGoodPoint.X},{lastGoodPoint.Y} and {(float)mappedStart.X},{(float)mappedStart.Y} (Progress: {progressAlongPlotStart})");
                //}

            }
        }
        public void PlotPoint(double x, double y, int r, RgbaVector color)
        {
            Canvas.StrokePath(
                new GraphicsPath().Arc(x, y, r, 0, ProjMath.TwoPi),
                color.ToVectColor(),
                1
            );
        }

        public Page ToSvg()
        {
            return _page;
        }

        public Image<Rgba32> ToRaster()
        {
            return ImageSharpContextInterpreter.SaveAsImage(_page);
        }

    }
}
