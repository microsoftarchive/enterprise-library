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
using System.IO.IsolatedStorage;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_repository_with_entries
{
    public abstract class Context : ArrangeActAssert
    {
        protected LogEntry logEntry0, logEntry1, logEntry2;
        protected string repositoryName;
        protected IsolatedStorageLogEntryRepository repository;

        protected const int originalSize = 2;

        protected override void Arrange()
        {
            base.Arrange();

            repositoryName = Guid.NewGuid().ToString();

            this.repository = new IsolatedStorageLogEntryRepository(repositoryName, originalSize);

            this.logEntry0 = new LogEntry
            {
                Message = "some message",
                Categories = new[] { "category1", "category2" }
            };

            this.logEntry1 = new LogEntry
            {
                Message = "some message 2",
                Categories = new[] { "category3", "category5" }
            };

            this.logEntry2 = new LogEntry
            {
                Message = "some message 3",
                Categories = new[] { "category1", "category2" },
                Severity = Diagnostics.TraceEventType.Verbose
            };

            this.repository.Add(this.logEntry0);
            this.repository.Add(this.logEntry1);
            this.repository.Add(this.logEntry2);
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
