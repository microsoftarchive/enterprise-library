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
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings that describe a <see cref="WmiTraceListener"/>.
    /// </summary>
    public class WmiTraceListenerData : TraceListenerData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WmiTraceListenerData"/> class.
        /// </summary>
        public WmiTraceListenerData()
            : base(typeof(WmiTraceListener))
        {
            ListenerDataType = typeof(WmiTraceListenerData);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WmiTraceListenerData"/> class.
        /// </summary>
        /// <param name="name">The name for the configuration object.</param>
        public WmiTraceListenerData(string name)
            : this(name, TraceOptions.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WmiTraceListenerData"/> class.
        /// </summary>
        /// <param name="name">The name for the configuration object.</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        public WmiTraceListenerData(string name, TraceOptions traceOutputOptions)
            : base(name, typeof(WmiTraceListener), traceOutputOptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WmiTraceListenerData"/> class.
        /// </summary>
        /// <param name="name">The name for the configuration object.</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        /// <param name="filter">The filter to apply.</param>
        public WmiTraceListenerData(string name, TraceOptions traceOutputOptions, SourceLevels filter)
            : base(name, typeof(WmiTraceListener), traceOutputOptions, filter)
        {
        }

        /// <summary>
        /// Returns a lambda expression that represents the creation of the trace listener described by this
        /// configuration object.
        /// </summary>
        /// <returns>A lambda expression to create a trace listener.</returns>
        protected override Expression<Func<TraceListener>> GetCreationExpression()
        {
            return () => new WmiTraceListener();
        }
    }
}
