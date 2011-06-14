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
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Extension methods to support configuration of <see cref="IsolatedStorageTraceListener"/>.
    /// </summary>
    public static class SendToIsolatedStorageTraceListenerExtensions
    {        
        /// <summary>
        /// Adds a new <see cref="IsolatedStorageTraceListener"/> to the logging settings and creates
        /// a reference to this Trace Listener for the current category source.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="listenerName">The name of the <see cref="IsolatedStorageTraceListener"/>.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="IsolatedStorageTraceListenerData"/>. </returns>
        /// <seealso cref="IsolatedStorageTraceListenerData"/>
        public static ILoggingConfigurationSendToIsolatedStorageTraceListener IsolatedStorage(this ILoggingConfigurationSendTo context, string listenerName)
        {
            if (string.IsNullOrEmpty(listenerName))
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "listenerName");

            return new SendToIsolatedStorageTraceListenerBuilder(context, listenerName);
        }

        private class SendToIsolatedStorageTraceListenerBuilder : SendToTraceListenerExtension, ILoggingConfigurationSendToIsolatedStorageTraceListener
        {
            IsolatedStorageTraceListenerData listenerData;
            public SendToIsolatedStorageTraceListenerBuilder(ILoggingConfigurationSendTo context, string listenerName)
                :base(context)
            {
                listenerData = new IsolatedStorageTraceListenerData
                {
                    Name = listenerName
                };

                base.AddTraceListenerToSettingsAndCategory(listenerData);
            }

            public ILoggingConfigurationSendToIsolatedStorageTraceListener WithRepositoryName(string repositoryName)
            {
                this.listenerData.RepositoryName = repositoryName;
                return this;
            }

            public ILoggingConfigurationSendToIsolatedStorageTraceListener SetMaxSizeInKilobytes(int maxSizeInKilobytes)
            {
                this.listenerData.MaxSizeInKilobytes = maxSizeInKilobytes;
                return this;
            }
        }
    }
}
