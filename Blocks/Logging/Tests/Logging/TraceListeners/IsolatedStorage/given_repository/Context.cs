using System;
using System.IO.IsolatedStorage;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_repository
{
    public abstract class Context : ArrangeActAssert
    {
        protected string repositoryName;
        protected IsolatedStorageLogEntryRepository repository;

        protected override void Arrange()
        {
            base.Arrange();

            repositoryName = Guid.NewGuid().ToString();

            this.repository = new IsolatedStorageLogEntryRepository(repositoryName, 2048);
        }

        protected override void Teardown()
        {
            if (this.repository != null)
            {
                this.repository.Dispose();
            }

            var path = IsolatedStorageLogEntryRepository.GetRepositoryFileName(repositoryName);
            if (IsolatedStorageFile.GetUserStoreForApplication().FileExists(path))
            {
                IsolatedStorageFile.GetUserStoreForApplication().DeleteFile(path);
            }

            base.Teardown();
        }
    }
}
