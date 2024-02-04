using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TOPRO.Test
{
    public class IListTest
    {
        [Fact]
        public void Test() 
        {
            IList<int> list = new List<int>{ 1, 2, 3, 4 };

            IList<int> data = list.ToList();

            foreach (var d in data) 
            {
                list.Remove(d);
            }
        }

        [Fact]
        public void Test1() 
        {
            string a = "";

            var m = JsonConvert.DeserializeObject<A>(a);
        }

        class A 
        {
            public string Name { get; set; }
        }
    }
}
