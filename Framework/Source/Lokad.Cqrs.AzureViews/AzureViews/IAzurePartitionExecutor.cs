using System;

namespace Lokad.Cqrs.AzureViews
{
	public interface IAzurePartitionExecutor
	{
		void Execute(Type type, string partition, Action<TableCommand> exec);
	}
}