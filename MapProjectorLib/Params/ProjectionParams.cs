namespace MapProjectorLib
{
    public class ProjectionParams
    {
        string _outImageFileName;

        public bool
            Adjust =
                false; //Set Source image to adjusted width and height options before processing

        public Image backImage;
        public string backImageFileName;


        //public CommandList commandList = new CommandList();
        public string[][] commandList = new string[0][];
        public int Height; //Target Height
        public bool Invert = false; //Invert the specified operation
        public LoopParams loopParams;
        public string outImageExt;
        public string outImageName;

        public Image srcImage;
        public string srcImageFileName;

        public MapProjection
            TargetProjection; //Target Projection (or Source Projection if Invert flag specified)

        public TransformParams transformParams;
        public int Width; //Target Width

        public string outImageFileName
        {
            get => _outImageFileName;
            set
            {
                _outImageFileName = value;
                var etxInx = _outImageFileName.LastIndexOf('.');
                outImageName = _outImageFileName.Substring(0, etxInx);
                outImageExt = _outImageFileName.Substring(etxInx + 1);
            }
        }

        //


        public override string ToString()
        {
            return string.Format(
                @"Projection Params:

    Target Projection: {0}
    Width: {1}
    Height: {2}
    Adjust: {3}
    Input File: {4}
    Output File: {5}
    Background File: {6}
    Invert Operation: {7}

{8}

{9}
",
                TargetProjection,
                Width,
                Height,
                Adjust,
                srcImageFileName,
                outImageFileName,
                backImageFileName,
                Invert,
                loopParams,
                transformParams
            );
        }
    }
}