using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.StorageClient;

namespace Lokad.Cqrs.AzureViews
{
	public sealed class TableCommand
	{
		readonly TableServiceContext _context;
		readonly IList<string> _filters = new List<string>();
		public readonly ActionPolicy Policy;
		readonly string _containerName;


		readonly CloudTableClient _client;

		public TableCommand(TableServiceContext context, ActionPolicy policy, string containerName)
		{
			_context = context;
			_containerName = containerName;
			Policy = policy;
		}


		static string OperandToData(ConstraintOperand operand)
		{
			switch (operand)
			{
				case ConstraintOperand.Equal:
					return "eq";
				case ConstraintOperand.GreaterThan:
					return "gt";
				case ConstraintOperand.GreaterThanOrEqual:
					return "le";
				case ConstraintOperand.LessThan:
					return "lt";
				case ConstraintOperand.LessThanOrEqual:
					return "le";
				case ConstraintOperand.NotEqual:
					return "ne";
				default:
					throw new ArgumentOutOfRangeException("operand");
			}
		}

		public void AddFilter(string left, ConstraintOperand op, string right)
		{
			var txt = HttpUtility.UrlEncode(right);
			var data = OperandToData(op);
			_filters.Add(string.Format("({0} {1} '{2}')", left, data, txt));
		}

		Maybe<int> _limit = Maybe<int>.Empty;

		public void AddRecordLimit(int limit)
		{
			_limit = limit;
		}

		public void GetWriteContext()
		{
			var context = _client.GetDataServiceContext();
			
		}

		public DataServiceQuery<ViewStorageEntity> CreateQuery()
		{
			var query = _context.CreateQuery<ViewStorageEntity>(_containerName);

			_limit.Apply(i => query.AddQueryOption("$top", i));

			if (_filters.Any())
			{
				query.AddQueryOption("$filter", _filters.Join(" AND "));
			}

			return query;
		}
	}
}