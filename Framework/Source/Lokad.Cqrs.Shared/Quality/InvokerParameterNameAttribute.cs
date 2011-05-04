#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;

namespace Lokad
{
    /// <summary>
    /// Indicates that the function argument should be string literal and match one  of the parameters of the caller function.
    /// For example, <see cref="ArgumentNullException"/> has such parameter.
    /// </summary>
    /// <remarks>This attribute helps R# in code analysis</remarks>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    [NoCodeCoverage]
    public sealed class InvokerParameterNameAttribute : Attribute
    {
    }
}