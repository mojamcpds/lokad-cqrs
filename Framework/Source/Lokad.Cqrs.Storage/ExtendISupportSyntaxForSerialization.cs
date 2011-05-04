#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using Lokad.Cqrs.Serialization;
using Lokad.Serialization;

namespace Lokad.Cqrs
{
    /// <summary>
    /// Syntax extensions for <see cref="ISupportSyntaxForSerialization"/>
    /// </summary>
    public static class ExtendISupportSyntaxForSerialization
    {
        /// <summary>
        /// Uses the binary formatter.
        /// </summary>
        /// <param name="module">The module to extend.</param>
        public static void UseBinaryFormatter(this ISupportSyntaxForSerialization module)
        {
            module.RegisterSerializer<BinaryMessageSerializer>();
        }

        /// <summary>
        /// Uses the data contract serializer.
        /// </summary>
        /// <param name="module">The module to extend.</param>
        public static void UseDataContractSerializer(this ISupportSyntaxForSerialization module)
        {
            module.RegisterSerializer<DataContractMessageSerializer>();
        }

        /// <summary>
        /// Uses the <see cref="ProtoBufMessageSerializer"/>.
        /// </summary>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        /// <param name="module">The module.</param>
        /// <returns></returns>
        public static TModule UseProtocolBuffers<TModule>(this TModule module)
            where TModule : ISupportSyntaxForSerialization
        {
            module.RegisterSerializer<ProtoBufMessageSerializer>();
            return module;
        }
    }
}