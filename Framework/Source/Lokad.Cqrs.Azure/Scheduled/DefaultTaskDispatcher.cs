#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Reflection;
using Autofac;

namespace Lokad.Cqrs.Scheduled
{
    public sealed class DefaultTaskDispatcher : IScheduledTaskDispatcher
    {
        readonly ILifetimeScope _scope;

        public DefaultTaskDispatcher(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public TimeSpan Execute(ScheduledTaskInfo info)
        {
            using (var scope = _scope.BeginLifetimeScope())
            {
                var instance = scope.Resolve(info.Task);
                try
                {
                    return (TimeSpan) info.Invoker.Invoke(instance, new object[0]);
                }
                catch (TargetInvocationException e)
                {
                    throw Errors.Inner(e);
                }
            }
        }
    }
}