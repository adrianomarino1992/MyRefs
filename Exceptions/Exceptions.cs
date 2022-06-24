namespace MyRefs.Exceptions
{
    public class FieldNotFoundException : Exception
    {
        public string Field { get; }
        public Type Type { get; }
        public override string Message { get; }
        public FieldNotFoundException(Type type,string field, string msg = "") 
        {

            Message = msg;

            if(String.IsNullOrEmpty(msg))
            {
                Message = $"The type {type.Name} do not have any public field named {field}";
            }            

            Field = field;
            Type = type;            
            
        }
    }

    public class PropertyNotFoundException : Exception
    {
        public string Property { get; }
        public Type Type { get; }
        public override string Message { get; }
        public PropertyNotFoundException(Type type, string prop, string msg = "")
        {

            Message = msg;

            if (String.IsNullOrEmpty(msg))
            {
                Message = $"The type {type.Name} do not have any public property named {prop}";
            }

            Property = prop;
            Type = type;

        }
    }

    public class ContructorNotFoundException : Exception
    {        
        public Type Type { get; }
        public override string Message { get; }
        public ContructorNotFoundException(Type type, string msg = "")
        {

            Message = msg;

            if (String.IsNullOrEmpty(msg))
            {
                Message = $"The type {type.Name} do not have any public parameterless constructor";
            }
                        
            Type = type;

        }
    }
}
