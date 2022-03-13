using System;
using System.Collections.Generic;

using MapProjectorLib.Plotters;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorLib
{
    public class DestinationImage : IDisposable
    {
        public readonly Image<RgbaVector> ImageData;
        public int Width => ImageData.Width;
        public int Height => ImageData.Height;

        public RgbaVector this[int x, int y]
        {
            get => ImageData[x, y];
            set => ImageData[x, y] = value;
        }

        public DestinationImage(Image<RgbaVector> image)
        {
            if(image == null)
            {
                throw new ArgumentNullException("Image cannot be null", nameof(image));
            }

            ImageData = image;
        }


        public DestinationImage(int width, int height)
        {
            ImageData = new Image<RgbaVector>(width, height);
        }

        public DestinationImage(int width, int height, RgbaVector backgroundColor)
        {
            ImageData = new Image<RgbaVector>(width, height, backgroundColor);
        }

        public void Dispose()
        {
            ImageData.Dispose();
        }

        //

        public void ProcessPixelRows(PixelAccessorAction<RgbaVector> pixelAccessor)
        {
            ImageData.ProcessPixelRows(pixelAccessor);
        }

        //



    }
}