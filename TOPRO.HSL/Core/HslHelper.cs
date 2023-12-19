using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TOPRO.HSL.BasicFramework;

namespace TOPRO.HSL.Core
{
    //
    // 摘要:
    //     HslCommunication的一些静态辅助方法
    //     Some static auxiliary methods of HslCommunication
    public class HslHelper
    {
        //
        // 摘要:
        //     项目的随机数信息
        public static Random HslRandom { get; private set; } = new Random();


        //
        // 摘要:
        //     解析地址的附加参数方法，比如你的地址是s=100;D100，可以提取出"s"的值的同时，修改地址本身，如果"s"不存在的话，返回给定的默认值
        //     The method of parsing additional parameters of the address, for example, if your
        //     address is s=100;D100, you can extract the value of "s" and modify the address
        //     itself. If "s" does not exist, return the given default value
        //
        // 参数:
        //   address:
        //     复杂的地址格式，比如：s=100;D100
        //
        //   paraName:
        //     等待提取的参数名称
        //
        //   defaultValue:
        //     如果提取的参数信息不存在，返回的默认值信息
        //
        // 返回结果:
        //     解析后的新的数据值或是默认的给定的数据值
        public static int ExtractParameter(ref string address, string paraName, int defaultValue)
        {
            OperateResult<int> operateResult = ExtractParameter(ref address, paraName);
            return operateResult.IsSuccess ? operateResult.Content : defaultValue;
        }

        //
        // 摘要:
        //     解析地址的附加参数方法，比如你的地址是s=100;D100，可以提取出"s"的值的同时，修改地址本身，如果"s"不存在的话，返回错误的消息内容
        //     The method of parsing additional parameters of the address, for example, if your
        //     address is s=100;D100, you can extract the value of "s" and modify the address
        //     itself. If "s" does not exist, return the wrong message content
        //
        // 参数:
        //   address:
        //     复杂的地址格式，比如：s=100;D100
        //
        //   paraName:
        //     等待提取的参数名称
        //
        // 返回结果:
        //     解析后的参数结果内容
        public static OperateResult<int> ExtractParameter(ref string address, string paraName)
        {
            try
            {
                Match match = Regex.Match(address, paraName + "=[0-9A-Fa-fxX]+;");
                if (!match.Success)
                {
                    return new OperateResult<int>("Address [" + address + "] can't find [" + paraName + "] Parameters. for example : " + paraName + "=1;100");
                }

                string text = match.Value.Substring(paraName.Length + 1, match.Value.Length - paraName.Length - 2);
                int value = ((text.StartsWith("0x") || text.StartsWith("0X")) ? Convert.ToInt32(text.Substring(2), 16) : (text.StartsWith("0") ? Convert.ToInt32(text, 8) : Convert.ToInt32(text)));
                address = address.Replace(match.Value, "");
                return OperateResult.CreateSuccessResult(value);
            }
            catch (Exception ex)
            {
                return new OperateResult<int>("Address [" + address + "] Get [" + paraName + "] Parameters failed: " + ex.Message);
            }
        }

        //
        // 摘要:
        //     解析地址的起始地址的方法，比如你的地址是 A[1] , 那么将会返回 1，地址修改为 A，如果不存在起始地址，那么就不修改地址，返回 -1
        //     The method of parsing the starting address of the address, for example, if your
        //     address is A[1], then it will return 1, and the address will be changed to A.
        //     If the starting address does not exist, then the address will not be changed
        //     and return -1
        //
        // 参数:
        //   address:
        //     复杂的地址格式，比如：A[0]
        //
        // 返回结果:
        //     如果存在，就起始位置，不存在就返回 -1
        public static int ExtractStartIndex(ref string address)
        {
            try
            {
                Match match = Regex.Match(address, "\\[[0-9]+\\]$");
                if (!match.Success)
                {
                    return -1;
                }

                string value = match.Value.Substring(1, match.Value.Length - 2);
                int result = Convert.ToInt32(value);
                address = address.Remove(address.Length - match.Value.Length);
                return result;
            }
            catch
            {
                return -1;
            }
        }

        //
        // 摘要:
        //     切割当前的地址数据信息，根据读取的长度来分割成多次不同的读取内容，需要指定地址，总的读取长度，切割读取长度
        //     Cut the current address data information, and divide it into multiple different
        //     read contents according to the read length. You need to specify the address,
        //     the total read length, and the cut read length
        //
        // 参数:
        //   address:
        //     整数的地址信息
        //
        //   length:
        //     读取长度信息
        //
        //   segment:
        //     切割长度信息
        //
        // 返回结果:
        //     切割结果
        public static OperateResult<int[], int[]> SplitReadLength(int address, ushort length, ushort segment)
        {
            int[] array = SoftBasic.SplitIntegerToArray(length, segment);
            int[] array2 = new int[array.Length];
            for (int i = 0; i < array2.Length; i++)
            {
                if (i == 0)
                {
                    array2[i] = address;
                }
                else
                {
                    array2[i] = array2[i - 1] + array[i - 1];
                }
            }

            return OperateResult.CreateSuccessResult(array2, array);
        }

