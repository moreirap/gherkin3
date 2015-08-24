using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gherkin.GRLCatalogueGenerator
{
    public static class ExtensionMethods
    {
        public static void Update<TSource>(this IEnumerable<TSource> outer, Action<TSource> updator)
        {
            foreach (var item in outer)
            {
                updator(item);
            }
        }
    }
}
