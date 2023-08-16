using HslCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC.Enums;

namespace TOPRO.PLC
{
    public class ActionNames
    {
        /// <summary>
        /// 读取byte[]
        /// </summary>
        public const string Read = "Read";

        public const string ReadBool = "ReadBool";

        public const string ReadInt16 = "ReadInt16";

        public const string ReadUInt16 = "ReadUInt16";

        public const string ReadInt32 = "ReadInt32";

        public const string ReadUInt32 = "ReadUInt32";

        public const string ReadInt64 = "ReadInt64";

        public const string ReadUInt64 = "ReadUInt64";

        public const string ReadFloat = "ReadFloat";

        public const string ReadDouble = "ReadDouble";

        public const string ReadString = "ReadString";

        public const string ReadCustomer = "ReadCustomer";

        /// <summary>
        /// modbus线圈读
        /// </summary>
        public const string ReadCoil = "ReadCoil";

        /// <summary>
        /// modbus离散读
        /// </summary>
        public const string ReadDiscrete = "ReadDiscrete";

        public const string Write = "Write";

        public const string WriteCustomer = "WriteCustomer";

        /// <summary>
        /// modbus线圈写
        /// </summary>
        public const string WriteCoil = "WriteCoil";

        private static readonly Dictionary<string, Type[]> datas =
            new Dictionary<string, Type[]>
            {
                #region 基本读写
                //read
                {
                    Read,
                    new[]{ typeof(byte[]) }
                },
                {
                    ReadBool,
                    new[]{ typeof(bool[]),typeof(bool) }
                },
                {
                    ReadInt16,
                    new[]{ typeof(short[]),typeof(short) }
                },
                {
                    ReadUInt16,
                    new[]{ typeof(ushort[]),typeof(ushort) }
                },
                {
                    ReadInt32,
                    new[]{ typeof(int[]),typeof(int) }
                },
                {
                    ReadUInt32,
                    new[]{ typeof(uint[]),typeof(uint) }
                },
                {
                    ReadInt64,
                    new[]{ typeof(long[]),typeof(long) }
                },
                {
                    ReadUInt64,
                    new[]{ typeof(ulong[]),typeof(ulong) }
                },
                {
                    ReadFloat,
                    new[]{ typeof(float[]),typeof(float) }
                },
                {
                    ReadDouble,
                    new[]{ typeof(double[]),typeof(double) }
                },
                {
                    ReadString,
                    new[]{ typeof(string[]), typeof(string) }
                },
                {
                    ReadCustomer,
                    new[]{ typeof(IDataTransfer) }
                },

                #endregion 基本读写
            };

        public static string GetReadMethodName(Type type)
        {
            return datas.FirstOrDefault(r => r.Value.Contains(type)).Key
                ?? datas.FirstOrDefault(r => r.Value.Any(t => t.IsAssignableFrom(type))).Key
                ?? throw new Exception($"获取{type.Name}类型数据的执行方式失败，请显示指定执行方法");
        }

        public static string GetWriteMethodName(Type type)
        {
            return typeof(IDataTransfer).IsAssignableFrom(type) ? WriteCustomer : Write;
        }
    }
}
