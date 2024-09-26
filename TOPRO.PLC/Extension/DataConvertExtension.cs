using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topro.Extension.Plc.Extension
{
    public static class DataConvertExtension
    {
        /// <summary>
        /// 将short类型转化成bit位数组,从低位到高位
        /// 操作与PLc的报警位---标准操作
        /// </summary>
        /// <param name="integer"></param>
        /// <param name="resultSize">word-16，int-32</param>
        /// <returns></returns>
        public static bool[] ToBinaryBits(this short integer, int resultSize = 16)
        {
            bool[] result = new bool[resultSize];
            byte[] Array = BitConverter.GetBytes(integer);
            BitArray bitArray = new BitArray(Array);
            bitArray.CopyTo(result, 0);
            return result;
        }

        /// <summary>
        /// bits右侧是高位，左侧是低位
        /// </summary>
        /// <param name="bits"></param>
        /// <param name="resultSize"></param>
        /// <returns></returns>
        public static short ToShort(this bool[] bits, int resultSize = 16) 
        {
            byte[] result = new byte[resultSize];
            BitArray bitArray = new BitArray(bits);
            bitArray.CopyTo(result, 0);

            return BitConverter.ToInt16(result, 0);
        }

        public static byte[] GetBytes(this string str, Encoding encoding) 
        {
            return encoding.GetBytes(str);
        }

        public static string BytesToString(this byte[] bytes, Encoding encoding) 
        {
            return encoding.GetString(bytes).Replace("\0","");
        }
    }
}
