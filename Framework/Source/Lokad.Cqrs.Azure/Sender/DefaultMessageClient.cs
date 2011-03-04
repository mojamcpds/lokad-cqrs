﻿#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;

namespace Lokad.Cqrs.Sender
{
	sealed class DefaultMessageClient : IMessageClient
	{
		readonly AzureWriteQueue _queue;

		public DefaultMessageClient(AzureWriteQueue queue)
		{
			_queue = queue;
		}

		public void Send(params object[] messages)
		{
			if (messages.Length == 0)
				return;

			foreach (var message in messages)
			{
				_queue.AddAsSingleMessage(new[] { message});
			}
		}

		public void SendAsBatch(params object[] messageItems)
		{
			if (messageItems.Length == 0)
				return;

			_queue.AddAsSingleMessage(messageItems);
		}
	}
}