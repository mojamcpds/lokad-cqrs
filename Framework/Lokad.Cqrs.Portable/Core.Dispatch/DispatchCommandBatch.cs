#region (c) 2010-2011 Lokad - CQRS for Windows Azure - New BSD License 

// Copyright (c) Lokad 2010-2011, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Collections.Generic;
using Lokad.Cqrs.Core.Directory;

namespace Lokad.Cqrs.Core.Dispatch
{
    /// <summary>
    /// Dispatch command batches to a single consumer. Uses sliding cache to 
    /// reduce message duplication
    /// </summary>
    public class DispatchCommandBatch : ISingleThreadMessageDispatcher
    {
        
        readonly IDictionary<Type, Type> _messageConsumers = new Dictionary<Type, Type>();
        readonly MessageActivationInfo[] _messageDirectory;
        readonly IMessageDispatchStrategy _strategy;

        public DispatchCommandBatch(MessageActivationInfo[] messageDirectory, IMessageDispatchStrategy strategy)
        {
            _messageDirectory = messageDirectory;
            _strategy = strategy;
        }

        void ISingleThreadMessageDispatcher.DispatchMessage(ImmutableEnvelope message)
        {
            // empty message, hm...
            if (message.Items.Length == 0)
                return;

            // verify that all consumers are available
            foreach (var item in message.Items)
            {
                if (!_messageConsumers.ContainsKey(item.MappedType))
                {
                    throw new InvalidOperationException("Couldn't find consumer for " + item.MappedType);
                }
            }

            using (var scope = _strategy.BeginEnvelopeScope())
            {
                foreach (var item in message.Items)
                {
                    var consumerType = _messageConsumers[item.MappedType];
                    scope.Dispatch(consumerType, message, item);
                }
                scope.Complete();
            }
        }

        public void Init()
        {
            DispatcherUtil.ThrowIfCommandHasMultipleConsumers(_messageDirectory);
            foreach (var messageInfo in _messageDirectory)
            {
                if (messageInfo.AllConsumers.Length > 0)
                {
                    _messageConsumers[messageInfo.MessageType] = messageInfo.AllConsumers[0];
                }
            }
        }
    }
}