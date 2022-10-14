using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrba.Extensions
{
    public static class CollectionExtensions
    {
        public static ICollection<T> AddExcept<T>(
            this ICollection<T> collection,
            IEnumerable<T> newItems,
            Func<T, bool> predicate)
        {

            newItems.ToList().ForEach(item =>
            {
                if (!collection.Any(predicate))
                    collection.Add(item);
            });

            return collection;
        }
    }
}
