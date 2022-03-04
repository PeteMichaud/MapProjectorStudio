using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapProjectorLib
{
    [Flags]
    public enum MapWidget
    {
        None = 0,
        Grid = 1,
        Analemma = 2,
        TemporaryHours = 4,
        LocalHours = 8,
        Altitudes = 16,
        Tropics = 32,
        Dateline = 64,
        Datetime = 128,
    }
}
