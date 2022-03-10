using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorLib
{
    public class TransformParams
    {
        public double radius = 0;
        public double tilt = 0.0;
        public double turn = 0.0;
        public double rotate = 0.0;
        public double scale = 1.0;
        public double lat = 0.0;
        public double lon = 0.0;

        public double xOffset = 0;
        public double yOffset = 0;

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
        public double date = 0.0; 
        //Hours from midnight, UTC, in decimal hours (so 4.5 is half past four in the morning). Relevant for the --sun option
        public double time = 0.0; 
        //X dimension of viewing position, measured in planet radii
        public double x = 8.0; 
        
        //Y dimension of viewing position, measured in planet radii
        public double y = 0.0; 

        //Z dimension of viewing position, measured in planet radii
        public double z = 0.0; 

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