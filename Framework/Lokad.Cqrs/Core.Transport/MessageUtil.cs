﻿#region (c) 2010-2011 Lokad - CQRS for Windows Azure - New BSD License 

// Copyright (c) Lokad 2010-2011, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.IO;
using System.Text;
using Lokad.Cqrs.Core.Transport.Contracts_v2;
using Lokad.Cqrs.Evil;
using ProtoBuf;

namespace Lokad.Cqrs.Core.Transport
{
	public static class MessageUtil
	{
		public static MessageHeader ReadHeader(byte[] buffer)
		{
			using (var stream = new MemoryStream(buffer, 0, MessageHeader.FixedSize))
			{
				return Serializer.Deserialize<MessageHeader>(stream);
			}
		}

		public static byte[] SaveReferenceMessage(MessageReference reference)
		{
			// important to use \r\n
			var builder = new StringBuilder();
			builder
				.Append("[cqrs-ref-r1]\r\n")
				.Append(reference.EnvelopeId).Append("\r\n")
				.Append(reference.StorageContainer).Append("\r\n")
				.Append(reference.StorageReference);

			return Encoding.Unicode.GetBytes(builder.ToString());
		}


		public static byte[] SaveDataMessage(MessageEnvelope builder, IMessageSerializer serializer)
		{
			return Schema2Util.SaveData(builder, serializer);
		}

		const string RefernceSignature = "[cqrs-ref-r1]";
		static readonly byte[] Reference = Encoding.Unicode.GetBytes(RefernceSignature);

		public static bool TryReadAsReference(byte[] buffer, out MessageReference reference)
		{
			if (BytesStart(buffer, Reference))
			{
				var text = Encoding.Unicode.GetString(buffer);
				var args = text.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
				reference = new MessageReference(args[1], args[2], args[3]);
				return true;
			}
			reference = null;
			return false;
		}

		static bool BytesStart(byte[] buffer, byte[] signature)
		{
			if (buffer.Length < signature.Length)
				return false;

			for (int i = 0; i < signature.Length; i++)
			{
				if (buffer[i] != signature[i])
					return false;
			}

			return true;
		}



		public static MessageEnvelope ReadMessage(byte[] buffer, IMessageSerializer serializer)
		{
			// unefficient reading for now, since protobuf-net does not support reading parts
			var header = ReadHeader(buffer);
			switch (header.MessageFormatVersion)
			{
				case MessageHeader.Schema2DataFormat:
					return Schema2Util.ReadDataMessage(buffer, serializer);
				default:
					throw Errors.InvalidOperation("Unknown message format: {0}", header.MessageFormatVersion);
			}
		}
	}
}