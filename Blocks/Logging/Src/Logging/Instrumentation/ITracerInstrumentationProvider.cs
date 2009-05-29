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
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation
{

    /// <summary/>
    public interface ITracerInstrumentationProvider
    {
        /// <summary/>
        void FireTraceOperationEnded(string operationName, long elapsedTimeInMilleseconds);

        /// <summary/>
        void FireTraceOperationStarted(string operationName);
    }
}
