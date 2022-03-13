namespace MapProjectorLib.Plotters
{
    internal class AnalemmaPlotCalculator : TransformPlotCalculator
    {
        public double Time;

        public AnalemmaPlotCalculator(
            DestinationImage image, TransformParams tParams, Transform transform)
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