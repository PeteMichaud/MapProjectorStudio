using System;
using NUnit.Framework;
using CommandLine;
using MapProjectorLib;
using System.Reflection;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Linq;

namespace MapProjectorCLI.Tests
{

    public class ProjectionTests : ExampleGeneratingTests
    {
        const string ioArgs = "-f ..\\..\\Tests\\Input\\{1}.png -o ..\\..\\Tests\\Output\\{0}.png";

        protected string[] ToArgs(
            string rawArgs,
            string inFileName = "earth_equirect",
            [CallerMemberName] string outFileName = "error")
        {
            rawArgs += $" {string.Format(ioArgs, outFileName, inFileName)}";
            return rawArgs.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static void Parse(string[] args, Action<CLIParams> success)
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


        [TestFixture]
        public class BasicUsage : ProjectionTests
        {

            [Test]
            public void RunAllProjections()
            {
                foreach (MapProjection proj in Enum.GetValues(typeof(MapProjection)))
                {
                    var title = $"To{proj}";
                    var args = ToArgs($"--projection {proj}", outFileName: title);
                    AddExample(title, args);
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
                var args = ToArgs($"--bg ..\\..\\Tests\\Input\\background.png --projection hammer");
                AddExample(args, "Many projections leave a blank area around the perimeter of the map. Fill that blank area with an optional background image");
                Parse(args, cliParams =>
                {
                    (var success, var projectionParams) = Program.ProcessParams(cliParams);
                    Projector.Project(projectionParams);
                });

            }

            [Test]
            public void WithBackgroundColor()
            {
                var args = ToArgs($"--bgcolor 255,0,0 --projection perspective");
                AddExample(args, "Many projections leave a blank area around the perimeter of the map. Fill that blank area with an optional background color");
                Parse(args, cliParams =>
                {
                    (var success, var projectionParams) = Program.ProcessParams(cliParams);
                    Projector.Project(projectionParams);
                });

            }

            [Test]
            public void InvertFromMercator()
            {
                var args = ToArgs($"--projection mercator --invert", inFileName: "earth_mercator");
                AddExample(args,
                    "If you start with a projection other than equirectangular, use the --invert flag to convert FROM the target projection. Notice that some projections don't include the necessary data to completely recreate an equirect map."
                );
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
                         outFileName: $"z{MethodBase.GetCurrentMethod().Name}_{proj}");

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });
                }
            }
        }

        public class WidgetUsage : ProjectionTests
        {
            [TestFixture]
            public class GridUsage : WidgetUsage
            {

                [Test]
                public void BasicGrid()
                {
                    var args = ToArgs($"--widget grid");
                    AddExample(args);
                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void GridSizing()
                {
                    var args = ToArgs($"--widget grid --gridx 15 --gridy 60");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void GridColor()
                {
                    var args = ToArgs($"--widget grid --gridcolor 0,255,0");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void GridWithProjection()
                {
                    var args = ToArgs($"--widget grid --projection hammer");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }
            }

            [TestFixture]
            public class AnalemmaUsage : WidgetUsage
            {
                [Test]
                public void AnalemmaBasic()
                {
                    var args = ToArgs($"--widget analemma");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void AnalemmaSpacing()
                {
                    var args = ToArgs($"--widget analemma --gridx 60");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void AnalemmaColor()
                {
                    var args = ToArgs($"--widget analemma --widgetcolor 0,255,255");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void AnalemmaWithProjection()
                {
                    var args = ToArgs($"--widget analemma --projection hammer");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

            }

            [TestFixture]
            public class TemporaryHoursUsage : WidgetUsage
            {
                [Test]
                public void BasicTemporaryHours()
                {
                    var args = ToArgs($"--widget temporaryhours");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void TemporaryHoursPosition()
                {
                    var args = ToArgs($"--widget temporaryhours --wlat 60 --wlon 60");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void TemporaryHoursColor()
                {
                    var args = ToArgs($"--widget temporaryhours --widgetcolor 128,128,255");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void TemporaryHoursWithProjection()
                {
                    var args = ToArgs($"--widget temporaryhours --projection sinusoidal");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

            }

            [TestFixture]
            public class LocalHoursUsage : WidgetUsage
            {
                [Test]
                public void BasicLocalHours()
                {
                    var args = ToArgs($"--widget localhours");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void LocalHoursPosition()
                {
                    var args = ToArgs($"--widget localhours --wlon 60");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void LocalHoursColor()
                {
                    var args = ToArgs($"--widget localhours --widgetcolor 128,128,255");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void LocalHoursWithProjection()
                {
                    var args = ToArgs($"--widget temporaryhours --projection orthographic");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });
                }
            }

            [TestFixture]
            public class AltitudesUsage : WidgetUsage
            {
                [Test]
                public void AltitudesBasic()
                {
                    var args = ToArgs($"--widget altitudes");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void AltitudesPosition()
                {
                    var args = ToArgs($"--widget Altitudes --wlat 45 --wlon 45");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void AltitudesColor()
                {
                    var args = ToArgs($"--widget Altitudes --widgetcolor 128,255,128");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void AltitudesWithProjection()
                {
                    var args = ToArgs($"--widget temporaryhours --projection azimuthal");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }
            }

            [TestFixture]
            public class TropicsUsage : WidgetUsage
            {
                [Test]
                public void BasicTropics()
                {
                    var args = ToArgs($"--widget Tropics");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void TropicsColor()
                {
                    var args = ToArgs($"--widget Tropics --widgetcolor 128,255,128");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void TropicsWithProjection()
                {
                    var args = ToArgs($"--widget tropics --projection gnomonic");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

            }

            [TestFixture]
            public class DatelineUsage : WidgetUsage
            {
                [Test]
                public void BasicDateline()
                {
                    var args = ToArgs($"--widget Dateline");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void DatelineDay()
                {
                    var args = ToArgs($"--widget Dateline --wday 180");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void DatelineColor()
                {
                    var args = ToArgs($"--widget Dateline --widgetcolor 128,255,128");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void DatelineWithProjection()
                {
                    var args = ToArgs($"--widget Dateline --projection azimuthal");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }
            }

            [TestFixture]
            public class DatetimeUsage : WidgetUsage
            {
                [Test]
                public void BasicDatetime()
                {
                    var args = ToArgs($"--widget Datetime");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void DatetimeDay()
                {
                    var args = ToArgs($"--widget Datetime --wday 180");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void DatetimeColor()
                {
                    var args = ToArgs($"--widget Datetime --widgetcolor 128,255,128");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void DatetimeWithProjection()
                {
                    var args = ToArgs($"--widget Datetime --projection azimuthal");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }
            }

            [TestFixture]
            public class IndicatrixUsage : WidgetUsage
            {
                [Test]
                public void BasicIndicatrix()
                {
                    var args = ToArgs($"--widget Indicatrix");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void IndicatrixColor()
                {
                    var args = ToArgs($"--widget Indicatrix --widgetcolor 128,255,128");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void IndicatrixSpacing()
                {
                    var args = ToArgs($"--widget Indicatrix --gridx 60 --gridy 60");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void IndicatrixWithProjection()
                {
                    var args = ToArgs($"--widget Indicatrix --projection azimuthal");
                    AddExample(args);

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }

                [Test]
                public void IndicatrixNaiveSpacing()
                {
                    var args = ToArgs($"--widget Indicatrix --wnaivespacing");
                    AddExample(args, "By default this widget tries to be smart about where it places the indicatrices by skipping some nearer the poles. Use this flag to disable the smartness.");

                    Parse(args, cliParams =>
                    {
                        (var success, var projectionParams) = Program.ProcessParams(cliParams);
                        Projector.Project(projectionParams);
                    });

                }
            }
        }
    }
}
