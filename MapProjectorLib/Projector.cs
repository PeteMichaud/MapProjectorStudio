using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors.Drawing;

namespace MapProjectorLib
{
    public static class Projector
    {
        public static void Project(ProjectionParams pParams)
        {
            var tParams = pParams.transformParams;
            var transform = Transform.GetTransform(pParams.TargetProjection);
            var outImage = new DestinationImage(pParams.Width, pParams.Height);

            if (pParams.Adjust)
            {
                (pParams.Width, pParams.Height) =
                    transform.AdjustSize(
                        pParams.Width, pParams.Height, tParams);
            }

            for (var i = 0; i < pParams.loopParams.LoopCount; i++)
            {
                transform.Init(tParams);

                if (pParams.Invert)
                {
                    transform.TransformImageInv(
                        pParams.SourceImage, outImage, tParams);
                    // No widgets for inverse transformation yet
                } 
                else
                {
                    transform.TransformImage(
                        pParams.SourceImage, outImage, tParams);
                    WidgetRenderer.Render(outImage, tParams, transform);
                }

                if (pParams.loopParams.LoopCount > 1)
                {
                    ApplyBackgroundAndSave(outImage, i, pParams);

                    tParams.turn += pParams.loopParams.TurnIncr;
                    tParams.tilt += pParams.loopParams.TiltIncr;
                    tParams.lat += pParams.loopParams.LatIncr;
                    tParams.lon += pParams.loopParams.LongIncr;
                    tParams.x += pParams.loopParams.xIncr;
                    tParams.y += pParams.loopParams.yIncr;
                    tParams.z += pParams.loopParams.zIncr;
                    tParams.time += pParams.loopParams.TimeIncr;
                    tParams.date += pParams.loopParams.DateIncr;
                } 
                else
                {
                    ApplyBackgroundAndSave(outImage, pParams);
                }
            }

            outImage.Dispose();
            pParams.SourceImage.Dispose();
            if (pParams.BackgroundImage != null) pParams.BackgroundImage.Dispose();
            if (pParams.BackgroundImage != null) pParams.BackgroundImage.Dispose();

        }

        static void MaybeApplyBackground(DestinationImage outImage, ProjectionParams pParams)
        {
            if(pParams.ShouldCompositeBackground)
            {
                var background = GetBackgroundCompositeImage(pParams);

                var processorCreator = new DrawImageProcessor(
                    background,
                    SixLabors.ImageSharp.Point.Empty,
                    PixelColorBlendingMode.Normal,
                    PixelAlphaCompositionMode.DestAtop,
                    1f
                );

                var pxProcessor = processorCreator.CreatePixelSpecificProcessor(
                    Configuration.Default,
                    outImage.ImageData,
                    outImage.ImageData.Bounds()
                );

                pxProcessor.Execute();

                //if this wasn't a real background image, but instead a throwaway solid color background image
                if(background != pParams.BackgroundImage?.ImageData)
                {
                    // I should maybe keep this around if we're in a looping scenario.
                    // If it causes performance issues, revisit
                    background.Dispose();
                }
            }
        }

        static Image<RgbaVector> GetBackgroundCompositeImage(ProjectionParams pParams)
        {
            if (pParams.BackgroundImage != null) return pParams.BackgroundImage.ImageData;
            return new Image<RgbaVector>(pParams.Width, pParams.Height, pParams.backgroundColor);
        }

        static void ApplyBackgroundAndSave(DestinationImage outImage, ProjectionParams pParams)
        {
            MaybeApplyBackground(outImage, pParams);
            outImage.Save(pParams.DestinationImageFileName, pParams.SourceImage.BitDepth);
        }

        static void ApplyBackgroundAndSave(DestinationImage outImage, int seriesNumber, ProjectionParams pParams)
        {
            MaybeApplyBackground(outImage, pParams);
            outImage.Save(pParams.DestinationImageFileName, seriesNumber, pParams.SourceImage.BitDepth);
        }

        public static SamplableImage LoadImageForSampling(string imagePath, ColorSampleMode mode = ColorSampleMode.Fast)
        {
            return SamplableImage.Load(imagePath, mode);
        }

        public static Image LoadImageForCopying(string fileName)
        {
            return Image.Load(fileName);
        }
    }
}