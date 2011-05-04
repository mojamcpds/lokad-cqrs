#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using Autofac;

namespace Lokad.Cqrs
{
    public sealed class AutofacBuilderForSerialization : Syntax, ISupportSyntaxForSerialization
    {
        readonly ContainerBuilder _builder;

        public AutofacBuilderForSerialization(ContainerBuilder builder)
        {
            _builder = builder;
        }

        public void RegisterSerializer<TSerializer>() where TSerializer : IMessageSerializer
        {
            _builder
                .RegisterType<TSerializer>()
                .As<IMessageSerializer, IDataContractMapper, IDataSerializer>().SingleInstance();
        }
    }

    public sealed class AutofacBuilderForDomain : Syntax
    {
        ContainerBuilder _builder;

        public AutofacBuilderForDomain(ContainerBuilder builder)
        {
            _builder = builder;
        }
    }
}