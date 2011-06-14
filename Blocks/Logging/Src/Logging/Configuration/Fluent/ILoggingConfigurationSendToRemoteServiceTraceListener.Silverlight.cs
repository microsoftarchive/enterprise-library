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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Service;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface used to specify settings on a <see cref="RemoteServiceTraceListenerData"/>.
    /// </summary>
    /// <seealso cref="RemoteServiceTraceListenerData"/>
    public interface ILoggingConfigurationSendToRemoteServiceTraceListener : ILoggingConfigurationContd, ILoggingConfigurationCategoryContd, IFluentInterface
    {
        /// <summary>
        /// Specifies the time interval that will be used for submitting the log entries to the server.
        /// </summary>
        /// <param name="submitInterval">The time interval that should be used.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="RemoteServiceTraceListenerData"/>.</returns>
        /// <seealso cref="RemoteServiceTraceListener"/>
        /// <seealso cref="RemoteServiceTraceListenerData"/>
        /// <seealso cref="RemoteServiceTraceListenerData.SubmitInterval"/>
        ILoggingConfigurationSendToRemoteServiceTraceListener SetSubmitInterval(TimeSpan submitInterval);

        /// <summary>
        /// Specifies that the log entries should be sent shortly after they have been logged, without waiting until 
        /// the <see cref="RemoteServiceTraceListenerData.SubmitInterval"/> interval value has elapsed.
        /// </summary>
        /// <remarks>
        /// Do not send immediately in order to buffer as many entries as possible during the <see cref="RemoteServiceTraceListenerData.SubmitInterval"/> time,
        /// and have potentially fewer and larger requests to the server.
        /// Send immediately if you prefer to try to submit the entries as soon as possible, potentially sending only one or few log 
        /// entries per server call.
        /// </remarks>
        /// <seealso cref="RemoteServiceTraceListener"/>
        /// <seealso cref="RemoteServiceTraceListenerData"/>
        /// <seealso cref="RemoteServiceTraceListenerData.SendImmediately"/>
        ILoggingConfigurationSendToRemoteServiceTraceListener SendsImmediately();

        /// <summary>
        /// Specifies the maximum size in kilobytes to be used when storing entries into the isolated storage as a backup strategy.
        /// </summary>
        /// <param name="maxSizeInKilobytes">The maximum size in kilobytes.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="RemoteServiceTraceListenerData"/>.</returns>
        /// <seealso cref="RemoteServiceTraceListener"/>
        /// <seealso cref="RemoteServiceTraceListenerData"/>
        /// <seealso cref="RemoteServiceTraceListenerData.IsolatedStorageBufferMaxSizeInKilobytes"/>
        ILoggingConfigurationSendToRemoteServiceTraceListener SetIsolatedStorageBufferMaxSizeInKilobytes(int maxSizeInKilobytes);

        /// <summary>
        /// Specifies the maximum amount of elements that will be buffered in memory for when there are connectivity issues that prevent the listener from submitting the log entries.
        /// </summary>
        /// <param name="maxElementsInBuffer">The maximum number of items.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="RemoteServiceTraceListenerData"/>.</returns>
        /// <seealso cref="RemoteServiceTraceListener"/>
        /// <seealso cref="RemoteServiceTraceListenerData"/>
        /// <seealso cref="RemoteServiceTraceListenerData.MaxElementsInBuffer"/>
        ILoggingConfigurationSendToRemoteServiceTraceListener SetMaxElementsInBuffer(int maxElementsInBuffer);

        /// <summary>
        /// Specifies the WCF endpoint configuration name for the logging service, which
        /// must be present in the ServiceReferences.ClientConfig file of the main application. 
        /// </summary>
        /// <param name="endpointConfigurationName">The endpoint configuration name.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="RemoteServiceTraceListenerData"/>.</returns>
        /// <seealso cref="RemoteServiceTraceListener"/>
        /// <seealso cref="RemoteServiceTraceListenerData"/>
        /// <seealso cref="RemoteServiceTraceListenerData.LoggingServiceFactory"/>
        ILoggingConfigurationSendToRemoteServiceTraceListener WithLoggingServiceEndpointConfigurationName(string endpointConfigurationName);

        /// <summary>
        /// Specifies the factory for creating transient <see cref="ILoggingService"/> type instances.
        /// </summary>
        /// <param name="loggingServiceFactory">The factory instance that will be used to create transient <see cref="ILoggingService"/> type instances.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="RemoteServiceTraceListenerData"/>.</returns>
        /// <seealso cref="RemoteServiceTraceListener"/>
        /// <seealso cref="RemoteServiceTraceListenerData"/>
        /// <seealso cref="RemoteServiceTraceListenerData.LoggingServiceFactory"/>
        ILoggingConfigurationSendToRemoteServiceTraceListener WithLoggingServiceFactory(ILoggingServiceFactory loggingServiceFactory);
    }
}
