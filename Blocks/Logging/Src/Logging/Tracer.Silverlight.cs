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
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    sealed partial class Tracer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name.
        /// </summary>
        /// <remarks>
        /// If an existing activity id is already set, it will be kept. Otherwise, a new activity id will be created.
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        /// <param name="writer">The <see cref="LogWriter"/> that is used to write trace messages</param>
        public Tracer(string operation, LogWriter writer)
            : this(operation, writer, NullTracerInstrumentationProvider.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tracer"/> class with the given logical operation name and activity id.
        /// </summary>
        /// <remarks>
        /// The activity id will override a previous activity id
        /// </remarks>
        /// <param name="operation">The operation for the <see cref="Tracer"/></param>
        /// <param name="activityId">The activity id</param>
        /// <param name="writer">The <see cref="LogWriter"/> that is used to write trace messages</param>
        public Tracer(string operation, Guid activityId, LogWriter writer)
            : this(operation, activityId, writer, NullTracerInstrumentationProvider.Default)
        {
        }

        /// <summary>
        /// Captures the tracing context on the current thread so tracing can be resumed in a different thread.
        /// </summary>
        /// <returns>A <see cref="ICapturedTracingContext"/> that can be used to restore the tracing context.</returns>
        /// <seealso cref="ICapturedTracingContext.Restore()"/>
        public static ICapturedTracingContext CaptureTracingContext()
        {
            return new CapturedTracingContext();
        }

        internal static bool IsTracingAvailable()
        {
            return true;
        }

        private static ITracerInstrumentationProvider GetTracerInstrumentationProvider(IServiceLocator serviceLocator)
        {
            Guard.ArgumentNotNull(serviceLocator, "serviceLocator");

            return NullTracerInstrumentationProvider.Default;
        }

        private LogWriter GetWriter()
        {
            return writer;
        }

        private class CapturedTracingContext : ICapturedTracingContext
        {
            private Guid capturedActivityId;
            private object[] capturedLogicalOperationStackElements;

            public CapturedTracingContext()
            {
                this.capturedActivityId = Trace.CorrelationManager.ActivityId;
                this.capturedLogicalOperationStackElements = Trace.CorrelationManager.LogicalOperationStack.ToArray();
            }

            public IDisposable Restore()
            {
                if (this.capturedLogicalOperationStackElements == null)
                {
                    throw new InvalidOperationException(Resources.ErrorCanOnlyRestoreTracingContextOnce);
                }

                var cleanup = new RestoredTracingContext(this.capturedActivityId, this.capturedLogicalOperationStackElements);

                this.capturedLogicalOperationStackElements = null;

                return cleanup;
            }

            private class RestoredTracingContext : IDisposable
            {
                private Guid currentActivityId;
                private object[] currentLogicalOperationStackElements;

                public RestoredTracingContext(Guid newActivityId, object[] newLogicalOperationStackElements)
                {
                    this.currentActivityId = Trace.CorrelationManager.ActivityId;
                    this.currentLogicalOperationStackElements = Trace.CorrelationManager.LogicalOperationStack.ToArray();

                    Trace.CorrelationManager.ActivityId = newActivityId;
                    ResetStack(Trace.CorrelationManager.LogicalOperationStack, newLogicalOperationStackElements);
                }

                public void Dispose()
                {
                    Trace.CorrelationManager.ActivityId = this.currentActivityId;
                    ResetStack(Trace.CorrelationManager.LogicalOperationStack, this.currentLogicalOperationStackElements);
                }

                private static void ResetStack(Stack<object> stack, object[] newElements)
                {
                    stack.Clear();
                    for (int i = 0; i < newElements.Length; i++)
                    {
                        stack.Push(newElements[i]);
                    }
                }
            }
        }
    }
}
