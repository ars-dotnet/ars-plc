using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TOPRO.Test
{
    public class TestIntPtr
    {
        [Fact]
        public void Test1() 
        {
            unsafe
            {
                int num = 123;

                int* p = &num; //建立指针P，指向变量num

                //内存地址
                int address = (int)p;

                Assert.True(*p == 123);

                IntPtr op = new IntPtr(address);//构造c#类型的指针

                Assert.True(Marshal.ReadInt32(op) == 123);//输出的是变量num的值
            }
        }
    }
}
