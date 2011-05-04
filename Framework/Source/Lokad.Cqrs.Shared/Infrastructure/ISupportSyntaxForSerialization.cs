#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

namespace Lokad
{
    /// <summary>
    /// Syntax support for .Serialization configurations.
    /// </summary>
    public interface ISupportSyntaxForSerialization
    {
        /// <summary>
        /// Registers the specified data serializer as singleton implementing <see cref="IMessageSerializer"/>, <see cref="IDataSerializer"/> and <see cref="IDataContractMapper"/>. It can import <see cref="IKnowSerializationTypes"/>
        /// </summary>
        /// <typeparam name="TSerializer">The type of the serializer.</typeparam>
        void RegisterSerializer<TSerializer>()
            where TSerializer : IMessageSerializer;
    }
}