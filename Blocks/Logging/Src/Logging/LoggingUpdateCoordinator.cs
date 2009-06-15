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
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Coordinates logging operations with updates to the logging stack.
    /// </summary>
    /// <remarks>
    /// Implements the <see cref="ILoggingUpdateCoordinator"/> interface using a <see cref="ReaderWriterLock"/>.
    /// </remarks>
    public class LoggingUpdateCoordinator : ILoggingUpdateCoordinator, IDisposable
    {
        private const int DefaltReadLockAcquireTimeout = 5000;
        private const int DefaltWriteLockAcquireTimeout = 5000;

        private readonly ConfigurationChangeEventSource eventSource;
        private readonly ILoggingInstrumentationProvider instrumentationProvider;
        private readonly ReaderWriterLockSlim accessLock;
        private readonly List<ILoggingUpdateHandler> handlers = new List<ILoggingUpdateHandler>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingUpdateCoordinator"/> class with 
        /// a <see cref="ConfigurationChangeEventSource"/>.
        /// </summary>
        /// <remarks>
        /// The LoggingUpdateCoordinator attaches itself to the <see cref="ConfigurationChangeEventSource"/> events.
        /// </remarks>
        /// <param name="eventSource">The source for configuration change events.</param>

        public LoggingUpdateCoordinator(ConfigurationChangeEventSource eventSource)
            : this(eventSource, new NullLoggingInstrumentationProvider())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingUpdateCoordinator"/> class with 
        /// a <see cref="ConfigurationChangeEventSource"/>.
        /// </summary>
        /// <remarks>
        /// The LoggingUpdateCoordinator attaches itself to the <see cref="ConfigurationChangeEventSource"/> events.
        /// </remarks>
        /// <param name="eventSource">The source for configuration change events.</param>
        /// <param name="instrumentationProvider">The <see cref="ILoggingInstrumentationProvider"/> to use for exception and instrumentation event notification.</param>
        public LoggingUpdateCoordinator(ConfigurationChangeEventSource eventSource,
            ILoggingInstrumentationProvider instrumentationProvider)
        {
            this.accessLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

            this.eventSource = eventSource;
            this.instrumentationProvider = instrumentationProvider;
            if (this.eventSource != null)
            {
                this.eventSource.GetSection<LoggingSettings>().SectionChanged += OnConfigurationChanged;
            }
        }


        ///<summary>
        /// Registers a logging update handler for responding to updated events.
        ///</summary>
        ///<param name="loggingUpdateHandler">The handler to register.</param>
        public void RegisterLoggingUpdateHandler(ILoggingUpdateHandler loggingUpdateHandler)
        {
            lock (handlers)
            {
                handlers.Add(loggingUpdateHandler);
            }
        }

        ///<summary>
        /// Unregisters a logging update handler for responding to updated events.
        ///</summary>
        ///<param name="loggingUpdateHandler">The handler to unregister.</param>
        public void UnregisterLoggingUpdateHandler(ILoggingUpdateHandler loggingUpdateHandler)
        {
            lock (handlers)
            {
                handlers.Remove(loggingUpdateHandler);
            }
        }

        /// <summary>
        /// Executes the supplied <see cref="Action"/> when no updates are being performed.
        /// </summary>
        /// <remarks>No updates to the logging objects should be performed by the supplied action.</remarks>
        /// <param name="action">The <see cref="Action"/> to execute.</param>
        public void ExecuteReadOperation(Action action)
        {
            this.accessLock.EnterReadLock();
            try
            {
                action();
            }
            finally
            {
                this.accessLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Executes the supplied <see cref="Action"/> in isolation.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to execute.</param>
        public void ExecuteWriteOperation(Action action)
        {
            this.accessLock.EnterWriteLock();
            try
            {
                action();
            }
            finally
            {
                this.accessLock.ExitWriteLock();
            }
        }

        private void OnConfigurationChanged(object sender, SectionChangedEventArgs<LoggingSettings> args)
        {
            ILoggingUpdateHandler[] executingHandlers = null;
            lock (handlers)
            {
                executingHandlers = handlers.ToArray();
            }

            var contextList = new Dictionary<ILoggingUpdateHandler, object>();

            try
            {
                Array.ForEach(executingHandlers, h => contextList.Add(h, h.PrepareForUpdate(args.Container)));
            }
            catch (ActivationException e)
            {
                instrumentationProvider.FireConfigurationFailureEvent(e);
                return;
            }
            catch (Exception e)
            {
                instrumentationProvider.FireReconfigurationErrorEvent(e);
                return;
            }

            ExecuteWriteOperation(() => Array.ForEach(executingHandlers, h => h.CommitUpdate(contextList[h])));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.eventSource != null)
                {
                    this.eventSource.GetSection<LoggingSettings>().SectionChanged -= OnConfigurationChanged;
                }
            }
        }
    }
}
