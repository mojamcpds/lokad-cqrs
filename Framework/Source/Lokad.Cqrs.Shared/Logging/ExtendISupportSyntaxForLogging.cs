#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

namespace Lokad
{
    /// <summary>
    /// Extends logging syntax
    /// </summary>
    public static class ExtendISupportSyntaxForLogging
    {
        /// <summary>
        /// Registers the <see cref="TraceLog"/>
        /// </summary>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        /// <param name="module">The module.</param>
        /// <returns>same module for the inlining</returns>
        [UsedImplicitly]
        public static TModule LogToTrace<TModule>(this TModule module)
            where TModule : ISupportSyntaxForLogging
        {
            module.RegisterLogProvider(TraceLog.Provider);
            return module;
        }

        /// <summary>
        /// Registers the <see cref="NullLog"/>
        /// </summary>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        /// <param name="module">The module.</param>
        /// <returns>same module for the inlining</returns>
        [UsedImplicitly]
        public static TModule LogToNull<TModule>(this TModule module)
            where TModule : ISupportSyntaxForLogging
        {
            module.RegisterLogProvider(TraceLog.Provider);
            return module;
        }
    }
}