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
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    /// <summary>
    /// Trace listener that wraps a normal trace listener to perform asynchronous tracing requests.
    /// </summary>
    /// <remarks>
    /// When disposed, the wrapper will attempt to complete all buffered tracing requests before completing the dispose request. To change this behavior
    /// specify a value for the disposeTimeout constructor parameter other than <see langword="null"/> or <see cref="Timeout.InfiniteTimeSpan"/>.
    /// </remarks>
    public sealed class AsynchronousTraceListenerWrapper : TraceListener, IAsynchronousTraceListener
    {
        internal const int DefaultBufferSize = 30000;
        private static readonly Lazy<TraceListener> reportingTraceListener = new Lazy<TraceListener>(() => new DefaultTraceListener());
        private readonly TraceListener wrappedTraceListener;
        private readonly bool ownsWrappedTraceListener;
        private readonly BlockingCollection<Action<TraceListener>> requests;
        private readonly int maxDegreeOfParallelism;
        private readonly TimeSpan disposeTimeout;
        private readonly Task asyncProcessingTask;
        private CancellationTokenSource closeSource;
        private int closed;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsynchronousTraceListenerWrapper" /> class.
        /// </summary>
        /// <param name="wrappedTraceListener">The wrapped trace listener.</param>
        /// <param name="ownsWrappedTraceListener">Indicates whether the wrapper should dispose the wrapped trace listener.</param>
        /// <param name="bufferSize">Size of the buffer for asynchronous requests.</param>
        /// <param name="maxDegreeOfParallelism">The max degree of parallelism for thread safe listeners. Specify <see langword="null"/> to use the current core count.</param>
        /// <param name="disposeTimeout">The timeout for waiting to complete buffered requests when disposing. When <see langword="null" /> the default of <see cref="Timeout.InfiniteTimeSpan" /> is used.</param>
        public AsynchronousTraceListenerWrapper(
            TraceListener wrappedTraceListener,
            bool ownsWrappedTraceListener = true,
            int? bufferSize = DefaultBufferSize,
            int? maxDegreeOfParallelism = null,
            TimeSpan? disposeTimeout = null)
        {
            Guard.ArgumentNotNull(wrappedTraceListener, "wrappedTraceListener");
            CheckBufferSize(bufferSize);
            CheckMaxDegreeOfParallelism(maxDegreeOfParallelism);
            CheckDisposeTimeout(disposeTimeout);

            this.wrappedTraceListener = wrappedTraceListener;
            this.ownsWrappedTraceListener = ownsWrappedTraceListener;
            this.disposeTimeout = disposeTimeout ?? Timeout.InfiniteTimeSpan;

            this.closeSource = new CancellationTokenSource();
            this.requests = bufferSize != null ? new BlockingCollection<Action<TraceListener>>(bufferSize.Value) : new BlockingCollection<Action<TraceListener>>();

            if (this.wrappedTraceListener.IsThreadSafe)
            {
                this.maxDegreeOfParallelism = maxDegreeOfParallelism.HasValue ? maxDegreeOfParallelism.Value : Environment.ProcessorCount;
                this.asyncProcessingTask = Task.Factory.StartNew(this.ProcessRequestsInParallel, CancellationToken.None, TaskCreationOptions.HideScheduler | TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
            else
            {
                this.asyncProcessingTask = Task.Factory.StartNew(this.ProcessRequests, CancellationToken.None, TaskCreationOptions.HideScheduler | TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
        }

        /// <summary>
        /// Gets the wrapped <see cref="TraceListener"/>.
        /// </summary>
        public TraceListener TraceListener
        {
            get { return this.wrappedTraceListener; }
        }

        /// <summary>
        /// When overridden in a derived class, closes the output stream so it no longer receives tracing or debugging output.
        /// </summary>
        public override void Close()
        {
            if (Interlocked.CompareExchange(ref this.closed, 1, 0) == 0)
            {
                this.TryCompleteBuffer();
                this.closeSource.Cancel();
                this.asyncProcessingTask.Wait();
                this.closeSource.Dispose();
                this.requests.Dispose();

                if (this.ownsWrappedTraceListener)
                {
                    this.wrappedTraceListener.Close();
                }
            }
        }

        /// <summary>
        /// Emits an error message to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener" /> class.
        /// </summary>
        /// <param name="message">A message to emit.</param>
        public override void Fail(string message)
        {
            this.AddRequest(tl => tl.Fail(message));
        }

        /// <summary>
        /// Emits an error message and a detailed error message to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener" /> class.
        /// </summary>
        /// <param name="message">A message to emit.</param>
        /// <param name="detailMessage">A detailed message to emit.</param>
        public override void Fail(string message, string detailMessage)
        {
            this.AddRequest(tl => tl.Fail(message, detailMessage));
        }

        /// <summary>
        /// When overridden in a derived class, flushes the output buffer.
        /// </summary>
        public override void Flush()
        {
            this.AddRequest(tl => tl.Flush());
        }

        /// <summary>
        /// Flushes the output buffer asynchronously.
        /// </summary>
        /// <param name="reportError">The delegate to use to report errors while tracing asynchronously.</param>
        public void Flush(ReportTracingError reportError)
        {
            this.AddRequest(tl =>
            {
                try
                {
                    tl.Flush();
                }
                catch (Exception e)
                {
                    if (reportError != null)
                    {
                        reportError(e, null, "");
                    }
                    else
                    {
                        throw;
                    }
                }
            });
        }

        /// <summary>
        /// Writes trace information, a data object and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache" /> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType" /> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="data">The trace data to emit.</param>
        /// <param name="reportError">The delegate to use to report errors while tracing asynchronously.</param>
        public void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data, ReportTracingError reportError)
        {
            if (this.CheckFilter(eventCache, source, eventType, id, null, null, data, null))
            {
                CaptureContextInformation(data);

                this.AddRequest(tl =>
                    {
                        try
                        {
                            tl.TraceData(eventCache, source, eventType, id, data);
                        }
                        catch (Exception e)
                        {
                            if (reportError != null)
                            {
                                reportError(e, data, source);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    });
            }
        }

        /// <summary>
        /// Writes trace information, a message, a related activity identity and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache" /> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="message">A message to write.</param>
        /// <param name="relatedActivityId">A <see cref="T:System.Guid" />  object identifying a related activity.</param>
        /// <param name="reportError">The delegate to use to report errors while tracing asynchronously.</param>
        public void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId, ReportTracingError reportError)
        {
            if (this.CheckFilter(eventCache, source, TraceEventType.Transfer, id, message, null, null, null))
            {
                this.AddRequest(tl =>
                    {
                        try
                        {
                            tl.TraceTransfer(eventCache, source, id, message, relatedActivityId);
                        }
                        catch (Exception e)
                        {
                            if (reportError != null)
                            {
                                reportError(e, message, source);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    });
            }
        }

        /// <summary>
        /// Writes trace information, a data object and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache" /> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType" /> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="data">The trace data to emit.</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
        ///   </PermissionSet>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (this.CheckFilter(eventCache, source, eventType, id, null, null, data, null))
            {
                CaptureContextInformation(data);

                this.AddRequest(tl => tl.TraceData(eventCache, source, eventType, id, data));
            }
        }

        /// <summary>
        /// Writes trace information, an array of data objects and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache" /> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType" /> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="data">An array of objects to emit as data.</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
        ///   </PermissionSet>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            if (this.CheckFilter(eventCache, source, eventType, id, null, null, null, data))
            {
                CaptureContextInformation(data);

                this.AddRequest(tl => tl.TraceData(eventCache, source, eventType, id, data));
            }
        }

        /// <summary>
        /// Writes trace and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache" /> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType" /> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
        ///   </PermissionSet>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            if (this.CheckFilter(eventCache, source, eventType, id, null, null, null, null))
            {
                this.AddRequest(tl => tl.TraceEvent(eventCache, source, eventType, id));
            }
        }

        /// <summary>
        /// Writes trace information, a formatted array of objects and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache" /> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType" /> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="format">A format string that contains zero or more format items, which correspond to objects in the <paramref name="args" /> array.</param>
        /// <param name="args">An object array containing zero or more objects to format.</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
        ///   </PermissionSet>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            if (this.CheckFilter(eventCache, source, eventType, id, format, args, null, null))
            {
                this.AddRequest(tl => tl.TraceEvent(eventCache, source, eventType, id, format, args));
            }
        }

        /// <summary>
        /// Writes trace information, a message, and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache" /> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType" /> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="message">A message to write.</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
        ///   </PermissionSet>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if (this.CheckFilter(eventCache, source, eventType, id, message, null, null, null))
            {
                this.AddRequest(tl => tl.TraceEvent(eventCache, source, eventType, id, message));
            }
        }

        /// <summary>
        /// Writes trace information, a message, a related activity identity and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache" /> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="message">A message to write.</param>
        /// <param name="relatedActivityId">A <see cref="T:System.Guid" />  object identifying a related activity.</param>
        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
            if (this.CheckFilter(eventCache, source, TraceEventType.Transfer, id, message, null, null, null))
            {
                this.AddRequest(tl => tl.TraceTransfer(eventCache, source, id, message, relatedActivityId));
            }
        }

        /// <summary>
        /// When overridden in a derived class, writes the specified message to the listener you create in the derived class.
        /// </summary>
        /// <param name="message">A message to write.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Write(string message)
        {
            this.AddRequest(tl => tl.Write(message));
        }

        /// <summary>
        /// Writes a category name and a message to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener" /> class.
        /// </summary>
        /// <param name="message">A message to write.</param>
        /// <param name="category">A category name used to organize the output.</param>
        public override void Write(string message, string category)
        {
            this.AddRequest(tl => tl.Write(message, category));
        }

        /// <summary>
        /// Writes the value of the object's <see cref="M:System.Object.ToString" /> method to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener" /> class.
        /// </summary>
        /// <param name="o">An <see cref="T:System.Object" /> whose fully qualified class name you want to write.</param>
        public override void Write(object o)
        {
            this.AddRequest(tl => tl.Write(o));
        }

        /// <summary>
        /// Writes a category name and the value of the object's <see cref="M:System.Object.ToString" /> method to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener" /> class.
        /// </summary>
        /// <param name="o">An <see cref="T:System.Object" /> whose fully qualified class name you want to write.</param>
        /// <param name="category">A category name used to organize the output.</param>
        public override void Write(object o, string category)
        {
            this.AddRequest(tl => tl.Write(o, category));
        }

        /// <summary>
        /// When overridden in a derived class, writes a message to the listener you create in the derived class, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void WriteLine(string message)
        {
            this.AddRequest(tl => tl.WriteLine(message));
        }

        /// <summary>
        /// Writes a category name and a message to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener" /> class, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write.</param>
        /// <param name="category">A category name used to organize the output.</param>
        public override void WriteLine(string message, string category)
        {
            this.AddRequest(tl => tl.WriteLine(message, category));
        }

        /// <summary>
        /// Writes the value of the object's <see cref="M:System.Object.ToString" /> method to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener" /> class, followed by a line terminator.
        /// </summary>
        /// <param name="o">An <see cref="T:System.Object" /> whose fully qualified class name you want to write.</param>
        public override void WriteLine(object o)
        {
            this.AddRequest(tl => tl.WriteLine(o));
        }

        /// <summary>
        /// Writes a category name and the value of the object's <see cref="M:System.Object.ToString" /> method to the listener you create when you implement the <see cref="T:System.Diagnostics.TraceListener" /> class, followed by a line terminator.
        /// </summary>
        /// <param name="o">An <see cref="T:System.Object" /> whose fully qualified class name you want to write.</param>
        /// <param name="category">A category name used to organize the output.</param>
        public override void WriteLine(object o, string category)
        {
            this.AddRequest(tl => tl.WriteLine(o, category));
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.Diagnostics.TraceListener" /> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "closeSource", Justification = "Disposed in the Close method")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "requests", Justification = "Disposed in the Close method")]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    this.Close();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        private static void CaptureContextInformation(object data)
        {
            var logEntry = data as LogEntry;
            if (logEntry != null)
            {
                logEntry.CollectThreadSpecificIntrinsicProperties();
            }
        }

        private static void CheckBufferSize(int? bufferSize)
        {
            if (bufferSize != null && bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException("bufferSize", Resources.ExceptionAsynchronousBufferSizeMustBePositive);
            }
        }

        private void CheckMaxDegreeOfParallelism(int? maxDegreeOfParallelism)
        {
            if (maxDegreeOfParallelism.HasValue && (maxDegreeOfParallelism.Value == 0 || maxDegreeOfParallelism.Value < -1))
            {
                throw new ArgumentOutOfRangeException("maxDegreeOfParallelism");
            }
        }

        private static void CheckDisposeTimeout(TimeSpan? disposeBufferPurgeTimeout)
        {
            if (disposeBufferPurgeTimeout.HasValue && disposeBufferPurgeTimeout.Value < TimeSpan.Zero && disposeBufferPurgeTimeout.Value != Timeout.InfiniteTimeSpan)
            {
                throw new ArgumentOutOfRangeException("disposeBufferPurgeTimeout", Resources.ExceptionAsynchronousBufferTimeoutMustBeNonNegative);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exception is logged")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2002:DoNotLockOnObjectsWithWeakIdentity", Justification = "Copied from System.Diagnostics.TraceSource")]
        private void ProcessRequests()
        {
            var token = this.closeSource.Token;
            var shouldLock = !(this.ownsWrappedTraceListener || this.wrappedTraceListener.IsThreadSafe);

            try
            {
                foreach (var request in this.requests.GetConsumingEnumerable(token))
                {
                    try
                    {
                        // It is possible that the trace listener is used elsewhere, so it must be careful about thread safety.
                        if (shouldLock)
                        {
                            lock (this.wrappedTraceListener)
                            {
                                request(this.wrappedTraceListener);
                            }
                        }
                        else
                        {
                            request(this.wrappedTraceListener);
                        }
                    }
                    catch (Exception e)
                    {
                        reportingTraceListener.Value.WriteLine(string.Format(CultureInfo.CurrentCulture, Resources.ExceptionUnknownErrorPerformingAsynchronousOperation, this.Name, e));
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // expected when the listener is closed
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exception is logged")]
        private void ProcessRequestsInParallel()
        {
            var token = this.closeSource.Token;

            try
            {
                var partitioner = Partitioner.Create(this.requests.GetConsumingEnumerable(token), EnumerablePartitionerOptions.NoBuffering);
                Parallel.ForEach(
                    partitioner,
                    new ParallelOptions { CancellationToken = token, TaskScheduler = TaskScheduler.Default, MaxDegreeOfParallelism = this.maxDegreeOfParallelism },
                    request =>
                    {
                        try
                        {
                            request(this.wrappedTraceListener);
                        }
                        catch (Exception e)
                        {
                            reportingTraceListener.Value.WriteLine(string.Format(CultureInfo.CurrentCulture, Resources.ExceptionUnknownErrorPerformingAsynchronousOperation, this.Name, e));
                        }
                    });
            }
            catch (OperationCanceledException)
            {
                // expected when the listener is closed
            }
        }

        private void AddRequest(Action<TraceListener> request)
        {
            if (!this.requests.TryAdd(request))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.ExceptionAsynchronousTraceListenerFullCapacity, this.Name));
            }
        }

        private bool CheckFilter(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data)
        {
            return this.Filter == null || this.Filter.ShouldTrace(eventCache, source, eventType, id, formatOrMessage, args, data1, data);
        }

        private void TryCompleteBuffer()
        {
            if (this.disposeTimeout == TimeSpan.Zero)
            {
                return; // no waiting
            }

            using (var bufferCompleted = new AutoResetEvent(false))
            {
                Action<TraceListener> signalComplete =
                    tl =>
                    {
                        try
                        {
                            bufferCompleted.Set();
                        }
                        catch (ObjectDisposedException)
                        {
                        }
                    };

                // don't care about the outcome of the attempt to add the buffer completed signal
                // no exceptions should result from it and it is assumed that the loop will eventually
                // complete when waiting for the signal times out
                var _ = this.TryAddBufferCompleteSignal(signalComplete, this.closeSource.Token);

                if (bufferCompleted.WaitOne(this.disposeTimeout))
                {
                    Debug.WriteLine("Completed buffered operations before timeout");
                }
                else
                {
                    Debug.WriteLine("Timed out before completing buffered operations");
                }
            }
        }

        private async Task TryAddBufferCompleteSignal(Action<TraceListener> signalComplete, CancellationToken cancellationToken)
        {
            var currentPower = 0;

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (this.requests.TryAdd(signalComplete))
                    {
                        Debug.WriteLine("Added buffer complete signal");

                        break;
                    }

                    Debug.WriteLine("Failed to add buffer complete signal");
                }
                catch (ObjectDisposedException)
                {
                    // the request will not be added
                    break;
                }
                catch (InvalidOperationException)
                {
                    // the request will not be added
                    break;
                }

                // the capacity of the request buffer was exhausted
                // attempt to add it a few more times asynchronously
                try
                {
                    var retryTimeout = Math.Pow(2.0, currentPower++) * 100; // exponentially longer waits to attempt to add the purge sentinel
                    await Task.Delay(TimeSpan.FromMilliseconds(retryTimeout), cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    // the loop is already completed, no need to add it
                    break;
                }
            }
        }
    }
}
