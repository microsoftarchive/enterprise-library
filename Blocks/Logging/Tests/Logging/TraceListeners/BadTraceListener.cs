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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners
{
    public class BadTraceListener : CustomTraceListener
    {
        private Exception exceptionToThrow;

        public BadTraceListener(Exception exceptionToThrow)
        {
            this.exceptionToThrow = exceptionToThrow;
        }

        public override void Write(string message)
        {
            throw exceptionToThrow;
        }

        public override void WriteLine(string message)
        {
            throw exceptionToThrow;
        }
    }
}
