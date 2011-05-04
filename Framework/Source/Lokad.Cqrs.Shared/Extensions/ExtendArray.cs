#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;

namespace Lokad
{
    /// <summary>
    /// Shortcuts for some common array operations
    /// </summary>
    public static class ExtendArray
    {
        /// <summary>
        /// Shorthand extension method for converting the arrays
        /// </summary>
        /// <typeparam name="TSource">The type of the source array.</typeparam>
        /// <typeparam name="TTarget">The type of the target array.</typeparam>
        /// <param name="source">The array to convert.</param>
        /// <param name="converter">The converter.</param>
        /// <returns>target array instance</returns>
        public static TTarget[] Convert<TSource, TTarget>(this TSource[] source, Converter<TSource, TTarget> converter)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (converter == null) throw new ArgumentNullException("converter");

            var outputArray = new TTarget[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                outputArray[i] = converter(source[i]);
            }
            return outputArray;
        }
    }
}