﻿#region (c) 2010-2011 Lokad - CQRS for Windows Azure - New BSD License 

// Copyright (c) Lokad 2010-2011, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Lokad.Cqrs.Evil;
using ProtoBuf;

namespace Lokad.Cqrs.Core.Serialization
{
    public static class ProtoBufUtil
    {
        public static string GetContractReference(Type type)
        {
            var attribs = type.GetCustomAttributes(false);
            var helper = new AttributeHelper(attribs);


            var name = Maybe.String
                .GetValue(() => helper.GetString<ProtoContractAttribute>(p => p.Name))
                .GetValue(() => helper.GetString<DataContractAttribute>(p => p.Name))
                .GetValue(() => helper.GetString<XmlTypeAttribute>(p => p.TypeName))
                .GetValue(() => type.Name);

            var ns = Maybe.String
                .GetValue(() => helper.GetString<DataContractAttribute>(p => p.Namespace))
                .GetValue(() => helper.GetString<XmlTypeAttribute>(p => p.Namespace))
                .Convert(s => s.Trim() + "/", "");

            return ns + name;
        }


        public static IFormatter CreateFormatter(Type type)
        {
            try
            {
                typeof (Serializer)
                    .GetMethod("PrepareSerializer")
                    .MakeGenericMethod(type)
                    .Invoke(null, null);
            }
            catch (TargetInvocationException tie)
            {
                var message = string.Format("Failed to prepare ProtoBuf serializer for '{0}'.", type);
                throw new InvalidOperationException(message, tie.InnerException);
            }

            try
            {
                return (IFormatter) typeof (Serializer)
                    .GetMethod("CreateFormatter")
                    .MakeGenericMethod(type)
                    .Invoke(null, null);
            }
            catch (TargetInvocationException tie)
            {
                var message = string.Format("Failed to create ProtoBuf formatter for '{0}'.", type);
                throw new InvalidOperationException(message, tie.InnerException);
            }
        }
    }
}