#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Reflection;
using System.Reflection.Emit;
using Lokad.Cqrs;

namespace Lokad.Reflection
{
    /// <summary>
    /// Helper class for the IL-based strongly-typed reflection
    /// </summary>
    /// <remarks>This class is not supported by Silverlight 2.0, yet</remarks>
    public static class Reflect
    {
        static readonly byte Ldarg_0 = (byte) OpCodes.Ldarg_0.Value;
        static readonly byte Ldfld = (byte) OpCodes.Ldfld.Value;
        static readonly byte Stloc_0 = (byte) OpCodes.Stloc_0.Value;
        static readonly byte Ret = (byte) OpCodes.Ret.Value;

        internal static string VariableName<T>(Func<T> expression)
        {
            return VariableNameSafely(expression).GetValue(ReflectCache<T>.ReferenceName);
        }


        static Maybe<string> VariableNameSafely<T>(Func<T> expression)
        {
#if SILVERLIGHT2
			return Maybe<string>.Empty;
#else
            return VariableSafely(expression)
                .Convert(e => e.Name);
#endif
        }


#if !SILVERLIGHT2

        /// <summary>
        /// Retrieves via IL the information of the <b>local</b> variable passed in the expression.
        /// <code>
        /// var myVar = "string";
        /// var info = Reflect.Variable(() =&gt; myVar)
        /// </code>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression containing the local variable to reflect.</param>
        /// <returns>information about the variable (if able to retrieve)</returns>
        public static Maybe<FieldInfo> VariableSafely<T>([NotNull] Func<T> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            var method = expression.Method;
            var body = method.GetMethodBody();
            if (null == body) return Maybe<FieldInfo>.Empty;
            var il = body.GetILAsByteArray();
            // in DEBUG we end up with stack
            // in release, there is a ret at the end
            if ((il[0] == Ldarg_0) && (il[1] == Ldfld) && ((il[6] == Stloc_0) || (il[6] == Ret)))
            {
                var fieldHandle = BitConverter.ToInt32(il, 2);

                var module = method.Module;
                var expressionType = expression.Target.GetType();

                if (!expressionType.IsGenericType)
                {
                    return module.ResolveField(fieldHandle);
                }
                var genericTypeArguments = expressionType.GetGenericArguments();
                // method does not have any generics
                //var genericMethodArguments = method.GetGenericArguments();
                return module.ResolveField(fieldHandle, genericTypeArguments, Type.EmptyTypes);
            }
            return Maybe<FieldInfo>.Empty;
        }

        // in DEBUG we end up with stack
        // in release, there is a ret at the end

#endif
    }
}