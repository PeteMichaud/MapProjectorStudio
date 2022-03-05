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

        public override bool GetXY(double t, ref double x, ref double y)
        {
            // AT = MT + EOT
            // lambda = -AT (measure longitude to the east)
            var eot = ProjMath.TwoPi * ProjMath.EquationOfTime(t) /
                      (24 * 60 * 60);
            var delta = ProjMath.SunDec(t);

            return _transform.MapXY(
                _image, _tParams, delta, -(Time + eot), ref x, ref y);
        }
    }
}