using System;
using System.Linq;
using System.Reflection;

namespace Aragas.Network.Extensions
{
    public static class EnumExtensions
    {
        /// <summary/>
        /// <param name="className"/><param name="assembly"/>
        /// <returns/>
        private static Type GetTypeFromName(string className, Assembly assembly) => assembly.DefinedTypes.Where(typeInfo => typeInfo.Name == className)
            .Select(typeInfo => typeInfo.AsType()).FirstOrDefault();
    }
}