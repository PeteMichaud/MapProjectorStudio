
namespace MapProjectorLib.Plotters
{
    class AnalemmaPlotter : TransformPlotter
    {
        public double time;
        public AnalemmaPlotter(Image image, TransformParams tParams, Transform transform)
            : base(image, tParams, transform)
        {
            time = 0;
        }
        public override bool GetXY(double t, ref double x, ref double y)
        {
            // AT = MT + EOT
            // lambda = -AT (measure longitude to the east)
            double eot = ProjMath.TwoPi * ProjMath.EquationOfTime(t) / (24 * 60 * 60);
            double delta = ProjMath.SunDec(t);
            
            return _transform.MapXY(_image, _tParams, delta, -(time + eot), ref x, ref y);
        }
    }
}
