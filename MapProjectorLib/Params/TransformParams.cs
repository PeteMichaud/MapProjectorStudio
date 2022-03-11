using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorLib
{
    public class TransformParams
    {
        public LoopParams loopParams;

        public int CurrentLoopIndex
        {
            get { return loopParams.CurrentLoopIndex; }
            set { loopParams.CurrentLoopIndex = value; }
        }

        double _tilt = 0;
        public double Tilt 
        { 
            get 
            { 
                return _tilt + loopParams.TiltIncr * loopParams.CurrentLoopIndex; 
            }
            set 
            { 
                _tilt = value; 
            }
        }

        double _turn = 0;
        public double Turn
        {
            get
            {
                return _turn + loopParams.TurnIncr * loopParams.CurrentLoopIndex;
            }
            set
            {
                _turn = value;
            }
        }

        double _lat = 0;
        public double Lat
        {
            get
            {
                return _lat + loopParams.LatIncr * loopParams.CurrentLoopIndex;
            }
            set
            {
                _lat = value;
            }
        }

        double _lon = 0;
        public double Lon
        {
            get
            {
                return _lon + loopParams.LongIncr * loopParams.CurrentLoopIndex;
            }
            set
            {
                _lon = value;
            }
        }

        public double Scale = 1.0;
        public double xOffset = 0;
        public double yOffset = 0;
        public double Radius = 0;
        public double Rotate = 0.0;

        //X dimension of Oblateness, measured in planet radii
        public double ox = 1.0; 

        //Y dimension of Oblateness, measured in planet radii
        public double oy = 1.0; 

        // z dimension of Oblateness, measured in planet radii
        public double oz = 1.1; 

        //

        public double a = 1.0; //Sinusoidal2 only
        
        //Relevant to polar projections: azimuthal, gnomonic, rectilinear, stereographic
        public double conic = 1.0; 
        //Conic Radius. Relevant to polar projections: azimuthal, gnomonic, rectilinear, stereographic
        public double conicr = 0.0; 

        //Reference parallel for projections that can use it: equalarea, sinusoidal, mollweide
        public double p = 0.0; 

        //Perspective
        public double aw = ProjMath.ToRadians(20);
        // Simulate the sun on the perspective view (date and time are relevant)
        public bool sun; 
        //Day number in year. Relevant for the -sun option
        double _date = 0.0;
        public double Date
        {
            get
            {
                return _date + loopParams.DateIncr * loopParams.CurrentLoopIndex;
            }
            set
            {
                _date = value;
            }
        }
        //Hours from midnight, UTC, in decimal hours (so 4.5 is half past four in the morning). Relevant for the --sun option
        double _time = 0.0;
        public double Time
        {
            get
            {
                return _time + loopParams.TimeIncr * loopParams.CurrentLoopIndex;
            }
            set
            {
                _time = value;
            }
        }
        //X dimension of viewing position, measured in planet radii
        double _x = 8.0;
        public double X
        {
            get
            {
                return _x + loopParams.xIncr * loopParams.CurrentLoopIndex;
            }
            set
            {
                _x = value;
            }
        }

        //Y dimension of viewing position, measured in planet radii
        double _y = 0.0;
        public double Y
        {
            get
            {
                return _y + loopParams.yIncr * loopParams.CurrentLoopIndex;
            }
            set
            {
                _y = value;
            }
        }
        //Z dimension of viewing position, measured in planet radii
        double _z = 0.0;
        public double Z
        {
            get
            {
                return _z + loopParams.zIncr * loopParams.CurrentLoopIndex;
            }
            set
            {
                _z = value;
            }
        }
        public bool SomeDestinationPixelsAreBlank = false;

        //Widgets

        public MapWidget Widgets;

        public RgbaVector widgetColor = new RgbaVector(255, 0, 0, 1);
        public double widgetDay = 0.0;
        public double widgetLat = 0.0; //radians
        public double widgetLon = 0.0; //radians
        public bool widgetSmartSpacing = true;
        public int gridX = 30; // X Spacing of Grid (Degrees)
        public int gridY = 30; // Y Spacing of Grid (Degrees)
        public RgbaVector gridColor = new RgbaVector(255, 255, 255, 1);

    }
}