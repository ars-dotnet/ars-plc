using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TOPRO.HSL.Extensions
{
    public static class StringExtension
    {
        public static string[] SplitDot(this string str)
        {
            return str.Split(new char[1] { '.' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static int CalculateBitStartIndex(this string bit)
        {
            if (Regex.IsMatch(bit, "[ABCDEF]", RegexOptions.IgnoreCase))
            {
                return Convert.ToInt32(bit, 16);
            }
            return Convert.ToInt32(bit);
        }

        public static bool StartsWithAndNumber(this string address, string code)
        {
            if (address.StartsWith(code, StringComparison.InvariantCultureIgnoreCase))
            {
                if (address.Length <= code.Length)
                {
                    return false;
                }
                if (char.IsNumber(address[code.Length]))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
