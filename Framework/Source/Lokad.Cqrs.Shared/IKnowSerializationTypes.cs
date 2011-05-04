#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Collections.Generic;

namespace Lokad
{
    /// <summary>
    /// Provides collection of known serialization types (for prebuilt serializers)
    /// </summary>
    public interface IKnowSerializationTypes
    {
        /// <summary>
        /// Gets the known serialization types.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Type> GetKnownTypes();
    }
}