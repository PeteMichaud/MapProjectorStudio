using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using CommandLine.Text;
using MapProjectorLib;
using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorCLI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var argParser = new Parser(
                config => {
                    config.CaseSensitive = false;
                    config.CaseInsensitiveEnumValues = true;
                });

            var parseErrors = new List<Error>();

            var result = argParser.ParseArguments<CLIParams>(args).WithParsed(
                cliParams => {
                    var (success, pParams) = ProcessParams(cliParams);
                    if (success)
                        DoProjection(pParams);
                    else
                        Console.WriteLine("Arguments Invalid, aborting.");
                }).WithNotParsed(errs => parseErrors = errs.ToList());

            if (parseErrors.Count > 0)
            {
                var helpText = HelpText.AutoBuild(
                    result,
                    h => HelpText.DefaultParsingErrorsHandler(result, h),
                    e => e);
                Console.WriteLine(helpText);
            }

            Console.ReadKey();
        }

        static (bool result, ProjectionParams pParams) ProcessParams(
            CLIParams cliParams)
        {
            Image srcImage = null;
            Image backImage = null;

            try
            {
                srcImage = Projector.LoadImage(cliParams.srcImageFileName);
            } catch (IOException)
            {
                Console.WriteLine(
                    $"Could not open file {cliParams.srcImageFileName}");
                return (false, null);
            }

            try
            {
                if (!string.IsNullOrEmpty(cliParams.backImageFileName))
                    backImage =
                        Projector.LoadImage(cliParams.backImageFileName);
            } catch (IOException)
            {
                Console.WriteLine(
                    $"Could not open background file {cliParams.backImageFileName}");
                return (false, null);
            }

            var tParams = new TransformParams
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
                gridAngularOffset = cliParams.gridoff,

                widgetLat = cliParams.widgetLat,
                widgetLon = cliParams.widgetLon,
                widgetDay = cliParams.widgetDay
            };

            if (cliParams._backgroundColorValues != null)
                tParams.backgroundColor =
                    ToColor(cliParams._backgroundColorValues);

            if (cliParams._gridColorValues != null)
                tParams.gridColor = ToColor(cliParams._gridColorValues);

            if (cliParams._widgetColorValues != null)
                tParams.widgetColor = ToColor(cliParams._widgetColorValues);

            var loopParams = new LoopParams
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
                TimeIncr = cliParams.TimeIncr
            };

            var pParams = new ProjectionParams
            {
                transformParams = tParams,
                loopParams = loopParams,
                TargetProjection = cliParams.TargetProjection,
                Adjust = cliParams.Adjust,
                Width = cliParams.Width > 0 ? cliParams.Width : srcImage.Width,
                Height = cliParams.Height > 0
                    ? cliParams.Height
                    : srcImage.Height,
                srcImageFileName = cliParams.srcImageFileName,
                srcImage = srcImage,
                backImageFileName = cliParams.backImageFileName,
                backImage = backImage,
                outImageFileName = cliParams.outImageFileName,
                Invert = cliParams.Invert
            };

            return (true, pParams);
        }

        static Rgb24 ToColor(string colStr)
        {
            var byteColors = colStr.Split(',', ':').Select(
                strV => {
                    byte.TryParse(strV, out var byteVal);
                    return byteVal;
                }).ToArray();

            return new Rgb24(byteColors[0], byteColors[1], byteColors[2]);
        }

        static void DoProjection(ProjectionParams pParams)
        {
            //Console.Write(pParams.ToString());
            Projector.Project(pParams);
            Console.WriteLine(
                $"Complete! Check {pParams.outImageFileName} for results.");
        }
    }
}