using System;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats;

namespace MapProjectorLib
{
    public class ProjectedImage : IDisposable
    {
        public Image<RgbaVector> ImageData;
        public BitDepthPerChannel TargetBitDepth;
        public IImageFormat SourceFormat;
        public string FilePath;
        public int? SeriesNumber;

        public int Width => ImageData.Width;
        public int Height => ImageData.Height;
        public RgbaVector this[int x, int y]
        {
            get => ImageData[x, y];
            set => ImageData[x, y] = value;
        }

        //public ProjectedImage(
        //    DestinationImage destImage, SamplableImage sourceImage, 
        //    string saveAsFilename)
        //{
        //    Initialize(destImage, sourceImage, saveAsFilename);
        //}

        public ProjectedImage(
            DestinationImage destImage, SamplableImage sourceImage, string saveAsFilename, 
            int seriesTotal, int seriesNumber)
        {
            if(seriesTotal > 1)
            {
                Initialize(destImage, sourceImage, saveAsFilename, seriesNumber);
            }
            else
            {
                Initialize(destImage, sourceImage, saveAsFilename);
            }
        }

        //public ProjectedImage(DestinationImage destImage, SamplableImage sourceImage, string saveAsFilename, int seriesNumber)
        //    : this(destImage, sourceImage, saveAsFilename)
        //{
        //    Initialize(destImage, sourceImage, saveAsFilename, seriesNumber);
        //}

        void Initialize(DestinationImage destImage, SamplableImage sourceImage, string saveAsFilename)
        {
            if (destImage == null)
            {
                throw new ArgumentNullException("Image cannot be null", nameof(destImage));
            }
            SourceFormat = sourceImage.OriginalFormat;
            TargetBitDepth = sourceImage.BitDepth;
            ImageData = destImage.ImageData;
            FilePath = saveAsFilename;
        }

        void Initialize(DestinationImage destImage, SamplableImage sourceImage, string saveAsFilename, int seriesNumber)
        {
            Initialize(destImage, sourceImage, saveAsFilename);
            SeriesNumber = seriesNumber;
        }

        public void Save()
        {
            var saver = (SeriesNumber == null)
                ? new ImageSaver(FilePath)
                : new ImageSaver(FilePath, (int)SeriesNumber);
            
            saver.Save(ImageData, TargetBitDepth);
        }

        public void Dispose()
        {
            ImageData.Dispose();
        }
    }
}
