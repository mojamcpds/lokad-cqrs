#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lokad
{
    /// <summary>
    /// Helper class allows to follow the principles defined by Microsoft P&amp;P team.
    /// </summary>
    public static class Enforce
    {
        // refactored methods go here.


        /// <summary>
        /// <para>Throws exception if the provided object is null. </para>
        /// <code>Enforce.Argument(() => args);</code> 
        /// </summary>
        /// <typeparam name="TValue">type of the class to check</typeparam>
        /// <param name="argumentReference">The argument reference to check.</param>
        /// <exception cref="ArgumentNullException">If the class reference is null.</exception>
        /// <remarks>Silverlight 2.0 does not support fast resolution of variable names, yet</remarks>
        [DebuggerNonUserCode]
        public static void Argument<TValue>(Func<TValue> argumentReference) where TValue : class
        {
            if (null == argumentReference())
                throw Errors.ArgumentNull(argumentReference);
        }

        /// <summary>
        /// 	<para>Throws exception if one of the provided objects is null. </para>
        /// 	<code>Enforce.Arguments(() =&gt; controller, () =&gt; service);</code>
        /// </summary>
        /// <param name="first">The first argument to check for</param>
        /// <param name="second">The second argument to check for.</param>
        /// <remarks>Silverlight 2.0 does not support fast resolution of variable names, yet</remarks>
        [DebuggerNonUserCode]
        public static void Arguments<T1, T2>(Func<T1> first, Func<T2> second)
            where T1 : class
            where T2 : class

        {
            if (null == first())
                throw Errors.ArgumentNull(first);

            if (null == second())
                throw Errors.ArgumentNull(second);
        }


        /// <summary>
        /// Throws proper exception if the provided string argument is null or empty. 
        /// </summary>
        /// <returns>Original string.</returns>
        /// <exception cref="ArgumentException">If the string argument is null or empty.</exception>
        /// <remarks>Silverlight 2.0 does not support fast resolution of variable names, yet</remarks>
        [DebuggerNonUserCode]
        [AssertionMethod]
        public static void ArgumentNotEmpty(Func<string> argumentReference)
        {
            var value = argumentReference();
            if (null == value)
                throw Errors.ArgumentNull(argumentReference);

            if (0 == value.Length)
                throw Errors.Argument(argumentReference, "String can't be empty");
        }


        /// <summary>
        /// Throws exception if the check does not pass.
        /// </summary>
        /// <param name="check">if set to <c>true</c> then check will pass.</param>
        /// <param name="name">The name of the assertion.</param>
        /// <exception cref="InvalidOperationException">If the assertion has failed.</exception>
        [DebuggerNonUserCode]
        [AssertionMethod]
        public static void That(
            [AssertionCondition(AssertionConditionType.IS_TRUE)] bool check, [NotNull] string name)
        {
            if (!check)
            {
                throw Errors.InvalidOperation("Failed assertion '{0}'", name);
            }
        }
    }
}