namespace MapProjectorLib.Plotters
{
    public abstract class LinePlotter
    {
        public abstract bool GetXY(double t, ref double x, ref double y);
    }
}