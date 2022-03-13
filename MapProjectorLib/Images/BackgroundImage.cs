using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorLib
{
    public class BackgroundImage
    {
        public readonly Image<RgbaVector> ImageData;

        public BackgroundImage(Image<RgbaVector> image)
        {
            ImageData = image;
        }

        public static BackgroundImage Load(string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException("No image found at location", imagePath);
            }

            return new BackgroundImage(SixLabors.ImageSharp.Image.Load<RgbaVector>(imagePath));
        }
        public void Dispose()
        {
            ImageData.Dispose();
        }

    }
}
