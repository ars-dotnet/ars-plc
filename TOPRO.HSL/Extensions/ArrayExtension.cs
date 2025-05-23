using System;
using System.Collections.Generic;
using System.Text;
using TOPRO.HSL.BasicFramework;

namespace TOPRO.HSL.Extensions
{
    public static class ArrayExtension
    {
        public static T[] SelectMiddle<T>(this T[] value, int index, int length)
        {
            return SoftBasic.ArraySelectMiddle(value, index, length);
        }

    }
}
