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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_disposed_storage
{
    public abstract class Context : ArrangeActAssert
    {
        protected MemoryStream stream;
        protected BoundedStreamStorage storage;

        protected override void Arrange()
        {
            base.Arrange();

            this.stream = new MemoryStream();
            BoundedStreamStorage.Initialize(this.stream, 1024, 1024);

            this.storage = new BoundedStreamStorage(this.stream);
            this.storage.Add(new byte[100]);

            this.storage.Dispose();
        }
    }
}
