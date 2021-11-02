using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programemein.Helpers
{
    public class FileSizeCalculator
    {
        public static long KiloBytes(long kb)
        {
            return kb * 1024;
        }

        public static long MegaBytes(long mb)
        {
            return mb * 1024 * 1024;
        }

        public static long GigaBytes(long gb)
        {
            return gb * 1024 * 1024 * 1024;
        }
    }
}
