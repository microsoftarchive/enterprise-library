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
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings for any trace listener.
    /// </summary>
    public class SystemDiagnosticsTraceListenerData
        : BasicCustomTraceListenerData
    {
        /// <summary>
        /// Initializes with default values.
        /// </summary>
        public SystemDiagnosticsTraceListenerData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SystemDiagnosticsTraceListenerData"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="initData">The init data.</param>
        public SystemDiagnosticsTraceListenerData(string name, Type type, string initData)
            : base(name, type, initData)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:SystemDiagnosticsTraceListenerData"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="typeName">The type.</param>
        /// <param name="initData">The init data.</param>
        public SystemDiagnosticsTraceListenerData(string name, string typeName, string initData)
            : base(name, typeName, initData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SystemDiagnosticsTraceListenerData"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="initData">The init data.</param>
        /// <param name="traceOutputOptions">The trace output options.</param>
        public SystemDiagnosticsTraceListenerData(string name, Type type, string initData,
                                                  TraceOptions traceOutputOptions)
            : base(name, type, initData, traceOutputOptions)
        {
        }
    }
}
