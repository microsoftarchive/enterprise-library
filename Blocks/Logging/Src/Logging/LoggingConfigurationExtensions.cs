#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Provides extension methods used for programmatic configuration of the Logging Application Block.
    /// </summary>
    public static class LoggingConfigurationExtensions
    {
        /// <summary>
        /// Adds the specified <see cref="TraceListener"/> to the log source.
        /// </summary>
        /// <param name="logSource">The log source to add the trace listener to.</param>
        /// <param name="traceListener">The trace listener to add.</param>
        public static void AddTraceListener(this LogSourceData logSource, TraceListener traceListener)
        {
            logSource.Listeners.Add(traceListener);
        }

        /// <summary>
        /// Adds the specified <see cref="TraceListener"/> to the special log source.
        /// </summary>
        /// <param name="logSource">The special log source to add the trace listener to.</param>
        /// <param name="traceListener">The trace listener to add.</param>
        public static void AddTraceListener(this SpecialLogSourceData logSource, TraceListener traceListener)
        {
            logSource.Listeners.Add(traceListener);
        }

        /// <summary>
        /// Adds the specified <see cref="TraceListener"/> to the log source to work asynchronously.
        /// </summary>
        /// <param name="logSource">The log source to add the trace listener to.</param>
        /// <param name="traceListener">The trace listener to add.</param>
        /// <param name="bufferSize">The size of the buffer for asynchronous requests.</param>
        /// <param name="maxDegreeOfParallelism">The max degree of parallelism for thread safe listeners. Specify <see langword="null"/> to use the current core count.</param>
        /// <param name="disposeTimeout">The timeout for waiting to complete buffered requests when disposing. When <see langword="null" /> the default of <see cref="System.Threading.Timeout.InfiniteTimeSpan" /> is used.</param>
        public static void AddAsynchronousTraceListener(
            this LogSourceData logSource, 
            TraceListener traceListener, 
            int? bufferSize = AsynchronousTraceListenerWrapper.DefaultBufferSize, 
            int? maxDegreeOfParallelism = null, 
            TimeSpan? disposeTimeout = null)
        {
            logSource.AddTraceListener(BuildAsynchronousWrapper(traceListener, bufferSize, maxDegreeOfParallelism, disposeTimeout));
        }

        /// <summary>
        /// Adds the specified <see cref="TraceListener"/> to the special log source to work asynchronously.
        /// </summary>
        /// <param name="logSource">The special log source to add the trace listener to.</param>
        /// <param name="traceListener">The trace listener to add.</param>
        /// <param name="bufferSize">The size of the buffer for asynchronous requests.</param>
        /// <param name="maxDegreeOfParallelism">The max degree of parallelism for thread safe listeners. Specify <see langword="null"/> to use the current core count.</param>
        /// <param name="disposeTimeout">The timeout for waiting to complete buffered requests when disposing. When <see langword="null" /> the default of <see cref="System.Threading.Timeout.InfiniteTimeSpan" /> is used.</param>
        public static void AddAsynchronousTraceListener(
            this SpecialLogSourceData logSource,
            TraceListener traceListener,
            int? bufferSize = AsynchronousTraceListenerWrapper.DefaultBufferSize,
            int? maxDegreeOfParallelism = null,
            TimeSpan? disposeTimeout = null)
        {
            logSource.AddTraceListener(BuildAsynchronousWrapper(traceListener, bufferSize, maxDegreeOfParallelism, disposeTimeout));
        }

        private static TraceListener BuildAsynchronousWrapper(TraceListener traceListener, int? bufferSize, int? maxDegreeOfParallelism, TimeSpan? disposeTimeout)
        {
            return
                new AsynchronousTraceListenerWrapper(traceListener, bufferSize: bufferSize, maxDegreeOfParallelism: maxDegreeOfParallelism, disposeTimeout: disposeTimeout)
                {
                    Name = traceListener.Name,
                    Filter = traceListener.Filter
                };
        }
    }
}
