# MyRefs

MyRefs is a lib to help us to acess object acpescts with reflection 

## Installation

.NET CLI

```bash
dotnet add package Adr.MyRefs --version 1.1.0
```

Nuget package manager

```bash
PM> Install-Package Adr.MyRefs-Version 1.1.0
```

packageReference

```bash
<PackageReference Include="Adr.MyRefs" Version="1.1.0" />
```

## Usage

**Get a public property:**
```csharp
            Person p = new Person(); // create instance

            p.Name = "Adriano"; //set the value

            // get the Name and print the value
            Console.WriteLine(p.GetPropertyValue<string>(nameof(p.Name))); //Adriano

```


**Set a public property value:**
```csharp
            Person p = new Person(); // create instance

            p.SetPropertyValue(nameof(p.Name), "Adriano"); //set the value

            // get the Name and print the value
            Console.WriteLine(p.GetPropertyValue<string>(nameof(p.Name))); //Adriano

```

**Get a public | protected | private field:**
```csharp
            Person p = new Person(); // create instance

            p.Name = "Adriano"; //set the value

            // get the Name and print the value
            Console.WriteLine(p.GetFieldValue<string>(nameof(p.Name))); //Adriano

```

**Set a public | protected | private property value:**
```csharp
            Person p = new Person(); // create instance

            p.GetFieldValue(nameof(p.Name), "Adriano"); //set the value

            // get the Name and print the value
            Console.WriteLine(p.GetFieldValue<string>(nameof(p.Name))); //Adriano


```

**Get parameterless constructor:**
```csharp
             // get constructor
             ConstructorInfo cTor = MyRefs.Extensions.ReflectionExtension.GetParameterlessCtor(typeof(Person))!;
        
            // create a instance of object
            Person p = (Person)cTor.Invoke(new object[] { });

```

**Get constructor:**
```csharp
             // get constructor
             ConstructorInfo cTor = MyRefs.Extensions.ReflectionExtension.GetCtorByParamsType<Person>(new Type[] {  typeof(string)})!;

            // create a instance of object
            Person p = (Person)cTor.Invoke(new object[] { "Adriano" });

```

**Call a method:**
```csharp
            // create a instance
            Person p = new Person("Adriano");            
            
            // call method
            p.CallMethodByReflection(nameof(p.AskAge)) // 23


```

**Call a generic method:**
```csharp
            // create a instance
            Person p = new Person("Adriano");            
            
            // call method
            p.CallMethodGenericByReflection<Person, string>(nameof(p.GetGeneric), new object[] { "Adriano" }) // Adriano


```


**Get the value of a index of collection:**
```csharp
            
            // create instance
            Person p = new Person();
            
            // set values
            p.SetNames(new List<string>
            {
                "Adriano",
                "Marino",
                "Balera"
            });
                       
            // get the value of list in index 0
            p.GetValueFromIndexOfCollection<string>("names", 0); // Adriano


```


**Setthe value of a index of collection:**
```csharp
            
            // create instance
            Person p = new Person();
            
            // set values
            p.SetNames(new List<string>
            {
                "Adriano",
                "Marino",
                "Balera"
            });

            // set the value of list in index 0
            p.SetValueInIndexOfCollection<string>("names", 0, "Camila");
                       
            // get the value of list in index 0
            p.GetValueFromIndexOfCollection<string>("names", 0); // Camila


```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
[MIT](https://choosealicense.com/licenses/mit/)
