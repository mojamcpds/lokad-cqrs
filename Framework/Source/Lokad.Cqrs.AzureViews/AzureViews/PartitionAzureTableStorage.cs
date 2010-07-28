using System;
using System.Data.Services.Client;
using Lokad.Quality;
using Lokad.Serialization;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Lokad.Cqrs.AzureViews
{
	[UsedImplicitly]
	public sealed class PartitionAzureTableStorage : 
		IPartitionAzureViews, IAzurePartitionExecutor
	{
		readonly IDataContractMapper _mapper;

		readonly TableServiceContext _context;
		readonly ActionPolicy _policy;


		static Type ResolveType(string s)
		{
			return typeof(ViewStorageEntity);
		}

		public PartitionAzureTableStorage(IDataContractMapper mapper, CloudTableClient client, ActionPolicy policy)
		{
			_mapper = mapper;

			_context = client.GetDataServiceContext();
			_context.MergeOption = MergeOption.NoTracking;
			_context.ResolveType = ResolveType;
			_policy = policy;
		}

		public PartitionAzureTableStorage(IDataContractMapper mapper, CloudStorageAccount account, ActionPolicy policy) : this(mapper, CreateClient(account), policy)
		{
		}

		static CloudTableClient CreateClient(CloudStorageAccount account)
		{
			var client = account.CreateCloudTableClient();
			client.RetryPolicy = RetryPolicies.NoRetry();
			return client;
		}

		public bool PartitionsShareContainer
		{
			get { return true; }
		}

		string GetContainerName(Type type, string partition)
		{
			return _mapper.GetContractNameByType(type).GetValue(type.Name).ToLowerInvariant();
		}

		public void Execute(Type type, string partition, Action<TableCommand> exec)
		{
			var containerName = GetContainerName(type, partition);
			var cmd = new TableCommand(_context, _policy, containerName);
			exec(cmd);
		}
	}
}