        //
        // 摘要:
        //     根据指定的长度切割数据数组，返回地址偏移量信息和数据分割信息
        //
        // 参数:
        //   address:
        //     起始的地址
        //
        //   value:
        //     实际的数据信息
        //
        //   segment:
        //     分割的基本长度
        //
        //   addressLength:
        //     一个地址代表的数据长度
        //
        // 类型参数:
        //   T:
        //     数组类型
        //
        // 返回结果:
        //     切割结果内容
        public static OperateResult<int[], List<T[]>> SplitWriteData<T>(int address, T[] value, ushort segment, int addressLength)
        {
            List<T[]> list = SoftBasic.ArraySplitByLength(value, segment * addressLength);
            int[] array = new int[list.Count];
            for (int i = 0; i < array.Length; i++)
            {
                if (i == 0)
                {
                    array[i] = address;
                }
                else
                {
                    array[i] = array[i - 1] + list[i - 1].Length / addressLength;
                }
            }

            return OperateResult.CreateSuccessResult(array, list);
        }

        //
        // 摘要:
        //     获取地址信息的位索引，在地址最后一个小数点的位置
        //
        // 参数:
        //   address:
        //     地址信息
        //
        // 返回结果:
        //     位索引的位置
        public static int GetBitIndexInformation(ref string address)
        {
            int result = 0;
            int num = address.LastIndexOf('.');
            if (num > 0 && num < address.Length - 1)
            {
                string text = address.Substring(num + 1);
                result = ((!text.Contains("A") && !text.Contains("B") && !text.Contains("C") && !text.Contains("D") && !text.Contains("E") && !text.Contains("F")) ? Convert.ToInt32(text) : Convert.ToInt32(text, 16));
                address = address.Substring(0, num);
            }

            return result;
        }

        //
        // 摘要:
        //     从当前的字符串信息获取IP地址数据，如果是ip地址直接返回，如果是域名，会自动解析IP地址，否则抛出异常
        //     Get the IP address data from the current string information, if it is an ip address,
        //     return directly, if it is a domain name, it will automatically resolve the IP
        //     address, otherwise an exception will be thrown
        //
        // 参数:
        //   value:
        //     输入的字符串信息
        //
        // 返回结果:
        //     真实的IP地址信息
        public static string GetIpAddressFromInput(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (Regex.IsMatch(value, "^[0-9]+\\.[0-9]+\\.[0-9]+\\.[0-9]+$"))
                {
                    if (!IPAddress.TryParse(value, out var _))
                    {
                        throw new Exception("IP地址错误");
                    }

                    return value;
                }

                IPHostEntry hostEntry = Dns.GetHostEntry(value);
                IPAddress[] addressList = hostEntry.AddressList;
                if (addressList.Length != 0)
                {
                    return addressList[0].ToString();
                }
            }

            return "127.0.0.1";
        }

        //
        // 摘要:
        //     从流中接收指定长度的字节数组
        //
        // 参数:
        //   stream:
        //     流
        //
        //   length:
        //     数据长度
        //
        // 返回结果:
        //     二进制的字节数组
        public static byte[] ReadSpecifiedLengthFromStream(Stream stream, int length)
        {
            byte[] array = new byte[length];
            int num = 0;
            while (num < length)
            {
                int num2 = stream.Read(array, num, array.Length - num);
                num += num2;
                if (num2 == 0)
                {
                    break;
                }
            }

            return array;
        }

        //
        // 摘要:
        //     将字符串的内容写入到流中去
        //
        // 参数:
        //   stream:
        //     数据流
        //
        //   value:
        //     字符串内容
        public static void WriteStringToStream(Stream stream, string value)
        {
            byte[] value2 = (string.IsNullOrEmpty(value) ? new byte[0] : Encoding.UTF8.GetBytes(value));
            WriteBinaryToStream(stream, value2);
        }

        //
        // 摘要:
        //     从流中读取一个字符串内容
        //
        // 参数:
        //   stream:
        //     数据流
        //
        // 返回结果:
        //     字符串信息
        public static string ReadStringFromStream(Stream stream)
        {
            byte[] bytes = ReadBinaryFromStream(stream);
            return Encoding.UTF8.GetString(bytes);
        }

        //
        // 摘要:
        //     将二进制的内容写入到数据流之中
        //
        // 参数:
        //   stream:
        //     数据流
        //
        //   value:
        //     原始字节数组
        public static void WriteBinaryToStream(Stream stream, byte[] value)
        {
            stream.Write(BitConverter.GetBytes(value.Length), 0, 4);
            stream.Write(value, 0, value.Length);
        }

