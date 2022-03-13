using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapProjectorLib.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<T> GetEnabledFlags<T>(this T flags)
            where T : Enum
        {
            foreach (Enum value in Enum.GetValues(flags.GetType()))
                if (flags.HasFlag(value))
                    yield return (T)value;
        }
    }
}
