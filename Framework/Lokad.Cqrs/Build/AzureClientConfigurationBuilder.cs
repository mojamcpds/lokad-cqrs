﻿#region (c) 2010-2011 Lokad - CQRS for Windows Azure - New BSD License 

// Copyright (c) Lokad 2010-2011, http://www.lokad.com
// This code is released as Open Source under the terms of the New BSD Licence

#endregion

using System;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Lokad.Cqrs.Build
{
    public sealed class AzureClientConfigurationBuilder
    {
        readonly CloudStorageAccount _account;

        Action<CloudQueueClient> _queueClientConfiguration;
        Action<CloudBlobClient> _blobClientConfiguration;
        readonly string _accountId;


        public AzureClientConfigurationBuilder ConfigureQueueClient(Action<CloudQueueClient> configure,
            bool replaceOld = false)
        {
            if (replaceOld)
            {
                _queueClientConfiguration = configure;
            }
            else
            {
                _queueClientConfiguration += configure;
            }
            return this;
        }

        public AzureClientConfigurationBuilder ConfigureBlobClient(Action<CloudBlobClient> configure,
            bool replaceOld = false)
        {
            if (replaceOld)
            {
                _blobClientConfiguration = configure;
            }
            else
            {
                _blobClientConfiguration += configure;
            }
            return this;
        }


        public AzureClientConfigurationBuilder(CloudStorageAccount account, string accountId)
        {
            // defaults
            _queueClientConfiguration = client => client.RetryPolicy = RetryPolicies.NoRetry();
            _blobClientConfiguration = client => client.RetryPolicy = RetryPolicies.NoRetry();
            _account = account;
            _accountId = accountId;
        }

        internal AzureClientConfiguration Build()
        {
            return new AzureClientConfiguration(_account, _queueClientConfiguration, _blobClientConfiguration,
                _accountId);
        }
    }
}