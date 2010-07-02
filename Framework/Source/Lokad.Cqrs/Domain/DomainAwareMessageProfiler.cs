﻿#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Collections.Generic;
using Lokad.Cqrs.Queue;
using Lokad.Quality;

namespace Lokad.Cqrs.Domain
{
	[UsedImplicitly]
	public sealed class DomainAwareMessageProfiler : IMessageProfiler
	{
		readonly IDictionary<Type, GetInfoDelegate> _delegates;

		public DomainAwareMessageProfiler(IMessageDirectory directory)
		{
			_delegates = BuildFrom(directory);
		}


		public string GetReadableMessageInfo(UnpackedMessage message)
		{
			GetInfoDelegate value;
			var type = message.GetType();
			if (_delegates.TryGetValue(type, out value))
			{
				return value(message);
			}
			return type.Name + " - " + message.TransportMessageId;
		}

		static IDictionary<Type, GetInfoDelegate> BuildFrom(IMessageDirectory directory)
		{
			var delegates = new Dictionary<Type, GetInfoDelegate>();
			foreach (var message in directory.Messages)
			{
				if (message.MessageType.IsInterface)
					continue;

				var type = message.MessageType;
				var hasStringOverride = type.GetMethod("ToString").DeclaringType != typeof (object);

				if (hasStringOverride)
				{
					delegates.Add(type, m => m.Content.ToString());
				}
				else
				{
					delegates.Add(type, m => type.Name + " - " + m.TransportMessageId);
				}
			}
			return delegates;
		}

		delegate string GetInfoDelegate(UnpackedMessage message);
	}
}