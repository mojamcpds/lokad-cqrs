#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

namespace Lokad.Cqrs.Default
{
	/// <summary>
	/// <para>Default CQRS message interface for the domain setup. By default Lokad.CQRS 
	/// scans assemblies for message contracts deriving from this interface.</para>
	/// <para>If you don't want to reference Lokad.CQRS assemblies in your domain, 
	/// you can declare your own message interface and point to it in the configuration,
	/// as shown in the samples.</para>
	/// </summary>
	/// <remarks>Look in the samples for more details on the usage</remarks>
	public interface IMessage
	{
	}

	/// <summary>
	/// Default CQRS entity
	/// </summary>
	public interface IEntity
	{
		
	}
}