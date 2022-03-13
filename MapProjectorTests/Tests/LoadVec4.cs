using System;
using System.IO;
using System.Reflection;

using NUnit.Framework;

using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors.Drawing;

using VectSharp;
using VectSharp.SVG;

namespace MapProjectorCLI.Tests
{
    //This is just a scratch file for experiments 
    [TestFixture]
    public class LoadVec4
    {
        //[Test]
        //public void VectorWidgetTests()
        //{
   
        //    Document document = new Document();
        //    Page page = new Page(1000, 1000);
        //    document.Pages.Add(page);

        //    //page.Graphics.FillRectangle(100, 100, 800, 50, Colour.FromRgb(128, 128, 128), tag: "linkToGitHub");
        //    //page.Graphics.FillRectangle(100, 300, 800, 50, Colour.FromRgb(255, 0, 0), tag: "linkToBlueRectangle");
        //    //page.Graphics.FillRectangle(100, 850, 800, 50, Colour.FromRgb(0, 0, 255), tag: "blueRectangle");

        //    var path = new GraphicsPath().MoveTo(0, 0);
        //    for(int i = 1; i < 11; i++)
        //    {
        //        path.LineTo(i*i, i*i*i);
        //    }
        //    path.AddSmoothSpline();
        //    page.Graphics.StrokePath(path, Colours.Red, 4);
        //    page.SaveAsSVG("D:\\Sync\\LandfallMap\\MapProjectorStudio\\MapProjectorTests\\Tests\\Output\\Vector\\out.svg");
        //}

        //[Test]
        //public void CompositeTest()
        //{
        //    var assm = typeof(LoadVec4).GetTypeInfo().Assembly;
        //    var currentDir = Path.GetDirectoryName(assm.Location);
        //    var subPath = "..\\..\\Tests\\Input";
        //    var imagePath = Path.Combine(currentDir, subPath, "Formats\\8bitRGBAWithAlpha.png");

        //    using (var inImg = Image.Load<RgbaVector>(imagePath))
        //    using (var background = new Image<RgbaVector>(inImg.Width, inImg.Height, new RgbaVector(1, 0, 0, 1)))
        //    {
        //        var processorCreator = new DrawImageProcessor(inImg, Point.Empty, PixelColorBlendingMode.Normal, PixelAlphaCompositionMode.SrcAtop, 1f);

        //        var pxProcessor = processorCreator.CreatePixelSpecificProcessor(Configuration.Default, background, inImg.Bounds());

        //        pxProcessor.Execute();

        //        background.Save(Path.Combine(currentDir, "..\\..\\Tests\\Output\\__out.png"));
        //    }
        //}

        //[Test]
        //public void LoadAsVec4()
        //{
        //    var assm = typeof(LoadVec4).GetTypeInfo().Assembly;
        //    var currentDir = Path.GetDirectoryName(assm.Location);

        //    var imagePath = Path.Combine(currentDir, "..\\..\\Tests\\Input\\earth_equirect.png");

        //    Image<HalfVector4> image;
        //    IImageFormat format;

        //    using (var imageStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
        //    {
        //        var loadTask = Image.LoadWithFormatAsync<HalfVector4>(imageStream);

        //        loadTask.Wait();
        //        (image, format) = loadTask.Result;
        //    }

        //    var color = image[25, 25];
        //    Console.WriteLine(color);

        //    var outPath = Path.Combine(currentDir, "..\\..\\Tests\\Output\\__out.png");
        //    image.Save(outPath);


        //}

    }
}
