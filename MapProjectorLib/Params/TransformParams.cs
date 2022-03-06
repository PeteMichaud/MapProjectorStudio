using SixLabors.ImageSharp.PixelFormats;

namespace MapProjectorLib
{
    public class TransformParams
    {
        //

        public double a = 1.0; //Only relevant to Sinusoidal2 projection

        // Projection Only

        public double aw = ProjMath.ToRadians(20);
        public Rgb24 backgroundColor = new Rgb24(0, 0, 0);

        public double
            conic = 1.0; //Relevant to polar projections: azimuthal, gnomonic, rectilinear, stereographic

        public double
            conicr =
                0.0; //Conic Radius.Relevant to polar projections: azimuthal, gnomonic, rectilinear, stereographic

        public double
            date = 0.0; //Day number in year. Relevant for the -sun option

        //public double gridAngularOffset = 0; //Angular Offset
        public Rgb24 gridColor = new Rgb24(255, 255, 255);

        // Grid

        public int gridX = 30; //"X Spacing of Grid (Degrees)
        public int gridY = 30; // Y Spacing of Grid (Degrees)
        public double lat = 0.0;
        public double lon = 0.0;

        public double
            ox = 1.0; //X dimension of Oblateness, measured in planet radii

        public double
            oy = 1.0; //Y dimension of Oblateness, measured in planet radii

        public double
            oz = 1.1; // z dimension of Oblateness, measured in planet radii

        //

        public double
            p = 0.0; //Reference parallel for projections that can use it: equalarea, sinusoidal, mollweide

        public double radius = 0;
        public double rotate = 0.0;
        public double scale = 1.0;

        public bool
            sun; //"Simulate the sun on the perspective view (date and time are relevant)

        public double tilt = 0.0;

        public double
            time = 0.0; //Hours from midnight, UTC, in decimal hours (so 4.5 is half past four in the morning). Relevant for the --sun option

        public double turn = 0.0;

        //Widgets

        public MapWidget Widgets;

        public Rgb24 widgetColor = new Rgb24(255, 0, 0);
        public double widgetDay = 0.0;
        public double widgetLat = 0.0; //radians
        public double widgetLon = 0.0; //radians
        public bool widgetSmartSpacing = true;

        public double
            x = 8.0; //X dimension of viewing position, measured in planet radii

        public double xOffset = 0;

        public double
            y = 0.0; //Y dimension of viewing position, measured in planet radii

        public double yOffset = 0;

        public double
            z = 0.0; //Z dimension of viewing position, measured in planet radii

        //

        public override string ToString()
        {
            return string.Format(
                @"Transform Params:

    Tilt: {0}
    Turn: {1}
    Rotate: {2}
    Lat: {3}
    Long: {4}
    Scale: {5}
    Radius: {6}
    X Offset: {8}
    Y Offset: {9}
    Background Color: {10}

    A: {7} (for Sinusoidal2)
    Parallel: {11}
    
    Polar:
        Conic: {12}
        Conic Radius: {13}
    
    Perspective:
        AW: {14}
        X: {15}
        Y: {16}
        Z: {17}
        Oblate X: {18}
        Oblate Y: {19}
        Oblate Z: {20}
        Sun: {21}
            Time: {22}
            Date: {23}

    Grid: {24}
        X Spacing: {25}
        Y Spacing: {26}
        Color: {27}

",
                tilt,
                turn,
                rotate,
                lat,
                lon,
                scale,
                radius,
                a,
                xOffset,
                yOffset,
                backgroundColor,
                p,
                conic,
                conicr,
                aw,
                x,
                y,
                z,
                ox,
                oy,
                oz,
                sun,
                time,
                date,
                Widgets.HasFlag(MapWidget.Grid),
                gridX,
                gridY,
                gridColor
            );
        }
    }
}