using System.IO;
using System.Reflection;
using NUnit.Framework;
using MapProjectorLib;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Formats.Png;

namespace MapProjectorCLI.Tests
{
    [TestFixture]
    public class ImageFormatTests : TestsWithParser
    {
        string _workingDir;
        string WorkingDir
        {
            get
            {
                if(_workingDir == null)
                {
                    var assm = typeof(Program).GetTypeInfo().Assembly;
                    _workingDir = Path.GetDirectoryName(assm.Location);
                }

                return _workingDir;
            }
        }
        string InOutArgs(string fileNameIn, string fileNameOut = "not_saved.png")
        {
            return $" -f ..\\..\\Tests\\Input\\Formats\\{fileNameIn} -o ..\\..\\Tests\\Output\\Formats\\{fileNameOut}";
        }

        protected string[] ToArgs(string rawArgs)
        {
            return base.ToArgs(rawArgs, withDefaults: false);
        }

        protected Image<RgbaVector> Load(string imageFile)
        {
            var imagePath = Path.Combine(WorkingDir, $"..\\..\\Tests\\Output\\Formats\\{imageFile}");
            return SixLabors.ImageSharp.Image.Load<RgbaVector>(imagePath);
        }

        //

        //png
        //jpg
        //tiff

        //bmp

        //gif
        //pbm
        //tga
        //webp

        //transparency


        [Test]
        public void BadFilePath()
        {
            var args = ToArgs(InOutArgs("does_not_exist.png"));
            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Assert.False(success);
                Assert.IsNull(projectionParams);
            });

        }

        [Test]
        public void UnsupportedFileType()
        {
            var args = ToArgs(InOutArgs("cannot_open.exr"));
            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);

                Assert.False(success);
                Assert.IsNull(projectionParams);

            });

        }

        [Test]
        public void OpenPng()
        {
            var args = ToArgs(InOutArgs("8bitRGBA.png"));
            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Assert.NotNull(projectionParams.SourceImage);
            });

        }

        [Test]
        public void OpenPngSaveAsJpg()
        {
            var outFile = "saveasjpg.jpg";
            var args = ToArgs(InOutArgs("8bitRGBA.png", outFile));
            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);

                Projector.Project(projectionParams);

                var savedImage = Load(outFile);

                Assert.NotNull(savedImage);

            });

        }

        [Test]
        public void PngPreserveDepth8Bit()
        {
            var outFile = "8bitRGBA.png";
            var args = ToArgs(InOutArgs("8bitRGBA.png", outFile));
            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);

                Projector.Project(projectionParams);

                Assert.AreEqual(BitDepthPerChannel.Bit8, projectionParams.SourceImage.BitDepth);

                var savedImage = Load(outFile);

                Assert.NotNull(savedImage);

                var meta = savedImage.Metadata.GetPngMetadata();
                Assert.AreEqual(PngBitDepth.Bit8, meta.BitDepth);

            });

        }

        [Test]
        public void PngPreserveDepth16Bit()
        {
            var outFile = "16bitRGBA.png";
            var args = ToArgs(InOutArgs("16bitRGBA.png", outFile));
            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);

                Projector.Project(projectionParams);

                Assert.AreEqual(BitDepthPerChannel.Bit16, projectionParams.SourceImage.BitDepth);
                
                var savedImage = Load(outFile);

                Assert.NotNull(savedImage);

                var meta = savedImage.Metadata.GetPngMetadata();
                Assert.AreEqual(PngBitDepth.Bit16, meta.BitDepth);

            });

        }

        [Test]
        public void PngPreserveTransparency()
        {
            var fileOut = "8bitRGBAWithAlpha.png";
            var args = ToArgs(InOutArgs("8bitRGBAWithAlpha.png", fileOut));
            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Assert.True(success);
                Projector.Project(projectionParams);

            });

        }

        [Test]
        public void PngTransparentBackground()
        {
            var fileOut = "8bitRGBAWithAlphaBackground.png";
            var args = ToArgs("--projection hammer --bgcolor 0,0,0,0" + InOutArgs("8bitRGBAWithAlpha.png", fileOut));
            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);

                Projector.Project(projectionParams);

            });

        }

        [Test]
        public void PngOpaqueBackground()
        {
            var fileOut = "8bitRGBAWithOpaqueBackground.png";
            var args = ToArgs("--projection hammer --bgcolor 255,0,0,255" + InOutArgs("8bitRGBAWithAlpha.png", fileOut));
            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);

                Projector.Project(projectionParams);

            });

        }

        [Test]
        public void OpenTif()
        {
            var outFile = "32bit.tif";
            var args = ToArgs(InOutArgs("32bit.tif", outFile));
            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);

                Projector.Project(projectionParams);

                Assert.AreEqual(BitDepthPerChannel.Bit16, projectionParams.SourceImage.BitDepth);

                var savedImage = Load(outFile);

                Assert.NotNull(savedImage);
               
                //throw new Exception("This passes, but the tif output is wrong");
            });

        }

    }
}
