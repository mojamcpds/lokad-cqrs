#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Collections.Generic;

namespace Lokad
{
    /// <summary>
    /// Simple helper extensions for <see cref="ICollection{T}"/>
    /// </summary>
    public static class ExtendICollection
    {
        /// <summary>
        /// Adds all items to the target collection
        /// </summary>
        /// <typeparam name="T">type of the item within the collection</typeparam>
        /// <param name="collection">The collection</param>
        /// <param name="items">items to add to the collection</param>
        /// <returns>same collection instance</returns>
        public static ICollection<T> AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            if (items == null) throw new ArgumentNullException("items");

            foreach (var item in items)
            {
                collection.Add(item);
            }
            return collection;
        }
    }
}