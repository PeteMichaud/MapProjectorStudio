using System;
using NUnit.Framework;
using CommandLine;
using MapProjectorLib;
using System.Reflection;
using System.Text;

namespace MapProjectorCLI.Tests
{
    [TestFixture]
    public class ProjectionTests
    {
        StringBuilder exampleText = new StringBuilder("# Examples\n\n");
        const string repoPath = @"https://github.com/PeteMichaud/MapProjectorStudio/blob/master/MapProjectorCLI/Tests/Output";
        const string ioArgs = "-f ..\\..\\Tests\\earth_equirect.png -o ..\\..\\Tests\\Output\\{0}.png";
        private string[] ToArgs(string rawArgs, string outFileName)
        {
            rawArgs += $" {string.Format(ioArgs, outFileName)}";
            AddExample(rawArgs, outFileName);
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
     
        public void AddExample(string argStr, string title)
        {
            exampleText.AppendLine($"## {title}\n\n`{argStr}`\n\n[[{repoPath}/{title}.png|alt={title}]]\n\n");
        }

        [TearDown]
        public void OutputExampleFile()
        {
            Console.WriteLine(exampleText.ToString());
        }

        //[Test]
        //public void ToLatLon()
        //{
        //    var args = ToArgs($"", MethodBase.GetCurrentMethod().Name);
        //    Parse(args, cliParams =>
        //    {
        //        (var success, var projectionParams) = Program.ProcessParams(cliParams);
        //        Projector.Project(projectionParams);
        //    });

        //}

        [Test]
        public void RunAllProjections()
        {
            foreach(MapProjection proj in Enum.GetValues(typeof(MapProjection)))
            {
                var args = ToArgs($"--projection {proj}", $"To{proj}");
                Parse(args, cliParams =>
                {
                    (var success, var projectionParams) = Program.ProcessParams(cliParams);
                    Projector.Project(projectionParams);
                });

            }

        }

    }
}
