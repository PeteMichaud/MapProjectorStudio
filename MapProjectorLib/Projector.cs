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

            if (tParams.loopParams.LoopCount == 1)
            {
                ApplyTransform(transform, outImage, pParams);

                outImage.Save(pParams.DestinationImageFileName,
                       pParams.SourceImage.BitDepth);
            } 
            else //loop
            {
                for (var i = 0; i < tParams.loopParams.LoopCount; i++)
                {
                    tParams.CurrentLoopIndex = i;
                    ApplyTransform(transform, outImage, pParams);
               
                    outImage.Save(pParams.DestinationImageFileName,
                        seriesNumber: i,
                        pParams.SourceImage.BitDepth);
                }
            } //end else loop

            outImage.Dispose();
            
            // if I dispose these I can't call the same projection twice 
            // in the same session, so I'm not doing that anymore
            //pParams.SourceImage.Dispose();
            //if (pParams.BackgroundImage != null) pParams.BackgroundImage.Dispose();
        }

        static void ApplyTransform(Transform transform, DestinationImage destImage, ProjectionParams pParams)
        {
            transform.Init(pParams.transformParams);

            if (pParams.Invert)
            {
                transform.TransformImageInv(
                    pParams.SourceImage, destImage, pParams.transformParams);
                // No widgets for inverse transformation yet
            }
            else
            {
                transform.TransformImage(
                    pParams.SourceImage, destImage, pParams.transformParams);
                WidgetRenderer.Render(destImage, pParams.transformParams, transform);
            }

            MaybeApplyBackground(destImage, pParams);
        }

        static void MaybeApplyBackground(DestinationImage destImage, ProjectionParams pParams)
        {
            // This can be kind of an expensive operation, so I try to avoid it if possible.
            // The basic logic is that if any of the following applies, then use a background:
            // 1. There is a background color with alpha > 0 or background image at all, and
            // 2. any destination pixels didn't get written to, or
            // 3. the source image might have transparent pixels (eg. a png with an alpha channel)

            if(pParams.ShouldCompositeBackground)
            {
                var background = (pParams.BackgroundImage != null) 
                    ? pParams.BackgroundImage.ImageData
                    : new Image<RgbaVector>(pParams.Width, pParams.Height, pParams.backgroundColor);

                var processorCreator = new DrawImageProcessor(
                    background,
                    SixLabors.ImageSharp.Point.Empty,
                    PixelColorBlendingMode.Normal,
                    PixelAlphaCompositionMode.DestAtop,
                    1f
                );

                var pxProcessor = processorCreator.CreatePixelSpecificProcessor(
                    Configuration.Default,
                    destImage.ImageData,
                    destImage.ImageData.Bounds()
                );

                pxProcessor.Execute();

                // if this wasn't a real background image, but 
                // instead a throwaway solid color background image
                if(background != pParams.BackgroundImage?.ImageData)
                {
                    // I should maybe keep this around if we're in a looping scenario.
                    // If it causes performance issues, revisit
                    background.Dispose();
                }
            }
        }

        public static SamplableImage LoadImageForSampling(string imagePath, 
            ColorSampleMode mode = ColorSampleMode.Fast)
        {
            return SamplableImage.Load(imagePath, mode);
        }

        public static Image LoadImageForCopying(string imagePath)
        {
            return Image.Load(imagePath);
        }
    }
}