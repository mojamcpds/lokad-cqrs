#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Threading;
using System.Threading.Tasks;
using Lokad.Cqrs.Dispatch;
using Lokad.Cqrs.Durability;
using Lokad.Cqrs.Evil;
using Lokad.Cqrs.Extensions;
using Lokad.Cqrs.Logging;

namespace Lokad.Cqrs.Consume
{
	
	public sealed class ConsumingProcess : IEngineProcess
	{
		
		readonly IMessageDispatcher _dispatcher;
		readonly ILog _log;
		readonly AzureReadQueue[] _queues;
		readonly Func<uint, TimeSpan> _threadSleepInterval;


		public ConsumingProcess(ILogProvider logProvider, 
			IMessageDispatcher dispatcher, Func<uint, TimeSpan> sleepWhenNoMessages, AzureReadQueue[] readQueues)
		{
		
			_queues = readQueues;
			_dispatcher = dispatcher;
			_log = logProvider.Get(typeof (ConsumingProcess).Name + "." + readQueues.ToArray(q => q.Name).JoinStrings(","));
			_threadSleepInterval = sleepWhenNoMessages;
		}
		
		public void Dispose()
		{
			_disposal.Dispose();
		}

		public void Initialize()
		{
			foreach (var queue in _queues)
			{
				queue.Init();
			}
		}

		readonly CancellationTokenSource _disposal = new CancellationTokenSource();

		public Task Start(CancellationToken token)
		{
			_log.DebugFormat("Starting transport for {0}", _queues.ToArray(q=> q.Name).JoinStrings(";"));
			return Task.Factory.StartNew(() => ReceiveMessages(token), token);
		}
	

		Maybe<Exception> GetProcessingFailure(AzureReadQueue queue, MessageEnvelope message)
		{
			try
			{
				_dispatcher.DispatchMessage(message);
				return Maybe<Exception>.Empty;
			}
			catch (Exception ex)
			{
				var text = string.Format("Failed to consume '{0}' from '{1}'", message, queue.Name);
				_log.Error(ex, text);
				return ex;
			}
		}

		void MessageHandlingProblem(MessageEnvelope message, Exception ex)
		{
			// notify all subscribers
			// do nothing. Message will show up in the queue with the increased enqueue count.
		}

		static void FinalizeSuccess(AzureReadQueue queue, AzureMessageContext message)
		{
			queue.AckMessage(message);
			//tx.Complete();
		}

		void ReceiveMessages(CancellationToken outer)
		{
			uint beenIdleFor = 0;

			using (var source = CancellationTokenSource.CreateLinkedTokenSource(_disposal.Token, outer))
			{
				var token = source.Token;

				while (!token.IsCancellationRequested)
				{
					var messageFound = false;
					foreach (var messageQueue in _queues)
					{
						if (token.IsCancellationRequested)
							return;

						// selector policy goes here
						if (ProcessQueueForMessage(messageQueue) == QueueProcessingResult.MoreWork)
						{
							messageFound = true;
						}
					}

					if (messageFound)
					{
						beenIdleFor = 0;
					}
					else
					{
						beenIdleFor += 1;
						var sleepInterval = _threadSleepInterval(beenIdleFor);
						token.WaitHandle.WaitOne(sleepInterval);
					}
				}
			}
		}

		QueueProcessingResult ProcessQueueForMessage(AzureReadQueue queue)
		{
			var result = queue.GetMessage();

			switch (result.State)
			{
				case GetMessageResultState.Success:
					GetProcessingFailure(queue, result.Message.Unpacked)
						.Apply(ex => MessageHandlingProblem(result.Message.Unpacked, ex))
						.Handle(() => FinalizeSuccess(queue, result.Message));
					return QueueProcessingResult.MoreWork;

				case GetMessageResultState.Wait:
					return QueueProcessingResult.Sleep;

				case GetMessageResultState.Exception:
					_log.DebugFormat(result.Exception, "Exception, while trying to get message");
					return QueueProcessingResult.MoreWork;

				case GetMessageResultState.Retry:
					//tx.Complete();
					// retry immediately
					return QueueProcessingResult.MoreWork;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		enum QueueProcessingResult
		{
			MoreWork,
			Sleep
		}
	}
}