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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_storage_with_entries
{
    public abstract class Context : ArrangeActAssert
    {
        protected MemoryStream stream;
        protected BoundedStreamStorage storage;
        protected byte[] existingEntry1;
        protected byte[] existingEntry2;
        protected byte[] existingEntry3;

        protected override void Arrange()
        {
            base.Arrange();

            this.stream = new MemoryStream();
            BoundedStreamStorage.Initialize(this.stream, 1024, 1024);

            this.storage = new BoundedStreamStorage(this.stream);

            this.existingEntry1 = Enumerable.Repeat<byte>(65, 256).ToArray();
            this.existingEntry2 = Enumerable.Repeat<byte>(66, 512).ToArray();
            this.existingEntry3 = Enumerable.Repeat<byte>(66, 64).ToArray();

            this.storage.Add(this.existingEntry1);
            this.storage.Add(this.existingEntry2);
            this.storage.Add(this.existingEntry3);
        }

        protected override void Teardown()
        {
            base.Teardown();

            this.stream.Dispose();
        }
    }
}
