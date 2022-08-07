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

            methodInfo = thing.GetType().GetMethods().FirstOrDefault(s => s.Name == method && s.GetGenericArguments().Count() == (generics?.Count() ?? 0));

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

        public static ConstructorInfo? GetCtorByParamsType<T>(Type[] args)
        {
            return GetCtorByParamsType(typeof(T), args);
        }

        public static ConstructorInfo? GetCtorByParamsType<T>(this T thing, Type[] args)
        {
            return GetCtorByParamsType(thing.GetType(), args);
        }

        public static ConstructorInfo? GetParameterlessCtor(Type type)
        {
            return type.GetConstructor(new Type[] { });
        }

        public static ConstructorInfo? GetCtorByParamsType(Type type, Type[] args)
        {
            return type.GetConstructor(args);
        }

        public static PropertyInfo[]? GetPublicProperties(Type thing, Func<PropertyInfo, bool> expression = null)
        {
            expression = expression ?? (Func<PropertyInfo, bool>)(s => true);
            return thing.GetProperties(BindingFlags.Public | BindingFlags.Instance)
               .Where(expression)
               .ToArray();
        }

        public static PropertyInfo[]? GetAllProperties(Type thing, Func<PropertyInfo, bool> expression = null)
        {
            expression = expression ?? (Func<PropertyInfo, bool>)(s => true);
            return thing.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
               .Where(expression)
               .ToArray();
        }

        public static FieldInfo[]? GetPublicFields(Type thing, Func<FieldInfo, bool> expression = null)
        {
            expression = expression ?? (Func<FieldInfo, bool>)(s => true);
            return thing.GetFields(BindingFlags.Public | BindingFlags.Instance).Where(expression).ToArray();               
        }

        public static FieldInfo[]? GetAllFields(Type thing, Func<FieldInfo, bool> expression = null)
        {
            expression = expression ?? (Func<FieldInfo, bool>)(s => true);
            return thing.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(expression).ToArray();
        }


        public static PropertyInfo? GetPublicProperty(Type thing, Func<PropertyInfo, bool> expression = null)
        {
            expression = expression ?? (Func<PropertyInfo, bool>)(s => true);
            return thing.GetProperties(BindingFlags.Public | BindingFlags.Instance)
               .Where(expression)
               .FirstOrDefault();
        }



        public static PropertyInfo[]? GetPublicProperties<T>(this T thing, Func<PropertyInfo, bool> expression = null)
        {
            expression = expression ?? (Func<PropertyInfo, bool>)(s => true);
            return thing.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
               .Where(expression)
               .ToArray();
        }

        public static PropertyInfo[]? GetAllProperties<T>(this T thing, Func<PropertyInfo, bool> expression = null)
        {
            expression = expression ?? (Func<PropertyInfo, bool>)(s => true);
            return thing.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
               .Where(expression)
               .ToArray();
        }

        public static FieldInfo[]? GetAllFields<T>(this T thing, Func<FieldInfo, bool> expression = null)
        {
            expression = expression ?? (Func<FieldInfo, bool>)(s => true);
            return thing.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
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
            FieldInfo[] fieldInfos = @object.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(s => s.Name == prop).ToArray();

            if (fieldInfos == null || fieldInfos.Count() == 0)
                throw new Exceptions.FieldNotFoundException(@object.GetType(), prop);
            else
                return fieldInfos[0].GetValue(@object);
        }

        public static T GetFieldValue<T>(this object @object, string prop)
        {
            FieldInfo[] fieldInfos = @object.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(s => s.Name == prop).ToArray();

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
            FieldInfo[] fieldInfos = @object.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(s => s.Name == field).ToArray();

            if (fieldInfos == null || fieldInfos.Count() == 0)
                throw new Exceptions.FieldNotFoundException(@object.GetType(), field);
            else
            {
                fieldInfos[0].SetValue(@object, value);
            }

        }

        public static object GetPropertyValue(this object @object, string prop)
        {
            PropertyInfo[] properties = @object.GetAllProperties(s => s.Name == prop);

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
            PropertyInfo[] properties = @object.GetAllProperties(s => s.Name == prop);

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
            PropertyInfo[] properties = @object.GetAllProperties(s => s.Name == prop);

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


        public static T GetValueFromIndexOfCollection<T>(this object @object, string prop, int index)
        {
            object list = @object.GetPropertyValue(prop);
                       

            if(list == null)
                throw new Exceptions.ArrayIsNullException(@object.GetType(), prop);

            Type objType = list.GetType();

            if(!objType.IsAssignableTo(typeof(IEnumerable)))
                throw new Exceptions.InvalidPropertyException(@object.GetType(), prop, 
                    $"{objType.Name}.{prop} is not a list or array");


            //check for list
            {
                if (objType.IsAssignableTo(typeof(IList)))
                {
                    if (!list.GetType().GetGenericArguments()[0].Equals(typeof(T)))
                    {
                        throw new Exceptions.InvalidPropertyException(@object.GetType(), prop,
                   $"{objType.Name}.{prop} is not a list of type {typeof(T).Name}");
                    }

                    return (T)((list as IList)[index]); 
                }
            }


            //check for array
            {
                if (objType.IsAssignableTo(typeof(System.Array)))
                {
                    if (!list.GetType().GetElementType().Equals(typeof(T)))
                    {
                        throw new Exceptions.InvalidPropertyException(@object.GetType(), prop,
                   $"{objType.Name}.{prop} is not a array of type {typeof(T).Name}");
                    }

                    return (T)((list as Array).GetValue(index));
                }
            }


            throw new Exceptions.InvalidPropertyException(@object.GetType(), prop,
                   $"{objType.Name}.{prop} is not a array or list of type {typeof(T).Name}");


            return default(T);

        }


        public static void SetValueInIndexOfCollection<T>(this object @object, string prop, int index, T value)
        {
            object list = @object.GetPropertyValue(prop);


            if (list == null)
                throw new Exceptions.ArrayIsNullException(@object.GetType(), prop);

            Type objType = list.GetType();

            if (!objType.IsAssignableTo(typeof(IEnumerable)))
                throw new Exceptions.InvalidPropertyException(@object.GetType(), prop,
                    $"{objType.Name}.{prop} is not a list or array");


            //check for list
            {
                if (objType.IsAssignableTo(typeof(IList)))
                {
                    if (!list.GetType().GetGenericArguments()[0].Equals(typeof(T)))
                    {
                        throw new Exceptions.InvalidPropertyException(@object.GetType(), prop,
                   $"{objType.Name}.{prop} is not a list of type {typeof(T).Name}");
                    }

                    (list as IList)[index] = value;

                    return;
                }
            }


            //check for array
            {
                if (objType.IsAssignableTo(typeof(System.Array)))
                {
                    if (!list.GetType().GetElementType().Equals(typeof(T)))
                    {
                        throw new Exceptions.InvalidPropertyException(@object.GetType(), prop,
                   $"{objType.Name}.{prop} is not a array of type {typeof(T).Name}");
                    }

                    (list as Array).SetValue(value, index);

                    return;
                }
            }


            throw new Exceptions.InvalidPropertyException(@object.GetType(), prop,
                   $"{objType.Name}.{prop} is not a array or list of type {typeof(T).Name}");


        }
    }
}
