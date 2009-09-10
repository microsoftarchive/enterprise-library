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
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    /// <summary>
    /// A <see cref="TraceListener"/> wrapper that reacts to update requests in the Logging Application Block disposing the
    /// listener it currently wraps and resolving a new one.
    /// </summary>
    public class ReconfigurableTraceListenerWrapper : TraceListenerWrapper, ILoggingUpdateHandler
    {
        private readonly ILoggingUpdateCoordinator coordinator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReconfigurableTraceListenerWrapper"/> class with an initial
        /// <see cref="TraceListener"/> to wrap and the <see cref="ILoggingUpdateCoordinator"/> that notifies of
        /// update requests.
        /// </summary>
        /// <param name="wrappedTraceListener">The <see cref="TraceListener"/> to wrap.</param>
        /// <param name="coordinator">The coordinator for updates in the Logging Application Block.</param>
        public ReconfigurableTraceListenerWrapper(
            TraceListener wrappedTraceListener,
            ILoggingUpdateCoordinator coordinator)
        {
            if (wrappedTraceListener == null)
            {
                throw new ArgumentNullException("wrappedTraceListener");
            }
            if (coordinator == null)
            {
                throw new ArgumentNullException("coordinator");
            }

            this.wrappedTraceListener = wrappedTraceListener;
            this.coordinator = coordinator;

            this.coordinator.RegisterLoggingUpdateHandler(this);
        }

        private TraceListener wrappedTraceListener;

        /// <summary>
        /// Gets the wrapped <see cref="TraceListener"/>.
        /// </summary>
        public override TraceListener InnerTraceListener
        {
            get { return this.wrappedTraceListener; }
        }

        ///<summary>
        /// Prepares to update it's internal state, but does not commit this until <see cref="ILoggingUpdateHandler.CommitUpdate"/>
        ///</summary>
        public object PrepareForUpdate(IServiceLocator serviceLocator)
        {
            var newTraceListener = serviceLocator.GetInstance<TraceListener>(this.wrappedTraceListener.Name);
            return newTraceListener;

        }

        ///<summary>
        /// Commits the update of internal state.
        ///</summary>
        public void CommitUpdate(object context)
        {
            var currentTraceListener = this.wrappedTraceListener;
            this.wrappedTraceListener = (TraceListener)context;
            currentTraceListener.Dispose();
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:SelfUpdatingTraceListenerWrapper"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources. 
        /// </param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                this.coordinator.UnregisterLoggingUpdateHandler(this);
            }
        }
    }
}
