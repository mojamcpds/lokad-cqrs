#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace Lokad
{
    /// <summary>
    /// Helper class with shortcut methods for managing enumerations.
    /// Useful for inlining object generation in tests
    /// </summary>
    public static class Range
    {
        /// <summary>
        /// Creates the array populated with the provided generator
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="count">The count.</param>
        /// <param name="generator">The generator.</param>
        /// <returns>array</returns>
        public static TValue[] Array<TValue>(int count, Func<int, TValue> generator)
        {
            if (generator == null) throw new ArgumentNullException("generator");

            var array = new TValue[count];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = generator(i);
            }

            return array;
        }
    }
}