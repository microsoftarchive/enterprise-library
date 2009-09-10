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
    /// Fluent interface used to configure a <see cref="CustomTraceListener"/>.
    /// </summary>
    /// <seealso cref="CustomTraceListener"/>
    /// <seealso cref="CustomTraceListenerData"/>
    public interface ILoggingConfigurationSendToCustomTraceListener : ILoggingConfigurationContd, ILoggingConfigurationCategoryContd, IFluentInterface
    {
        /// <summary>
        /// Specifies the formatter used to format log messages send by this <see cref="CustomTraceListener"/>.<br/>
        /// </summary>
        /// <param name="formatBuilder">The <see cref="FormatterBuilder"/> used to create an <see cref="LogFormatter"/> .</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomTraceListenerData"/>. </returns>
        /// <seealso cref="CustomTraceListener"/>
        /// <seealso cref="CustomTraceListenerData"/>
        ILoggingConfigurationSendToCustomTraceListener FormatWith(IFormatterBuilder formatBuilder);

        /// <summary>
        /// Specifies the formatter used to format log messages send by this <see cref="CustomTraceListener"/>.<br/>
        /// </summary>
        /// <param name="formatterName">The name of a <see cref="FormatterData"/> configured elsewhere in this section.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomTraceListenerData"/>. </returns>
        /// <seealso cref="CustomTraceListener"/>
        /// <seealso cref="CustomTraceListenerData"/>
        ILoggingConfigurationSendToCustomTraceListener FormatWithSharedFormatter(string formatterName);

        /// <summary>
        /// Specifies the <see cref="SourceLevels"/> that should be used to filter trace output by this <see cref="CustomTraceListener"/>.
        /// </summary>
        /// <param name="sourceLevel">The <see cref="SourceLevels"/> that should be used to filter trace output .</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomTraceListenerData"/>. </returns>
        /// <seealso cref="CustomTraceListener"/>
        /// <seealso cref="CustomTraceListenerData"/>
        /// <seealso cref="SourceLevels"/>
        ILoggingConfigurationSendToCustomTraceListener Filter(SourceLevels sourceLevel);

        /// <summary>
        /// Specifies which options, or elements, should be included in messages send by this <see cref="CustomTraceListener"/>.<br/>
        /// </summary>
        /// <param name="traceOptions">The options that should be included in the trace output.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomTraceListenerData"/>. </returns>
        /// <seealso cref="CustomTraceListener"/>
        /// <seealso cref="CustomTraceListenerData"/>
        /// <seealso cref="TraceOptions"/>
        ILoggingConfigurationSendToCustomTraceListener WithTraceOptions(TraceOptions traceOptions);

        /// <summary>
        /// Specifies the intialization data passed to the custom trace listener type.
        /// </summary>
        /// <param name="initData">The intialization data.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="CustomTraceListenerData"/>. </returns>
        /// <seealso cref="CustomTraceListener"/>
        /// <seealso cref="CustomTraceListenerData"/>
        /// <seealso cref="TraceOptions"/>
        ILoggingConfigurationSendToCustomTraceListener UsingInitData(string initData);
    }
}
