#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;

namespace Lokad
{
    /// <summary>
    /// Interface that abstracts away providers
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <remarks>
    /// things like IDataCache (from the Database layers) or IResolver (from the IoC layers) 
    /// are just samples of this interface
    /// </remarks>
    [CLSCompliant(true)]
    public interface IProvider<TKey, TValue>
    {
        /// <summary>
        /// Retrieves <typeparamref name="TValue"/> given the
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ResolutionException">when the key can not be resolved</exception>
        TValue Get(TKey key);
    }
}