﻿#region (c) 2010-2011 Lokad - CQRS for Windows Azure - New BSD License 

// Copyright (c) Lokad 2010-2011, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using Autofac;
using Lokad.Cqrs.Core.Serialization;
using Lokad.Cqrs.Core.Transport;
using Lokad.Cqrs.Envelope;
using Lokad.Cqrs.Serialization;

namespace Lokad.Cqrs.Build
{
	public sealed class AutofacBuilderForSerialization : AutofacBuilderBase
	{
		public AutofacBuilderForSerialization(ContainerBuilder builder) : base(builder)
		{
			builder.RegisterType<EnvelopeStreamer>().As<IEnvelopeStreamer>()
				.SingleInstance();
		}

		public void RegisterDataSerializer<TSerializer>() where TSerializer : IDataSerializer
		{
			Builder
				.RegisterType<TSerializer>()
				.As<IDataSerializer>()
				.SingleInstance();
		}

		public void RegisterEnvelopeSerializer<TEnvelopeSerializer>() where TEnvelopeSerializer : IEnvelopeSerializer
		{
			Builder
				.RegisterType<TEnvelopeSerializer>()
				.As<IEnvelopeSerializer>()
				.SingleInstance();
		}

		public void UseDataContractSerializer()
		{
			RegisterDataSerializer<DataSerializerWithDataContracts>();
			RegisterEnvelopeSerializer<EnvelopeSerializerWithDataContracts>();
		}

		public void AutoDetectSerializer()
		{
			RegisterDataSerializer<DataSerializerWithAutoDetection>();
			RegisterEnvelopeSerializer<EnvelopeSerializerWithProtoBuf>();
		}

		public void UseProtoBufSerialization()
		{
			RegisterDataSerializer<DataSerializerWithProtoBuf>();
			RegisterEnvelopeSerializer<EnvelopeSerializerWithProtoBuf>();
		}
	}
}