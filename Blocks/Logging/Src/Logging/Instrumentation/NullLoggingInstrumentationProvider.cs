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
    public class NullLoggingInstrumentationProvider : ILoggingInstrumentationProvider
    {
        /// <summary/>
        public void FireLockAcquisitionError(string message)
        {
        }

        /// <summary/>
        public void FireConfigurationFailureEvent(Exception configurationException)
        {
        }

        /// <summary/>
        public void FireFailureLoggingErrorEvent(string message, Exception exception)
        {
        }

        /// <summary/>
        public void FireLogEventRaised()
        {
        }

        /// <summary/>
        public void FireTraceListenerEntryWrittenEvent()
        {
        }

        /// <summary/>
        public void FireReconfigurationErrorEvent(Exception exception)
        {
        }
    }
}
