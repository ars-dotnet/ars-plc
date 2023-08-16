using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPRO.Test
{
    public class ConcurrentBagTest
    {
        [Fact]
        public void Test()
        {
            ConcurrentBag<int> a = new ConcurrentBag<int>();
            a.Add(123);
            a.Add(123);

            HashSet<object> aa = new HashSet<object>();
            aa.Add(new { a = 1, b = 2 });
            aa.Add(new { a = 1, b = 2 });
            aa.Add(new { b = 2, a = 1 });
        }
    }
}
