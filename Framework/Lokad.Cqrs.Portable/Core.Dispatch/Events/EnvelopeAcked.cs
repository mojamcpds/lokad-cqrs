﻿using System;
using System.Collections.Generic;

namespace Lokad.Cqrs.Core.Dispatch.Events
{
    public sealed class EnvelopeAcked : ISystemEvent
    {
        public string QueueName { get; private set; }
        public string EnvelopeId { get; private set; }
        public ICollection<KeyValuePair<string, string>> Attributes { get; private set; }
        
        public EnvelopeAcked(string queueName, string envelopeId, ICollection<KeyValuePair<string,string>> attributes)
        {
            QueueName = queueName;
            EnvelopeId = envelopeId;
            Attributes = attributes;
        }

        public override string ToString()
        {
            return string.Format("[{0}] acked at '{1}'", EnvelopeId, QueueName);
        }
    }
}