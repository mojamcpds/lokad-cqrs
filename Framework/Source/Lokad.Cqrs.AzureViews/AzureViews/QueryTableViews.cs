using System;
using System.Data.Services.Client;
using Lokad.Quality;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Lokad.Cqrs.AzureViews
{

	public sealed class PublishTableViews : IPublishViews
	{

		AzureTableStorageDialect _dialect;
		IAzurePartitionExecutor _executor;

		public PublishTableViews(AzureTableStorageDialect dialect, IAzurePartitionExecutor executor)
		{
			_dialect = dialect;
			_executor = executor;
		}


		public void Write(Type view, string partition, string identity, object item)
		{
			_executor.Execute(view, partition, cmd => _dialect.WriteRecord(cmd, view, partition, identity, item));
		}

		public void Patch(Type type, string partition, string identity, Action<object> patch)
		{
			throw new NotImplementedException();
		}

		public void Delete(Type type, string partition, string identity)
		{
			throw new NotImplementedException();
		}

		public void DeletePartition(Type type, string partition)
		{
			throw new NotImplementedException();
		}
	}

	public sealed class QueryTableViews : IQueryViews
	{
		readonly AzureTableStorageDialect _dialect;
		readonly IAzurePartitionExecutor _executor;

		public QueryTableViews(AzureTableStorageDialect dialect, IAzurePartitionExecutor executor)
		{
			_dialect = dialect;
			_executor = executor;
		}


		public void QueryPartition(Type type, string partition, Maybe<IViewQuery> query, Action<ViewEntity> process)
		{
			_executor.Execute(type, partition, cmd => _dialect.ReadList(cmd, type, partition, query, process));
		}
	}
}