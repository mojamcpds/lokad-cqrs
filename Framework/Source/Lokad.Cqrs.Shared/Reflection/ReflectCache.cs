#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

namespace Lokad.Reflection
{
    static class ReflectCache<T>
    {
        public static string ReferenceName = "{" + typeof (T).Name + "}";
        public static string SequenceName = "{" + typeof (T).Name + "}[]";
    }
}