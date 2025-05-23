using System;
using System.Collections.Generic;
using System.Text;
using TOPRO.HSL.BasicFramework;

namespace TOPRO.HSL.Extensions
{
    public static class BoolExtension
    {
        public static byte[] ToByteArray(this bool[] array)
        {
            return SoftBasic.BoolArrayToByte(array);
        }
    }
}
