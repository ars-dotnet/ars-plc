using System;
using System.Collections.Generic;
using System.Text;
using TOPRO.HSL.BasicFramework;

namespace TOPRO.HSL.Extensions
{
    public static class ByteExtension
    {
        public static bool[] ToBoolArray(this byte[] InBytes)
        {
            return SoftBasic.ByteToBoolArray(InBytes);
        }

        public static byte[] ReverseByWord(this byte[] inBytes)
        {
            return SoftBasic.BytesReverseByWord(inBytes);
        }
    }
}
