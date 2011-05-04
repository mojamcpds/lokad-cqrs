#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;

namespace Lokad
{
    /// <summary>
    /// Attribute used to inform code coverage tool to ignore marked code block
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Struct,
        Inherited = false, AllowMultiple = false)]
    [NoCodeCoverage]
    public sealed class NoCodeCoverageAttribute : Attribute
    {
        /// <summary> Gets or sets the justification for removing 
        /// the member from the unit test code coverage. </summary>
        /// <value>The justification.</value>
        public string Justification { get; set; }
    }
}