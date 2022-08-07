namespace MyRefs.Tests.Tests
{
    public class TestFieldsAcess
    {
        [Fact]
        public void AcessPublicFieldOfObjectWithGenericArgs()
        {
            Person p = new Person();
            p.nome = "Adriano";

            Assert.Equal("Adriano", p.GetFieldValue<string>(nameof(p.nome)));
        }

        [Fact]
        public void AcessPublicFieldOfObject()
        {
            Person p = new Person();
            p.nome = "Adriano";

            Assert.Equal("Adriano", p.GetFieldValue(nameof(p.nome)));
        }


        [Fact]
        public void SetPublicFieldOfObject()
        {
            Person p = new Person();

            p.SetFieldValue(nameof(p.nome), "Adriano");

            Assert.Equal("Adriano", p.GetFieldValue<string>(nameof(p.nome)));
        }


        [Fact]
        public void AcessProtectedFieldOfObject()
        {
            Person p = new Person();
            p.SetToken("MyToken");

            Assert.Equal("MyToken", p.GetFieldValue<string>("token"));
        }


        [Fact]
        public void SetProtectedFieldOfObject()
        {
            Person p = new Person();

            p.SetFieldValue("token", "MyToken");

            Assert.Equal("MyToken", p.GetFieldValue<string>("token"));
        }



        [Fact]
        public void AcessPrivateFieldOfObject()
        {
            Person p = new Person();
            p.SetDoc(123456789);

            Assert.Equal(123456789, p.GetFieldValue<int>("doc"));
        }


        [Fact]
        public void SetPrivateFieldOfObject()
        {
            Person p = new Person();

            p.SetFieldValue("doc", 123456789);

            Assert.Equal(123456789, p.GetFieldValue<int>("doc"));
        }

        [Fact]
        public void AcessIndexOfArrayOfAFieldOfObject()
        {
            Person p = new Person();

            p.SetNames(new List<string>
            {
                "Adriano",
                "Marino",
                "Balera"
            });

            Assert.Equal("Adriano", p.GetFieldValue<List<string>>("names")[0]);
           
        }


        [Fact]
        public void AcessWithSpecificMethodIndexOfArrayOfAFieldOfObject()
        {
            Person p = new Person();

            p.SetNames(new List<string>
            {
                "Adriano",
                "Marino",
                "Balera"
            });
                       

            Assert.Equal("Adriano", p.GetValueFromIndexOfCollection<string>("names", 0));           

        }

        [Fact]
        public void FailIndexOutOfRangeIndexOfArrayOfAFieldOfObject()
        {
            Person p = new Person();

            p.SetNames(new List<string>
            {
                "Adriano",
                "Marino",
                "Balera"
            });

            _ = Assert.Throws<System.ArgumentOutOfRangeException>(() => _ = p.GetValueFromIndexOfCollection<string>("names", 4));    
            
            _ = Assert.Throws<MyRefs.Exceptions.InvalidPropertyException>(() => _ = p.GetValueFromIndexOfCollection<int>("names", 4));   
            
            _ = Assert.Throws<MyRefs.Exceptions.InvalidPropertyException>(() => _ = p.GetValueFromIndexOfCollection<int>("doc", 4));            

        }


        [Fact]
        public void SetWithSpecificMethodIndexOfArrayOfAFieldOfObject()
        {
            Person p = new Person();

            p.SetNames(new List<string>
            {
                "Adriano",
                "Marino",
                "Balera"
            });

            p.SetValueInIndexOfCollection<string>("names", 0, "Camila");

            Assert.Equal("Camila", p.GetValueFromIndexOfCollection<string>("names", 0));

        }
    }
}