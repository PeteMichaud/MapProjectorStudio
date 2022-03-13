//using System;
//using System.IO;
//using System.Reflection;

//using NUnit.Framework;

//using SixLabors.ImageSharp.Formats;
//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.PixelFormats;
//using SixLabors.ImageSharp.Processing.Processors.Drawing;

//namespace MapProjectorCLI.Tests
//{
//    //This is just a scratch file for experiments 
//    [TestFixture]
//    public class LoadVec4
//    {
//        [Test]
//        public void CompositeTest()
//        {
//            var assm = typeof(LoadVec4).GetTypeInfo().Assembly;
//            var currentDir = Path.GetDirectoryName(assm.Location);
//            var subPath = "..\\..\\Tests\\Input";
//            var imagePath = Path.Combine(currentDir, subPath, "Formats\\8bitRGBAWithAlpha.png");

//            using (var inImg = Image.Load<RgbaVector>(imagePath))
//            using (var background = new Image<RgbaVector>(inImg.Width, inImg.Height, new RgbaVector(1, 0, 0, 1)))
//            {
//                var processorCreator = new DrawImageProcessor(inImg, Point.Empty, PixelColorBlendingMode.Normal, PixelAlphaCompositionMode.SrcAtop, 1f);
                
//                var pxProcessor = processorCreator.CreatePixelSpecificProcessor(Configuration.Default, background, inImg.Bounds());

//                pxProcessor.Execute();

//                background.Save(Path.Combine(currentDir, "..\\..\\Tests\\Output\\__out.png"));
//            }
//        }

//        [Test]
//        public void LoadAsVec4()
//        {
//            var assm = typeof(LoadVec4).GetTypeInfo().Assembly;
//            var currentDir = Path.GetDirectoryName(assm.Location);

//            var imagePath = Path.Combine(currentDir, "..\\..\\Tests\\Input\\earth_equirect.png");
            
//            Image<HalfVector4> image; 
//            IImageFormat format;
            
//            using (var imageStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
//            {
//                var loadTask = Image.LoadWithFormatAsync<HalfVector4>(imageStream);

//                loadTask.Wait();
//                (image, format) = loadTask.Result;
//            }

//            var color = image[25, 25];
//            Console.WriteLine(color);

//            var outPath = Path.Combine(currentDir, "..\\..\\Tests\\Output\\__out.png");
//            image.Save(outPath);


//        }

//    }
//}
