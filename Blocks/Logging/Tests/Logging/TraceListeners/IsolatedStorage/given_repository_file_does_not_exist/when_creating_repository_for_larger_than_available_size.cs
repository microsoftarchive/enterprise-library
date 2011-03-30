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
        private int maxSizeInBytes;

        protected override void Act()
        {
            base.Act();

            this.maxSizeInBytes = (int)IsolatedStorageFile.GetUserStoreForApplication().AvailableFreeSpace * 11 / 10;
            this.repository = new IsolatedStorageLogEntryRepository(this.repositoryName, maxSizeInBytes);
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
            Assert.AreEqual(this.maxSizeInBytes, this.repository.MaxSizeInBytes);
            Assert.IsTrue(this.maxSizeInBytes > this.repository.ActualMaxSizeInBytes);
        }
    }
}
