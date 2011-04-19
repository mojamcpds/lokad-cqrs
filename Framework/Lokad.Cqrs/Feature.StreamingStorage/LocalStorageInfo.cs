﻿using System;

namespace Lokad.Cqrs.Feature.StreamingStorage
{
	public sealed class LocalStorageInfo
	{
		public readonly DateTime LastModifiedUtc;
		public readonly string ETag;

		public LocalStorageInfo(DateTime lastModifiedUtc, string eTag)
		{
			LastModifiedUtc = lastModifiedUtc;
			ETag = eTag;
		}
	}
}