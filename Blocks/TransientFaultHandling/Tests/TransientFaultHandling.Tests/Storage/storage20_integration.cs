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

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests.Storage.storage20_integration
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;
    using Microsoft.WindowsAzure.Storage.Table;

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
            client.RetryPolicy = new NoRetry();

            try
            {
                string inexistantBlobName = Guid.NewGuid().ToString();
                using (var stream = new MemoryStream())
                {
                    client.GetContainerReference("inexistant").GetBlockBlobReference(inexistantBlobName).DownloadToStream(stream);
                }
                Assert.Fail("Exception not thrown");
            }
            catch (StorageException ex)
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

            this.tableName = "TopazStorageIntegrationTest" + new Random(unchecked((int)DateTime.Now.Ticks)).Next(1000000).ToString();
            account.CreateCloudTableClient().GetTableReference(this.tableName).CreateIfNotExists();
        }

        protected override void Teardown()
        {
            base.Teardown();
            account.CreateCloudTableClient().GetTableReference(tableName).DeleteIfExists();
        }

        [TestMethod]
        public void then_has_some_transient_errors_when_saving()
        {
            var exceptions = SaveConcurrent(15, false);

            foreach (var ex in exceptions)
            {
                if (!strategy.IsTransient(ex) && ex.GetType() == typeof(StorageException) && ex.InnerException.GetType() == typeof(WebException))
                {
                    var status = ((WebException)ex.InnerException).Status;
                    Assert.Fail(status.ToString());
                }
                Assert.IsTrue(strategy.IsTransient(ex), ex.ToString());
            }
        }

        [TestMethod]
        public void then_has_some_transient_errors_when_saving_using_batch()
        {
            var exceptions = SaveConcurrent(15, true);

            foreach (var ex in exceptions)
            {
                if (!strategy.IsTransient(ex) && ex.GetType() == typeof(StorageException) && ex.InnerException.GetType() == typeof(WebException))
                {
                    var status = ((WebException)ex.InnerException).Status;
                    Assert.Fail(status.ToString());
                }
                Assert.IsTrue(strategy.IsTransient(ex), ex.ToString());
            }
        }

        private List<Exception> SaveConcurrent(int numberOfCalls, bool useBatch)
        {
            // Do several calls to force some timeouts
            // or manually disconnect the network while this test is running.
            string partitionKey = Guid.NewGuid().ToString();
            var exceptions = new ConcurrentBag<Exception>();

            var entities = Enumerable.Range(0, numberOfCalls)
                .Select(x => new TestTableEntity { PartitionKey = partitionKey, RowKey = x.ToString() })
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
                        client.RetryPolicy = new NoRetry();
                        // Explicitly set a VERY short timeout, to force a timeout very frequently.
                        client.ServerTimeout = TimeSpan.FromSeconds(1);
                        client.MaximumExecutionTime = TimeSpan.FromSeconds(1);

                        var table = client.GetTableReference(tableName);
                        // Create the TableOperation that inserts the customer entity.
                        var insertOperation = TableOperation.Insert((TestTableEntity)x);

                        barrier.SignalAndWait();

                        // Execute the insert operation.
                        if (useBatch)
                        {
                            var batch = new TableBatchOperation();
                            batch.Add(insertOperation);
                            table.ExecuteBatch(batch);
                        }
                        else
                        {
                            table.Execute(insertOperation);
                        }
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

        public class TestTableEntity : TableEntity
        {
        }
    }

    [TestClass]
    public class when_accessing_invalid_account : ArrangeActAssert
    {
        protected StorageTransientErrorDetectionStrategy strategy;
        protected CloudStorageAccount account;

        protected override void Arrange()
        {
            const string validNotExisting = "DefaultEndpointsProtocol=https;AccountName=InexistantDoesntReallyMatter;AccountKey=aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa==";

            this.account = CloudStorageAccount.Parse(validNotExisting);
            this.strategy = new StorageTransientErrorDetectionStrategy();
        }

        [TestMethod]
        public void then_error_is_not_transient()
        {
            var client = account.CreateCloudBlobClient();
            client.RetryPolicy = new NoRetry();

            try
            {
                this.account.CreateCloudBlobClient().ListContainers().ToList();

                Assert.Fail("Exception not thrown");
            }
            catch (StorageException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(WebException));
                Assert.IsFalse(strategy.IsTransient(ex), ex.ToString());
            }
        }
    }
}
