﻿#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;

namespace Lokad.Cqrs
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