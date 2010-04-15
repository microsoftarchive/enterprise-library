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
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Extension methods to support configuration of <see cref="System.Diagnostics.TraceListener"/>.
    /// </summary>
    public static class SendToSystemDiagnosticsTraceListenerExtensions
    {
        /// <summary>
        /// Adds a new <see cref="System.Diagnostics.TraceListener"/> to the logging settings and creates
        /// a reference to this Trace Listener for the current category source.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="listenerName">The name of the <see cref="System.Diagnostics.TraceListener"/>.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="SystemDiagnosticsTraceListenerData"/>. </returns>
        /// <seealso cref="System.Diagnostics.TraceListener"/>
        /// <seealso cref="SystemDiagnosticsTraceListenerData"/>
        public static ILoggingConfigurationSendToSystemDiagnosticsTraceListener SystemDiagnosticsListener(this ILoggingConfigurationSendTo context, string listenerName)
        {
            if (string.IsNullOrEmpty(listenerName))
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "listenerName");

            return new SendToSystemDiagnosticsTraceListenerBuilder(context, listenerName);
        }

        private class SendToSystemDiagnosticsTraceListenerBuilder : SendToTraceListenerExtension, ILoggingConfigurationSendToSystemDiagnosticsTraceListener
        {
            SystemDiagnosticsTraceListenerData systemDiagnosticsData;

            public SendToSystemDiagnosticsTraceListenerBuilder(ILoggingConfigurationSendTo context, string listenerName)
                :base(context)
            {
                systemDiagnosticsData = new SystemDiagnosticsTraceListenerData()
                {
                    Name = listenerName
                };

                base.AddTraceListenerToSettingsAndCategory(systemDiagnosticsData);
            }

            public ILoggingConfigurationSendToSystemDiagnosticsTraceListener ForTraceListenerType(Type tracelistenerType)
            {
                if (tracelistenerType == null) throw new ArgumentNullException("tracelistenerType");

                if (!typeof(TraceListener).IsAssignableFrom(tracelistenerType))
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                        Resources.ExceptionTypeMustDeriveFromType, typeof(TraceListener)), "tracelistenerType");

                systemDiagnosticsData.Type = tracelistenerType;

                return this;
            }

            public ILoggingConfigurationSendToSystemDiagnosticsTraceListener ForTraceListenerType<TTraceListener>() where TTraceListener : TraceListener
            {
                return ForTraceListenerType(typeof(TTraceListener));
            }

            public ILoggingConfigurationSendToSystemDiagnosticsTraceListener UsingInitData(string initData)
            {
                systemDiagnosticsData.InitData = initData;

                return this;
            }

            public ILoggingConfigurationSendToSystemDiagnosticsTraceListener Filter(SourceLevels sourceLevel)
            {
                systemDiagnosticsData.Filter = sourceLevel;

                return this;
            }

            public ILoggingConfigurationSendToSystemDiagnosticsTraceListener WithTraceOptions(TraceOptions traceOptions)
            {
                systemDiagnosticsData.TraceOutputOptions = traceOptions;

                return this;
            }
        }

    }
}
