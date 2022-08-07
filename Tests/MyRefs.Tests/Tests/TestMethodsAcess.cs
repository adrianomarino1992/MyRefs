using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MyRefs.Tests.Tests
{
    public class TestMethodsAcess
    {
        [Fact]
        public void TestParameterlessCtor()
        {
            ConstructorInfo cTor = MyRefs.Extensions.ReflectionExtension.GetParameterlessCtor(typeof(Person))!;

            Person p = (Person)cTor.Invoke(new object[] { });

            Assert.NotNull(p);

        }

        [Fact]
        public void TestCtorByParametersAndTestTheValueOfProperty()
        {
            ConstructorInfo cTor = MyRefs.Extensions.ReflectionExtension.GetCtorByParamsType<Person>(new Type[] {  typeof(string)})!;

            Person p = (Person)cTor.Invoke(new object[] { "Adriano" });

            Assert.NotNull(p);

            Assert.Equal("Adriano", p.Name);

        }


        [Fact]
        public void CallMethodWithParameters()
        {

            Person p = new Person("Adriano");            

            Assert.Equal(23, p.CallMethodByReflection(nameof(p.AskAge)));

        }


        [Fact]
        public void CallMethodWithParametersAndGenerics()
        {

            Person p = new Person("Adriano");

            Assert.Equal("Adriano", p.CallMethodGenericByReflection<Person, string>(nameof(p.GetGeneric), new object[] { "Adriano" }));

        }
    }
}
