﻿using System;
using Lokad.Cqrs.Properties;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using NUnit.Framework;

namespace Lokad.Cqrs.Feature.TapeStorage
{
    [TestFixture]
    public class BlobTapeStorageTests : TapeStorageTests
    {
        const string ContainerName = "blob-tape-test";

        CloudStorageAccount _cloudStorageAccount;
        ITapeStorageFactory _storageFactory;

        protected override void PrepareEnvironment()
        {
            CloudStorageAccount.SetConfigurationSettingPublisher(
                (configName, configSetter) => configSetter((string) Settings.Default[configName]));

            _cloudStorageAccount = CloudStorageAccount.FromConfigurationSetting("StorageConnectionString");
            var cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();

            try
            {
                cloudBlobClient.GetContainerReference(ContainerName).FetchAttributes();
                throw new InvalidOperationException("Container '" + ContainerName + "' already exists!");
            }
            catch (StorageClientException e)
            {
                if (e.ErrorCode != StorageErrorCode.ResourceNotFound)
                    throw new InvalidOperationException("Container '" + ContainerName + "' already exists!");
            }
        }

        protected override Factories GetTapeStorageInterfaces()
        {
            var config = AzureStorage.CreateConfig(_cloudStorageAccount);
            _storageFactory = new BlobTapeStorageFactory(config, ContainerName);
            _storageFactory.Initialize();

            const string name = "test";

            return new Factories
            {
                Writer = _storageFactory.GetOrCreateWriter(name),
                Reader = _storageFactory.GetReader(name)
            };
        }

        protected override void FreeResources()
        {
            _storageFactory = null;
            _storageFactory = null;
        }

        protected override void CleanupEnvironment()
        {
            var cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
            var container = cloudBlobClient.GetContainerReference(ContainerName);

            if (container.Exists())
                container.Delete();
        }
    }
}
