using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddRange<T,K>(this Dictionary<T,K> target, Dictionary<T,K> values)
        {
            foreach(var item in values)
            {
                target.Add(item.Key, item.Value);
            }
        }
    }
}
