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
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Represents a performance tracing class to log method entry/exit and duration.    
    /// </summary>
    public class TraceManager
    {
        private LogWriter logWriter;
        private TracerInstrumentationListener instrumentationListener;

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
        public TracerInstrumentationListener InstrumentationListener
        {
            get { return this.instrumentationListener; }
        }

        /// <summary>
        /// Create an instance of <see cref="TraceManager"/> giving the <see cref="LogWriter"/>.
        /// </summary>
        /// <param name="logWriter">The <see cref="LogWriter"/> that is used to write trace messages.</param>
        public TraceManager(LogWriter logWriter):
            this(logWriter, null)
        {
        }

        /// <summary>
        /// Create an instance of <see cref="TraceManager"/> giving the <see cref="LogWriter"/>.
        /// </summary>
        /// <param name="logWriter">The <see cref="LogWriter"/> that is used to write trace messages.</param>
        /// <param name="instrumentationListener">The <see cref="TracerInstrumentationListener"/> used to determine if instrumentation should be enabled</param>
        public TraceManager(LogWriter logWriter, TracerInstrumentationListener instrumentationListener)
        {
            if (logWriter == null)
            {
                throw new ArgumentNullException("logWriter");
            }

            this.logWriter = logWriter;
            this.instrumentationListener = instrumentationListener;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name.
        /// </summary>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        /// <returns></returns>
        public Tracer StartTrace(string operation)
        {
            return new Tracer(operation, this.logWriter, this.instrumentationListener);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name and activity id.
        /// </summary>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        /// <param name="activityId">The activity id</param>
        /// <returns></returns>
        public Tracer StartTrace(string operation, Guid activityId)
        {
            return new Tracer(operation, activityId, this.logWriter, this.instrumentationListener);
        }
    }
}
