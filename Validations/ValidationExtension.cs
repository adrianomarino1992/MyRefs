using MyRefs.Extensions;
using System.Reflection;

namespace MyRefs.Validations
{
    public static class ValidationExtension
    {
        public static bool IsNull<T>(this T thing)
        {
            return thing == null;
        }

        public static bool IsEquals<T>(this T thing, T other)
        {
            if (other == null || thing == null)
                return false;

            if (thing.GetType().IsValueType)
                return thing.Equals(other);

            PropertyInfo[]? props = thing.GetPublicProperties(s => s.PropertyType.IsValueType || s.PropertyType == typeof(string));

            if (props != null)
            {
                for (int i = 0; i < props.Length; i++)
                {
                    object? v1 = props[i].GetValue(thing);
                    object? v2 = props[i].GetValue(other);

#pragma warning disable CS8602 // Desreferência de uma referência possivelmente nula.
                    if (!((v1 == null && v2 == null) || v1.Equals(v2)))
                    {
                        return false;
                    }
#pragma warning restore CS8602 // Desreferência de uma referência possivelmente nula.
                }
            }

            props = thing.GetPublicProperties(s => s.PropertyType.IsClass && s.PropertyType != typeof(string));

            if (props != null)
            {
                for (int i = 0; i < props.Length; i++)
                {
                    object? v1 = props[i].GetValue(thing);
                    object? v2 = props[i].GetValue(other);

#pragma warning disable 
                    if (!((v1 == null && v2 == null) || v1.IsEquals(v2)))
                    {
                        return false;
                    }
#pragma warning restore 
                }
            }

            return true;

        }

        public static bool IsEmpty<T>(this T thing) where T : notnull
        {
            if (thing.GetType() == typeof(string))
                return String.IsNullOrWhiteSpace((string)(object)thing) || String.IsNullOrEmpty((string)(object)thing);


            ConstructorInfo? ctor = thing.GetParameterlessCtor();

            if (ctor == null)
            {
                throw new Exceptions.ContructorNotFoundException(thing.GetType());
            }

            T i = (T)ctor.Invoke(new object[] { });

            return i.IsEquals(thing);

        }
    }
}
