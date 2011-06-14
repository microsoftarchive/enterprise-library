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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_repository_file_does_not_exist
{
    public abstract class Context : ArrangeActAssert
    {
        protected string repositoryName;

        protected override void Arrange()
        {
            base.Arrange();

            repositoryName = Guid.NewGuid().ToString();
        }

        protected override void Teardown()
        {
            base.Teardown();

            var path = Path.Combine("__logging", repositoryName);
            if (IsolatedStorageFile.GetUserStoreForApplication().FileExists(path))
            {
                IsolatedStorageFile.GetUserStoreForApplication().DeleteFile(path);
            }
        }
    }
}
