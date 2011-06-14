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

using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_trace_listener
{
    public abstract class Context : ArrangeActAssert
    {
        protected Mock<ILogEntryRepository> repositoryMock;
        protected IsolatedStorageTraceListener traceListener;

        protected override void Arrange()
        {
            base.Arrange();

            this.repositoryMock = new Mock<ILogEntryRepository>();
            this.traceListener = new IsolatedStorageTraceListener(this.repositoryMock.Object);
        }
    }
}
