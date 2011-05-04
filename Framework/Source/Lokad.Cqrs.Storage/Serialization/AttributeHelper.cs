#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Linq;

namespace Lokad.Serialization
{
    sealed class AttributeHelper
    {
        readonly object[] _attributes;

        public AttributeHelper(object[] attributes)
        {
            _attributes = attributes;
        }

        public Maybe<string> GetString<TAttribute>(Func<TAttribute, string> retriever)
            where TAttribute : Attribute
        {
            var v = ExtendIEnumerable.FirstOrEmpty<TAttribute>(_attributes
                .OfType<TAttribute>())
                .Convert(retriever, "");

            if (string.IsNullOrEmpty(v))
                return Maybe<string>.Empty;

            return v;
        }
    }
}