namespace MapProjectorLib.Projections
{
    internal abstract class Simple : Transform
    {
        protected abstract (bool result, double phi, double lambda) ProjectSimple(
            double x, double y,
            double phi, double lambda);

        protected abstract (bool inBounds, PointD mappedPoint) ProjectInvSimple(
            double phi, double lambda);

        public override (bool inProjectionBounds, double x1, double y1, double z1, double phi, double lambda) Project(
            TransformParams tParams,
            double x, double y,
            double x0, double y0, double z0,
            double phi, double lambda)
        {
            bool result = false;
            (result, phi, lambda) = ProjectSimple(x, y, phi, lambda);
            (phi, lambda) = ConvertLatLong(phi, lambda, transformMatrix);

            return (result, x0, y0, z0, phi, lambda);
        }

        public override (bool inBounds, PointD mappedPoint) ProjectInv(
            TransformParams tParams,
            double phi, double lambda)
        {
            (phi, lambda) = transformMatrixInv.ApplyLatLong(phi, lambda);
            return ProjectInvSimple(phi, lambda);
        }
    }
}