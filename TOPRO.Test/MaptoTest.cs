using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPRO.Test
{
    public class MaptoTest
    {
        [Fact]
        public void Test() 
        {
            A a = new A { Name = "Bill" };
            var ex = Assert.ThrowsAny<Exception>(() => { B b = (B)a; }); 

            A a1 = new B { Name = "Bill2" };
            B b2 = (B)a1;
        }
    }

    public class A 
    {
        public string Name { get; set; }
    }

    public class B : A { }
}
