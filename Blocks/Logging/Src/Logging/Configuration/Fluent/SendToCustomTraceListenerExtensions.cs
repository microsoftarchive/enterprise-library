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
    /// Extension methods to support configuration of <see cref="CustomTraceListener"/>.
    /// </summary>
    public static class SendToCustomTraceListenerExtensions
    {
        /// <summary>
        /// Adds a new <see cref="CustomTraceListener"/> to the logging settings and creates
        /// a reference to this Trace Listener for the current category source.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="listenerName">The name of the <see cref="CustomTraceListener"/>.</param>
        /// <param name="customTraceListenerType">The concrete type of <see cref="CustomTraceListener"/> that should be added to the configuration.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomTraceListenerData"/>. </returns>
        /// <seealso cref="CustomTraceListenerData"/>
        public static ILoggingConfigurationSendToCustomTraceListener Custom(this ILoggingConfigurationSendTo context, string listenerName, Type customTraceListenerType)
        {
            return Custom(context, listenerName, customTraceListenerType, new NameValueCollection());
        }

        /// <summary>
        /// Adds a new <see cref="CustomTraceListener"/> to the logging settings and creates
        /// a reference to this Trace Listener for the current category source.
        /// </summary>
        /// <typeparam name="TCustomListenerType">The concrete type of <see cref="CustomTraceListener"/> that should be added to the configuration.</typeparam>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="listenerName">The name of the <see cref="CustomTraceListener"/>.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomTraceListenerData"/>. </returns>
        /// <seealso cref="CustomTraceListenerData"/>
        public static ILoggingConfigurationSendToCustomTraceListener Custom<TCustomListenerType>(this ILoggingConfigurationSendTo context, string listenerName)
            where TCustomListenerType : CustomTraceListener
        {
            return Custom(context, listenerName, typeof(TCustomListenerType), new NameValueCollection());
        }

        /// <summary>
        /// Adds a new <see cref="CustomTraceListener"/> to the logging settings and creates
        /// a reference to this Trace Listener for the current category source.
        /// </summary>
        /// <typeparam name="TCustomListenerType">The concrete type of <see cref="CustomTraceListener"/> that should be added to the configuration.</typeparam>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="listenerName">The name of the <see cref="CustomTraceListener"/>.</param>
        /// <param name="attributes">Attributes that should be passed to <typeparamref name="TCustomListenerType"/> when creating an instance.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomTraceListenerData"/>. </returns>
        /// <seealso cref="CustomTraceListenerData"/>
        public static ILoggingConfigurationSendToCustomTraceListener Custom<TCustomListenerType>(this ILoggingConfigurationSendTo context, string listenerName, NameValueCollection attributes)
            where TCustomListenerType : CustomTraceListener
        {
            return Custom(context, listenerName, typeof(TCustomListenerType), attributes);
        }

        /// <summary>
        /// Adds a new <see cref="CustomTraceListener"/> to the logging settings and creates
        /// a reference to this Trace Listener for the current category source.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="listenerName">The name of the <see cref="CustomTraceListener"/>.</param>
        /// <param name="customTraceListenerType">The concrete type of <see cref="CustomTraceListener"/> that should be added to the configuration.</param>
        /// <param name="attributes">Attributes that should be passed to <paramref name="customTraceListenerType"/> when creating an instance.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomTraceListenerData"/>. </returns>
        /// <seealso cref="CustomTraceListenerData"/>
        public static ILoggingConfigurationSendToCustomTraceListener Custom(this ILoggingConfigurationSendTo context, string listenerName, Type customTraceListenerType, NameValueCollection attributes)
        {
            if (string.IsNullOrEmpty(listenerName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "listenerName");
            if (customTraceListenerType == null) throw new ArgumentNullException("customTraceListenerType");
            if (attributes == null) throw new ArgumentNullException("attributes");
            
            if (!typeof(CustomTraceListener).IsAssignableFrom(customTraceListenerType))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                    Resources.ExceptionTypeMustDeriveFromType, typeof(CustomTraceListener)), "customTraceListenerType");

            return new SendToCustomTraceListenerBuilder(context, listenerName, customTraceListenerType, attributes);
        }

        private class SendToCustomTraceListenerBuilder : SendToTraceListenerExtension, ILoggingConfigurationSendToCustomTraceListener
        {
            CustomTraceListenerData customListener;

            public SendToCustomTraceListenerBuilder(ILoggingConfigurationSendTo context, string listenerName, Type customTraceListenerType, NameValueCollection attributes)
                :base(context)
            {
                customListener = new CustomTraceListenerData() 
                { 
                    Name = listenerName, 
                    Type = customTraceListenerType
                };
                
                customListener.Attributes.Add(attributes);

                base.AddTraceListenerToSettingsAndCategory(customListener);
            }

            public ILoggingConfigurationSendToCustomTraceListener FormatWith(IFormatterBuilder formatBuilder)
            {
                FormatterData formatter = formatBuilder.GetFormatterData();
                customListener.Formatter = formatter.Name;
                LoggingSettings.Formatters.Add(formatter);
                
                return this;
            }

            public ILoggingConfigurationSendToCustomTraceListener FormatWithSharedFormatter(string formatterName)
            {
                customListener.Formatter = formatterName;

                return this;
            }

            public ILoggingConfigurationSendToCustomTraceListener Filter(SourceLevels sourceLevel)
            {
                customListener.Filter = sourceLevel;

                return this;
            }

            public ILoggingConfigurationSendToCustomTraceListener WithTraceOptions(TraceOptions traceOptions)
            {
                customListener.TraceOutputOptions = traceOptions;

                return this;
            }


            public ILoggingConfigurationSendToCustomTraceListener UsingInitData(string initData)
            {
                customListener.InitData = initData;

                return this;
            }
        }
    }
}
