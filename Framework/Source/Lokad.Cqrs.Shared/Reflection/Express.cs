#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Lokad.Reflection
{
    /// <summary>
    /// Helper class for the Expression-based strongly-typed reflection
    /// </summary>
    public static class Express
    {
        /// <summary>
        /// Gets the <see cref="MethodInfo"/> 
        /// from the provided <paramref name="method"/>.
        /// </summary>
        /// <param name="method">The method expression.</param>
        /// <returns>method information</returns>
        public static MethodInfo MethodWithLambda(LambdaExpression method)
        {
            if (method == null) throw new ArgumentNullException("method");
            if (method.Body.NodeType != ExpressionType.Call)
                throw new ArgumentException("Expression should be of call type");

            return ((MethodCallExpression) method.Body).Method;
        }
    }

    /// <summary>
    /// Helper class for the Expression-based strongly-typed reflection
    /// </summary>
    public static class Express<TTarget>
    {
        /// <summary> Gets the <see cref="MethodInfo"/> from 
        /// the provided <paramref name="method"/> expression. </summary>
        /// <param name="method">The expression.</param>
        /// <returns>method information</returns>
        /// <seealso cref="Express.MethodWithLambda"/>
        public static MethodInfo Method(Expression<Action<TTarget>> method)
        {
            return Express.MethodWithLambda(method);
        }
    }
}