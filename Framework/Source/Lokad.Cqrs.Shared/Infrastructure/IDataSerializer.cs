#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.IO;

namespace Lokad
{
    /// <summary>
    /// Generic data serializer interface.
    /// </summary>
    public interface IDataSerializer
    {
        /// <summary>
        /// Serializes the object to the specified stream
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="destinationStream">The destination stream.</param>
        void Serialize(object instance, Stream destinationStream);

        /// <summary>
        /// Deserializes the object from specified source stream.
        /// </summary>
        /// <param name="sourceStream">The source stream.</param>
        /// <param name="type">The type of the object to deserialize.</param>
        /// <returns>deserialized object</returns>
        object Deserialize(Stream sourceStream, Type type);
    }
}