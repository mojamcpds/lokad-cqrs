#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;

namespace Lokad
{
    /// <summary>
    /// Indicates that marked method builds string by format pattern and (optional) arguments. 
    /// Parameter, which contains format string, should be given in constructor.
    /// The format string should be in <see cref="string.Format(IFormatProvider,string,object[])"/> -like form
    /// </summary>
    /// <remarks>
    /// This attribute helps R# in code analysis
    /// </remarks>
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    [NoCodeCoverage]
    public sealed class StringFormatMethodAttribute : Attribute
    {
        readonly string _formatParameterName;

        /// <summary>
        /// Initializes new instance of StringFormatMethodAttribute
        /// </summary>
        /// <param name="formatParameterName">Specifies which parameter of an annotated method should be treated as format-string</param>
        public StringFormatMethodAttribute(string formatParameterName)
        {
            _formatParameterName = formatParameterName;
        }

        /// <summary>
        /// Gets format parameter name
        /// </summary>
        public string FormatParameterName
        {
            get { return _formatParameterName; }
        }
    }
}