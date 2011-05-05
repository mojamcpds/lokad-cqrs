#region (c) 2010-2011 Lokad - CQRS for Windows Azure - New BSD License 

// Copyright (c) Lokad 2010-2011, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Transactions;
using Autofac;
using Lokad.Cqrs.Core.Directory;
using Lokad.Cqrs.Core.Dispatch.Events;

namespace Lokad.Cqrs.Core.Dispatch
{
    ///<summary>
    /// Dispatcher that sends a single event to multiple consumers within this worker.
    /// No transactions are used here, we keep track of duplication.
    ///</summary>
    public sealed class DispatchOneEvent : ISingleThreadMessageDispatcher
    {
        readonly ILifetimeScope _container;
        readonly MessageActivationMap _directory;
        readonly IDictionary<Type, Type[]> _dispatcher = new Dictionary<Type, Type[]>();
        readonly ISystemObserver _observer;
        readonly IMethodInvoker _invoker;
        Func<TransactionScope> _scopeFactory;

        public DispatchOneEvent(
            ILifetimeScope container, 
            MessageActivationMap directory, 
            ISystemObserver observer, 
            IMethodInvoker invoker)
        {
            _container = container;
            _invoker = invoker;
            _observer = observer;
            _directory = directory;
            Transactional(TransactionScopeOption.RequiresNew);
        }


        public void NoTransactions()
        {
            Transactional(TransactionScopeOption.Suppress);
        }

        public void Transactional(Func<TransactionScope> factory)
        {
            _scopeFactory = factory;
        }
        public void Transactional(TransactionScopeOption option, IsolationLevel level = IsolationLevel.Serializable, TimeSpan timeout = default(TimeSpan))
        {
            if (timeout == (default(TimeSpan)))
            {
                timeout = TimeSpan.FromMinutes(10);
            }
            _scopeFactory = () => new TransactionScope(option, new TransactionOptions()
            {
                IsolationLevel = level,
                Timeout = Debugger.IsAttached ? TimeSpan.MaxValue : timeout
            });
        }

        public void Init()
        {
            foreach (var message in _directory.Infos)
            {
                if (message.AllConsumers.Length > 0)
                {
                    _dispatcher.Add(message.MessageType, message.AllConsumers);
                }
            }
        }

        public void DispatchMessage(ImmutableEnvelope envelope)
        {
            if (envelope.Items.Length != 1)
                throw new InvalidOperationException(
                    "Batch message arrived to the shared scope. Are you batching events or dispatching commands to shared scope?");

            using (var tx = _scopeFactory())
            {
                DispatchEnvelope(envelope);
                tx.Complete();
            }
        }

        void DispatchEnvelope(ImmutableEnvelope envelope) 
        {
            // we get failure if one of the subscribers fails
            // hence, if any of the handlers fail - we give up

            var item = envelope.Items[0];
            Type[] consumerTypes;
            if (_dispatcher.TryGetValue(item.MappedType, out consumerTypes))
            {
                using (var unit = _container.BeginLifetimeScope(DispatchLifetimeScopeTags.MessageEnvelopeScopeTag))
                {
                    foreach (var consumerType in consumerTypes)
                    {
                        using (var scope = unit.BeginLifetimeScope(DispatchLifetimeScopeTags.MessageItemScopeTag))
                        {
                            var consumer = scope.Resolve(consumerType);
                            _invoker.InvokeConsume(consumer, item, envelope);
                            
                        }
                    }
                }
            }
            else
            {
                // else -> we don't have consumers. It's OK for the event
                _observer.Notify(new EventHadNoConsumers(envelope.EnvelopeId, item.MappedType));
            }
        }
    }
}