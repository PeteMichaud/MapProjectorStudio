namespace MapProjectorLib.Plotters
{
    internal class AnalemmaPlotter : TransformPlotter
    {
        public double Time;

        public AnalemmaPlotter(
            Image image, TransformParams tParams, Transform transform)
            : base(image, tParams, transform)
        {
            Time = 0;
        }

        public override (bool inBounds, PointD mappedPoint) GetXY(double progressAlongPlot)
        {
            // AT = MT + EOT
            // lambda = -AT (measure longitude to the east)
            var eot = ProjMath.TwoPi * ProjMath.EquationOfTime(progressAlongPlot) 
                        / ProjMath.SecondsPerDay;
            var delta = ProjMath.SunDec(progressAlongPlot); 

            return _transform.MapXY(
                _image, _tParams, delta, -(Time + eot));
        }
    }
}