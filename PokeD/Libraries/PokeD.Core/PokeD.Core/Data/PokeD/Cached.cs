using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PokeD.Core.Data.PokeD
{
    public class Cached<T2> where T2 : class
    {
        private static Dictionary<long, T2> Cache { get; } = new Dictionary<long, T2>();

        public static T2 Get(long id)
        {
            if (Cache.TryGetValue(id, out var value))
                return value;
            else
            {
                value = (T2) Activator.CreateInstance(typeof(T2), new object[] { Convert.ChangeType(id, typeof(T2).GetTypeInfo().DeclaredConstructors.FirstOrDefault().GetParameters().FirstOrDefault().ParameterType ?? typeof(byte)) });
                Cache.Add(id, value);
                return value;
            }
        }
    }

    public class Cached<T1, T2> where T2 : class
    {
        protected static Dictionary<T1, T2> Cache { get; } = new Dictionary<T1, T2>();

        public static T2 Get(T1 id)
        {
            if (Cache.TryGetValue(id, out var value))
                return value;
            else
            {
                value = (T2) Activator.CreateInstance(typeof(T2), new object[] {id});
                Cache.Add(id, value);
                return value;
            }
        }
    }
}
