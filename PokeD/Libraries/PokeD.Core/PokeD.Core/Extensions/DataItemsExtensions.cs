using System.Collections.Generic;
using System.Linq;

using PokeD.Core.Data.P3D;
using PokeD.Core.Data.PokeD;

namespace PokeD.Core.Extensions
{
    public static class DataItemsExtensions
    {
        public static Monster[] DataItemsToMonsters(this DataItems data) => data.ToString().Split('|').Select(str => str).Select(items => new Monster(items)).ToArray();

        public static Dictionary<string, string> ToDictionary(this DataItems data)
        {
            var dict = new Dictionary<string, string>();
            var str = data.ToString();
            str = str.Replace("{", "");
            //str = str.Replace("}", ",");
            var array = str.Split('}');
            foreach (var s in array.Reverse().Skip(1))
            {
                var v = s.Split('"');
                dict.Add(v[1], v[2].Replace("[", "").Replace("]", ""));
            }

            return dict;
        }
    }
}