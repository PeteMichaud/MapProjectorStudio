using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorLib
{
    public class Image
    {
        public readonly Image<RgbaVector> ImageData;

        public Image(Image<RgbaVector> image)
        {
            ImageData = image;
        }

        public static Image Load(string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException("No image found at location", imagePath);
            }

            return new Image(SixLabors.ImageSharp.Image.Load<RgbaVector>(imagePath));
        }
        public void Dispose()
        {
            ImageData.Dispose();
        }

    }
}
