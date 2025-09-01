using System;
using System.Collections.Generic;
using System.Text;
using TOPRO.HSL.Extensions;

namespace TOPRO.HSL.ModBus
{
    public static class ModbusHelper
    {
        public static bool TransAddressToModbus(string station, string address, string[] code, int[] offset, Func<string, int> prase, out string newAddress)
        {
            newAddress = string.Empty;
            for (int i = 0; i < code.Length; i++)
            {
                if (address.StartsWithAndNumber(code[i]))
                {
                    newAddress = station + (prase(address.Substring(code[i].Length)) + offset[i]);
                    return true;
                }
            }
            return false;
        }

        public static bool TransAddressToModbus(string station, string address, string[] code, int[] offset, out string newAddress)
        {
            return TransAddressToModbus(station, address, code, offset, int.Parse, out newAddress);
        }

        public static bool TransPointAddressToModbus(string station, string address, string[] code, int[] offset, Func<string, int> prase, out string newAddress)
        {
            newAddress = string.Empty;
            int num = address.IndexOf('.');
            if (num > 0)
            {
                string text = address.Substring(num);
                address = address.Substring(0, num);
                if (TransAddressToModbus(station, address, code, offset, prase, out newAddress))
                {
                    newAddress += text;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 针对带有小数点的地址进行转换，例如 D100.0 转成 100.0 
        /// </summary>
        /// <param name="station">站号信息</param>
        /// <param name="address">地址</param>
        /// <param name="code">地址类型</param>
        /// <param name="offset">起始偏移地址</param>
        /// <param name="newAddress">返回的新的地址</param>
        /// <returns>是否匹配当前的地址类型</returns>
        public static bool TransPointAddressToModbus(string station, string address, string[] code, int[] offset, out string newAddress)
        {
            return TransPointAddressToModbus(station, address, code, offset, int.Parse, out newAddress);
        }
    }
}
