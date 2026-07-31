using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonsContentMod.Utilities
{
    internal class Clonerators
    {
        public static T ShallowClone<T>(T source)
            where T : class
        {
            if (source == null)
                return null;

            var method = typeof(object).GetMethod(
                "MemberwiseClone",
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic);

            return (T)method.Invoke(source, null);
        }
    }
}
