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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface used to specify settings on a <see cref="FormattedEventLogTraceListenerData"/>.
    /// </summary>
    /// <seealso cref="FormattedEventLogTraceListener"/>
    /// <seealso cref="FormattedEventLogTraceListenerData"/>
    public interface ILoggingConfigurationSendToEventLogTraceListener : ILoggingConfigurationContd, ILoggingConfigurationCategoryContd, IFluentInterface
    {
        /// <summary>
        /// Specifies the event log that should be used to send messages to by this <see cref="FormattedEventLogTraceListener"/>.<br/>
        /// </summary>
        /// <param name="logName">The event log that should be used to send messages to.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="FormattedEventLogTraceListenerData"/>. </returns>
        /// <seealso cref="FormattedEventLogTraceListener"/>
        /// <seealso cref="FormattedEventLogTraceListenerData"/>
        ILoggingConfigurationSendToEventLogTraceListener ToLog(string logName);

        /// <summary>
        /// Specifies the machine that should be used to send messages to by this <see cref="FormattedEventLogTraceListener"/>.<br/>
        /// The default machine is '.'.
        /// </summary>
        /// <param name="machineName">The machine that should be used to send messages to.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="FormattedEventLogTraceListenerData"/>. </returns>
        /// <seealso cref="FormattedEventLogTraceListener"/>
        /// <seealso cref="FormattedEventLogTraceListenerData"/>
        ILoggingConfigurationSendToEventLogTraceListener ToMachine(string machineName);

        /// <summary>
        /// Specifies the source that should be used when sending messages by this <see cref="FormattedEventLogTraceListener"/>.<br/>
        /// The default source is 'Enterprise Library Logging'.
        /// </summary>
        /// <param name="source">The source that should be used when sending messages.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="FormattedEventLogTraceListenerData"/>. </returns>
        /// <seealso cref="FormattedEventLogTraceListener"/>
        /// <seealso cref="FormattedEventLogTraceListenerData"/>
        ILoggingConfigurationSendToEventLogTraceListener UsingEventLogSource(string source);

        /// <summary>
        /// Specifies the formatter used to format log messages send by this <see cref="FormattedEventLogTraceListener"/>.<br/>
        /// </summary>
        /// <param name="formatBuilder">The <see cref="FormatterBuilder"/> used to create an <see cref="LogFormatter"/> .</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="FormattedEventLogTraceListenerData"/>. </returns>
        /// <seealso cref="FormattedEventLogTraceListener"/>
        /// <seealso cref="FormattedEventLogTraceListenerData"/>
        ILoggingConfigurationSendToEventLogTraceListener FormatWith(IFormatterBuilder formatBuilder);

        /// <summary>
        /// Specifies the formatter used to format log messages send by this <see cref="FormattedEventLogTraceListener"/>.<br/>
        /// </summary>
        /// <param name="formatterName">The name of a <see cref="FormatterData"/> configured elsewhere in this section.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="FormattedEventLogTraceListenerData"/>. </returns>
        /// <seealso cref="FormattedEventLogTraceListenerData"/>
        ILoggingConfigurationSendToEventLogTraceListener FormatWithSharedFormatter(string formatterName);


        /// <summary>
        /// Specifies the <see cref="SourceLevels"/> that should be used to filter trace output by this <see cref="FormattedEventLogTraceListener"/>.
        /// </summary>
        /// <param name="sourceLevel">The <see cref="SourceLevels"/> that should be used to filter trace output .</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="FormattedEventLogTraceListenerData"/>. </returns>
        /// <seealso cref="FormattedEventLogTraceListenerData"/>
        /// <seealso cref="SourceLevels"/>
        ILoggingConfigurationSendToEventLogTraceListener Filter(SourceLevels sourceLevel);


        /// <summary>
        /// Specifies which options, or elements, should be included in messages send by this <see cref="FormattedEventLogTraceListener"/>.<br/>
        /// </summary>
        /// <param name="traceOptions">The options that should be included in the trace output.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="FormattedEventLogTraceListenerData"/>. </returns>
        /// <seealso cref="FormattedEventLogTraceListenerData"/>
        /// <seealso cref="TraceOptions"/>
        ILoggingConfigurationSendToEventLogTraceListener WithTraceOptions(TraceOptions traceOptions);
    }
}
