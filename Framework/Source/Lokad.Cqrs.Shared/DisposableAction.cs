#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;

namespace Lokad
{
    /// <summary>
    /// Class that allows action to be executed, when it is disposed
    /// </summary>
    [Serializable]
    public sealed class DisposableAction : IDisposable
    {
        readonly Action _action;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableAction"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        public DisposableAction(Action action)
        {
            _action = action;
        }

        /// <summary>
        /// Executes the action
        /// </summary>
        public void Dispose()
        {
            _action();
        }
    }
}