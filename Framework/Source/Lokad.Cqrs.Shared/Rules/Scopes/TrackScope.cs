#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;

namespace Lokad.Rules
{
    /// <summary>
    /// <see cref="IScope"/> that merely keeps track of the worst level. 
    /// </summary>
    [Serializable]
    public sealed class TrackScope : IScope
    {
        RuleLevel _level;
        readonly Action<RuleLevel> _report = level => { };

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackScope"/> class.
        /// </summary>
        public TrackScope()
        {
        }

        TrackScope(Action<RuleLevel> report)
        {
            _report = report;
        }

        void IDisposable.Dispose()
        {
            _report(_level);
        }

        IScope IScope.Create(string name)
        {
            return new TrackScope(level =>
                {
                    if (level > _level)
                        _level = level;
                });
        }

        void IScope.Write(RuleLevel level, string message)
        {
            if (level > _level)
                _level = level;
        }

        RuleLevel IScope.Level
        {
            get { return _level; }
        }
    }
}