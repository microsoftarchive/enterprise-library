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
using System.ServiceModel;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service
{
    /// <summary>
    /// Proxy to the WCF logging service.
    /// </summary>
    public class LoggingServiceProxy : ILoggingService, IDisposable
    {
        private ILoggingService channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingServiceProxy"/> class.
        /// </summary>
        /// <param name="factory">The channel factory for the logging service.</param>
        public LoggingServiceProxy(ChannelFactory<ILoggingService> factory)
        {
            Guard.ArgumentNotNull(factory, "factory");

            this.channel = factory.CreateChannel();
        }

        /// <summary>
        /// Asynchronously submits log entries to the server log.
        /// </summary>
        /// <param name="entries">The log entries to send to the server.</param>
        /// <param name="callback">The delegate to call when the operation finishes.</param>
        /// <param name="asyncState">The user-defined state object used to pass context data to the callback method.</param>
        /// <returns>An <see cref="IAsyncResult"/> that represents the status of the asynchronous operation.</returns>
        public IAsyncResult BeginAdd(LogEntryMessage[] entries, AsyncCallback callback, object asyncState)
        {
            return this.channel.BeginAdd(entries, callback, asyncState);
        }

        /// <summary>
        /// Called to complete the <see cref="BeginAdd"/> operation.
        /// </summary>
        /// <param name="asyncResult">An <see cref="IAsyncResult"/> that represents the status of the asynchronous operation.</param>
        public void EndAdd(IAsyncResult asyncResult)
        {
            this.channel.EndAdd(asyncResult);
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
        /// <param name="disposing"><see langword="true"/> if the method is being called from the <see cref="Dispose()"/> method. <see langword="false"/> if it is being called from within the object finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var clientChannel = (IClientChannel)this.channel;
                try
                {
                    clientChannel.Close();
                }
                catch (CommunicationException)
                {
                    clientChannel.Abort();
                }
                catch (TimeoutException)
                {
                    clientChannel.Abort();
                }
                catch (Exception)
                {
                    clientChannel.Abort();
                    throw;
                }

                using (this.channel as IDisposable) this.channel = null;
            }
        }

        /// <summary>
        /// Releases resources for a <see cref="LoggingServiceProxy"/> before garbage collection.
        /// </summary>
        ~LoggingServiceProxy()
        {
            Dispose(false);
        }
    }
}
