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
    /// Fluent interface used to specify settings on a <see cref="FlatFileTraceListenerData"/>.
    /// </summary>
    /// <seealso cref="FlatFileTraceListenerData"/>
    public interface ILoggingConfigurationSendToFlatFileTraceListener : ILoggingConfigurationContd, ILoggingConfigurationCategoryContd, IFluentInterface
    {
        /// <summary>
        /// Specifies the file name that should be used to send messages to by this <see cref="FlatFileTraceListener"/>.<br/>
        /// The default file name is 'trace.log'.
        /// </summary>
        /// <param name="filename">The file name that should be used to send message to.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="FlatFileTraceListenerData"/>. </returns>
        /// <seealso cref="FlatFileTraceListener"/>
        /// <seealso cref="FlatFileTraceListenerData"/>
        ILoggingConfigurationSendToFlatFileTraceListener ToFile(string filename);

        /// <summary>
        /// Specifies a footer for messages that are send to this <see cref="FlatFileTraceListener"/>.<br/>
        /// </summary>
        /// <param name="footer">The footer that should be used when sending messages.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="FlatFileTraceListenerData"/>. </returns>
        /// <seealso cref="FlatFileTraceListener"/>
        /// <seealso cref="FlatFileTraceListenerData"/>
        ILoggingConfigurationSendToFlatFileTraceListener WithFooter(string footer);

        /// <summary>
        /// Specifies a header for messages that are send to this <see cref="FlatFileTraceListener"/>.<br/>
        /// </summary>
        /// <param name="header">The header that should be used when sending messages.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="FlatFileTraceListenerData"/>. </returns>
        /// <seealso cref="FlatFileTraceListener"/>
        /// <seealso cref="FlatFileTraceListenerData"/>
        ILoggingConfigurationSendToFlatFileTraceListener WithHeader(string header);

        /// <summary>
        /// Specifies the formatter used to format log messages send by this <see cref="FlatFileTraceListener"/>.<br/>
        /// </summary>
        /// <param name="formatBuilder">The <see cref="FormatterBuilder"/> used to create an <see cref="LogFormatter"/> .</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="FlatFileTraceListenerData"/>. </returns>
        /// <seealso cref="FlatFileTraceListener"/>
        /// <seealso cref="FlatFileTraceListenerData"/>
        ILoggingConfigurationSendToFlatFileTraceListener FormatWith(IFormatterBuilder formatBuilder);

        /// <summary>
        /// Specifies the formatter used to format log messages send by this <see cref="FlatFileTraceListener"/>.<br/>
        /// </summary>
        /// <param name="formatterName">The name of a <see cref="FormatterData"/> configured elsewhere in this section.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="FlatFileTraceListenerData"/>. </returns>
        /// <seealso cref="FlatFileTraceListener"/>
        /// <seealso cref="FlatFileTraceListenerData"/>
        ILoggingConfigurationSendToFlatFileTraceListener FormatWithSharedFormatter(string formatterName);

        /// <summary>
        /// Specifies the <see cref="SourceLevels"/> that should be used to filter trace output by this <see cref="FlatFileTraceListener"/>.
        /// </summary>
        /// <param name="sourceLevel">The <see cref="SourceLevels"/> that should be used to filter trace output .</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="FlatFileTraceListenerData"/>. </returns>
        /// <seealso cref="FlatFileTraceListener"/>
        /// <seealso cref="FlatFileTraceListenerData"/>
        ILoggingConfigurationSendToFlatFileTraceListener Filter(SourceLevels sourceLevel);

        /// <summary>
        /// Specifies which options, or elements, should be included in messages send by this <see cref="FlatFileTraceListener"/>.<br/>
        /// </summary>
        /// <param name="traceOptions">The options that should be included in the trace output.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="FlatFileTraceListenerData"/>. </returns>
        /// <seealso cref="FlatFileTraceListener"/>
        /// <seealso cref="FlatFileTraceListenerData"/>
        ILoggingConfigurationSendToFlatFileTraceListener WithTraceOptions(TraceOptions traceOptions);
    }
}
