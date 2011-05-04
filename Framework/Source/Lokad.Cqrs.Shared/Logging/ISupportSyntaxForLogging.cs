#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

namespace Lokad
{
    /// <summary>
    /// Syntax extensions for Logging configurations
    /// </summary>
    public interface ISupportSyntaxForLogging
    {
        /// <summary>
        /// Registers the specified log provider instance as singleton.
        /// </summary>
        /// <param name="provider">The provider.</param>
        void RegisterLogProvider(ILogProvider provider);
    }
}