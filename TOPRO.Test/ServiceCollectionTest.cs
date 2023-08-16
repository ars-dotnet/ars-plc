using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOPRO.Test
{
    public class ServiceCollectionTest
    {
        IServiceCollection services;
        public ServiceCollectionTest()
        {
            services = new ServiceCollection();
            services.TryAddEnumerable(
                new List<ServiceDescriptor> 
                {
                     new ServiceDescriptor(typeof(IAnimal), typeof(Cat), ServiceLifetime.Transient),
                     new ServiceDescriptor(typeof(IAnimal),typeof(Dog),ServiceLifetime.Transient),
                });
            services.AddTransient<Action>();
        }

        [Fact]
        public void Test1() 
        {
            var a = services.BuildServiceProvider().GetRequiredService<IAnimal>();
            Assert.True(a.GetType() != typeof(Cat));
        }

        [Fact]
        public void Test2() 
        {
            var a = (ICustomAnimal)services.BuildServiceProvider().GetRequiredService<IAnimal>();
            Assert.True(a.GetType() == typeof(Dog));
        }

        [Fact]
        public void Test() 
        {
            var a = services.BuildServiceProvider().GetRequiredService<Action>();
        }
    }

    public class Action
    {
        public Action(IAnimal animal)
        {

        }
    }

    public interface IAnimal
    {
        string Name { get; }
    }

    public interface ICustomAnimal : IAnimal{ }

    public class Dog : IAnimal
    {
        public string Name => "狗";
    }

    public class Cat : IAnimal, ICustomAnimal
    {
        public string Name => "猫";
    }
}
