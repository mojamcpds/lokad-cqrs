#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Data;

namespace Lokad.Cqrs.SqlViews
{
	public interface IPartitionViews
	{
		bool PartitionsShareContainer { get; }
		bool ViewsShareContainer { get; }
		string GetContainerName(Type type, string partition);
		string GetViewName(Type type);
	}
}