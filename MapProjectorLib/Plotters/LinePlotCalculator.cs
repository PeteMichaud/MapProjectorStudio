namespace MapProjectorLib.Plotters
{
    public abstract class LinePlotCalculator
    {
        public abstract (bool inBounds, PointD mappedPoint) GetXY(double progressAlongPlot);
    }
}