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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Service;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Extension methods to support configuration of <see cref="RemoteServiceTraceListener"/>.
    /// </summary>
    public static class SendToRemoteServiceTraceListenerExtensions
    {        
        /// <summary>
        /// Adds a new <see cref="RemoteServiceTraceListener"/> to the logging settings and creates
        /// a reference to this Trace Listener for the current category source.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="listenerName">The name of the <see cref="RemoteServiceTraceListener"/>.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="RemoteServiceTraceListenerData"/>. </returns>
        /// <seealso cref="RemoteServiceTraceListenerData"/>
        public static ILoggingConfigurationSendToRemoteServiceTraceListener RemoteService(this ILoggingConfigurationSendTo context, string listenerName)
        {
            if (string.IsNullOrEmpty(listenerName))
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "listenerName");

            return new SendToRemoteServiceTraceListenerBuilder(context, listenerName);
        }

        private class SendToRemoteServiceTraceListenerBuilder : SendToTraceListenerExtension, ILoggingConfigurationSendToRemoteServiceTraceListener
        {
            RemoteServiceTraceListenerData listenerData;
            public SendToRemoteServiceTraceListenerBuilder(ILoggingConfigurationSendTo context, string listenerName)
                :base(context)
            {
                listenerData = new RemoteServiceTraceListenerData
                {
                    Name = listenerName
                };

                base.AddTraceListenerToSettingsAndCategory(listenerData);
            }

            public ILoggingConfigurationSendToRemoteServiceTraceListener SetSubmitInterval(TimeSpan submitInterval)
            {
                this.listenerData.SubmitInterval = submitInterval;
                return this;
            }

            public ILoggingConfigurationSendToRemoteServiceTraceListener SendsImmediately()
            {
                this.listenerData.SendImmediately = true;
                return this;
            }

            public ILoggingConfigurationSendToRemoteServiceTraceListener SetIsolatedStorageBufferMaxSizeInKilobytes(int maxSizeInKilobytes)
            {
                this.listenerData.IsolatedStorageBufferMaxSizeInKilobytes = maxSizeInKilobytes;
                return this;
            }

            public ILoggingConfigurationSendToRemoteServiceTraceListener SetMaxElementsInBuffer(int maxElementsInBuffer)
            {
                this.listenerData.MaxElementsInBuffer = maxElementsInBuffer;
                return this;
            }

            public ILoggingConfigurationSendToRemoteServiceTraceListener WithLoggingServiceEndpointConfigurationName(string endpointConfigurationName)
            {
                var converter = new LoggingServiceFactoryEndpointConfigurationNameConverter();
                var loggingServiceFactory = converter.ConvertFromString(endpointConfigurationName) as ILoggingServiceFactory;
                return WithLoggingServiceFactory(loggingServiceFactory);
            }

            public ILoggingConfigurationSendToRemoteServiceTraceListener WithLoggingServiceFactory(ILoggingServiceFactory loggingServiceFactory)
            {
                this.listenerData.LoggingServiceFactory = loggingServiceFactory;
                return this;
            }
        }
    }
}
