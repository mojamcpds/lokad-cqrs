#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;

namespace Lokad
{
    /// <summary>
    /// Shortcut interface for <see cref="IProvider{TKey,TValue}"/> that uses <see cref="string"/> as the key.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    [CLSCompliant(true)]
    public interface INamedProvider<TValue> : IProvider<string, TValue>
    {
    }
}