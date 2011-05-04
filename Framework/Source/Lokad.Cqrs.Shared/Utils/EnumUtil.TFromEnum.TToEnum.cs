﻿#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Lokad
{
    /// <summary>
    /// Ensures that enums can be converted between each other
    /// </summary>
    /// <typeparam name="TFromEnum">The type of from enum.</typeparam>
    /// <typeparam name="TToEnum">The type of to enum.</typeparam>
    static class EnumUtil<TFromEnum, TToEnum>
        where TFromEnum : struct, IComparable
        where TToEnum : struct, IComparable
    {
        static readonly IDictionary<TFromEnum, TToEnum> Enums;
        static readonly TFromEnum[] Unmatched;

        static EnumUtil()
        {
            var fromEnums = EnumUtil.GetValues<TFromEnum>();
            Enums = new Dictionary<TFromEnum, TToEnum>(fromEnums.Length, EnumUtil<TFromEnum>.Comparer);
            var unmatched = new List<TFromEnum>();

            foreach (var fromEnum in fromEnums)
            {
                var @enum = fromEnum;
                MaybeParse
                    .Enum<TToEnum>(fromEnum.ToString())
                    .Handle(() => unmatched.Add(@enum))
                    .Apply(match => Enums.Add(@enum, match));
            }

            Unmatched = unmatched.ToArray();
        }

        public static TToEnum Convert(TFromEnum from)
        {
            ThrowIfInvalid();
            return Enums[from];
        }

        static void ThrowIfInvalid()
        {
            if (Unmatched.Length > 0)
            {
                var list = ExtendIEnumerable.Join(Unmatched.Select(e => e.ToString()), ", ");
                var message = string.Format(CultureInfo.InvariantCulture,
                    "Can't convert from {0} to {1} because of unmatched entries: {2}",
                    typeof (TFromEnum), typeof (TToEnum), list);
                throw new ArgumentException(message);
            }
        }
    }
}