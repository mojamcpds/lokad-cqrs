#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using Lokad.Cqrs;

namespace Lokad
{
    /// <summary>
    /// Helper class that allows to implement non-deterministic 
    /// reproducible testing.
    /// </summary>
    /// <remarks>
    /// Keep in mind, that this implementation is not thread-safe.
    /// </remarks>
    public static partial class Rand
    {
        static Func<int, int> NextInt;
        static Func<Func<int, int>> Activator;

        /// <summary>
        /// Resets everything to the default, using <see cref="Random"/> generator and random seed. 
        /// </summary>
        public static void ResetToDefault()
        {
            ResetToDefault(new Random().Next());
        }

        /// <summary>
        /// Resets everything to the default, using <see cref="Random"/> generator and the specified
        /// rand seed.
        /// </summary>
        /// <param name="randSeed">The rand seed.</param>
        [UsedImplicitly]
        public static void ResetToDefault(int randSeed)
        {
            Activator = () =>
                {
                    var r = new Random(randSeed);
                    return i => r.Next(i);
                };
            NextInt = Activator();
        }

        static Rand()
        {
            ResetToDefault();
        }

      
        /// <summary>
        /// Generates random value between 0 and <paramref name="upperBound"/> (exclusive)
        /// </summary>
        /// <param name="upperBound">The upper bound.</param>
        /// <returns>random integer</returns>
        public static int Next(int upperBound)
        {
            return NextInt(upperBound);
        }

        /// <summary>
        /// Generates random value between <paramref name="lowerBound"/>
        /// and <paramref name="upperBound"/> (exclusive)
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        /// <returns>random integer</returns>
        public static int Next(int lowerBound, int upperBound)
        {
            var range = upperBound - lowerBound;
            return NextInt(range) + lowerBound;
        }

        /// <summary> Picks random item from the provided array </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <param name="items">The items.</param>
        /// <returns>random item from the array</returns>
        public static TItem NextItem<TItem>(TItem[] items)
        {
            var index = NextInt(items.Length);
            return items[index];
        }


        /// <summary>
        /// Returns <em>true</em> with the specified probability.
        /// </summary>
        /// <param name="probability">The probability (between 0 and 1).</param>
        /// <returns><em>true</em> with the specified probability</returns>
        public static bool NextBool(double probability)
        {
            return NextDouble() < probability;
        }

  

        /// <summary> Returns random double value with lowered precision </summary>
        /// <returns>random double value</returns>
        public static double NextDouble()
        {
            return (double) NextInt(int.MaxValue)/int.MaxValue;
        }

    }
}