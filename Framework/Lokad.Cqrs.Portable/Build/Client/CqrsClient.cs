﻿using System;
using Autofac;
using Lokad.Cqrs.Core.Envelope;

namespace Lokad.Cqrs.Build.Engine
{
    public sealed class CqrsClient 
    {
        readonly ILifetimeScope _scope;


        public CqrsClient(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public IMessageSender Sender
        {
            get
            {
                IMessageSender sender;
                if(_scope.TryResolve(out sender))
                {
                    return sender;
                }
                var message = string.Format("Failed to discover default {0}, have you added it to the system?", typeof(IMessageSender).Name);
                throw new InvalidOperationException(message);
            }
        }
    }
}