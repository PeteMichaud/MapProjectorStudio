using System;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats;

using VectSharp;

namespace MapProjectorLib
{
    public class ProjectedImage : IDisposable
    {
        public Image<RgbaVector> ImageData;
        public Page WidgetVector;
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

        public ProjectedImage(
            DestinationImage destImage, Page widgetVector, SamplableImage sourceImage, string saveAsFilename, 
            int seriesTotal, int seriesNumber)
        {
            if(seriesTotal > 1)
            {
                Initialize(destImage, widgetVector, sourceImage, saveAsFilename, seriesNumber);
            }
            else
            {
                Initialize(destImage, widgetVector, sourceImage, saveAsFilename);
            }
        }

        void Initialize(DestinationImage destImage, Page widgetVector, SamplableImage sourceImage, string saveAsFilename)
        {
            SourceFormat = sourceImage.OriginalFormat;
            TargetBitDepth = sourceImage.BitDepth;
            FilePath = saveAsFilename;
            ImageData = destImage?.ImageData;
            WidgetVector = widgetVector;
        }

        void Initialize(DestinationImage destImage, Page widgetVector, SamplableImage sourceImage, string saveAsFilename, int seriesNumber)
        {
            Initialize(destImage, widgetVector, sourceImage, saveAsFilename);
            SeriesNumber = seriesNumber;
        }

        public void Save()
        {
            if(ImageData != null)
            {
                var saver = (SeriesNumber == null)
                    ? new ImageSaver(FilePath)
                    : new ImageSaver(FilePath, (int)SeriesNumber);
            
                saver.Save(ImageData, TargetBitDepth);
            }
            if(WidgetVector != null)
            {
                var saver = (SeriesNumber == null)
                    ? new SvgImageSaver(FilePath)
                    : new SvgImageSaver(FilePath, (int)SeriesNumber);

                saver.Save(WidgetVector);
            }
        }

        public void Dispose()
        {
            ImageData.Dispose();
        }
    }
}
