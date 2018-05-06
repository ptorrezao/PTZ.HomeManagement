using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTZ.HomeManagement.Utils
{
    public static class Conversion
    {
        internal static double ConvertBytesToMegabytes(long bytes, int? decimalPlaces)
        {
            return Math.Round(ConvertBytesToMegabytes(bytes), decimalPlaces ?? 10);
        }

        internal static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
    }
}
