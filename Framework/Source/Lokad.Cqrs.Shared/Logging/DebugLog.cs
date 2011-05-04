#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

#if !SILVERLIGHT2

using System;
using System.Diagnostics;

namespace Lokad
{
    /// <summary>
    /// Simple <see cref="ILog"/> that writes to the <see cref="Trace.Listeners"/>, if the
    /// <em>DEBUG</em> symbol is defined
    /// </summary>
    /// <remarks>Use Logging stack, if more flexibility is needed</remarks>
    [Serializable]
    [NoCodeCoverage, UsedImplicitly]
    public sealed class DebugLog : ILog
    {
        /// <summary>  Singleton instance </summary>
        [UsedImplicitly] public static readonly ILog Instance = new DebugLog("");

        /// <summary>
        /// Named provider for the <see cref="DebugLog"/>
        /// </summary>
        [UsedImplicitly] public static readonly ILogProvider Provider =
            new LambdaLogProvider(s => new DebugLog(s));

        readonly string _logName;

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugLog"/> class.
        /// </summary>
        /// <param name="logName">Name of the log.</param>
        public DebugLog(string logName)
        {
            _logName = logName;
        }

        void ILog.Log(LogLevel level, object message)
        {
            Debug.WriteLine("[" + level + "] " + message, _logName);
            Debug.Flush();
        }

        void ILog.Log(LogLevel level, Exception ex, object message)
        {
            Debug.WriteLine("[" + level + "] " + message, _logName);
            Debug.WriteLine("[" + level + "] " + ex, _logName);
            Debug.Flush();
        }

        bool ILog.IsEnabled(LogLevel level)
        {
            return true;
        }
    }
}

#endif