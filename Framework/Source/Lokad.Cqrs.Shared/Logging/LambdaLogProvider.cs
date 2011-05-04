#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;

namespace Lokad
{
    /// <summary>
    /// Log provider, that uses lambda expression
    /// </summary>
    public sealed class LambdaLogProvider : ILogProvider
    {
        readonly Func<string, ILog> _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaLogProvider"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public LambdaLogProvider(Func<string, ILog> factory)
        {
            _factory = factory;
        }

        ILog IProvider<string, ILog>.Get(string key)
        {
            return _factory(key);
        }
    }
}