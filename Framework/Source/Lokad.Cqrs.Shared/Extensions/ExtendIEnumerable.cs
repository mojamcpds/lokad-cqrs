#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Lokad.Cqrs;

namespace Lokad
{
    /// <summary>
    /// Helper methods for the <see cref="IEnumerable{T}"/>
    /// </summary>
    public static class ExtendIEnumerable
    {
      

        /// <summary>
        /// Returns <em>True</em> as soon as the first member of <paramref name="enumerable"/>
        /// mathes <paramref name="predicate"/>
        /// </summary>
        /// <typeparam name="TSource">Type of the elements in <paramref name="enumerable"/></typeparam>
        /// <param name="enumerable">The enumerable</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>true if the <paramref name="enumerable"/> contains any elements
        /// matching <paramref name="predicate"/></returns>
        public static bool Exists<TSource>(this IEnumerable<TSource> enumerable, Predicate<TSource> predicate)
        {
            if (enumerable == null) throw new ArgumentNullException("enumerable");
            if (predicate == null) throw new ArgumentNullException("predicate");

            foreach (var t in enumerable)
            {
                if (predicate(t))
                {
                    return true;
                }
            }
            return false;
        }

     

    

      

#if !SILVERLIGHT2
        /// <summary>
        /// Converts the enumerable to <see cref="HashSet{T}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>hashset instance</returns>
        public static HashSet<T> ToSet<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null) throw new ArgumentNullException("enumerable");

            return new HashSet<T>(enumerable);
        }

        /// <summary>
        /// Converts the enumerable to <see cref="HashSet{T}"/>
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>hashset instance</returns>
        public static HashSet<TKey> ToSet<TKey, TItem>(this IEnumerable<TItem> enumerable, Func<TItem, TKey> selector)
        {
            if (enumerable == null) throw new ArgumentNullException("enumerable");
            if (selector == null) throw new ArgumentNullException("selector");

            return new HashSet<TKey>(enumerable.Select(selector));
        }
#endif

      


        /// <summary>
        /// Concatenates a specified separator between each element of a specified <paramref name="strings"/>, 
        /// yielding a single concatenated string.
        /// </summary>
        /// <param name="strings">The strings.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>concatenated string</returns>
        public static string Join(this IEnumerable<string> strings, string separator)
        {
            if (strings == null) throw new ArgumentNullException("strings");

            return string.Join(separator, strings.ToArray());
        }


        /// <summary>
        /// Shorthand extension method for converting enumerables into the arrays
        /// </summary>
        /// <typeparam name="TSource">The type of the source array.</typeparam>
        /// <typeparam name="TTarget">The type of the target array.</typeparam>
        /// <param name="self">The collection to convert.</param>
        /// <param name="converter">The converter.</param>
        /// <returns>target array instance</returns>
        public static TTarget[] ToArray<TSource, TTarget>(this IEnumerable<TSource> self,
            Func<TSource, TTarget> converter)
        {
            if (self == null) throw new ArgumentNullException("self");
            if (converter == null) throw new ArgumentNullException("converter");

            return self.Select(converter).ToArray();
        }


        /// <summary>
        /// Retrieves first value from the <paramref name="sequence"/>
        /// </summary>
        /// <typeparam name="TSource">The type of the source sequence.</typeparam>
        /// <param name="sequence">The source.</param>
        /// <returns>first value or empty result, if it is not found</returns>
        public static Maybe<TSource> FirstOrEmpty<TSource>([NotNull] this IEnumerable<TSource> sequence)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");
            foreach (var source in sequence)
            {
                return source;
            }
            return Maybe<TSource>.Empty;
        }
    }
}