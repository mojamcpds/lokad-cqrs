#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;

namespace Lokad
{
    /// <summary>
    /// This class provides way to create providers out of lambda shortcuts
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public sealed class NamedProvider<T> : IProvider<string, T>, INamedProvider<T>
    {
        readonly Func<string, T> _resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedProvider{T}"/> class.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        public NamedProvider(Func<string, T> resolver)
        {
            _resolver = resolver;
        }

        /// <summary>
        /// Retrieves <typeparamref name="T"/> given the <paramref name="key"/>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ResolutionException">when the key cannot be resolved</exception>
        public T Get(string key)
        {
            try
            {
                return _resolver(key);
            }
            catch (Exception ex)
            {
                throw Errors.Resolution(typeof (T), key, ex);
            }
        }
    }

    /// <summary>
    /// Shortcuts for <see cref="NamedProvider{T}"/>
    /// </summary>
    [NoCodeCoverage]
    public static class NamedProvider
    {
        /// <summary>
        /// Creates new instance of the <see cref="INamedProvider{TValue}"/> out of
        /// the provider function (shortcut syntax)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="providerFunction">The provider function.</param>
        /// <returns></returns>
        public static INamedProvider<T> For<T>(Func<string, T> providerFunction)
        {
            if (providerFunction == null) throw new ArgumentNullException("providerFunction");

            return new NamedProvider<T>(providerFunction);
        }
    }
}