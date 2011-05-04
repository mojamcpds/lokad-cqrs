#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;

namespace Lokad
{
    /// <summary>
    /// Indicates that the marked method is assertion method, i.e. it halts control flow if one of the conditions is satisfied. 
    /// To set the condition, mark one of the parameters with <see cref="AssertionConditionAttribute"/> attribute
    /// </summary>
    /// <seealso cref="AssertionConditionAttribute"/>
    /// <remarks>This attribute helps R# in code analysis</remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    [NoCodeCoverage]
    public sealed class AssertionMethodAttribute : Attribute
    {
    }
}