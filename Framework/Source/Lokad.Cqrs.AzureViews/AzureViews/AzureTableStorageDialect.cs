using System;
using System.Data.Services.Client;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Lokad.Serialization;
using Microsoft.WindowsAzure.StorageClient;

namespace Lokad.Cqrs.AzureViews
{
	public sealed class AzureTableStorageDialect
	{
		readonly IPartitionAzureViews _manager;
		readonly IDataSerializer _serializer;

		const string ContinuationNextRowKeyToken = "x-ms-continuation-NextRowKey";
		const string ContinuationNextPartitionKeyToken = "x-ms-continuation-NextPartitionKey";
		const string NextRowKeyToken = "NextRowKey";
		const string NextPartitionKeyToken = "NextPartitionKey";

		public AzureTableStorageDialect(IPartitionAzureViews manager, IDataSerializer serializer)
		{
			_manager = manager;
			_serializer = serializer;
		}

		public void WriteRecord(TableCommand cmd, Type type, string partition, string identity, object data)
		{
			
		}


		public void DeleteRecord(TableCommand cmd, Type type, string partition, string identity)
		{
			var builder = new StringBuilder();
			
			
			cmd.CreateQuery()
		}

		public void DeletePartition(IDbCommand cmd, Type type, string partition)
		{
			var builder = new StringBuilder();
			builder.Append("DELETE " + _manager.GetContainerName(type, partition));
			AddFilterClause(cmd, builder, type, partition, Maybe<IdentityConstraint>.Empty);
			cmd.CommandText = builder.ToString();
			cmd.ExecuteNonQuery();
		}



		static string GetErrorCode(DataServiceQueryException ex)
		{
			var r = new Regex(@"<code>(\w+)</code>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			var match = r.Match(ex.InnerException.Message);
			return match.Groups[1].Value;
		}


		public void ReadList(TableCommand cmd, Type type, string partition, Maybe<IViewQuery> viewQuery, 
			Action<ViewEntity> processor)
		{
			viewQuery.Combine(q => q.RecordLimit).Apply(cmd.AddRecordLimit);

			if (_manager.PartitionsShareContainer)
			{
				cmd.AddFilter("PartitionKey", ConstraintOperand.Equal, partition);
			}

			viewQuery.Combine(q => q.Constraint).Apply(iq => cmd.AddFilter("RowKey", iq.Operand, iq.Value));

			
			string continuationRowKey = null;
			string continuationPartitionKey = null;
			while (true)
			{
				var query = cmd.CreateQuery();

				if (null != continuationRowKey)
				{
					query = query
						.AddQueryOption(NextRowKeyToken, continuationRowKey)
						.AddQueryOption(NextPartitionKeyToken, continuationPartitionKey);
				}

				try
				{
					var result = cmd.Policy.Get(query.Execute);

					if (null == result)
						return;

					foreach (var entity in result)
					{
						using (var memory = new MemoryStream(entity.GetData()))
						{
							var o = _serializer.Deserialize(memory, type);
							var e = new ViewEntity(entity.PartitionKey, entity.RowKey, o);
							processor(e);
						}
					}

					var response = (QueryOperationResponse)result;

					if (!response.Headers.ContainsKey(ContinuationNextRowKeyToken))
					{
						return;
					}

					continuationRowKey = response.Headers[ContinuationNextRowKeyToken];
					continuationPartitionKey = response.Headers[ContinuationNextPartitionKeyToken];
					
				}
				catch (DataServiceQueryException ex)
				{
					var errorCode = GetErrorCode(ex);
					if (TableErrorCodeStrings.TableNotFound == errorCode
						|| StorageErrorCodeStrings.ResourceNotFound == errorCode)
					{
						return;
					}

					throw;
				}
			}
		}
	}
}