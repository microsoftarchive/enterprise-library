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
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service
{
    /// <summary>
    /// Factory that creates a transient WCF <see cref="IChannel"/> for accessing the logging service.
    /// </summary>
    public class LoggingServiceFactory : ILoggingServiceFactory, IDisposable
    {
        private ChannelFactory<ILoggingService> channelFactory;
        private string endpointConfigurationName;

        /// <summary>
        /// The WCF endpoint configuration name. This configuration must be present in the
        /// ServiceReferences.ClientConfig file of the main application.
        /// </summary>
        public string EndpointConfigurationName
        {
            get { return this.endpointConfigurationName; }
            set
            {
                using (this.channelFactory) { this.channelFactory = null; }

                if (!string.IsNullOrEmpty(value))
                {
                    this.channelFactory = new ChannelFactory<ILoggingService>(value);
                }

                this.endpointConfigurationName = value;
            }
        }

        /// <summary>
        /// Creates an <see cref="ILoggingService"/> channel that is used to send messages to the service.
        /// </summary>
        /// <returns>The transient <see cref="ILoggingService"/> of type <see cref="IChannel"/> created by the factory.</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public virtual ILoggingService CreateChannel()
        {
            if (this.channelFactory == null)
            {
                throw new InvalidOperationException(Resources.LoggingServiceFactory_EndpointConfigurationNameNotSet);
            }

            return new LoggingServiceProxy(this.channelFactory);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if the method is being called from the <see cref="Dispose()"/> method. <see langword="false"/> if it is being called from within the object finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                using (this.channelFactory) { this.channelFactory = null; }
            }
        }

        /// <summary>
        /// Releases resources for the <see cref="LoggingServiceFactory"/> instance before garbage collection.
        /// </summary>
        ~LoggingServiceFactory()
        {
            this.Dispose(false);
        }
    }
}
