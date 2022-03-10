using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorLib
{
    public class ProjectionParams
    {
        public TransformParams transformParams;
        public LoopParams loopParams;

        //Set Source image to adjusted width and height options before processing
        public bool Adjust = false;
        //Invert the specified operation
        public bool Invert = false; 
        
        //Target Projection (or Source Projection if Invert flag specified)
        public MapProjection TargetProjection; 
        
        public int Width; //Target Width
        public int Height; //Target Height

        public string BackImageFileName;
        public Image BackgroundImage;
        public RgbaVector backgroundColor = new RgbaVector(0, 0, 0, 1);

        public string srcImageFileName;
        public SamplableImage SourceImage;
        
        public string outImageExt;
        public string outImageName;
        string _destinationImageFileName;
        public string DestinationImageFileName
        {
            get => _destinationImageFileName;
            set
            {
                _destinationImageFileName = value;
                var etxInx = _destinationImageFileName.LastIndexOf('.');
                outImageName = _destinationImageFileName.Substring(0, etxInx);
                outImageExt = _destinationImageFileName.Substring(etxInx + 1);
            }
        }
        public bool HasBackgroundColor => BackgroundImage != null 
            || backgroundColor.A > 0f;
        public bool ShouldCompositeBackground
        {
            get => HasBackgroundColor 
                && (transformParams.SomeDestinationPixelsAreBlank 
                    || SourceImage.SourceMayHaveTransparency);
        }
    }
}