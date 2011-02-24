﻿using System;

namespace Lokad.Cqrs.Logging
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

		public ILog Get(string key)
		{
			return _factory(key);
		}
	}
}