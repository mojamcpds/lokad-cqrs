#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using Autofac;
using Lokad.Cqrs.Core.Outbox;
using Lokad.Cqrs.Core.Transport;
using Lokad.Cqrs.Feature.AzurePartition.Sender;
using Lokad.Cqrs.Feature.MemoryPartition;

namespace Lokad.Cqrs.Build.Client
{
	/// <summary>
	/// Fluent API for creating and configuring <see cref="CloudClient"/>
	/// </summary>
// ReSharper disable UnusedMember.Global
	public sealed class CloudClientBuilder : AutofacBuilderBase
	{
		public CloudClientBuilder()
		{
			Azure.UseDevelopmentStorageAccount();
			Serialization.AutoDetectSerializer();
			Logging.LogToTrace();

			Builder.RegisterType<AzureWriteQueueFactory>().As<IQueueWriterFactory>().SingleInstance();
			Builder.RegisterType<MemoryPartitionFactory>().As<IQueueWriterFactory>().SingleInstance();
			

			Builder.RegisterType<CloudClient>().SingleInstance();
		}


		public AutofacBuilderForLogging Logging
		{
			get { return new AutofacBuilderForLogging(Builder); }
		}

		public AutofacBuilderForSerialization Serialization
		{
			get { return new AutofacBuilderForSerialization(Builder); }
		}

		public AutofacBuilderForAzure Azure
		{
			get { return new AutofacBuilderForAzure(Builder); }
		}

		

		/// <summary>
		/// Creates default message sender for the instance of <see cref="CloudClient"/>
		/// </summary>
		/// <returns>same builder for inline multiple configuration statements</returns>
		public CloudClientBuilder AddMessageClient(string queueName)
		{
			Builder.RegisterModule(new SendMessageModule(queueName));
			return this;
		}

		/// <summary>
		/// Configures the message domain for the instance of <see cref="CloudClient"/>.
		/// </summary>
		/// <param name="config">configuration syntax.</param>
		/// <returns>same builder for inline multiple configuration statements</returns>
		public CloudClientBuilder Domain(Action<DomainBuildModule> config)
		{
			RegisterModule(config);
			return this;
		}
		
		public CloudClient Build()
		{
			return Builder.Build().Resolve<CloudClient>();
		}
	}
}