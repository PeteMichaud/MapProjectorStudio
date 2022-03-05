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
            int numInitialPoints)
        {
            var inc = (t1 - t0) / numInitialPoints;
            for (var i = 0; i < numInitialPoints; i++)
                PlotLineAux(
                    t0 + i * inc, t0 + (i + 1) * inc,
                    linePlotter, color, 10);
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

        void DrawLine(int x0, int y0, int x1, int y1, Rgb24 color)
        {
            var nx = Math.Abs(x1 - x0) + 1;
            var ny = Math.Abs(y1 - y0) + 1;

            if (ny > nx)
            {
                var xIncr = x0 > x1 ? -1 : 1;
                var yIncr = y0 > y1 ? -1 : 1;
                var x = x0;
                var c = 0;

                for (var y = y0; y != y1 + yIncr; y += yIncr)
                {
                    SafeSetPixel(x, y, color);

                    if (xIncr == 1 && x > x1 || xIncr == -1 && x < x1)
                        throw new Exception("foo");

                    c += nx;

                    if (c >= ny)
                    {
                        c -= ny;
                        x += xIncr;
                    }
                }
            } else
            {
                var xIncr = x0 > x1 ? -1 : 1;
                var yIncr = y0 > y1 ? -1 : 1;
                var y = y0;
                var c = 0;

                for (var x = x0; x != x1 + xIncr; x += xIncr)
                {
                    SafeSetPixel(x, y, color);

                    //I'm not sure this can happen. Seems like it maybe can't, but the clause is here so I'm afraid to remove it
                    if (yIncr == 1 && y > y1 || yIncr == -1 && y < y1)
                        throw new Exception("foo");

                    c += ny;

                    if (c >= nx)
                    {
                        c -= nx;
                        y += yIncr;
                    }
                }
            }
        }

        void PlotLineAux(
            double t0, double t1,
            LinePlotter linePlotter,
            Rgb24 color,
            int depth)
        {
            if (depth == 0) return;

            var width = _image.Width;
            var height = _image.Height;

            double x0 = 0.0, y0 = 0.0;
            double x1 = 0.0, y1 = 0.0;

            // The bools tell us if the inverse mapping is even defined
            var defined0 = linePlotter.GetXY(t0, ref x0, ref y0);
            var defined1 = linePlotter.GetXY(t1, ref x1, ref y1);

            // This really ought to take note of the curvature.
            if (defined0 && defined1 &&
                Math.Abs(x0 - x1) + Math.Abs(y0 - y1) < 10)
            {
                var ix0 = (int) x0;
                var iy0 = (int) y0;
                var ix1 = (int) x1;
                var iy1 = (int) y1;
                DrawLine(ix0, iy0, ix1, iy1, color);
            } else
            {
                var margin = 50;
                // Consider whether to recurse
                // If both points are off to one side or above or below the image,
                // don't recurse, we assume that the line doesn't pass through.
                long sidex0 = x0 > width + margin ? 1 : x0 < -margin ? -1 : 0;
                long sidey0 = y0 > height + margin ? 1 : y0 < -margin ? -1 : 0;
                long sidex1 = x1 > width + margin ? 1 : x1 < -margin ? -1 : 0;
                long sidey1 = y1 > height + margin ? 1 : y1 < -margin ? 1 : 0;

                if ((defined0 || defined1) &&
                    sidex0 * sidex1 + sidey0 * sidey1 <= 0)
                {
                    var t2 = (t0 + t1) / 2;
                    PlotLineAux(0, t2, linePlotter, color, depth - 1);
                    PlotLineAux(t2, t1, linePlotter, color, depth - 1);
                }
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

        //public void Map(ImageMapper f, double xOffset, double yOffset)
        //{
        //    var height = _image.Height;
        //    var width = _image.Width;

        //    double hh = height / 2;
        //    double hw = width / 2;
        //    var scale = f.Scale(width, height);

        //    for (var y = 0; y < height; y++)
        //    {
        //        var y0 = (hh - y) * scale + yOffset;
        //        f.InitY(y0);
        //        for (var x = 0; x < width; x++)
        //        {
        //            var x0 = (x - hw) * scale + xOffset;
        //            SafeSetPixel(x, y, f.Map(x0, y0));
        //        }
        //    }
        //}
    }
}