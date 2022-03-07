using System;
using CommandLine;
using MapProjectorLib;

namespace MapProjectorCLI
{
    public class CLIParams
    {
        static Parser CliParser => new Parser(config =>
        {
            config.CaseSensitive = false;
            config.CaseInsensitiveEnumValues = true;
        });

        public static ParserResult<CLIParams> Parse(string[] args, Parser parser = null)
        {
            return (parser ?? CliParser).ParseArguments<CLIParams>(args);
        }

        const double OneDay = 2 * Math.PI / 365.25;
        const double OneDegree = 2 * Math.PI / 360.0;
        const double OneHour = 2 * Math.PI / 24.0;

        double _dateIncr;

        double _lat;
        double _latIncr;

        double _lon;
        double _longIncr;

        double _rotate;

        int _loopCount;

        ////Transform Params


        double _tilt;
        double _tiltIncr;
        double _timeIncr;
        double _turn;
        double _turnIncr;
        double _radius;
        double _aw;

        double _widgetLat;

        double _widgetLon;

        //Projection Params

        [Option(
            "projection", Required = false,
            HelpText =
                "Target Projection (or Source Projection if Invert flag specified)")]
        public MapProjection TargetProjection { get; set; }

        [Option('f', "file", Required = true, HelpText = "Source File Name")]
        public string srcImageFileName { get; set; }

        [Option('o', "out", Required = true, HelpText = "Output File Name")]
        public string outImageFileName { get; set; }

        [Option(
            "adjust", Default = false,
            HelpText =
                "Set Source image to adjusted width and height options before processing")]
        public bool Adjust { get; set; }

        [Option('w', "width", Required = false, HelpText = "Target Width")]
        public int Width { get; set; }

        [Option('h', "height", Required = false, HelpText = "Target Height")]
        public int Height { get; set; }

        [Option(
            "bg", Required = false, HelpText = "Background Image File Name")]
        public string backImageFileName { get; set; }

        [Option(
            'i', "invert", Required = false,
            HelpText = "Invert the specified operation")]
        public bool Invert { get; set; }

        ////Loop Params

        [Option(
            "loop", Default = 1,
            HelpText = "Number of Images to Output along given increments")]
        public int LoopCount 
        { 
            get => _loopCount; 
            set 
            { 
                _loopCount = Math.Max(1, value); 
            } 
        }

        [Option("tiltinc", Default = 0, HelpText = "Tilt Increment (Degrees)")]
        public double TiltIncr
        {
            get => _tiltIncr;
            set => _tiltIncr = value * OneDegree;
        }

        [Option("turninc", Default = 0, HelpText = "Turn Increment (Degrees)")]
        public double TurnIncr
        {
            get => _turnIncr;
            set => _turnIncr = value * OneDegree;
        }

        [Option(
            "latinc", Default = 0, HelpText = "Latitude Increment (Degrees)")]
        public double LatIncr
        {
            get => _latIncr;
            set => _latIncr = value * OneDegree;
        }

        [Option(
            "longinc", Default = 0, HelpText = "Longitude Increment (Degrees)")]
        public double LongIncr
        {
            get => _longIncr;
            set => _longIncr = value * OneDegree;
        }

        [Option(
            "xinc", Default = 0,
            HelpText = "X (Horizontal) Increment (Pixels)")]
        public double xIncr { get; set; }

        [Option(
            "yinc", Default = 0, HelpText = "Y (Vertical) Increment (Pixels)")]
        public double yIncr { get; set; }

        [Option("zinc", Default = 0, HelpText = "Z (Zoom) Increment (Pixels)")]
        public double zIncr { get; set; }

        [Option("dateinc", Default = 0, HelpText = "Date Increment (Days)")]
        public double DateIncr
        {
            get => _dateIncr;
            set => _dateIncr = value * OneDay;
        }

        [Option("timeinc", Default = 0, HelpText = "Time Increment (Hours)")]
        public double TimeIncr
        {
            get => _timeIncr;
            set => _timeIncr = value * OneHour;
        }

        [Option(
            "tilt", Default = 0,
            HelpText = "Rotation Around Center Point (Degrees)")]
        public double tilt
        {
            get => _tilt;
            set => _tilt = value * OneDegree;
        }

        [Option("turn", Default = 0, HelpText = "Vertical Rotation (Degrees)")]
        public double turn
        {
            get => _turn;
            set => _turn = value * OneDegree;
        }

        [Option(
            "rotate", Default = 0, HelpText = "Rotate in 2D plane (Degrees)")]
        public double rotate
        {
            get => _rotate;
            set => _rotate = value * OneDegree;
        }

        [Option("lat", Default = 0, HelpText = "Latitude of Center (Degrees)")]
        public double lat
        {
            get => _lat;
            set => _lat = value * OneDegree;
        }

        [Option("lon", Default = 0, HelpText = "Longitude of Center (Degrees)")]
        public double lon
        {
            get => _lon;
            set => _lon = value * OneDegree;
        }

