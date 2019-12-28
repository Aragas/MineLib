using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Aragas.Network
{
    /// <summary>
    /// Cached implementation of <see cref="Activator"/>.
    /// </summary>
    public static class ActivatorCached
    {
        private delegate object ObjectActivator(params object[] args);

        private static readonly ConcurrentDictionary<Type, ObjectActivator> Cache = new ConcurrentDictionary<Type, ObjectActivator>();

        public static TType CreateInstanceGeneric<TType>(Type type) => (TType) CreateInstance(type.GetType());
        public static TType CreateInstanceGeneric<TType>() => (TType) CreateInstance(typeof(TType));

        /// <summary>
        /// Faster implementation of <see cref="Activator.CreateInstance(Type, object[])"/>.
        /// <para/>
        /// First <see cref="Type"/> creation is slow.
        /// </summary>
        public static object CreateInstance(Type input)
        {
            if (Cache.TryGetValue(input, out var objectActivator))
                return objectActivator();

            var constructors = input.GetTypeInfo().DeclaredConstructors;
            var constructor = constructors.First();

            var newex = Expression.New(constructor);
            var lambda = Expression.Lambda(typeof(ObjectActivator), newex, Expression.Parameter(typeof(object[]), "args"));
            var result = (ObjectActivator) lambda.Compile();
            Cache.TryAdd(input, result);
            return result();
        }

        public static object CreateInstance(Type input, params object[] args)
        {
            if (Cache.TryGetValue(input, out var objectActivator))
                return objectActivator(args);

            var types = args.Select(p => p.GetType());

            var constructors = input.GetTypeInfo().DeclaredConstructors;
            var constructor = args.Length == 0
                ? constructors.First()
                : constructors.Single(constr => constr.GetParameters().Select(param => param.ParameterType).SequenceEqual(types));

            var paraminfo = constructor.GetParameters();

            var paramex = Expression.Parameter(typeof(object[]), "args");

            var argex = new Expression[paraminfo.Length];
            for (var i = 0; i < paraminfo.Length; i++)
            {
                var index = Expression.Constant(i);
                var paramType = paraminfo[i].ParameterType;
                var accessor = Expression.ArrayIndex(paramex, index);
                argex[i] = Expression.Convert(accessor, paramType);
            }

            var newex = Expression.New(constructor, argex);
            var lambda = Expression.Lambda(typeof(ObjectActivator), newex, paramex);
            var result = (ObjectActivator)lambda.Compile();
            Cache.TryAdd(input, result);
            return result(args);
        }


        /// <summary>
        /// Clear the cache.
        /// </summary>
        public static void ClearCache() => Cache.Clear();
    }
}