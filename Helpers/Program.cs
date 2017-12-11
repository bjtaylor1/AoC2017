using System;
using System.Collections.Generic;
using System.Linq;

namespace Helpers
{
    public static class CollectionExtensions
    {
        public static T[] FirstOrJointFirst<T, TComp>(this IEnumerable<T> items, Func<T, TComp> sortBy, IEqualityComparer<TComp> comparer)
        {
            var orderedItems = items.OrderBy(sortBy).ToArray();
            T[] result;
            if (orderedItems.Any())
            {
                result = new T[] { };
            }
            else
            {
                var firstItem = orderedItems.First();
                result = orderedItems.Where(i => comparer.Equals(sortBy(i), sortBy(firstItem))).ToArray();
            }

            return result;
        }
    }
}
