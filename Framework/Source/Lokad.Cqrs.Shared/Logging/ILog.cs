#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;

namespace Lokad
{
    /// <summary>
    /// Shared interface to abstract away from the specific
    /// logging library
    /// </summary>
    public interface ILog
    {
        /// <summary> Writes the message to the logger </summary>
        /// <param name="level">The importance level</param>
        /// <param name="message">The actual message</param>
        void Log(LogLevel level, object message);

        /// <summary>
        /// Writes the exception and associated information 
        /// to the logger
        /// </summary>
        /// <param name="level">The importance level</param>
        /// <param name="ex">The actual exception</param>
        /// <param name="message">Information related to the exception</param>
        void Log(LogLevel level, Exception ex, object message);

        /// <summary>
        /// Determines whether the messages of specified level are being logged down
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns>
        /// 	<c>true</c> if the specified level is logged; otherwise, <c>false</c>.
        /// </returns>
        bool IsEnabled(LogLevel level);
    }
}