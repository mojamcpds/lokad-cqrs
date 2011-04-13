﻿#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using Lokad.Cqrs.Evil;

namespace Lokad.Cqrs.Feature.Recurrent
{
	public sealed class RecurrentTaskState
	{
		public readonly RecurrentTaskInfo Task;
		public readonly string Name;

		public RecurrentTaskState(string name, RecurrentTaskInfo task)
		{
			NextRun = DateTime.UtcNow;
			LastException = Maybe<Exception>.Empty;
			Name = name;
			Task = task;
		}

		public DateTime NextRun { get; private set; }
		public int ExceptionCount { get; private set; }
		public Maybe<Exception> LastException { get; private set; }

		
		public void Completed()
		{
			NextRun = DateTime.UtcNow;
		}

		public void RecordException(Exception exception)
		{
			LastException = exception;
			ExceptionCount += 1;
		}

		public void ScheduleIn(TimeSpan span)
		{
			NextRun = DateTime.UtcNow + span;
		}
	}
}