        //
        // 摘要:
        //     从流中读取二进制的内容
        //
        // 参数:
        //   stream:
        //     数据流
        //
        // 返回结果:
        //     字节数组
        public static byte[] ReadBinaryFromStream(Stream stream)
        {
            byte[] value = ReadSpecifiedLengthFromStream(stream, 4);
            int num = BitConverter.ToInt32(value, 0);
            if (num <= 0)
            {
                return new byte[0];
            }

            return ReadSpecifiedLengthFromStream(stream, num);
        }

        //
        // 摘要:
        //     从字符串的内容提取UTF8编码的字节，加了对空的校验
        //
        // 参数:
        //   message:
        //     字符串内容
        //
        // 返回结果:
        //     结果
        public static byte[] GetUTF8Bytes(string message)
        {
            return string.IsNullOrEmpty(message) ? new byte[0] : Encoding.UTF8.GetBytes(message);
        }

        //
        // 摘要:
        //     将多个路径合成一个更完整的路径，这个方法是多平台适用的
        //
        // 参数:
        //   paths:
        //     路径的集合
        //
        // 返回结果:
        //     总路径信息
        public static string PathCombine(params string[] paths)
        {
            return Path.Combine(paths);
        }

        //
        // 摘要:
        //     [商业授权] 将原始的字节数组，转换成实际的结构体对象，需要事先定义好结构体内容，否则会转换失败
        //     [Authorization] To convert the original byte array into an actual structure object,
        //     the structure content needs to be defined in advance, otherwise the conversion
        //     will fail
        //
        // 参数:
        //   content:
        //     原始的字节内容
        //
        // 类型参数:
        //   T:
        //     自定义的结构体
        //
        // 返回结果:
        //     是否成功的结果对象
        public static OperateResult<T> ByteArrayToStruct<T>(byte[] content) where T : struct
        {
            int num = Marshal.SizeOf(typeof(T));
            IntPtr intPtr = Marshal.AllocHGlobal(num);
            try
            {
                Marshal.Copy(content, 0, intPtr, num);
                T value = Marshal.PtrToStructure<T>(intPtr);
                Marshal.FreeHGlobal(intPtr);
                return OperateResult.CreateSuccessResult(value);
            }
            catch (Exception ex)
            {
                Marshal.FreeHGlobal(intPtr);
                return new OperateResult<T>(ex.Message);
            }
        }

        //
        // 摘要:
        //     根据当前的位偏移地址及读取位长度信息，计算出实际的字节索引，字节数，字节位偏移
        //
        // 参数:
        //   addressStart:
        //     起始地址
        //
        //   length:
        //     读取的长度
        //
        //   newStart:
        //     返回的新的字节的索引，仍然按照位单位
        //
        //   byteLength:
        //     字节长度
        //
        //   offset:
        //     当前偏移的信息
        public static void CalculateStartBitIndexAndLength(int addressStart, ushort length, out int newStart, out ushort byteLength, out int offset)
        {
            byteLength = (ushort)((addressStart + length - 1) / 8 - addressStart / 8 + 1);
            offset = addressStart % 8;
            newStart = addressStart - offset;
        }

        //
        // 摘要:
        //     根据字符串内容，获取当前的位索引地址，例如输入 6,返回6，输入15，返回15，输入B，返回11
        //
        // 参数:
        //   bit:
        //     位字符串
        //
        // 返回结果:
        //     结束数据
        public static int CalculateBitStartIndex(string bit)
        {
            if (bit.Contains("A") || bit.Contains("B") || bit.Contains("C") || bit.Contains("D") || bit.Contains("E") || bit.Contains("F"))
            {
                return Convert.ToInt32(bit, 16);
            }

            return Convert.ToInt32(bit);
        }

        //
        // 摘要:
        //     将一个一维数组中的所有数据按照行列信息拷贝到二维数组里，返回当前的二维数组
        //
        // 参数:
        //   array:
        //     一维数组信息
        //
        //   row:
        //     行
        //
        //   col:
        //     列
        //
        // 类型参数:
        //   T:
        //     数组的类型对象
        public static T[,] CreateTwoArrayFromOneArray<T>(T[] array, int row, int col)
        {
            T[,] array2 = new T[row, col];
            int num = 0;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    array2[i, j] = array[num];
                    num++;
                }
            }

            return array2;
        }

        //
        // 摘要:
        //     判断当前的字符串表示的地址，是否以索引为结束
        //
        // 参数:
        //   address:
        //     PLC的字符串地址信息
        //
        // 返回结果:
        //     是否以索引结束
        public static bool IsAddressEndWithIndex(string address)
        {
            return Regex.IsMatch(address, "\\[[0-9]+\\]$");
        }
    }
}
