using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors.Drawing;

using VectSharp;

namespace MapProjectorLib
{
    public static class Projector
    {
        public static void Project(ProjectionParams pParams, Action<ProjectedImage> processProjectedImage = null)
        {
            //Default action after projection is to save the file
            if (processProjectedImage == null)
            {
                processProjectedImage = projectedImage => projectedImage.Save();
            }

            var tParams = pParams.transformParams;
            var transform = Transform.GetTransform(pParams.TargetProjection);

            for (var i = 0; i < tParams.loopParams.LoopCount; i++)
            {
                tParams.CurrentLoopIndex = i;
                TransformThenProcess(transform, pParams, processProjectedImage);
            }
            
            // if I dispose these I can't call the same projection twice 
            // in the same session, so I'm not doing that anymore
            //pParams.SourceImage.Dispose();
            //if (pParams.BackgroundImage != null) pParams.BackgroundImage.Dispose();
        }

        static DestinationImage GetFreshDestinationImage(ProjectionParams pParams, Transform transform)
        {
            var outImage = new DestinationImage(pParams.Width, pParams.Height);

            if (pParams.Adjust)
            {
                (pParams.Width, pParams.Height) =
                    transform.AdjustSize(
                        pParams.Width, pParams.Height, pParams.transformParams);
            }

            return outImage;
        }

        static void TransformThenProcess(
            Transform transform, ProjectionParams pParams, 
            Action<ProjectedImage> processProjectedImage)
        {
            var destImage = GetFreshDestinationImage(pParams, transform);

            transform.Init(pParams.transformParams);

            if (pParams.Invert)
            {
                transform.TransformImageInv(
                    pParams.SourceImage, destImage, pParams.transformParams);

                // No widgets for inverse transformation yet
                MaybeApplyBackground(destImage, pParams);
                ProcessThenDispose(destImage, widgetVector: null);
            }
            else
            {
                var widgetrender = WidgetRenderer
                    .Render(destImage, pParams.transformParams, transform);
                
                switch (pParams.WidgetRenderMode)
                {
                    case WidgetRenderMode.Combined:
                        
                        transform.TransformImage(
                            pParams.SourceImage, destImage, pParams.transformParams);
                        var widgetRaster = widgetrender.ToRaster();
                        
                        ApplyWidgets(destImage, widgetRaster);
                        MaybeApplyBackground(destImage, pParams);
                        
                        ProcessThenDispose(destImage, widgetVector: null);
                        
                        break;
                    
                    case WidgetRenderMode.WidgetOnly:
                        
                        ProcessThenDispose(outputImage: null, widgetrender.ToSvg());
                        
                        break;

                    case WidgetRenderMode.Separate:
                        
                        transform.TransformImage(
                            pParams.SourceImage, destImage, pParams.transformParams);
                        MaybeApplyBackground(destImage, pParams);

                        ProcessThenDispose(destImage, widgetVector: null);
                        ProcessThenDispose(outputImage: null, widgetrender.ToSvg());

                        break;
                }
            }

            void ProcessThenDispose(DestinationImage outputImage, Page widgetVector)
            {
                processProjectedImage(new ProjectedImage(
                    outputImage, 
                    widgetVector,
                    pParams.SourceImage,
                    pParams.DestinationImageFileName,
                    pParams.transformParams.loopParams.LoopCount,
                    pParams.transformParams.loopParams.CurrentLoopIndex
                ));
                outputImage?.Dispose();
            }
        }

        static void ApplyWidgets(DestinationImage destImage, Image<Rgba32> widgets)
        {
            
            var processorCreator = new DrawImageProcessor(
                widgets,
                SixLabors.ImageSharp.Point.Empty,
                PixelColorBlendingMode.Normal,
                PixelAlphaCompositionMode.SrcOver,
                1f
            );

            var pxProcessor = processorCreator.CreatePixelSpecificProcessor(
                Configuration.Default,
                destImage.ImageData,
                destImage.ImageData.Bounds()
            );

            pxProcessor.Execute();

            widgets.Dispose();
            
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

        public static BackgroundImage LoadImageForCopying(string imagePath)
        {
            return BackgroundImage.Load(imagePath);
        }
    }
}