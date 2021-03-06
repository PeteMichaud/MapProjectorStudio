using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Diagnostics;

using MapProjectorLib;

using CommandLine;
using CommandLine.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

[assembly:AssemblyVersion("1.3.0")]

namespace MapProjectorCLI
{
    public class Program
    {
        const string AUTHOR = "Pete Michaud";
        const int COPYRIGHT_YEAR = 2022;
        static void Main(string[] args)
        {
            var parseErrors = new List<Error>();
            var result = CLIParams.Parse(args).WithParsed(cliParams =>
            {
                Console.Write("Loading... ");
                (var success, var pParams) = ProcessParams(cliParams);
                if (success)
                {
                    //Console.Write(pParams.ToString());

                    Console.WriteLine($"Loaded! (Input file: {pParams.srcImageFileName})");
                    Console.WriteLine("Processing...");

                    var timer = new Stopwatch();
                    timer.Start();

                    Projector.Project(pParams, projectedImage =>
                    {
                        projectedImage.Save();
                    });

                    timer.Stop();
                    Console.WriteLine($"Completed in {timer.Elapsed}s!\nCheck {pParams.DestinationImageFileName} for results.");
                }
                else
                {
                    Console.WriteLine("Arguments Invalid, aborting.");
                }
            }).WithNotParsed(errs => parseErrors = errs.ToList());

            if(parseErrors.Count > 0)
            {
                Console.WriteLine("");
                var helpText = HelpText.AutoBuild(result,
                                              h => HelpText.DefaultParsingErrorsHandler(result, h),
                                              e => e);
                helpText.Copyright = new CopyrightInfo(AUTHOR, COPYRIGHT_YEAR);
                Console.WriteLine(helpText);
            }
        }


        public static (bool result, ProjectionParams pParams) ProcessParams(CLIParams cliParams)
        {
            SamplableImage srcImage = null;
            BackgroundImage backgroundImage = null;

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
                    srcImage = Projector.LoadImageForSampling(cliParams.srcImageFileName, cliParams.ColorSampleMode);
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine($"Source file not found: {cliParams.srcImageFileName}");
                    return (false, null);
                }
                catch (UnknownImageFormatException ex)
                {
                    Console.Write($"Could not open source file type: {cliParams.srcImageFileName}\n{ex.Message}");
                    return (false, null);
                }
                catch(AggregateException ex) //ImageSharp throws this for some reason
                {
                    if (ex.InnerException is UnknownImageFormatException)
                    {
                        Console.Write($"Could not open source file type: {cliParams.srcImageFileName}\n{ex.InnerException.Message}");
                        return (false, null);
                    }
                    else
                    {
                        throw;
                    }
                }

                try
                {
                    if (!string.IsNullOrEmpty(cliParams.backImageFileName))
                    {
                        cliParams.backImageFileName = QualifiedPath(cliParams.backImageFileName);
                        backgroundImage = Projector.LoadImageForCopying(cliParams.backImageFileName);
                    }
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine($"Background file not found: {cliParams.backImageFileName}");
                    return (false, null);
                }
                catch (UnknownImageFormatException ex)
                {
                    Console.Write($"Could not open background file type: {cliParams.srcImageFileName}\n{ex.Message}");
                    return (false, null);
                }
                catch (AggregateException ex) //ImageSharp throws this for some reason
                {
                    if (ex.InnerException is UnknownImageFormatException)
                    {
                        Console.Write($"Could not open background file type: {cliParams.srcImageFileName}\n{ex.InnerException.Message}");
                        return (false, null);
                    }
                    else
                    {
                        throw;
                    }
                }

            }

            var loopParams = new LoopParams()
            {
                LoopCount = cliParams.LoopCount,

                TiltIncr = cliParams.TiltIncr,
                TurnIncr = cliParams.TurnIncr,
                LatIncr = cliParams.LatIncr,
                LongIncr = cliParams.LongIncr,
                RotateIncr = cliParams.RotateIncr,

                xIncr = cliParams.xIncr,
                yIncr = cliParams.yIncr,
                zIncr = cliParams.zIncr,

                DateIncr = cliParams.DateIncr,
                TimeIncr = cliParams.TimeIncr,
            };

            var tParams = new TransformParams()
            {
                loopParams = loopParams,
                Widgets = cliParams.Widgets,

                Tilt = cliParams.tilt,
                Turn = cliParams.turn,
                Rotate = cliParams.rotate,
                Lat = cliParams.lat,
                Lon = cliParams.lon,
                Scale = cliParams.scale,
                Radius = cliParams.radius,
                xOffset = cliParams.xOffset,
                yOffset = cliParams.yOffset,

                //

                a = cliParams.a,

                // Projection Only

                aw = cliParams.aw,

                X = cliParams.x,
                Y = cliParams.y,
                Z = cliParams.z,

                ox = cliParams.ox,
                oy = cliParams.oy,
                oz = cliParams.oz,

                sun = cliParams.sun,
                Time = cliParams.time,
                Date = cliParams.date,

                //

                p = ProjMath.Clamp(cliParams.p, 0, ProjMath.PiOverTwo),

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

            if (cliParams._gridColorValues != null)
            {
                tParams.GridColor = ToColor(cliParams._gridColorValues);
            }

            if (cliParams._widgetColorValues != null)
            {
                tParams.WidgetColor = ToColor(cliParams._widgetColorValues);
            }

            var pParams = new ProjectionParams()
            {
                transformParams = tParams,
                
                WidgetRenderMode = cliParams.WidgetRenderMode,
                TargetProjection = cliParams.TargetProjection,
                Adjust = cliParams.Adjust,
                Width = cliParams.Width > 0 ? cliParams.Width : srcImage.Width,
                Height = cliParams.Height > 0 ? cliParams.Height : srcImage.Height,
                srcImageFileName = cliParams.srcImageFileName,
                SourceImage = srcImage,
                BackImageFileName = cliParams.backImageFileName,
                BackgroundImage = backgroundImage,
                DestinationImageFileName = cliParams.outImageFileName,
                Invert = cliParams.Invert,
            };

            if (cliParams._backgroundColorValues != null)
            {
                pParams.backgroundColor = ToColor(cliParams._backgroundColorValues);
            }

            return (true, pParams);
        }

        static RgbaVector ToColor(string colStr)
        {
            var byteColors = colStr.Split(',', ':').Select<string, byte>(strV =>
            {
                byte.TryParse(strV, out var byteVal);
                return byteVal;
            }).ToList();

            if(byteColors.Count == 4)
            {
                //all channels present, do nothing
            }
            else if(byteColors.Count == 3)
            {
                byteColors.Add(255); //add alpha channel 
            }
            else
            {
                throw new ArgumentException($"Bad Color Format \"{colStr}\". Provide either R,G,B or R,G,B,A (0-255)");
            }

            //Colors are 0-255, Alpha is 0-1
            return new RgbaVector(
                (float)byteColors[0], 
                (float)byteColors[1],
                (float)byteColors[2],
                (float)byteColors[3] / 255
                );
        }
    }
}
