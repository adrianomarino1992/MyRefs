using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRefs.Tests.Tests
{
    public class TestPropertiesAcess
    {
        [Fact]
        public void AcessPublicPropertyOfObjectWithGenericArgs()
        {
            Person p = new Person();
            p.Name = "Adriano";

            Assert.Equal("Adriano", p.GetPropertyValue<string>(nameof(p.Name)));
        }


        [Fact]
        public void SetPublicPropertyOfObject()
        {
            Person p = new Person();
            p.nome = "Adriano";

            Assert.Equal("Adriano", p.GetPropertyValue(nameof(p.nome)));
        }


        [Fact]
        public void SetPublicFieldOfObject()
        {
            Person p = new Person();

            p.SetPropertyValue(nameof(p.nome), "Adriano");

            Assert.Equal("Adriano", p.GetPropertyValue<string>(nameof(p.nome)));
        }

    }
}
