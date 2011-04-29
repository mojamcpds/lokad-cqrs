﻿using System;
using NUnit.Framework;

namespace Lokad.Cqrs.Core.Envelope
{
    // ReSharper disable UnusedMember.Global
    // ReSharper disable InconsistentNaming
    // ReSharper disable DoNotCallOverridableMethodsInConstructor

    public abstract class When_envelope_is_serialized
    {
        protected abstract ImmutableMessageEnvelope RoundtripViaSerializer(MessageEnvelopeBuilder builder);

        protected static IEnvelopeStreamer BuildStreamer(IEnvelopeSerializer serializer)
        {
            return new EnvelopeStreamer(serializer, new DataSerializerWithBinaryFormatter());
        }

        [Test]
        public void Empty_roundtrip_should_work()
        {
            var builder = new MessageEnvelopeBuilder("my-id");
            var envelope = RoundtripViaSerializer(builder);
            Assert.AreEqual(envelope.EnvelopeId, "my-id");
        }

        [Test]
        public void Envelope_attributes_should_be_present()
        {
            var time = DateTimeOffset.UtcNow;
            var builder = new MessageEnvelopeBuilder("my-id");
            builder.Attributes["Custom"] = 1;
            

            var envelope = RoundtripViaSerializer(builder);

            Assert.AreEqual(1, envelope.GetAttribute("Custom"));

            Assert.IsTrue(envelope.CreatedOn >= time);
            Assert.IsTrue(envelope.CreatedOn <= DateTimeOffset.UtcNow);
        }

        [Test]
        public void Payload_should_be_serialized()
        {
            var builder = new MessageEnvelopeBuilder("my-id");
            builder.AddItem(new MyMessage("42"));

            var envelope = RoundtripViaSerializer(builder);

            Assert.AreEqual(1, envelope.Items.Length);
            Assert.AreEqual("42", ((MyMessage)envelope.Items[0].Content).Value);
        }

        [Test]
        public void Multiple_payloads_are_handled_in_sequence()
        {
            var builder = new MessageEnvelopeBuilder("my-id");

            for (int i = 0; i < 5; i++)
            {
                var content = new string('*',i);
                var added = builder.AddItem(new MyMessage(content));

                added.AddNumber("hum", i);
            }

            var envelope = RoundtripViaSerializer(builder);

            Assert.AreEqual(5, envelope.Items.Length);

            for (int i = 0; i < 5; i++)
            {
                var messageItem = envelope.Items[i];
                Assert.AreEqual(new string('*', i), ((MyMessage)messageItem.Content).Value);
                Assert.AreEqual(i, messageItem.GetAttributeNumber("hum", -1)); 
            }
        }

        [Serializable]
        sealed class MyMessage 
        {
            
            public readonly string Value;

            public MyMessage(string value)
            {
                Value = value;
            }
            public override string ToString()
            {
                return Value.ToString();
            }
        }
    }
}