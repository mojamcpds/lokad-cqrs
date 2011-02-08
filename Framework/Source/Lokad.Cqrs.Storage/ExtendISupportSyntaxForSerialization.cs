﻿#region Copyright (c) 2009-2010 LOKAD SAS. All rights reserved.

// Copyright (c) 2009-2010 LOKAD SAS. All rights reserved.
// You must not remove this notice, or any other, from this software.
// This document is the property of LOKAD SAS and must not be disclosed.

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