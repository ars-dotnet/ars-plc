using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPRO.Test
{
    public class GenericTest
    {
        [Fact]
        public void Test1() 
        {
            Generic generic = new Generic();
            var classtype = typeof(Generic);
            var getype = Type.MakeGenericMethodParameter(0);

            //ref
            //getype.MakeByRefType();

            var method = classtype.GetMethod(
                "WriteCustomer",genericParameterCount:1,new Type[] { typeof(string),getype})!;

            method = method.MakeGenericMethod(typeof(People));
            method.Invoke(generic,new object[] { "123", new PeopleA { Id = 123 } });
        }

        [Fact]
        public void Test2() 
        {
            Generic generic = new Generic();
            var classtype = typeof(Generic);

            var method = classtype.GetMethod(
                "ReadCustomer")!;

            method = method.MakeGenericMethod(typeof(People));
            method.Invoke(generic, new object[] { "123" });
        }
    }

    public class Generic 
    {
        public Result<T> ReadCustomer<T>(string address) where T : People, new()
        {
            return new Result<T> { data = new T()};
        }

        public Result WriteCustomer<T>(string address, T data) where T : People, new()
        {
            return null;
        }
    }

    public class People { }

    public class PeopleA : People 
    {
        public int Id { get; set; }
    }


    public class Result { }

    public class Result<T> : Result
    {
        public T data { get; set; }
    }
}
