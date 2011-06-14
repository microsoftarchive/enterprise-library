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

using System;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_repository_on_existing_locked_file
{
    public abstract class Context : ArrangeActAssert
    {
        protected string repositoryName;
        protected IsolatedStorageLogEntryRepository repository;
        private Stream lockedStream;

        protected override void Arrange()
        {
            base.Arrange();

            repositoryName = Guid.NewGuid().ToString();
            using (var stream =
                IsolatedStorageLogEntryRepository.InitializeRepositoryStream(
                    IsolatedStorageLogEntryRepository.GetRepositoryFileName(repositoryName),
                    1024))
            { }

            this.lockedStream =
                IsolatedStorageFile.GetUserStoreForApplication()
                    .OpenFile(
                        IsolatedStorageLogEntryRepository.GetRepositoryFileName(repositoryName),
                        FileMode.Open,
                        FileAccess.ReadWrite,
                        FileShare.None);

            this.repository = new IsolatedStorageLogEntryRepository(repositoryName, 1024);
        }

        protected override void Teardown()
        {
            this.lockedStream.Dispose();
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
