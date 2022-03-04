
namespace MapProjectorLib.Projections
{
    abstract class Simple : Transform
    {
        abstract public bool ProjectSimple(double x, double y,
                 ref double phi, ref double lambda);
        abstract public bool ProjectInvSimple(double phi, double lambda,
                      ref double x, ref double y);


        public override bool Project(TransformParams tParams,
                  double x, double y,
                  ref double x1, ref double y1, ref double z1,
                  ref double phi, ref double lambda)
        {
            bool result = ProjectSimple(x, y, ref phi, ref lambda);
            ConvertLatLong(ref phi, ref lambda, x1, y1, z1, transformMatrix);
            return result;
        }

        public override bool ProjectInv(TransformParams tParams,
                     double phi, double lambda, ref double x, ref double y)
        {
            transformMatrixInv.ApplyLatLong(ref phi, ref lambda);
            return ProjectInvSimple(phi, lambda, ref x, ref y);
        }
    }
}
