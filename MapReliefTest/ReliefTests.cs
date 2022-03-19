using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using MapReliefLib;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using NUnit.Framework;

namespace MapReliefLib.Tests
{
    [TestFixture]
    public class ReliefTests
    {
        const string inFilePath = "..\\..\\..\\Input\\{0}.tif";
        const string outFilePath = "..\\..\\..\\Output\\{0}.png";

        protected static Image<L16> GetHeightMap(string inFileName = "small")
        {
            return Image.Load<L16>(string.Format(inFilePath, inFileName));
        }

        protected static string OutFile(
                [CallerMemberName] string outFileName = "error")
        {
            return string.Format(outFilePath, outFileName);
        }

        [Test]
        public void ReliefMap()
        {
            var heightMap = GetHeightMap("earth");
            var rmParams = new ReliefMapParams();

            var reliefMap = (Image)Relief.Light(heightMap, rmParams);

            reliefMap.Save(OutFile());
        }
        

    }
}
