namespace MapProjectorLib.Plotters
{
    public abstract class LinePlotter
    {
        public abstract (bool inBounds, PointD mappedPoint) GetXY(double progressAlongPlot);
    }
}