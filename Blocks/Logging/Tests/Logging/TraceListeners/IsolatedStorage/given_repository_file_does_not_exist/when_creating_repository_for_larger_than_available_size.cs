//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_repository_file_does_not_exist
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_creating_repository_for_larger_than_available_size : Context
    {
        private IsolatedStorageLogEntryRepository repository;
        private int maxSizeInKilobytes;

        protected override void Act()
        {
            base.Act();

            this.maxSizeInKilobytes = (int)IsolatedStorageFile.GetUserStoreForApplication().AvailableFreeSpace * 11 / 10 / 1024;
            this.repository = new IsolatedStorageLogEntryRepository(this.repositoryName, maxSizeInKilobytes);
        }

        protected override void Teardown()
        {
            this.repository.Dispose();

            base.Teardown();
        }

        [TestMethod]
        public void then_creates_repository_file()
        {
            Assert.IsTrue(
                IsolatedStorageFile.GetUserStoreForApplication().FileExists(Path.Combine("__logging", this.repositoryName)));
        }

        [TestMethod]
        public void then_repository_has_lower_than_specified_max_size()
        {
            Assert.AreEqual(this.maxSizeInKilobytes, this.repository.MaxSizeInKilobytes);
            Assert.IsTrue(this.maxSizeInKilobytes > this.repository.ActualMaxSizeInKilobytes);
        }
    }
}
