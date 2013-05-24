#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport.TraceListeners
{
    public class MockDisposableTraceListener : TraceListener, IDisposable
    {
        public int DisposedCalls;

        public override void Write(string message)
        {
        }

        public override void WriteLine(string message)
        {
        }

        public new void Dispose()
        {
            this.DisposedCalls++;
        }
    }
}
