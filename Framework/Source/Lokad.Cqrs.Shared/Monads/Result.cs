#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;

namespace Lokad
{
    /// <summary> Helper class for creating <see cref="Result{T}"/> instances </summary>
    [NoCodeCoverage]
    public static class Result
    {
        /// <summary> Creates success result </summary>
        /// <typeparam name="TValue">The type of the result.</typeparam>
        /// <param name="value">The item.</param>
        /// <returns>new result instance</returns>
        /// <seealso cref="Result{T}.CreateSuccess"/>
        [NotNull]
        public static Result<TValue> CreateSuccess<TValue>([NotNull] TValue value)
        {
            return Result<TValue>.CreateSuccess(value);
        }

        /// <summary> Creates success result </summary>
        /// <typeparam name="TValue">The type of the result.</typeparam>
        /// <param name="value">The item.</param>
        /// <returns>new result instance</returns>
        /// <seealso cref="Result{T}.CreateSuccess"/>
        [Obsolete("Use CreateSuccess instead")]
        public static Result<TValue> Success<TValue>([NotNull] TValue value)
        {
            return CreateSuccess(value);
        }
    }
}