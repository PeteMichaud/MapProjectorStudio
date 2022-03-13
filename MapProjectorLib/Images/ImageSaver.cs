using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Webp;

namespace MapProjectorLib
{
    public class ImageSaver
    {
        readonly string FileExt;
        readonly string FileNameWithoutExt;
        readonly string FileName;

        public ImageSaver(string fileName)
        {
            var etxInx = fileName.LastIndexOf('.');
            FileNameWithoutExt = fileName.Substring(0, etxInx);
            FileExt = fileName.Substring(etxInx + 1).ToLower();

            FileName = fileName;
        }

        public ImageSaver(string fileName, int seriesNumber)
            : this(fileName)
        {
            FileName = string.Format(
                "{0}{1,4:0000}.{2}",
                FileNameWithoutExt,
                seriesNumber,
                FileExt
            );
        }

        public void Save(Image image, BitDepthPerChannel targetBitDepth)
        {
            var encoder = GetEncoder(targetBitDepth);
            if(encoder != null)
            {
                image.Save(FileName, encoder);
            } 
            else // this isn't a supported format, but we'll try to save it anyway
            {
                image.Save(FileName);
            }
        }

        IImageEncoder GetEncoder(BitDepthPerChannel targetBitDepth)
        {
            switch (FileExt)
            {
                case "png":
                    return new PngEncoder() 
                    {
                        TransparentColorMode = PngTransparentColorMode.Clear,
                        BitDepth = (PngBitDepth)targetBitDepth,
                    };
                case "jpg":
                case "jpeg":
                    return new JpegEncoder()
                    {
                        ColorType = JpegColorType.Rgb,
                    };
                case "bmp":                    
                    return new BmpEncoder()
                    {
                        BitsPerPixel = GetBmpBitsPerPixel(targetBitDepth)
                    };
                case "tif":
                case "tiff":
                    return new TiffEncoder()
                    {
                        BitsPerPixel = (TiffBitsPerPixel)targetBitDepth
                    };
                case "webp":
                    return new WebpEncoder()
                    {
                        //defaults all good for now
                    };
                default:
                    return null; 
            }
        }

        private BmpBitsPerPixel GetBmpBitsPerPixel(BitDepthPerChannel bitDepth)
        {
            if (bitDepth == BitDepthPerChannel.Bit16) return BmpBitsPerPixel.Pixel32;
            return BmpBitsPerPixel.Pixel24;
        }
    }
}
