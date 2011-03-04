﻿using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Lokad.Cqrs.Dispatch
{
	public sealed class MessageDuplicationMemory 
	{
		readonly ConcurrentDictionary<string,DateTime> _memory = new ConcurrentDictionary<string, DateTime>();

		public void Memorize(string memoryId)
		{
			_memory.TryAdd(memoryId, DateTime.UtcNow);
		}

		public bool DoWeRemember(string memoryId)
		{
			DateTime value;
			return _memory.TryGetValue(memoryId, out value);
		}

		public void ForgetOlderThan(TimeSpan older)
		{
			// suboptimal for now
			var deleteBefore = DateTime.UtcNow - older;
			var keys = _memory.ToArray()
				.Where(p => p.Value < deleteBefore)
				.Select(v => v.Key);

			foreach (var key in keys)
			{
				DateTime deleted;
				_memory.TryRemove(key, out deleted);
			}
		}

	}
}