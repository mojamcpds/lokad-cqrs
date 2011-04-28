#region (c) 2010-2011 Lokad - CQRS for Windows Azure - New BSD License 

// Copyright (c) Lokad 2010-2011, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Threading;
using Lokad.Cqrs.Core.Inbox;
using Lokad.Cqrs.Core.Transport;
using Lokad.Cqrs.Feature.AzurePartition.Events;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Lokad.Cqrs.Feature.AzurePartition
{
    public sealed class StatelessAzureQueueReader
    {
        readonly IEnvelopeStreamer _streamer;
        readonly TimeSpan _visibilityTimeout;
        readonly ISystemObserver _observer;

        readonly CloudBlobContainer _cloudBlob;
        readonly Lazy<CloudQueue> _posionQueue;
        readonly CloudQueue _queue;
        readonly string _queueName;

        public string Name
        {
            get { return _queueName; }
        }


        public StatelessAzureQueueReader(
            string name,
            CloudQueue primaryQueue,
            CloudBlobContainer container,
            Lazy<CloudQueue> poisonQueue,
            ISystemObserver provider,
            IEnvelopeStreamer streamer, TimeSpan visibilityTimeout)
        {
            _cloudBlob = container;
            _queue = primaryQueue;
            _posionQueue = poisonQueue;
            _observer = provider;
            _queueName = name;
            _streamer = streamer;
            _visibilityTimeout = visibilityTimeout;
        }

        public void SetupForTesting()
        {
            if (_queue.Exists())
            {
                _queue.Clear();
            }
            _cloudBlob.CreateIfNotExist();
            foreach (var blob in _cloudBlob.ListBlobs())
            {
                ((CloudBlockBlob)blob).DeleteIfExists();
            }
        }


        public void Initialize()
        {
            _queue.CreateIfNotExist();
            _cloudBlob.CreateIfNotExist();
            
        }

        public GetMessageResult TryGetMessage()
        {
            CloudQueueMessage message;
            try
            {
                message = _queue.GetMessage(_visibilityTimeout);
            }
            catch (Exception ex)
            {
                _observer.Notify(new FailedToReadMessage(ex, _queueName));
                return GetMessageResult.Error();
            }

            if (null == message)
            {
                return GetMessageResult.Empty;
            }
            
            try
            {
                var unpacked = DownloadPackage(message);
                return GetMessageResult.Success(unpacked);
            }
            catch (StorageClientException ex)
            {
                _observer.Notify(new FailedToAccessStorage(ex, _queue.Name, message.Id));
                return GetMessageResult.Retry;
            }
            catch (Exception ex)
            {
                _observer.Notify(new FailedToDeserializeMessage(ex, _queue.Name, message.Id));

                // new poison details
                _posionQueue.Value.AddMessage(message);
                _queue.DeleteMessage(message);
                return GetMessageResult.Retry;
            }
        }

        MessageContext DownloadPackage(CloudQueueMessage message)
        {
            var buffer = message.AsBytes;

            EnvelopeReference reference;
            if (_streamer.TryReadAsReference(buffer, out reference))
            {
                if (reference.StorageContainer != _cloudBlob.Uri.ToString())
                    throw new InvalidOperationException("Wrong container used!");
                var blob = _cloudBlob.GetBlobReference(reference.StorageReference);
                buffer = blob.DownloadByteArray();
            }

            var m = _streamer.ReadDataMessage(buffer);
            return new MessageContext(message, m, _queueName);
        }


        /// <summary>
        /// ACKs the message by deleting it from the queue.
        /// </summary>
        /// <param name="message">The message context to ACK.</param>
        public void AckMessage(MessageContext message)
        {
            if (message == null) throw new ArgumentNullException("message");
            _queue.DeleteMessage((CloudQueueMessage) message.TransportMessage);
        }

        public void TryNotifyNack(MessageContext context)
        {
            // we don't do anything. Azure queues have visibility
        }
    }
}