using System;
using MapProjectorLib.Plotters;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorLib
{
    //Just a wrapper class for ImageSharp so I can easily change it out later
    public class Image : IDisposable
    {
        readonly Image<Rgb24> _image;

        Image(Image<Rgb24> image)
        {
            _image = image;
        }

        public Image(int width, int height)
        {
            _image = new Image<Rgb24>(width, height);
        }

        public Image(int width, int height, Rgb24 backgroundColor)
        {
            _image = new Image<Rgb24>(width, height, backgroundColor);
        }


        public int Width => _image.Width;
        public int Height => _image.Height;

        public Rgb24 this[int x, int y]
        {
            get => _image[x, y];
            set => _image[x, y] = value;
        }

        public Image Clone()
        {
            return new Image(_image.Clone());
        }

        public void Save(string fileName)
        {
            _image.Save(fileName);
        }

        public static Image Load(string fileName)
        {
            return new Image(SixLabors.ImageSharp.Image.Load<Rgb24>(fileName));
        }

        public void Dispose()
        {
            _image.Dispose();
        }

        public void ProcessPixelRows(PixelAccessorAction<Rgb24> pixelAccessor)
        {
            _image.ProcessPixelRows(pixelAccessor);
        }

        public void PlotLine(
            double t0, double t1,
            LinePlotter linePlotter,
            Rgb24 color,
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

        public void PlotPoint(double x, double y, int r, Rgb24 color)
        {
            for (var j = -r; j <= r; j++)
            for (var i = -r; i <= r; i++)
                SafeSetPixel((int) (x + i), (int) (y + j), color);
        }

        void SafeSetPixel(int x, int y, Rgb24 color)
        {
            if (x < 0 || x >= _image.Width) return;
            if (y < 0 || y >= _image.Height) return;

            _image[x, y] = color;
        }

        void DrawLine(int xStart, int yStart, int xEnd, int yEnd, Rgb24 color)
        {
            foreach(var pt in GetPointsOnLine(xStart, yStart, xEnd, yEnd))
            {
                SafeSetPixel(pt.X, pt.Y, color);
            }
        }

        // Bresenham's Line Algorithm
        internal static System.Collections.Generic.IEnumerable<Point> GetPointsOnLine(int x0, int y0, int x1, int y1)
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
            LinePlotter linePlotter,
            Rgb24 color,
            int recursionLimit)
        {
            DebugCheckRecursionLimit(recursionLimit);

            void SplitSegment()
            {
                var progressAlongPlotMid = (progressAlongPlotStart + progressAlongPlotEnd) / 2;
                PlotLineSegment(progressAlongPlotStart, progressAlongPlotMid, linePlotter, color, recursionLimit - 1);
                PlotLineSegment(progressAlongPlotMid, progressAlongPlotEnd, linePlotter, color, recursionLimit - 1);
            }

            // The bools tell us if the inverse mapping is even defined
            (var startInBounds, var mappedStart) = linePlotter.GetXY(progressAlongPlotStart);
            (var endInBounds, var mappedEnd) = linePlotter.GetXY(progressAlongPlotEnd);


            // This really ought to take note of the curvature.

            //right now if one of the points isn't defined, it sometimes just tries to place it anyway,
            //relying on the pixel placement check to bail out if it's not legal, but that pixel placement
            //check is whack. It's multiple branches right at the center of the hot loop, plus it's just
            //shitty logic not to figure out where the real line should be.

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
                else //the length is fine, just draw the line
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
                //that no line will be legal, but 

                var truncationLimit = 10;

                if(startInBounds)
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
                //else //neither point is in bounds
                //{
                //    //if we had defined out of bounds points we could try to figure
                //    //out if any portion of the line overlaps with the image, and then 
                //    //draw that line, but we don't have any defined points, so give up
                //}

                ////if only one point is outside
                //if (startInBounds || endInBounds)
                //{

                //    //the following code is wrong
                //    // whichever points are out of bounds have mapped points that are not defined
                //    // this code acts like they are defined, but the definitions are beyond the bounds of the image

                //    var width = _image.Width;
                //    var height = _image.Height;

                //    //todo: probably should make this scale with image size?
                //    var margin = 50;

                //    // Consider whether to recurse
                //    // If both points are off to one side or above or below the image,
                //    // don't recurse, we assume that the line doesn't pass through.
                //    long sidex0 = mappedStart.X > width + margin
                //        ? 1
                //        : mappedStart.X < -margin
                //            ? -1
                //            : 0;
                //    long sidey0 = mappedStart.Y > height + margin
                //        ? 1
                //        : mappedStart.Y < -margin
                //            ? -1
                //            : 0;
                //    long sidex1 = mappedEnd.X > width + margin
                //        ? 1
                //        : mappedEnd.X < -margin
                //            ? -1
                //            : 0;
                //    long sidey1 = mappedEnd.Y > height + margin
                //        ? 1
                //        : mappedEnd.Y < -margin
                //            ? 1
                //            : 0;

                //    // one of the points is inside the image,
                //    // and we have an educated guess that some of the line is visible 
                //    if (sidex0 * sidex1 + sidey0 * sidey1 <= 0)
                //    {
                //        //if we're not at the recursion limit, then try to draw a better line
                //        if (recursionLimit > 0)
                //        {
                //            SplitSegment();
                //        }
                //        else //otherwise draw the dumb line and let the out of bounds pixel writes fail
                //        {
                //            DrawLine((int)mappedStart.X, (int)mappedStart.Y, (int)mappedEnd.X, (int)mappedEnd.Y, color);
                //        }
                //    }
                //} //else both points are outside
            }
        }


        // Add a latitude, longitude grid to the image
        public void AddGrid(double gridX, double gridY, Rgb24 color)
        {
            var height = _image.Height;
            var width = _image.Width;

            var yStep = gridY * height / 180.0;
            var xStep = gridX * width / 360.0;
            // Quicker this way around with big images
            for (var y = 0; y < height; y++)
            for (var xgrad = 0.0; (int) xgrad < width; xgrad += xStep)
            {
                var x = (int) xgrad;
                SafeSetPixel(x, y, color);
            }

            for (var ygrad = 0.0; (int) ygrad < height; ygrad += yStep)
            {
                var y = (int) ygrad;
                for (var x = 0; x < width; x++) SafeSetPixel(x, y, color);
            }
        }

    }
}