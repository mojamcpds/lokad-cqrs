#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using Autofac;

namespace Lokad.Cqrs
{
    public sealed class AutofacBuilderForLogging : Syntax, ISupportSyntaxForLogging
    {
        readonly ContainerBuilder _builder;

        public AutofacBuilderForLogging(ContainerBuilder builder)
        {
            _builder = builder;
        }

        public void RegisterLogProvider(ILogProvider provider)
        {
            _builder.RegisterInstance(provider);
        }
    }
}