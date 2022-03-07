using System;
using System.Collections.Generic;
using System.Linq;
using MapProjectorLib;
using CommandLine;
using CommandLine.Text;
using SixLabors.ImageSharp.PixelFormats;
using System.Reflection;
using System.IO;
using System.Diagnostics;
namespace MapProjectorCLI
{
    public class Program
    {
        static void Main(string[] args)
        {
            var parseErrors = new List<Error>();
            Console.Write("Loading... ");
            var result = CLIParams.Parse(args).WithParsed(cliParams =>
            {
                (var success, var pParams) = ProcessParams(cliParams);
                if (success)
                {
                    //Console.Write(pParams.ToString());

                    Console.WriteLine("Loaded!");
                    Console.WriteLine("Processing...");

                    Stopwatch timer = new Stopwatch();
                    timer.Start();

                    Projector.Project(pParams);

                    timer.Stop();
                    Console.WriteLine($"Completed in {timer.Elapsed}s!\nCheck {pParams.outImageFileName} for results.");
                }
                else
                {
                    Console.WriteLine("Arguments Invalid, aborting.");
                }
            }).WithNotParsed(errs => parseErrors = errs.ToList());

            if(parseErrors.Count > 0)
            {
                var helpText = HelpText.AutoBuild(result,
                                              h => HelpText.DefaultParsingErrorsHandler(result, h),
                                              e => e);
                Console.WriteLine(helpText);
            }

            Console.ReadKey();

        }


        internal static (bool result, ProjectionParams pParams) ProcessParams(CLIParams cliParams)
        {
            Image srcImage = null;
            Image backImage = null;

            //Qualify File Paths and Load Files
            {
                //cross platform
                var assm = typeof(Program).GetTypeInfo().Assembly;
                var currentDir = Path.GetDirectoryName(assm.Location);

                string QualifiedPath(string maybeQualifiedPath)
                {
                    if (Path.IsPathRooted(maybeQualifiedPath)) return maybeQualifiedPath;
                    return Path.Combine(currentDir, maybeQualifiedPath);
                }

                cliParams.srcImageFileName = QualifiedPath(cliParams.srcImageFileName);
                cliParams.outImageFileName = QualifiedPath(cliParams.outImageFileName);

                try
                {
                    srcImage = Projector.LoadImage(cliParams.srcImageFileName, cliParams.ColorSampleMode);
                }
                catch (IOException)
                {
                    Console.WriteLine($"Could not open file {cliParams.srcImageFileName}");
                    return (false, null);
                }

                try
                {
                    if (!string.IsNullOrEmpty(cliParams.backImageFileName))
                    {
                        cliParams.backImageFileName = QualifiedPath(cliParams.backImageFileName);
                        backImage = Projector.LoadImage(cliParams.backImageFileName);
                    }
                }
                catch (IOException)
                {
                    Console.WriteLine($"Could not open background file {cliParams.backImageFileName}");
                    return (false, null);
                }
            }
            
            var tParams = new TransformParams()
            {
                Widgets = cliParams.Widgets,

                tilt = cliParams.tilt,
                turn = cliParams.turn,
                rotate = cliParams.rotate,
                lat = cliParams.lat,
                lon = cliParams.lon,
                scale = cliParams.scale,
                radius = cliParams.radius,
                xOffset = cliParams.xOffset,
                yOffset = cliParams.yOffset,

                //

                a = cliParams.a,

                // Projection Only

                aw = cliParams.aw,

                x = cliParams.x,
                y = cliParams.y,
                z = cliParams.z,

                ox = cliParams.ox,
                oy = cliParams.oy,
                oz = cliParams.oz,

                sun = cliParams.sun,
                time = cliParams.time,
                date = cliParams.date,

                //

                p = cliParams.p,

                conic = cliParams.conic,
                conicr = cliParams.conicr,

                // Grid

                gridX = cliParams.gridx,
                gridY = cliParams.gridy,

                widgetLat = cliParams.widgetLat,
                widgetLon = cliParams.widgetLon,
                widgetDay = cliParams.widgetDay,
                widgetSmartSpacing = !cliParams.widgetNaiveSpacing,
            };

            if (cliParams._backgroundColorValues != null)
            {
                tParams.backgroundColor = ToColor(cliParams._backgroundColorValues);
            }

            if (cliParams._gridColorValues != null)
            {
                tParams.gridColor = ToColor(cliParams._gridColorValues);
            }

            if (cliParams._widgetColorValues != null)
            {
                tParams.widgetColor = ToColor(cliParams._widgetColorValues);
            }

            var loopParams = new LoopParams()
            {
                LoopCount = cliParams.LoopCount,

                TiltIncr = cliParams.TiltIncr,
                TurnIncr = cliParams.TurnIncr,
                LatIncr = cliParams.LatIncr,
                LongIncr = cliParams.LongIncr,

                xIncr = cliParams.xIncr,
                yIncr = cliParams.yIncr,
                zIncr = cliParams.zIncr,

                DateIncr = cliParams.DateIncr,
                TimeIncr = cliParams.TimeIncr,
            };

            var pParams = new ProjectionParams()
            {
                transformParams = tParams,
                loopParams = loopParams,
                TargetProjection = cliParams.TargetProjection,
                Adjust = cliParams.Adjust,
                Width = cliParams.Width > 0 ? cliParams.Width : srcImage.Width,
                Height = cliParams.Height > 0 ? cliParams.Height : srcImage.Height,
                srcImageFileName = cliParams.srcImageFileName,
                srcImage = srcImage,
                backImageFileName = cliParams.backImageFileName,
                backImage = backImage,
                outImageFileName = cliParams.outImageFileName,
                Invert = cliParams.Invert,
            };

            return (true, pParams);
        }

        static Rgb24 ToColor(string colStr)
        {
            var byteColors = colStr.Split(',', ':').Select<string, byte>(strV =>
            {
                byte.TryParse(strV, out var byteVal);
                return byteVal;
            }).ToArray();

            return new Rgb24(byteColors[0], byteColors[1], byteColors[2]);
        }
    }
}
