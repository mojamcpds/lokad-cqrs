#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Runtime.Serialization;

namespace Lokad.Cqrs
{
    [Serializable]
    public class StorageContainerNotFoundException : StorageBaseException
    {
        public StorageContainerNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected StorageContainerNotFoundException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}