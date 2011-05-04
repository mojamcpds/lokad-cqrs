#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Runtime.Serialization;

namespace Lokad.Reflection
{
    /// <summary>
    /// Exception thrown, when <see cref="Reflect"/> fails to parse some lambda
    /// </summary>
    [Serializable]
    public sealed class ReflectLambdaException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectLambdaException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ReflectLambdaException(string message) : base(message)
        {
        }

#if !SILVERLIGHT2


        ReflectLambdaException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

#endif
    }
}