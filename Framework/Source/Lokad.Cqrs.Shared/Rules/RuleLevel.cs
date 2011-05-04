#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

namespace Lokad.Rules
{
    /// <summary>
    /// Levels leveraged by the <see cref="Rule{T}"/> implementations
    /// </summary>
    public enum RuleLevel
    {
        /// <summary> Default value for the purposes of good citizenship </summary>
        None = 0,
        /// <summary> The rule raises a warning </summary>
        Warn,
        /// <summary> The rule raises an error </summary>
        Error
    }
}