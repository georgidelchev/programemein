﻿namespace Programemein.Web.Infrastructure.Helpers
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
