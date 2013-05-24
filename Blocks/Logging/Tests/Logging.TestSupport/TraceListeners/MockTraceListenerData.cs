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

using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport.TraceListeners
{
    public class MockTraceListenerData : TraceListenerData
    {
        public MockTraceListenerData()
        {
        }

        public MockTraceListenerData(string name)
            : base(name, typeof(MockTraceListener), TraceOptions.None, SourceLevels.All)
        {
        }

        protected override TraceListener CoreBuildTraceListener(LoggingSettings settings)
        {
            return new MockTraceListener(this.Name);
        }
    }
}
