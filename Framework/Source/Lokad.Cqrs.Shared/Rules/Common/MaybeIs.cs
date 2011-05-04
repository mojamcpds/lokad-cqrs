#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

namespace Lokad.Rules
{
    /// <summary>
    /// Common rules for handling Maybe{T} values.
    /// </summary>
    public static class MaybeIs
    {
        /// <summary>
        /// Generates rule that is valid if the provided optional
        /// is either empty or passes the supplied value rules
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="valueRules">The value rules.</param>
        /// <returns>new validation rule</returns>
        public static Rule<Maybe<TValue>> EmptyOr<TValue>(params Rule<TValue>[] valueRules)
        {
            return (maybe, scope) => maybe.Apply(v => scope.ValidateInScope(v, valueRules));
        }

        /// <summary>
        /// Generates the rule that ensures the provided optional to be
        /// valid and passing the provided rules
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="valueRules">The value rules.</param>
        /// <returns>new validation rules</returns>
        public static Rule<Maybe<TValue>> ValidAnd<TValue>(params Rule<TValue>[] valueRules)
        {
            return (maybe, scope) => maybe
                .Handle(() => scope.Error(RuleResources.Maybe_X_cant_be_empty, typeof (Maybe<TValue>).Name))
                .Apply(value => scope.ValidateInScope(value, valueRules));
        }
    }
}