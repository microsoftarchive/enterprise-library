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
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_trace_listener
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when : Context
    {
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void then_write_is_not_supported()
        {
            this.traceListener.Write("");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void then_write_line_is_not_supported()
        {
            this.traceListener.WriteLine("");
        }
    }
}
