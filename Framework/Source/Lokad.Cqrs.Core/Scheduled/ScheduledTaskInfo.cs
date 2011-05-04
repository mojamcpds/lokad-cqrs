#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using System.Reflection;

namespace Lokad.Cqrs.Scheduled
{
    public sealed class ScheduledTaskInfo
    {
        public readonly string Name;
        public readonly Type Task;

        public readonly MethodInfo Invoker;

        public ScheduledTaskInfo(string name, Type task, MethodInfo invoker)
        {
            Name = name;
            Task = task;
            Invoker = invoker;
        }
    }
}