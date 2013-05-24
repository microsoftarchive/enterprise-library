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

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests.Storage.given_storage_detection_strategy
{
    using System;
    using System.IO;
    using System.Net;
    using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class Context : ArrangeActAssert
    {
        protected StorageTransientErrorDetectionStrategy strategy;

        protected override void Arrange()
        {
            this.strategy = new StorageTransientErrorDetectionStrategy();
        }
    }

    [TestClass]
    public class when_checking_exceptions : Context
    {
        [TestMethod]
        public void then_determines_null_failure_is_non_transient()
        {
            Assert.IsFalse(this.strategy.IsTransient(null));
        }

        [TestMethod]
        public void then_determines_transient_failure_is_transient()
        {
            Assert.IsTrue(this.strategy.IsTransient(new TimeoutException()));
        }

        [TestMethod]
        public void then_determines_non_transient_failure_is_non_transient()
        {
            Assert.IsFalse(this.strategy.IsTransient(new ApplicationException()));
        }

        [TestMethod]
        public void then_determines_transient_failure_inner_of_non_transient_failure_is_transient()
        {
            Assert.IsTrue(this.strategy.IsTransient(new ApplicationException("transient", new WebException("transient", WebExceptionStatus.Timeout))));
        }

        [TestMethod]
        public void then_determines_transient_failure_inner_of_non_transient_failure_inner_of_non_transient_failure_is_transient()
        {
            Assert.IsFalse(this.strategy.IsTransient(new ApplicationException("transient", new ApplicationException("transient", new WebException("transient", WebExceptionStatus.ProtocolError)))));
        }

        [TestMethod]
        public void then_determines_assembly_load_exeption_is_not_transient()
        {
            Assert.IsFalse(this.strategy.IsTransient(new FileLoadException()));
        }
        
        [TestMethod]
        public void then_determines_IOException_is_transient()
        {
            Assert.IsTrue(this.strategy.IsTransient(new IOException("Unable to read data from the transport connection: The connection was closed.")));
        }
    }

    [TestClass]
    public class TypesCheckerFixture
    {
        [TestMethod]
        public void ValidateConstantsAreInSyncWithManagedApiV1()
        {
            // because we are avoiding referencing external constants due to possible assembly
            // load failures, this test validates that the constants are in sync accross releases.

            string[] expected = new[] 
            {
                Microsoft.WindowsAzure.StorageClient.StorageErrorCodeStrings.InternalError, 
                Microsoft.WindowsAzure.StorageClient.StorageErrorCodeStrings.ServerBusy, 
                Microsoft.WindowsAzure.StorageClient.StorageErrorCodeStrings.OperationTimedOut, 
                Microsoft.WindowsAzure.StorageClient.TableErrorCodeStrings.TableServerOutOfMemory, 
                Microsoft.WindowsAzure.StorageClient.TableErrorCodeStrings.TableBeingDeleted 
            };

            CollectionAssert.AreEquivalent(expected, StorageTransientErrorDetectionStrategy.TransientStorageErrorCodeStrings);
        }

        [TestMethod]
        public void ValidateConstantsAreInSyncWithManagedApiV2()
        {
            // because we are avoiding referencing external constants due to possible assembly
            // load failures, this test validates that the constants are in sync accross releases.

            string[] expected = new[] 
            {
                Microsoft.WindowsAzure.Storage.Shared.Protocol.StorageErrorCodeStrings.InternalError, 
                Microsoft.WindowsAzure.Storage.Shared.Protocol.StorageErrorCodeStrings.ServerBusy, 
                Microsoft.WindowsAzure.Storage.Shared.Protocol.StorageErrorCodeStrings.OperationTimedOut, 
                Microsoft.WindowsAzure.Storage.Table.Protocol.TableErrorCodeStrings.TableServerOutOfMemory, 
                Microsoft.WindowsAzure.Storage.Table.Protocol.TableErrorCodeStrings.TableBeingDeleted 
            };

            CollectionAssert.AreEquivalent(expected, StorageTransientErrorDetectionStrategy.TransientStorageErrorCodeStrings);
        }
    }
}
