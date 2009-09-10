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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using System.Diagnostics;
using System.Messaging;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Extension methods to support configuration of a <see cref="WmiTraceListener"/>.
    /// </summary>
    public static class SendToWmiTraceListenerExtensions
    {
        /// <summary>
        /// Adds a new <see cref="WmiTraceListener"/> to the logging settings and creates
        /// a reference to this Trace Listener for the current category source.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="listenerName">The name of the <see cref="WmiTraceListener"/>.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="WmiTraceListenerData"/>. </returns>
        /// <seealso cref="WmiTraceListener"/>
        /// <seealso cref="WmiTraceListenerData"/>
        public static ILoggingConfigurationSendToWmiTraceListener Wmi(this ILoggingConfigurationSendTo context, string listenerName)
        {

            if (string.IsNullOrEmpty(listenerName))
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "listenerName");


            return new SendToWmiTraceListenerBuilder(context, listenerName);
        }

        private class SendToWmiTraceListenerBuilder : SendToTraceListenerExtension, ILoggingConfigurationSendToWmiTraceListener
        {
            WmiTraceListenerData wmiTraceListener;
            public SendToWmiTraceListenerBuilder(ILoggingConfigurationSendTo context, string listenerName)
                :base(context)
            {
                wmiTraceListener = new WmiTraceListenerData
                {
                    Name = listenerName
                };

                base.AddTraceListenerToSettingsAndCategory(wmiTraceListener);
            }

            public ILoggingConfigurationSendToWmiTraceListener Filter(SourceLevels sourceLevel)
            {
                wmiTraceListener.Filter = sourceLevel;

                return this;
            }

            public ILoggingConfigurationSendToWmiTraceListener WithTraceOptions(TraceOptions traceOptions)
            {
                wmiTraceListener.TraceOutputOptions = traceOptions;

                return this;
            }
        }
    }

}
