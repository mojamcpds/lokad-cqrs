#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;

namespace Lokad
{
    /// <summary>
    /// Used by <see cref="MeansImplicitUseAttribute"/>
    /// </summary>
    [Flags]
    public enum ImplicitUseFlags
    {
        /// <summary>
        /// Standard
        /// </summary>
        STANDARD = 0,
        /// <summary>
        /// All members used
        /// </summary>
        ALL_MEMBERS_USED = 1
    }
}