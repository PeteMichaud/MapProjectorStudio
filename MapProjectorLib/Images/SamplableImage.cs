using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.Formats.Webp;

namespace MapProjectorLib
{
    public class SamplableImage : IDisposable
    {
        public readonly Image<RgbaVector> ImageData;
        public ColorSampleMode ColorSampleMode;

        public readonly IImageFormat OriginalFormat;
        public readonly BitDepthPerChannel BitDepth;
        public readonly bool SourceMayHaveTransparency;

        public int Width => ImageData.Width;
        public int Height => ImageData.Height;

        public RgbaVector this[int x, int y]
        {
            get => ImageData[x, y];
            set => ImageData[x, y] = value;
        }

        SamplableImage(Image<RgbaVector> image, ColorSampleMode mode, 
            IImageFormat originalFormat, BitDepthPerChannel bitDepth, bool useTransparency)
        {
            ImageData = image ?? throw new ArgumentException("Image must not be null", nameof(image));
            OriginalFormat = originalFormat;
            BitDepth = bitDepth;
            SourceMayHaveTransparency = useTransparency;
            ColorSampleMode = mode;
        }

        public static SamplableImage Load(string imagePath, ColorSampleMode mode)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException("No image found at location", imagePath);
            }

            IImageFormat originalFormat;
            Image<RgbaVector> image;

            using (var imageStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                var loadTask = SixLabors.ImageSharp.Image.LoadWithFormatAsync<RgbaVector>(imageStream);

                loadTask.Wait();
                (image, originalFormat) = loadTask.Result;
            }

            //consider doing this at the end, just before saving the destination file
            var ext = GetExtension(originalFormat);
            (var bitDepth, var useTransparency) = GetMetaInfo(ext, image);
           
            return new SamplableImage(image, mode, originalFormat, bitDepth, useTransparency);
        }

        public void Dispose()
        {
            ImageData.Dispose();
        }


        static (BitDepthPerChannel bitDepth, bool useTransparency) GetMetaInfo(string ext, SixLabors.ImageSharp.Image image)
        {
            var genericMetadata = image.Metadata;
            switch (ext)
            {
                case "png":
                    var pngMeta = genericMetadata.GetPngMetadata();
                    return (GetBitDepth(pngMeta), pngMeta.HasTransparency);
                case "jpg":
                case "jpeg":
                    var jpgMeta = genericMetadata.GetJpegMetadata();
                    return (GetBitDepth(jpgMeta), false);
                case "bmp":
                    var bmpMeta = genericMetadata.GetBmpMetadata();
                    return (GetBitDepth(bmpMeta), false);
                case "tif":
                case "tiff":
                    var frameMeta = image.Frames[0].Metadata.GetTiffMetadata();
                    return (GetBitDepth(frameMeta), true);
                case "webp":
                    var webpMeta = genericMetadata.GetWebpMetadata();
                    return (GetBitDepth(webpMeta), true);
                default:
                    throw new ArgumentException($"Image format \".{ext}\" not supported. Legal formats: png, jpg, bmp, tiff, webp", nameof(ext));
            }
        }

        static string GetExtension(IImageFormat format)
        {
            //using this instead of LINQ
            string ext;
            using (var enumer = format.FileExtensions.GetEnumerator())
            {
                if (enumer.MoveNext()) ext = enumer.Current;
                else throw new ArgumentException($"Unknown file format \"{format.Name}\"", nameof(format));
            }

            return ext;
        }

        #region Bit Depths for all image formats

        static BitDepthPerChannel GetBitDepth(PngMetadata meta)
        {
            return GetBitDepth((short)meta.BitDepth);
        }

        static BitDepthPerChannel GetBitDepth(short depth)
        {
            return (BitDepthPerChannel)depth;
        }

        //jpg only supports 8bits per channel
        static BitDepthPerChannel GetBitDepth(JpegMetadata meta)
        {
            return BitDepthPerChannel.Bit8;
        }

        static BitDepthPerChannel GetBitDepth(BmpMetadata meta)
        {
            var bits = (short)meta.BitsPerPixel;
            if(bits <= 24)
            {
                return BitDepthPerChannel.Bit8;
            }
            return BitDepthPerChannel.Bit16;
        }

        static BitDepthPerChannel GetBitDepth(TiffFrameMetadata meta)
        {
            var bits = (short)meta.BitsPerPixel;
            if (bits <= 24)
            {
                return BitDepthPerChannel.Bit8;
            }
            return BitDepthPerChannel.Bit16;
        }

        static BitDepthPerChannel GetBitDepth(WebpMetadata meta)
        {
            return BitDepthPerChannel.Bit8;
        }

        #endregion
    }
}