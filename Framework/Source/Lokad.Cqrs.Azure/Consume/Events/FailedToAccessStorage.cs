﻿using Microsoft.WindowsAzure.StorageClient;

namespace Lokad.Cqrs.Consume
{
	public sealed class FailedToAccessStorage : ISystemEvent
	{
		public StorageClientException Exception { get; private set; }
		public string QueueName { get; private set; }
		public string MessageId { get; private set; }

		public FailedToAccessStorage(StorageClientException exception, string queueName, string messageId)
		{
			Exception = exception;
			QueueName = queueName;
			MessageId = messageId;
		}
	}
}