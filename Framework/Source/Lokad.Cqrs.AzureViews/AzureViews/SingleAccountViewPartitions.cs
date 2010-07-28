using System;
using Lokad.Serialization;

namespace Lokad.Cqrs.AzureViews
{
	public sealed class SingleAccountViewPartitions : IPartitionAzureViews
	{
		readonly IDataContractMapper _mapper;

		public SingleAccountViewPartitions(IDataContractMapper mapper)
		{
			_mapper = mapper;
		}

		public bool PartitionsShareContainer
		{
			get { return true; }
		}

		public string GetContainerName(Type type, string partition)
		{
			return _mapper
				.GetContractNameByType(type)
				.GetValue(type.Name)
				.ToLowerInvariant();
		}
	}
}