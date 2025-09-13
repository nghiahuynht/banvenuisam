using GM.CORE.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GM.CORE.Extensions
{
    public static class DictionaryExtensions
    {
        public static Dictionary<string, object> ToDict<T>(this T target) => target == null
           ? new Dictionary<string, object>()
           : typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => !Attribute.IsDefined(p, typeof(SkipPropertyAttribute)))
                    .ToDictionary(
                           x => x.Name,
                           x => x.GetValue(target)
                       );
    }
}