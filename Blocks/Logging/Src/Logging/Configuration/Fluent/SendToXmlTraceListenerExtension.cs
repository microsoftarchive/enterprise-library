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
    /// Extension methods to support configuration of <see cref="XmlTraceListener"/>.
    /// </summary>
    public static class SendToXmlTraceListenerExtensions
    {    
        /// <summary>
        /// Adds a new <see cref="XmlTraceListener"/> to the logging settings and creates
        /// a reference to this Trace Listener for the current category source.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="listenerName">The name of the <see cref="XmlTraceListener"/>.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="XmlTraceListenerData"/>. </returns>
        /// <seealso cref="XmlTraceListener"/>
        /// <seealso cref="XmlTraceListenerData"/>
        public static ILoggingConfigurationSendToXmlTraceListener XmlFile(this ILoggingConfigurationSendTo context, string listenerName)
        {
            if (string.IsNullOrEmpty(listenerName))
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "listenerName");

            return new SendToXmlTraceListenerBuilder(context, listenerName);
        }

        private class SendToXmlTraceListenerBuilder : SendToTraceListenerExtension, ILoggingConfigurationSendToXmlTraceListener
        {            
            XmlTraceListenerData xmlTraceListener;

            public SendToXmlTraceListenerBuilder(ILoggingConfigurationSendTo context, string listenerName)
                :base(context)
            {
                xmlTraceListener = new XmlTraceListenerData
                {
                    Name = listenerName
                };

                base.AddTraceListenerToSettingsAndCategory(xmlTraceListener);
            }

            public ILoggingConfigurationSendToXmlTraceListener ToFile(string filename)
            {
                if (string.IsNullOrEmpty(filename))
                    throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "filename");

                xmlTraceListener.FileName = filename;
                
                return this;
            }

            public ILoggingConfigurationSendToXmlTraceListener Filter(SourceLevels sourceLevel)
            {
                xmlTraceListener.Filter = sourceLevel;

                return this;
            }

            public ILoggingConfigurationSendToXmlTraceListener WithTraceOptions(TraceOptions traceOptions)
            {
                xmlTraceListener.TraceOutputOptions = traceOptions;

                return this;
            }
        }

    }
}
