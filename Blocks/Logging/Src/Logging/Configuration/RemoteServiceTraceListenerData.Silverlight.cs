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
using System.ComponentModel;
using System.Globalization;
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Service;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Configuration for the <see cref="RemoteServiceTraceListener"/>.
    /// </summary>
    public class RemoteServiceTraceListenerData : TraceListenerData
    {
        /// <summary>
        /// Initializes a new instance of <see cref="RemoteServiceTraceListenerData"/>.
        /// </summary>
        public RemoteServiceTraceListenerData()
        {
            this.SubmitInterval = TimeSpan.FromMinutes(1);
            this.MaxElementsInBuffer = 100;
        }

        /// <summary>
        /// Gets the creation expression used to produce a <see cref="TypeRegistration"/> during
        /// <see cref="TraceListenerData.GetRegistrations"/>.
        /// </summary>
        /// <returns>A <see cref="Expression"/> that creates a <see cref="RemoteServiceTraceListener"/>.</returns>
        protected override Expression<Func<TraceListener>> GetCreationExpression()
        {
            if (this.SubmitInterval <= TimeSpan.Zero)
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorSubmitIntervalInvalidRange, this.Name));

            if (this.LoggingServiceFactory == null)
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorLoggingServiceFactoryNotSet, this.Name));

            LogEntryMessageStore.GuardMaxSizeInKilobytes(IsolatedStorageBufferMaxSizeInKilobytes);

            return () => new RemoteServiceTraceListener(
                this.SendImmediately,
                this.LoggingServiceFactory,
                new RecurringWorkScheduler(this.SubmitInterval),
                new LogEntryMessageStore(this.Name, this.MaxElementsInBuffer, this.IsolatedStorageBufferMaxSizeInKilobytes),
                Container.Resolved<IAsyncTracingErrorReporter>(),
                new NetworkStatus());
        }

        /// <summary>
        /// Gets or sets the time interval that will be used for submitting the log entries to the server.
        /// </summary>
        public TimeSpan SubmitInterval { get; set; }

        /// <summary>
        /// Gets or sets the maximum amount of elements that will be buffered in memory for when there are connectivity issues that prevent the listener from submitting the log entries.
        /// </summary>
        public int MaxElementsInBuffer { get; set; }

        /// <summary>
        /// Gets or sets the maximum size in kilobytes to be used when storing entries into the isolated storage as a backup strategy.
        /// </summary>
        public int IsolatedStorageBufferMaxSizeInKilobytes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the log entries should be sent shortly after they have been logged, or else 
        /// wait until the <see cref="SubmitInterval"/> interval value has elapsed.
        /// </summary>
        /// <remarks>
        /// Set this value to <see langword="false"/> in order to buffer as many entries as possible during the <see cref="SubmitInterval"/> time,
        /// and have potentially fewer and larger requests to the server.
        /// Set this value to <see langword="true"/> if you prefer to try to submit the entries as soon as possible, potentially sending only one or few log 
        /// entries per server call.
        /// </remarks>
        public bool SendImmediately { get; set; }

        /// <summary>
        /// Gets or set a factory for creating transient <see cref="ILoggingService"/> type instances.
        /// In XAML, you can set this property value to a string that represents the WCF endpoint configuration name, which
        /// must be present in the ServiceReferences.ClientConfig file of the main application. </summary>
        /// <remarks>
        /// Set this property to an instance of <see cref="ILoggingServiceFactory"/> if supplying an endpoint configuration
        /// name is not enough to configure how you connect to the server. 
        /// </remarks>
        [TypeConverter(typeof(LoggingServiceFactoryEndpointConfigurationNameConverter))]
        public ILoggingServiceFactory LoggingServiceFactory { get; set; }
    }
}
