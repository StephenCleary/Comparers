using System;
using System.Reflection;

namespace Nito
{
    internal static class ReflectionHelpers
    {
        public static Type TryGetEnumeratorType(Type source)
        {
            return TryFindInterfaceType(source, "IEnumerable`1");
        }

        public static Type TryFindInterfaceType(Type type, string name)
        {
            if (type.Name == name)
                return type;
            foreach (var interfaceType in type.GetTypeInfo().ImplementedInterfaces)
            {
                if (interfaceType.Name == name)
                    return interfaceType;
            }

            return null;
        }

        public static PropertyInfo TryFindDeclaredProperty(Type type, string name)
        {
            foreach (var property in type.GetTypeInfo().DeclaredProperties)
            {
                if (property.Name == name)
                    return property;
            }

            return null;
        }
    }
}