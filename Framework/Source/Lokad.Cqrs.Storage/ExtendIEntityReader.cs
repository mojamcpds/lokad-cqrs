#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

namespace Lokad.Cqrs
{
    /// <summary>
    /// Extends <see cref="IEntityReader"/>
    /// </summary>
    [UsedImplicitly]
    public static class ExtendIEntityReader
    {
        /// <summary>
        /// Retrieves the specified entity from the store, if it is found.
        /// Underlying storage could be event, cloud blob or whatever
        /// </summary>
        /// <typeparam name="T">type of the entity</typeparam>
        /// <param name="store">The store.</param>
        /// <param name="identity">The identity.</param>
        /// <returns>loaded entity (if found)</returns>
        public static Maybe<T> Read<T>(this IEntityReader store, object identity)
        {
            return store.Read(typeof (T), identity).Convert(o => (T) o);
        }
    }
}