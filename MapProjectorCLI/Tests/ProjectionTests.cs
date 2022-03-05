using System;
using NUnit.Framework;
using CommandLine;
using MapProjectorLib;
using System.Reflection;
using System.Text;
using System.IO;

namespace MapProjectorCLI.Tests
{
    [TestFixture]
    public class ProjectionTests
    {
        StringBuilder exampleText = new StringBuilder("# Examples\n\n");
        const string repoPath = @"https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output";
        const string ioArgs = "-f ..\\..\\Tests\\{1}.png -o ..\\..\\Tests\\Output\\{0}.png";
        private string[] ToArgs(string rawArgs, string outFileName, string readmeNotes = "", string inFileName= "earth_equirect")
        {
            rawArgs += $" {string.Format(ioArgs, outFileName, inFileName)}";
            AddExample(rawArgs, outFileName, readmeNotes);
            return rawArgs.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        static void Parse(string[] args, Action<CLIParams> success)
        {
            CLIParams.Parse(args)
                .WithParsed(success)
                .WithNotParsed(errs =>
            {
                foreach (var err in errs)
                {
                    throw new Exception(err.ToString());
                }
            });
        }
     
        public void AddExample(string argStr, string title, string readmeNotes)
        {
            exampleText.AppendLine($"## {title}\n\n{readmeNotes}\n\n`{argStr}`\n\n![{title}]({repoPath}/{title}.png)\n\n");
        }

        [TearDown]
        public void OutputExampleFile()
        {
            var assm = typeof(Program).GetTypeInfo().Assembly;
            var currentDir = Path.GetDirectoryName(assm.Location);
            string exampleFile = Path.Combine(currentDir, "..\\..\\..\\examples.md");

            File.WriteAllText(exampleFile, exampleText.ToString());
            Console.WriteLine($"Examples written to {exampleFile}");
        }

        [Test]
        public void RunAllProjections()
        {
            foreach (MapProjection proj in Enum.GetValues(typeof(MapProjection)))
            {

                var args = ToArgs($"--projection {proj}", $"To{proj}");
                Parse(args, cliParams =>
                {
                    (var success, var projectionParams) = Program.ProcessParams(cliParams);
                    Projector.Project(projectionParams);
                });

            }
        }

        [Test]
        public void WithBackgroundImage()
        {
            var args = ToArgs($"--bg ..\\..\\Tests\\background.png --projection hammer", 
                MethodBase.GetCurrentMethod().Name, 
                "Many projections leave a blank area around the perimeter of the map. Fill that blank area with an optional background image");
            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WithBackgroundColor()
        {
            var args = ToArgs($"--bgcolor 255,0,0 --projection perspective",
                MethodBase.GetCurrentMethod().Name,
                "Many projections leave a blank area around the perimeter of the map. Fill that blank area with an optional background color");
            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void InvertFromMercator()
        {
            var args = ToArgs($"--projection mercator --invert",
                MethodBase.GetCurrentMethod().Name,
                "If you start with a projection other than equirectangular, use the --invert flag to convert FROM the target projection", 
                "earth_mercator");
            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

    }
}
