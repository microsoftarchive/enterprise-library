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
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Represents a performance tracing class to log method entry/exit and duration.    
    /// </summary>
    public class TraceManager
    {
        private readonly LogWriter logWriter;
        private readonly ITracerInstrumentationProvider instrumentationProvider;

        /// <summary>
        /// For testing purpose
        /// </summary>
        public LogWriter LogWriter
        { 
            get { return this.logWriter; }
        }

        /// <summary>
        /// For testing purpose
        /// </summary>
        public ITracerInstrumentationProvider InstrumentationProvider
        {
            get { return this.instrumentationProvider; }
        }

        /// <summary>
        /// Create an instance of <see cref="TraceManager"/> giving the <see cref="LogWriter"/>.
        /// </summary>
        /// <param name="logWriter">The <see cref="LogWriter"/> that is used to write trace messages.</param>
        public TraceManager(LogWriter logWriter):
            this(logWriter, new NullTracerInstrumentationProvider())
        {
        }

        /// <summary>
        /// Create an instance of <see cref="TraceManager"/> giving the <see cref="LogWriter"/>.
        /// </summary>
        /// <param name="logWriter">The <see cref="LogWriter"/> that is used to write trace messages.</param>
        /// <param name="instrumentationProvider">The <see cref="ITracerInstrumentationProvider"/> used to determine if instrumentation should be enabled</param>
        public TraceManager(LogWriter logWriter, ITracerInstrumentationProvider instrumentationProvider)
        {
            if (logWriter == null)
            {
                throw new ArgumentNullException("logWriter");
            }

            this.logWriter = logWriter;
            this.instrumentationProvider = instrumentationProvider;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name.
        /// </summary>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        /// <returns></returns>
        public Tracer StartTrace(string operation)
        {
            return new Tracer(operation, this.logWriter, this.instrumentationProvider);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name and activity id.
        /// </summary>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        /// <param name="activityId">The activity id</param>
        /// <returns></returns>
        public Tracer StartTrace(string operation, Guid activityId)
        {
            return new Tracer(operation, activityId, this.logWriter, this.instrumentationProvider);
        }
    }
}
