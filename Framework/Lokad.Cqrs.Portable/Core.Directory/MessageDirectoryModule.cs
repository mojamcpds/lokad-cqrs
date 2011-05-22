#region (c) 2010-2011 Lokad - CQRS for Windows Azure - New BSD License 

// Copyright (c) Lokad 2010-2011, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Transactions;
using Autofac;
using Autofac.Core;
using Lokad.Cqrs.Core.Directory.Default;
using Lokad.Cqrs.Core.Dispatch;
using Lokad.Cqrs.Evil;
using System.Linq;

namespace Lokad.Cqrs.Core.Directory
{
    /// <summary>
    /// Module for building CQRS domains.
    /// </summary>
    public class MessageDirectoryModule : HideObjectMembersFromIntelliSense
    {
        readonly DomainAssemblyScanner _scanner = new DomainAssemblyScanner();
        IMethodContextManager _contextManager;
        MethodInvokerHint _hint;
        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDirectoryModule"/> class.
        /// </summary>
        public MessageDirectoryModule()
        {
            HandlerSample<IConsume<IMessage>>(a => a.Consume(null));
            ContextFactory((envelope, message) => new MessageContext(envelope.EnvelopeId, message.Index, envelope.CreatedOnUtc));
        }


        public void ContextFactory<TContext>(Func<ImmutableEnvelope,ImmutableMessage, TContext> manager)
            where TContext : class 
        {
            var instance = new MethodContextManager<TContext>(manager);
            _contextManager = instance;
        }

		public void HandlerSample<THandler>(Expression<Action<THandler>> action)
		{
		    if (action == null) throw new ArgumentNullException("action");
		    _hint = MethodInvokerHint.FromConsumerSample(action);
		}


        /// <summary>
        /// Includes assemblies of the specified types into the discovery process
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>same module instance for chaining fluent configurations</returns>
        public MessageDirectoryModule InAssemblyOf<T>()
        {
            _scanner.WithAssemblyOf<T>();
            return this;
        }


        public void Configure(IComponentRegistry container, ICollection<Type> types)
        {
            _scanner.Constrain(_hint);
            var mappings = _scanner.Build(_hint.ConsumerTypeDefinition);


            var messageTypes = mappings
                .Select(m => m.Message)
                .Where(m => !m.IsAbstract)
                .Distinct();

            foreach (var messageType in messageTypes)
            {
                types.Add(messageType);
            }
            
            var builder = new MessageDirectoryBuilder(mappings);
            
            var provider = _contextManager.GetContextProvider();

            var consumers = mappings
                .Select(x => x.Consumer)
                .Where(x => !x.IsAbstract)
                .Distinct()
                .ToArray();

            var cb = new ContainerBuilder();
            foreach (var consumer in consumers)
            {
                cb.RegisterType(consumer);
            }
            cb.RegisterInstance(provider).AsSelf();
            cb.Update(container);
            container.Register<IMessageDispatchStrategy>(c =>
            {
                var scope = c.Resolve<ILifetimeScope>();
                var tx = TransactionEvil.Factory(TransactionScopeOption.RequiresNew);
                return new AutofacDispatchStrategy(scope, tx, _hint.Lookup, _contextManager);
            });

            container.Register(builder);
            
        }
    }
}