#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;

namespace Lokad
{
    /// <summary>
    /// Extension methods for the <see cref="INamedProvider{TValue}"/>
    /// of <see cref="ILog"/>
    /// </summary>
    [NoCodeCoverage, UsedImplicitly]
    public static class ExtendILogProvider
    {
        /// <summary>
        /// Creates new log using the type as name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [Obsolete("Obsolete, Use LogForType overrides to specify log name precisely")]
        public static ILog CreateLog<T>(this INamedProvider<ILog> logProvider) where T : class
        {
            return logProvider.Get(typeof (T).Name);
        }

        /// <summary>
        /// Creates new log with using <see cref="Type.FullName"/> for the name.
        /// </summary>
        /// <typeparam name="T">type to use for naming</typeparam>
        /// <param name="logProvider">The log provider.</param>
        /// <returns>new instance of the log</returns>
        public static ILog LogForType<T>(this ILogProvider logProvider)
        {
            return logProvider.Get(typeof (T).FullName);
        }

        /// <summary>
        /// Creates new log with using <see cref="Type.FullName"/> for the name.
        /// </summary>
        /// <param name="logProvider">The log provider.</param>
        /// <param name="instance">The instance to retrieve type for naming from.</param>
        /// <returns>new instance of the log</returns>
        public static ILog LogForType(this ILogProvider logProvider, object instance)
        {
            return logProvider.Get(instance.GetType().FullName);
        }

        /// <summary>
        /// Creates new log with named with class name.
        /// </summary>
        /// <typeparam name="T">type to use for naming</typeparam>
        /// <param name="logProvider">The log provider.</param>
        /// <returns>new instance of the log</returns>
        public static ILog LogForName<T>(this ILogProvider logProvider)
        {
            return logProvider.Get(typeof (T).Name);
        }

        /// <summary>
        /// Creates new log with named with the class name.
        /// </summary>
        /// <param name="logProvider">The log provider.</param>
        /// <param name="instance">The instance to retrieve type for naming from.</param>
        /// <returns>new instance of the log</returns>
        public static ILog LogForName(this ILogProvider logProvider, object instance)
        {
            return logProvider.Get(instance.GetType().Name);
        }

        /// <summary>
        /// Creates new log with using <see cref="Type.Namespace"/> for the name.
        /// </summary>
        /// <typeparam name="T">type to use for naming</typeparam>
        /// <param name="logProvider">The log provider.</param>
        /// <returns>new instance of the log</returns>
        public static ILog LogForNamespace<T>(this ILogProvider logProvider)
        {
            return logProvider.Get(typeof (T).Namespace);
        }

        /// <summary>
        /// Creates new log with using <see cref="Type.Namespace"/> for the name.
        /// </summary>
        /// <param name="logProvider">The log provider.</param>
        /// <param name="instance">The instance to retrieve type for naming from.</param>
        /// <returns>new instance of the log</returns>
        public static ILog LogForNamespace(this ILogProvider logProvider, object instance)
        {
            return logProvider.Get(instance.GetType().Namespace);
        }
    }
}