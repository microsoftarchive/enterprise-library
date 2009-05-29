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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation
{
    /// <summary/>
    public class NullTracerInstrumentationProvider : ITracerInstrumentationProvider
    {
        /// <summary/>
        public void FireTraceOperationEnded(string operationName, long elapsedTimeInMilleseconds)
        {
        }

        /// <summary/>
        public void FireTraceOperationStarted(string operationName)
        {
        }
    }
}
