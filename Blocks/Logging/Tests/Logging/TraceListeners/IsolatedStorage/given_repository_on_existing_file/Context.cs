using System;
using System.IO.IsolatedStorage;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_repository_on_existing_file
{
    public abstract class Context : ArrangeActAssert
    {
        protected string repositoryName;
        protected IsolatedStorageLogEntryRepository repository;

        protected override void Arrange()
        {
            base.Arrange();

            repositoryName = Guid.NewGuid().ToString();
            using (var stream =
                IsolatedStorageLogEntryRepository.InitializeRepositoryStream(
                    IsolatedStorageLogEntryRepository.GetRepositoryFileName(repositoryName),
                    1))
            { }

            this.repository = new IsolatedStorageLogEntryRepository(repositoryName, 1);
        }

        protected override void Teardown()
        {
            this.repository.Dispose();

            var path = IsolatedStorageLogEntryRepository.GetRepositoryFileName(repositoryName);
            if (IsolatedStorageFile.GetUserStoreForApplication().FileExists(path))
            {
                IsolatedStorageFile.GetUserStoreForApplication().DeleteFile(path);
            }

            base.Teardown();
        }
    }
}
