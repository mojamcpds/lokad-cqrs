using System;
using Autofac;
using Lokad.Cqrs.AzureViews;

namespace Lokad.Cqrs
{
	public static class ExtendSyntax
	{
		public static TModule QueryViewsFromAzure<TModule>(this TModule module, 
			ActionPolicy policy,
			AzureViewPartitioning partitioning = AzureViewPartitioning.Default)
			where TModule : ISyntax<ContainerBuilder>
		{
			module.Target.RegisterType<QueryTableViews>()
				.As<IQueryViews>().SingleInstance()
				.WithParameter(TypedParameter.From(policy)); 
			module.Target
				.RegisterType<AzureTableStorageDialect>()
				.SingleInstance();

			switch (partitioning)
			{
				case AzureViewPartitioning.Default:
					module.Target.RegisterType<PartitionAzureTableStorage>().As<IPartitionAzureViews>().SingleInstance();
					break;
				default:
					throw new ArgumentOutOfRangeException("partitioning");
			}

			return module;
		}
	}
}