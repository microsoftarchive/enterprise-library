using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Service;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    /// <summary>
    /// Trace listener that sends the log entries to a remote server.
    /// </summary>
    public class RemoteServiceTraceListener : TraceListener
    {
        private readonly bool sendImmediately;

        private readonly Func<ILoggingService> loggingServiceFactory;
        private readonly IRecurringWorkScheduler timer;
        private readonly ILogEntryMessageStore store;
        private readonly IAsyncTracingErrorReporter asyncTracingErrorReporter;
        private readonly INetworkStatus networkStatus;

        private int concurrentSendRequests;

        /// <summary>
        /// Initializes a new instance of <see cref="RemoteServiceTraceListener"/>.
        /// </summary>
        /// <param name="sendImmediately">Value indicating if the log entries should be sent shortly after they have been logged, or else 
        ///   wait until the <paramref name="timer"/> interval has elapsed.</param>
        /// <param name="loggingServiceFactory">The ffactory to create new channel instances to submit the entries to the server.</param>
        /// <param name="timer">A scheduler to retry sending the log entries when there are connectivity issues.</param>
        /// <param name="store">The store used for buffering entries.</param>
        /// <param name="asyncTracingErrorReporter"></param>
        /// <param name="networkStatus">Provides notifications for when there is a network connection to try to send the entries.</param>
        public RemoteServiceTraceListener(bool sendImmediately, 
                                          Func<ILoggingService> loggingServiceFactory,
                                          IRecurringWorkScheduler timer,
                                          ILogEntryMessageStore store,
                                          IAsyncTracingErrorReporter asyncTracingErrorReporter,
                                          INetworkStatus networkStatus)
        {
            if (loggingServiceFactory == null) throw new ArgumentNullException("loggingServiceFactory");
            if (timer == null) throw new ArgumentNullException("timer");
            if (store == null) throw new ArgumentNullException("store");
            if (asyncTracingErrorReporter == null) throw new ArgumentNullException("asyncTracingErrorReporter");
            if (networkStatus == null) throw new ArgumentNullException("networkStatus");

            this.sendImmediately = sendImmediately;
            this.loggingServiceFactory = loggingServiceFactory;
            this.timer = timer;
            this.store = store;
            this.asyncTracingErrorReporter = asyncTracingErrorReporter;
            this.networkStatus = networkStatus;

            this.timer.SetAction(DispatchEntries);
            this.networkStatus.NetworkStatusUpdated += (s, a) => this.UpdateTimerStatus();
            this.UpdateTimerStatus();
        }

        private void UpdateTimerStatus()
        {
            if (this.networkStatus.GetIsNetworkAvailable())
            {
                this.timer.Start();
                this.timer.ForceDoWork();
            }
            else
            {
                this.timer.Stop();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is thread safe.
        /// </summary>
        public override bool IsThreadSafe
        {
            get { return true; }
        }

        /// <summary>
        /// Writes trace information, a data object and event information to the listener specific output.
        /// </summary>
        /// <param name="traceEventCache">A <see cref="TraceEventCache"/> object that contains context information.</param>
        /// <param name="source">A name used to identify the output.</param>
        /// <param name="eventType">One of the <see cref="TraceEventType"/> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="data">The data.</param>
        public override void TraceData(TraceEventCache traceEventCache, string source, TraceEventType eventType, int id, object data)
        {
            var logEntry = data as LogEntry ?? new LogEntry
                                                   {
                                                       Message = (data ?? string.Empty).ToString(),
                                                       Categories = new[] { source },
                                                       Severity = eventType,
                                                       EventId = id
                                                   };

            logEntry.CollectIntrinsicProperties();

            var translatedEntry = Translate(logEntry);
            this.store.Add(translatedEntry);

            if (this.sendImmediately)
            {
                this.timer.ForceDoWork();
            }
        }

        private void DispatchEntries()
        {
            LogEntryMessage[] entriesToSend = this.store.GetEntries();

            if (entriesToSend.Length == 0)
                return;

            if (!this.networkStatus.GetIsNetworkAvailable())
                return;

            if (Interlocked.Increment(ref concurrentSendRequests) > 1)
                return;

            ILoggingService channel = null;
            try
            {
                channel = this.loggingServiceFactory.Invoke();
            }
            catch (Exception ex)
            {
                Interlocked.Exchange(ref concurrentSendRequests, 0);
                using (channel as IDisposable) { }
                this.ReportProxyError(ex);
                return;
            }

            try
            {
                channel.BeginAdd(entriesToSend, r =>
                {
                    try
                    {
                        channel.EndAdd(r);

                        this.store.RemoveUntil(entriesToSend[entriesToSend.Length - 1]);
                    }
                    catch (CommunicationException)
                    {
                        // Will retry automatically after the interval has ellapsed
                    }
                    catch (TimeoutException)
                    {
                        // Will retry automatically after the interval has ellapsed
                    }
                    catch (Exception ex)
                    {
                        this.store.RemoveUntil(entriesToSend[entriesToSend.Length - 1]);
                        this.ReportTracingError(ex, entriesToSend);
                    }
                    finally
                    {
                        int previousRequestsCount = Interlocked.Exchange(ref concurrentSendRequests, 0);
                        using (channel as IDisposable) { }

                        if (previousRequestsCount > 1)
                        {
                            this.timer.ForceDoWork();
                        }
                    }
                }, null);
            }
            catch (Exception ex)
            {
                Interlocked.Exchange(ref concurrentSendRequests, 0);
                using (channel as IDisposable) { }
                this.ReportTracingError(ex);
            }
        }

        private void ReportProxyError(Exception exception)
        {
            var message = string.Format(
                CultureInfo.CurrentCulture,
                Resources.RemoteServiceTraceListener_ReportProxyError_MessageFormat,
                this.Name,
                exception.GetType(),
                exception.Message);
            this.asyncTracingErrorReporter.ReportErrorDuringTracing(message);
        }

        private void ReportTracingError(Exception exception, LogEntryMessage[] entries = null)
        {
            var message = string.Format(
                CultureInfo.CurrentCulture,
                Resources.RemoteServiceTraceListener_ReportTracingError_GeneralExceptionMessageFormat,
                this.Name,
                exception.GetType(),
                exception.Message);

            if (entries != null && entries.Length > 0)
            {
                message += Environment.NewLine +
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.RemoteServiceTraceListener_ReportTracingError_EntriesSummaryMessageFormat,
                        CreateLogEntriesSummary(entries));
            }

            this.asyncTracingErrorReporter.ReportErrorDuringTracing(message);
        }

        private static string CreateLogEntriesSummary(IEnumerable<LogEntryMessage> entries)
        {
            const int messageMaxLength = 60;
            var entriesSummary = new StringBuilder();
            foreach (var entry in entries)
            {
                var normalizedMessage = entry.Message;
                if (normalizedMessage != null && normalizedMessage.Length > messageMaxLength)
                {
                    normalizedMessage = string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.RemoteServiceTraceListener_ReportTracingError_ShortenedLongLogEntryMessageFormat,
                        normalizedMessage.Substring(0, messageMaxLength));
                }

                entriesSummary.AppendLine(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.RemoteServiceTraceListener_ReportTracingError_LogEntryFormat,
                        entry.Severity,
                        normalizedMessage));
            }

            return entriesSummary.ToString();
        }

        protected virtual LogEntryMessage Translate(LogEntry entry)
        {
            return entry.ToLogEntryMessage();
        }

        /// <summary>
        /// This method is not supported.
        /// </summary>
        /// <param name="message">The message.</param>
        public override void Write(string message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is not supported.
        /// </summary>
        /// <param name="message">The message.</param>
        public override void WriteLine(string message)
        {
            throw new NotImplementedException();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.timer.Stop();
                using (this.timer as IDisposable) { }
                using (this.networkStatus as IDisposable) { }
                using (this.store as IDisposable) { }
            }
        }
    }
}
