using System.Reflection;
using System.Linq;
using System.Collections;

namespace MyRefs.Extensions
{
    public static class ReflectionExtension
    {
        public static object? CallMethodGenericByReflection<T, G>(this T thing, string method, object?[]? args)
        {
            return thing.CallMethodByReflection<T>(method: method, generics: new Type[] { typeof(G) }, argsTypes: args?.Select(s => s?.GetType()).ToArray(), args: args);
        }

        public static object? CallMethodByReflection<T>(this T thing, string method)
        {
            return thing.CallMethodByReflection<T>(method, null);
        }

        public static object? CallMethodByReflection<T>(this T thing, string method, object?[]? args)
        {
            return thing.CallMethodByReflection<T>(method: method, generics: null, argsTypes: args?.Select(s => s?.GetType()).ToArray(), args: args);
        }

        public static object? CallMethodByReflection<T>(this T thing, string method, Type?[]? generics, Type?[]? argsTypes, object?[]? args)
        {
#pragma warning disable
            MethodInfo? methodInfo = null;

            methodInfo = thing.GetType().GetMethod(method, argsTypes ?? new Type[] { });

            if (argsTypes != null && argsTypes.Length > 0)
            {
                if (methodInfo == null)
                    ThrowMissingMethodException($"The type {thing.GetType().Name} do not have a method called {method} with {argsTypes?.Length} args");
            }


            if (generics != null && generics.Length > 0)
            {
                methodInfo = methodInfo.MakeGenericMethod(generics) ?? thing.GetType().GetMethod(method, new Type[] { }).MakeGenericMethod(generics);

                if (methodInfo == null)
                    ThrowMissingMethodException($"The type {thing.GetType().Name} do not have a method called {method} with {generics?.Length} generics args");
            }

            if (methodInfo == null)
                ThrowMissingMethodException($"The type {thing.GetType().Name} do not have a method called {method}");

            return methodInfo?.Invoke(thing, args ?? new object[] { });
#pragma warning enable

            void ThrowMissingMethodException(string msg) => throw new MissingMethodException(msg);
        }


        public static ConstructorInfo? GetParameterlessCtor<T>(this T thing)
        {
            return GetParameterlessCtor(thing.GetType());
        }

        public static ConstructorInfo? GetParameterlessCtor(Type type)
        {
            return type.GetConstructor(new Type[] { });
        }

        public static PropertyInfo[]? GetPublicProperties(Type thing, Func<PropertyInfo, bool> expression)
        {
            return thing.GetProperties(BindingFlags.Public | BindingFlags.Instance)
               .Where(expression)
               .ToArray();
        }

        public static FieldInfo[]? GetPublicFields(Type thing, Func<FieldInfo, bool> expression)
        {
            return thing.GetFields(BindingFlags.Public | BindingFlags.Instance).Where(expression).ToArray();               
        }

       

        public static PropertyInfo? GetPublicProperty(Type thing, Func<PropertyInfo, bool> expression)
        {
            return thing.GetProperties(BindingFlags.Public | BindingFlags.Instance)
               .Where(expression)
               .FirstOrDefault();
        }

        public static PropertyInfo[]? GetPublicProperties<T>(this T thing, Func<PropertyInfo, bool> expression)
        {
            return thing.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
               .Where(expression)
               .ToArray();
        }

        public static FieldInfo[]? GetPublicFields<T>(this T thing, Func<FieldInfo, bool> expression)
        {
            return thing.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance)
               .Where(expression)
               .ToArray();
        }

        public static PropertyInfo? GetPublicProperty<T>(this T thing, Func<PropertyInfo, bool> expression)
        {
            return thing.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
               .Where(expression)
               .FirstOrDefault();
        }

        public static object GetFieldValue(this object @object, string prop)
        {
            FieldInfo[] fieldInfos = @object.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            if (fieldInfos == null || fieldInfos.Count() == 0)
                throw new Exceptions.FieldNotFoundException(@object.GetType(), prop);
            else
                return fieldInfos[0].GetValue(@object);
        }

        public static T GetFieldValue<T>(this object @object, string prop)
        {
            FieldInfo[] fieldInfos = @object.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            if (fieldInfos == null || fieldInfos.Count() == 0)
                throw new Exceptions.FieldNotFoundException(@object.GetType(), prop);
            else
            {
                if (fieldInfos[0].FieldType.Equals(typeof(T)))
                {
                    return (T)fieldInfos[0].GetValue(@object);
                }
                else
                    return default(T);

            }
               
        }

        public static void SetFieldValue(this object @object, string field, object? value)
        {
            FieldInfo[] fieldInfos = @object.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            if (fieldInfos == null || fieldInfos.Count() == 0)
                throw new Exceptions.FieldNotFoundException(@object.GetType(), field);
            else
            {
                fieldInfos[0].SetValue(@object, value);
            }

        }

        public static object GetPropertyValue(this object @object, string prop)
        {
            PropertyInfo[] properties = @object.GetPublicProperties(s => s.Name == prop);

            if (properties == null || properties.Count() == 0)
                try
                {
                    return @object.GetFieldValue(prop);

                }
                catch (Exceptions.FieldNotFoundException)
                {
                    throw new Exceptions.PropertyNotFoundException(@object.GetType(), prop);
                }
            else
            {
                return properties[0].GetValue(@object);
            }    
        }

        public static T GetPropertyValue<T>(this object @object, string prop)
        {
            PropertyInfo[] properties = @object.GetPublicProperties(s => s.Name == prop);

            if (properties == null || properties.Count() == 0)
                try
                {
                    return @object.GetFieldValue<T>(prop);

                }
                catch (Exceptions.FieldNotFoundException)
                {
                    throw new Exceptions.PropertyNotFoundException(@object.GetType(), prop);
                }
            else
            {
                if (properties[0].PropertyType.Equals(typeof(T)))
                {
                    return (T)properties[0].GetValue(@object);
                }
                else
                    return default(T);
            }
        }

        public static void SetPropertyValue(this object @object, string prop, object? value)
        {
            PropertyInfo[] properties = @object.GetPublicProperties(s => s.Name == prop);

            if (properties == null || properties.Count() == 0)
                try
                {
                    @object.SetFieldValue(prop, value);

                }
                catch (Exceptions.FieldNotFoundException)
                {
                    throw new Exceptions.PropertyNotFoundException(@object.GetType(), prop);
                }
            else
            {
                properties[0].SetValue(@object, value);
            }

        }

    }
}
