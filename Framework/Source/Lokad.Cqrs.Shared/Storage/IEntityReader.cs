#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using Lokad.Cqrs;

namespace Lokad
{
    /// <summary>
    /// Handles read-side operations for the entity storage
    /// </summary>
    public interface IEntityReader
    {
        /// <summary>
        /// Retrieves the specified entity from the store, if it is found.
        /// Underlying storage could be event, cloud blob or whatever
        /// </summary>
        /// <param name="type">The type of the state (needed to deserialize).</param>
        /// <param name="identity">The identity.</param>
        /// <returns>loaded entity (if found)</returns>
        Maybe<object> Read(Type type, object identity);
    }
}