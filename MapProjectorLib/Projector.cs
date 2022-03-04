using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorLib
{
    public class Projector
    {
       
        static public void Project(ProjectionParams pParams)
        {
            var tParams = pParams.transformParams;

            Transform transform = Transform.GetTransform(pParams.TargetProjection);

            var outImage = new Image(pParams.Width, pParams.Height);

            if (pParams.Adjust)
            {
                (pParams.Width, pParams.Height) = 
                    transform.AdjustSize(pParams.Width, pParams.Height, tParams);
            }

            for (int i = 0; i < pParams.loopParams.LoopCount; i++)
            {
                if (pParams.backImage != null)
                {
                    outImage = pParams.backImage.Clone();
                    tParams.noback = true;
                }

                transform.Init(tParams);

                if (pParams.Invert)
                {
                    transform.TransformImageInv(pParams.srcImage, outImage, tParams);
                    // No commands for inverse transformation yet
                }
                else
                {
                    transform.TransformImage(pParams.srcImage, outImage, tParams);
                    transform.DoCommands(outImage, tParams);
                }

                if (pParams.loopParams.LoopCount > 1)
                {
                    outImage.Save(string.Format("{0}{1,4:0000}.{2}", pParams.outImageName, i, pParams.outImageExt));

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
                    outImage.Save(pParams.outImageFileName);
                }      
            }
        }
    
        static public Rgb24 ToColor(string colStr)
        {
            var colors = colStr.Split(',', ':');
            byte.TryParse(colors[0], out var r);
            byte.TryParse(colors[1], out var g);
            byte.TryParse(colors[2], out var b);

            return new Rgb24(r, g, b);
        }

        static public Image LoadImage(string fileName)
        {
            return Image.Load(fileName);
        }
    }
}
