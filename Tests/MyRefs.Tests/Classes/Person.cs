namespace MyRefs.Tests.Classes
{
    public class Person : IPerson
    {
        public Person(string name)
        {
            Name = name;
        }

        public Person() 
        {
            
        }

        private List<string> names;

        public void SetNames(List<string> list) => names = list;

        public T GetGeneric<T>(T obj) => obj;

        public string nome;

        protected string token;

        public void SetToken(string token) => this.token = token;

        private int doc;

        public void SetDoc(int doc) => this.doc = doc;

        public string Name { get; set; }
        public int AskAge() => 23;
        public IEnumerable<IPerson> GetParents() => new List<IPerson> { new Person("Father"), new Person("Mother") };
        public void Run() => Console.WriteLine($"{Name}, run !");
        public void Talk(string msg) => Console.WriteLine($"{Name} said: \"{msg}\"");

    }
}
