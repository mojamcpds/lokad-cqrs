#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

namespace Lokad.Rules
{
    ///<summary>
    /// Typed delegate for holding the validation logics
    ///</summary>
    ///<param name="obj">Object to validate</param>
    ///<param name="scope">Scope that will hold all validation results</param>
    ///<typeparam name="T">type of the item to validate</typeparam>
    public delegate void Rule<T>(T obj, IScope scope);
}