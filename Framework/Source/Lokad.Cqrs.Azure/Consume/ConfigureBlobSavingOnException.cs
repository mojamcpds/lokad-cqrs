#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Collections.Generic;
using Autofac;
using Microsoft.WindowsAzure;

namespace Lokad.Cqrs.Consume.Build
{
    public sealed class ConfigureBlobSavingOnException
    {
        public string ContainerName { get; set; }

        readonly IList<PrintMessageErrorDelegate> _printers = new List<PrintMessageErrorDelegate>();

        public void WithTextAppender(PrintMessageErrorDelegate @delegate)
        {
            _printers.Add(@delegate);
        }

        public ConfigureBlobSavingOnException()
        {
            ContainerName = "errors";
        }

        internal void Apply(IMessageTransport transport, IComponentContext context)
        {
            var account = context.Resolve<CloudStorageAccount>();
            var logger = new BlobExceptionLogger(account, ContainerName);

            foreach (var @delegate in _printers)
            {
                logger.OnRender += @delegate;
            }

            Action<UnpackedMessage, Exception> action = logger.Handle;
            transport.MessageHandlerFailed += action;

            context.WhenDisposed(() => { transport.MessageHandlerFailed -= action; });
        }
    }
}