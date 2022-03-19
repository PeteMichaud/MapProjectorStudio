
namespace MapReliefLib.Extensions
{
    internal static class DoubleExtensions
    {
        public static double ToRadians(this double deg)
        {
            return deg * Math.PI / 180.0d;
        }
    }
}
