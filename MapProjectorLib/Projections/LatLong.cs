namespace MapProjectorLib.Projections
{
    internal class LatLong : CylindricalBase
    {
        
        protected override double GetMaxHeight(TransformParams tParams)
        {
            return ProjMath.PiOverTwo;
        }

        protected override double GetLat(double _, double y)
        {
            return y;
        }

        protected override double GetLong(double x, double _)
        {
            return x;
        }

        protected override (bool inBounds, PointD mappedPoint) GetXY(
            double phi, double lambda)
        {
            return (true, new PointD(lambda,phi));
        }
    }
}