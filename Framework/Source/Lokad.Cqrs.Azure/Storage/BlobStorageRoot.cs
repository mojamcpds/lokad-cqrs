#region (c) 2010 Lokad Open Source - New BSD License 

// Copyright (c) Lokad 2010, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using Microsoft.WindowsAzure.StorageClient;

namespace Lokad.Cqrs.Storage
{
    /// <summary>
    /// Windows Azure implementation of storage 
    /// </summary>
    public sealed class BlobStorageRoot : IStorageRoot
    {
        readonly CloudBlobClient _client;
        readonly ILogProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobStorageRoot"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="provider">The provider.</param>
        public BlobStorageRoot(CloudBlobClient client, ILogProvider provider)
        {
            _client = client;
            _provider = provider;
        }

        public IStorageContainer GetContainer(string name)
        {
            return new BlobStorageContainer(_client.GetBlobDirectoryReference(name));
        }
    }
}