#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;

namespace Lokad.Rules
{
    /// <summary>
    /// Rules for the <see cref="double"/>
    /// </summary>
    public static class DoubleIs
    {
        /// <summary>
        /// Checks if the specified double is valid.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="scope">The scope.</param>
        public static void Valid(double value, IScope scope)
        {
            if (Double.IsNaN(value) || Double.IsInfinity(value))
                scope.Error(RuleResources.Double_must_represent_valid_value);
        }
    }
}