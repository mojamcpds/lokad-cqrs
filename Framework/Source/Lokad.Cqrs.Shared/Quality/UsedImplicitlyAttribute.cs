#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;

namespace Lokad
{
    /// <summary>
    /// Indicates that the marked symbol is used implicitly (ex. reflection, external library), 
    /// so this symbol will not be marked as unused (as well as by other usage inspections)
    /// </summary>
    /// <remarks>This attribute helps R# in code analysis</remarks>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    [MeansImplicitUse(ImplicitUseFlags.ALL_MEMBERS_USED)]
    public sealed class UsedImplicitlyAttribute : Attribute
    {
    }
}