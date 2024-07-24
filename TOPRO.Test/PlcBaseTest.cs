using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TOPRO.PLC;
using Xunit;
using Topro.Extension.Plc.Extension;
using TOPRO.PLC.Extension;

namespace TOPRO.Test
{
    public class PlcBaseTest
    {
        /// <summary>
        /// 单个地址读写
        /// </summary>
        /// <param name="operationManager"></param>
        /// <param name="dbNamePrefix">寄存器地址前缀</param>
        /// <param name="dbFrom">寄存器开始地址</param>
        public void TestSingle(IOperationManager operationManager,string dbNamePrefix,int dbFrom) 
        {
            #region 单个读写
            //short
            var res = operationManager.Write($"{dbNamePrefix}{dbFrom+10}", (short)123);
            Assert.True(res.IsSuccess);
            var shortdata = operationManager.Read<short>($"{dbNamePrefix}{dbFrom + 10}");
            Assert.True(123 == shortdata.Content);

            //int
            res = operationManager.Write($"{dbNamePrefix}{dbFrom + 20}", 6666);
            Assert.True(res.IsSuccess);
            var intdata = operationManager.Read<int>($"{dbNamePrefix}{dbFrom + 20}");
            Assert.True(6666 == intdata.Content);

            //long
            res = operationManager.Write($"{dbNamePrefix}{dbFrom + 30}", 99999999999999);
            Assert.True(res.IsSuccess);
            var longdata = operationManager.Read<long>($"{dbNamePrefix}{dbFrom + 30}");
            Assert.True(99999999999999 == longdata.Content);

            //float
            res = operationManager.Write($"{dbNamePrefix}{dbFrom + 40}", 12.9f);
            Assert.True(res.IsSuccess);
            var floatdata = operationManager.Read<float>($"{dbNamePrefix}{dbFrom + 40}");
            Assert.True(12.9f == floatdata.Content);

            //double
            res = operationManager.Write($"{dbNamePrefix}{dbFrom + 50}", 99.89d);
            Assert.True(res.IsSuccess);
            var doubledata = operationManager.Read<double>($"{dbNamePrefix}{dbFrom + 50}");
            Assert.True(99.89 == doubledata.Content);

            //string
            res = operationManager.Write($"{dbNamePrefix}{dbFrom + 60}", "aabb1212");
            Assert.True(res.IsSuccess);
            var stringdata = operationManager.Read<string>($"{dbNamePrefix}{dbFrom + 60}", 8);
            Assert.True("aabb1212" == stringdata.Content);
            #endregion
        }

        /// <summary>
        /// 批量读写
        /// </summary>
        /// <param name="operationManager"></param>
        /// <param name="dbNamePrefix"></param>
        /// <param name="dbFrom"></param>
        public void TestMulti(IOperationManager operationManager, string dbNamePrefix, int dbFrom) 
        {
            #region 批量读写
            //short
            var res = operationManager.Write($"{dbNamePrefix}{dbFrom + 10}", new short[] { 123, 234, 889 });
            Assert.True(res.IsSuccess);
            var shortdata = operationManager.Read<short[]>($"{dbNamePrefix}{dbFrom + 10}", 3);
            Assert.True(123 == shortdata.Content[0]);
            Assert.True(234 == shortdata.Content[1]);
            Assert.True(889 == shortdata.Content[2]);

            //int
            res = operationManager.Write($"{dbNamePrefix}{dbFrom + 20}", new int[] { 6666, 7777 });
            Assert.True(res.IsSuccess);
            var intdata = operationManager.Read<int[]>($"{dbNamePrefix}{dbFrom + 20}", 2);
            Assert.True(6666 == intdata.Content[0]);
            Assert.True(7777 == intdata.Content[1]);

            //long
            res = operationManager.Write($"{dbNamePrefix}{dbFrom + 30}", new long[] { 99999999999999, 8888888888888888 });
            Assert.True(res.IsSuccess);
            var longdata = operationManager.Read<long[]>($"{dbNamePrefix}{dbFrom + 30}", 2);
            Assert.True(99999999999999 == longdata.Content[0]);
            Assert.True(8888888888888888 == longdata.Content[1]);

            //float
            res = operationManager.Write($"{dbNamePrefix}{dbFrom + 40}", new float[] { 12.9f, 234.56f });
            Assert.True(res.IsSuccess);
            var floatdata = operationManager.Read<float[]>($"{dbNamePrefix}{dbFrom + 40}", 2);
            Assert.True(12.9f == floatdata.Content[0]);
            Assert.True(234.56f == floatdata.Content[1]);

            //double
            res = operationManager.Write($"{dbNamePrefix}{dbFrom + 50}", new double[] { 99.89d, 897.567d });
            Assert.True(res.IsSuccess);
            var doubledata = operationManager.Read<double[]>($"{dbNamePrefix}{dbFrom + 50}", 2);
            Assert.True(99.89 == doubledata.Content[0]);
            Assert.True(897.567 == doubledata.Content[1]);

            //string
            res = operationManager.Write($"{dbNamePrefix}{dbFrom + 60}", new string[] { "aabb121212", "1212aabb" });
            Assert.True(res.IsSuccess);
            var stringdata = operationManager.Read<string[]>($"{dbNamePrefix}{dbFrom + 60}", 20);
            Assert.True("aabb121212" == stringdata.Content[0]);
            Assert.True("1212aabb" == stringdata.Content[1]);
            #endregion
        }
    }
}
