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
    /// Fluent interface used to specify settings on a <see cref="XmlTraceListenerData"/>.
    /// </summary>
    /// <seealso cref="XmlTraceListener"/>
    /// <seealso cref="XmlTraceListenerData"/>
    public interface ILoggingConfigurationSendToXmlTraceListener : ILoggingConfigurationContd, ILoggingConfigurationCategoryContd, IFluentInterface
    {
        /// <summary>
        /// Specifies the filename used to log to by the <see cref="XmlTraceListener"/>.<br/>
        /// The default is trace-xml.log.
        /// </summary>
        /// <param name="filename">The filename used to log.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="XmlTraceListenerData"/>. </returns>
        /// <seealso cref="XmlTraceListener"/>
        /// <seealso cref="XmlTraceListenerData"/>
        ILoggingConfigurationSendToXmlTraceListener ToFile(string filename);

        /// <summary>
        /// Specifies the <see cref="SourceLevels"/> that should be used to filter trace output by this <see cref="XmlTraceListener"/>.
        /// </summary>
        /// <param name="sourceLevel">The <see cref="SourceLevels"/> that should be used to filter trace output .</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="XmlTraceListenerData"/>. </returns>
        /// <seealso cref="XmlTraceListener"/>
        /// <seealso cref="XmlTraceListenerData"/>
        /// <seealso cref="SourceLevels"/>
        ILoggingConfigurationSendToXmlTraceListener Filter(SourceLevels sourceLevel);

        /// <summary>
        /// Specifies which options, or elements, should be included in messages send by this <see cref="XmlTraceListener"/>.<br/>
        /// </summary>
        /// <param name="traceOptions">The options that should be included in the trace output.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="XmlTraceListenerData"/>. </returns>
        /// <seealso cref="XmlTraceListener"/>
        /// <seealso cref="XmlTraceListenerData"/>
        /// <seealso cref="TraceOptions"/>
        ILoggingConfigurationSendToXmlTraceListener WithTraceOptions(TraceOptions traceOptions);
    }
}
