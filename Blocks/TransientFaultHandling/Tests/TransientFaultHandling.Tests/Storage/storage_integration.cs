#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Transient Fault Handling Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.TestSupport;

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests.Storage.storage_integration
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data.Services.Client;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.StorageClient;

    public class Context : ArrangeActAssert
    {
        protected StorageTransientErrorDetectionStrategy strategy;
        protected CloudStorageAccount account;

        protected override void Arrange()
        {
            var storageConnectionString = ConfigurationHelper.GetSetting("StorageConnectionString");

            if (string.IsNullOrEmpty(storageConnectionString)
                || storageConnectionString.Contains("[AccountName]")
                || storageConnectionString.Contains("[AccountKey]"))
            {
                Assert.Inconclusive("Cannot run tests because the Windows Azures Storage credentials are not configured in app.config");
            }

            this.account = CloudStorageAccount.Parse(storageConnectionString);
            this.strategy = new StorageTransientErrorDetectionStrategy();

            // Check to see that the account is valid.
            Assert.IsNotNull(this.account.CreateCloudBlobClient().ListContainers().ToList());
        }
    }

    [TestClass]
    public class when_blob_does_not_exists : Context
    {
        [TestMethod]
        public void then_not_found_error_is_not_transient()
        {
            // Fixes http://social.msdn.microsoft.com/forums/en-us/windowsazuredevelopment/thread/0fad898b-2fa2-4d8f-b918-3723194c2ef9
            var client = account.CreateCloudBlobClient();
            client.RetryPolicy = RetryPolicies.NoRetry();

            try
            {
                string inexistantBlobName = Guid.NewGuid().ToString();
                var blob = client.GetBlobReference(inexistantBlobName).DownloadByteArray();
                Assert.Fail("Exception not thrown");
            }
            catch (StorageClientException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(WebException));
                Assert.IsFalse(strategy.IsTransient(ex));
            }
        }
    }

    [TestClass]
    public class when_accessing_table_entity : Context
    {
        private string tableName;

        protected override void Arrange()
        {
            base.Arrange();

            var client = account.CreateCloudTableClient();
            this.tableName = "TopazStorageIntegrationTest" + new Random(unchecked((int)DateTime.Now.Ticks)).Next(1000000).ToString();
            client.CreateTableIfNotExist(tableName);
        }

        protected override void Teardown()
        {
            base.Teardown();
            account.CreateCloudTableClient().DeleteTableIfExist(tableName);
        }

        [TestMethod]
        public void then_has_some_transient_errors_when_saving()
        {
            var exceptions = SaveConcurrent(15, SaveChangesOptions.None);

            foreach (var ex in exceptions)
            {
                Assert.IsTrue(strategy.IsTransient(ex), ex.ToString());
            }
        }

        [TestMethod]
        public void then_has_some_transient_errors_when_saving_using_batch()
        {
            var exceptions = SaveConcurrent(15, SaveChangesOptions.Batch);

            foreach (var ex in exceptions)
            {
                Assert.IsTrue(strategy.IsTransient(ex), ex.ToString());
            }
        }

        private List<Exception> SaveConcurrent(int numberOfCalls, SaveChangesOptions options)
        {
            // Do several calls to force some timeouts
            // or manually disconnect the network while this test is running.
            string partitionKey = Guid.NewGuid().ToString();
            var exceptions = new ConcurrentBag<Exception>();

            var entities = Enumerable.Range(0, numberOfCalls)
                .Select(x => new TestServiceEntity { PartitionKey = partitionKey, RowKey = x.ToString() })
                .ToList();

            var barrier = new Barrier(numberOfCalls);
            var countdown = new CountdownEvent(numberOfCalls);
            foreach (var entity in entities)
            {
                ThreadPool.QueueUserWorkItem(x =>
                {
                    try
                    {
                        var client = account.CreateCloudTableClient();
                        client.RetryPolicy = RetryPolicies.NoRetry();
                        // Explicitly set a VERY short timeout, to force a timeout very frequently.
                        client.Timeout = TimeSpan.FromSeconds(1);
                        var context = client.GetDataServiceContext();
                        context.AddObject(tableName, x);
                        barrier.SignalAndWait();
                        context.SaveChanges(options);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                    finally
                    {
                        countdown.Signal();
                    }
                }, entity);
            }

            countdown.Wait();
            if (exceptions.Count == 0)
            {
                Assert.Inconclusive("No exceptions were thrown to check if they are transient");
            }

            return exceptions.ToList();
        }

        class TestServiceEntity : TableServiceEntity
        {
        }
    }
}
