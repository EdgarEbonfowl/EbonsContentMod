using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonsContentMod.Utilities
{
    public static partial class ListFunctions
    {
        public static List<T> AddUnique<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
                list.Add(item);
            return list;
        }

        public static List<T> AddUnique<T>(this List<T> list, Func<T, bool> pred, Func<T> getter)
        {
            if (!list.Any(pred))
                list.Add(getter());
            return list;
        }
    }
}