        [Option(
            "scale", Default = 1.0,
            HelpText = "Output Scale (Percent, 1 = 100%)")]
        public double scale { get; set; }

        [Option(
            "radius", Default = 0.0,
            HelpText =
                "Radius around center point that the output is rendered (Degrees)")]
        public double radius 
        {
            get => _radius;
            set => _radius = value * OneDegree;
        }
        [Option("xoff", Default = 0.0, HelpText = "X Offset")]
        public double xOffset { get; set; }

        [Option("yoff", Default = 0.0, HelpText = "Y Offset")]
        public double yOffset { get; set; }

        [Option(
            "bgColor", Required = false,
            HelpText = "Background color R,G,B (0-255)")]
        public string _backgroundColorValues { get; set; }

        ////

        [Option(
            'a', Default = 1.0,
            HelpText = "Only relevant to Sinusoidal2 projection")]
        public double a { get; set; }

        ////

        [Option(
            "aw", Default = 20,
            HelpText = "Only relevant to Perspective projection (Radians)")]
        public double aw 
        { 
            get => _aw; 
            set 
            {
                _aw = ToRadians(value);
            } 
        }

        [Option(
            'x', Default = 8.0,
            HelpText =
                "X dimension of viewing position, measured in planet radii")]
        public double x { get; set; }

        [Option(
            'y', Default = 0.0,
            HelpText =
                "Y dimension of viewing position, measured in planet radii")]
        public double y { get; set; }

        [Option(
            'z', Default = 0.0,
            HelpText =
                "Z dimension of viewing position, measured in planet radii")]
        public double z { get; set; }

        [Option(
            "ox", Default = 1.0,
            HelpText = "X dimension of Oblateness, measured in planet radii")]
        public double ox { get; set; }

        [Option(
            "oy", Default = 1.0,
            HelpText = "Y dimension of Oblateness, measured in planet radii")]
        public double oy { get; set; }

        [Option(
            "oz", Default = 1.1,
            HelpText = "Z dimension of Oblateness, measured in planet radii")]
        public double oz { get; set; }

        [Option(
            "sun", Default = false,
            HelpText =
                "Simulate the sun on the perspective view (date and time are relevant)")]
        public bool sun { get; set; }

        [Option(
            "time", Default = 0.0,
            HelpText =
                "Hours from midnight, UTC, in decimal hours (so 4.5 is half past four in the morning). Relevant for the -sun option")]
        public double time { get; set; }

        [Option(
            "date", Default = 0.0,
            HelpText = "Day number in year. Relevant for the -sun option")]
        public double date { get; set; }

        ////

        [Option(
            'p', Default = 0.0,
            HelpText =
                "Reference parallel for projections that can use it: equalarea, sinusoidal, mollweide")]
        public double p { get; set; }

        [Option(
            "conic", Default = 1.0,
            HelpText =
                "Relevant to polar projections: azimuthal, gnomonic, rectilinear, stereographic")]
        public double conic { get; set; }

        [Option(
            "conicr", Default = 0.0,
            HelpText =
                "Conic Radius. Relevant to polar projections: azimuthal, gnomonic, rectilinear, stereographic")]
        public double conicr { get; set; }

        //Widgets

        [Option(
            "widget", Required = false,
            HelpText =
                "Comma separated list of map widgets: Grid, Analemma, TemporaryHours, LocalHours, Altitudes, Tropics, Dateline, Datetime")]
        public MapWidget Widgets { get; set; }

        [Option(
            "gridx", Default = 30,
            HelpText = "X Spacing of Grid (Degrees) (use --widget grid)")]
        public int gridx { get; set; }

        [Option(
            "gridy", Default = 30,
            HelpText = "Y Spacing of Grid (Degrees) (use --widget grid)")]
        public int gridy { get; set; } = 0;

        [Option(
            "gridcolor", Required = false,
            HelpText = "Grid line color R,G,B (0-255) (use --widget)")]
        public string _gridColorValues { get; set; }

        [Option("widgetColor", HelpText = "Widget Color  R,G,B (0-255)")]
        public string _widgetColorValues { get; set; }

        [Option(
            "wlat", Default = 0.0,
            HelpText = "Widget Origin Latitude (Radians)")]
        public double widgetLat
        {
            get => _widgetLat;
            set => _widgetLat = ToRadians(value);
        }

        [Option(
            "wlon", Default = 0.0,
            HelpText = "Widget Origin Longitude (Radians)")]
        public double widgetLon
        {
            get => _widgetLon;
            set => _widgetLon = ToRadians(value);
        }

        [Option(
            "wday", Default = 0.0,
            HelpText = "Widget Day (for Dateline and Datetime widgets)")]
        public double widgetDay { get; set; }

        [Option(
            "wnaivespacing", Default = false,
            HelpText = "Disable smart spacing for Indictrix Widget (smart spacing can help with overalp at the poles by skipping some)")]
        public bool widgetNaiveSpacing { get; set; }

        //

        static double ToRadians(double deg)
        {
            return deg * Math.PI / 180.0;
        }

    }
}