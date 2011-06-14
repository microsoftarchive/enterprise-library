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
    /// Extension methods to support configuration of <see cref="NotificationTraceListener"/>.
    /// </summary>
    public static class SendToNotificationTraceListenerExtensions
    {        
        /// <summary>
        /// Adds a new <see cref="NotificationTraceListener"/> to the logging settings and creates
        /// a reference to this Trace Listener for the current category source.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="listenerName">The name of the <see cref="NotificationTraceListener"/>.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="NotificationTraceListenerData"/>. </returns>
        /// <seealso cref="NotificationTraceListenerData"/>
        public static ILoggingConfigurationSendToNotificationTraceListener Notification(this ILoggingConfigurationSendTo context, string listenerName)
        {
            if (string.IsNullOrEmpty(listenerName))
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "listenerName");

            return new SendToNotificationTraceListenerBuilder(context, listenerName);
        }

        private class SendToNotificationTraceListenerBuilder : SendToTraceListenerExtension, ILoggingConfigurationSendToNotificationTraceListener
        {
            NotificationTraceListenerData listenerData;
            public SendToNotificationTraceListenerBuilder(ILoggingConfigurationSendTo context, string listenerName)
                :base(context)
            {
                listenerData = new NotificationTraceListenerData
                {
                    Name = listenerName
                };

                base.AddTraceListenerToSettingsAndCategory(listenerData);
            }
        }
    }
}
