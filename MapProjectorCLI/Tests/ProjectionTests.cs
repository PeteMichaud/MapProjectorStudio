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
        const string ioArgs = "-f ..\\..\\Tests\\Input\\{1}.png -o ..\\..\\Tests\\Output\\{0}.png";
        private string[] ToArgs(string rawArgs, string outFileName, string readmeNotes = "", string inFileName= "earth_equirect", bool addExample = true)
        {
            rawArgs += $" {string.Format(ioArgs, outFileName, inFileName)}";
            if(addExample) AddExample(rawArgs, outFileName, readmeNotes);
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
            var args = ToArgs($"--bg ..\\..\\Tests\\Input\\background.png --projection hammer", 
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
                "If you start with a projection other than equirectangular, use the --invert flag to convert FROM the target projection. Notice that some projections don't include the necessary data to completely recreate an equirect map.", 
                "earth_mercator");

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void InvertAllProjections()
        {

            foreach (MapProjection proj in Enum.GetValues(typeof(MapProjection)))
            {
                var args = ToArgs($"--projection {proj} --invert",
                     $"z{MethodBase.GetCurrentMethod().Name}_{proj}",
                    addExample: false);

                Parse(args, cliParams =>
                {
                    (var success, var projectionParams) = Program.ProcessParams(cliParams);
                    Projector.Project(projectionParams);
                });
            }
        }

        //Widgets

        //Grid
        [Test]
        public void WidgetGridBasic()
        {
            var args = ToArgs($"--widget grid",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });
            
        }

        [Test]
        public void WidgetGridSizing()
        {
            var args = ToArgs($"--widget grid --gridx 15 --gridy 60",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetGridColor()
        {
            var args = ToArgs($"--widget grid --gridcolor 0,255,0",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetGridWithProjection()
        {
            var args = ToArgs($"--widget grid --projection hammer",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        //Analemma

        [Test]
        public void WidgetAnalemmaBasic()
        {
            var args = ToArgs($"--widget analemma",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetAnalemmaSpacing()
        {
            var args = ToArgs($"--widget analemma --gridx 60",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetAnalemmaColor()
        {
            var args = ToArgs($"--widget analemma --widgetcolor 0,255,255",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetAnalemmaWithProjection()
        {
            var args = ToArgs($"--widget analemma --projection hammer",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        //TemporaryHours

        [Test]
        public void WidgetTemporaryHoursBasic()
        {
            var args = ToArgs($"--widget temporaryhours",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetTemporaryHoursPosition()
        {
            var args = ToArgs($"--widget temporaryhours --wlat 60 --wlon 60",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetTemporaryHoursColor()
        {
            var args = ToArgs($"--widget temporaryhours --widgetcolor 128,128,255",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetTemporaryHoursWithProjection()
        {
            var args = ToArgs($"--widget temporaryhours --projection sinusoidal",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        //LocalHours

        [Test]
        public void WidgetLocalHoursBasic()
        {
            var args = ToArgs($"--widget localhours",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetLocalHoursPosition()
        {
            var args = ToArgs($"--widget localhours --wlon 60",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetLocalHoursColor()
        {
            var args = ToArgs($"--widget localhours --widgetcolor 128,128,255",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetLocalHoursWithProjection()
        {
            var args = ToArgs($"--widget temporaryhours --projection orthographic",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        //Altitudes

        [Test]
        public void WidgetAltitudesBasic()
        {
            var args = ToArgs($"--widget altitudes",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetAltitudesPosition()
        {
            var args = ToArgs($"--widget Altitudes --wlat 45 --wlon 45",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetAltitudesColor()
        {
            var args = ToArgs($"--widget Altitudes --widgetcolor 128,255,128",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetAltitudesWithProjection()
        {
            var args = ToArgs($"--widget temporaryhours --projection azimuthal",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        //Tropics

        [Test]
        public void WidgetTropicsBasic()
        {
            var args = ToArgs($"--widget Tropics",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetTropicsColor()
        {
            var args = ToArgs($"--widget Tropics --widgetcolor 128,255,128",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetTropicsWithProjection()
        {
            var args = ToArgs($"--widget tropics --projection gnomonic",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        //Dateline

        [Test]
        public void WidgetDatelineBasic()
        {
            var args = ToArgs($"--widget Dateline",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetDatelineDay()
        {
            var args = ToArgs($"--widget Dateline --wday 180",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetDatelineColor()
        {
            var args = ToArgs($"--widget Dateline --widgetcolor 128,255,128",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetDatelineWithProjection()
        {
            var args = ToArgs($"--widget Dateline --projection azimuthal",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        //Datetime


        [Test]
        public void WidgetDatetimeBasic()
        {
            var args = ToArgs($"--widget Datetime",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetDatetimeDay()
        {
            var args = ToArgs($"--widget Datetime --wday 180",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetDatetimeColor()
        {
            var args = ToArgs($"--widget Datetime --widgetcolor 128,255,128",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetDatetimeWithProjection()
        {
            var args = ToArgs($"--widget Datetime --projection azimuthal",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        //Indicatrix

        [Test]
        public void WidgetIndicatrixBasic()
        {
            var args = ToArgs($"--widget Indicatrix",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetIndicatrixColor()
        {
            var args = ToArgs($"--widget Indicatrix --widgetcolor 128,255,128",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetIndicatrixSpacing()
        {
            var args = ToArgs($"--widget Indicatrix --gridx 60 --gridy 60",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetIndicatrixWithProjection()
        {
            var args = ToArgs($"--widget Indicatrix --projection azimuthal",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

        [Test]
        public void WidgetIndicatrixNaiveSpacing()
        {
            var args = ToArgs($"--widget Indicatrix --wnaivespacing",
                    MethodBase.GetCurrentMethod().Name);

            Parse(args, cliParams =>
            {
                (var success, var projectionParams) = Program.ProcessParams(cliParams);
                Projector.Project(projectionParams);
            });

        }

    }
}
