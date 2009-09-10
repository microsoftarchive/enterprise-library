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
    /// Fluent interface used to configure a <see cref="System.Diagnostics.TraceListener"/> instance.
    /// </summary>
    /// <seealso cref="System.Diagnostics.TraceListener"/>
    /// <seealso cref="SystemDiagnosticsTraceListenerData"/>
    public interface ILoggingConfigurationSendToSystemDiagnosticsTraceListener : ILoggingConfigurationContd, ILoggingConfigurationCategoryContd, IFluentInterface
    {
        /// <summary>
        /// Specifies the type of <see cref="System.Diagnostics.TraceListener"/> that should be used to log messages.
        /// </summary>
        /// <param name="tracelistenerType">The type of <see cref="System.Diagnostics.TraceListener"/> that should be used to log messages.</param>
        /// <seealso cref="System.Diagnostics.TraceListener"/>
        /// <seealso cref="SystemDiagnosticsTraceListenerData"/>
        ILoggingConfigurationSendToSystemDiagnosticsTraceListener ForTraceListenerType(Type tracelistenerType);

        /// <summary>
        /// Specifies the type of <see cref="System.Diagnostics.TraceListener"/> that should be used to log messages.
        /// </summary>
        /// <typeparam name="TTraceListener">The type of <see cref="System.Diagnostics.TraceListener"/> that should be used to log messages.</typeparam>
        /// <seealso cref="System.Diagnostics.TraceListener"/>
        /// <seealso cref="SystemDiagnosticsTraceListenerData"/>
        ILoggingConfigurationSendToSystemDiagnosticsTraceListener ForTraceListenerType<TTraceListener>() where TTraceListener : TraceListener;

        /// <summary>
        /// Specifies the initialization data, which, when specified will be passed to the <see cref="System.Diagnostics.TraceListener"/>'s contructor.<br/>
        /// </summary>
        /// <param name="initData">The <see cref="System.String"/> used as initizalition data.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="SystemDiagnosticsTraceListenerData"/>. </returns>
        /// <seealso cref="System.Diagnostics.TraceListener"/>
        /// <seealso cref="SystemDiagnosticsTraceListenerData"/>
        ILoggingConfigurationSendToSystemDiagnosticsTraceListener UsingInitData(string initData);

        /// <summary>
        /// Specifies the <see cref="SourceLevels"/> that should be used to filter trace output by this <see cref="System.Diagnostics.TraceListener"/>.
        /// </summary>
        /// <param name="sourceLevel">The <see cref="SourceLevels"/> that should be used to filter trace output .</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="SystemDiagnosticsTraceListenerData"/>. </returns>
        /// <seealso cref="System.Diagnostics.TraceListener"/>
        /// <seealso cref="SystemDiagnosticsTraceListenerData"/>
        /// <seealso cref="SourceLevels"/>
        ILoggingConfigurationSendToSystemDiagnosticsTraceListener Filter(SourceLevels sourceLevel);

        /// <summary>
        /// Specifies which options, or elements, should be included in messages send by this <see cref="System.Diagnostics.TraceListener"/>.<br/>
        /// </summary>
        /// <param name="traceOptions">The options that should be included in the trace output.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="SystemDiagnosticsTraceListenerData"/>. </returns>
        /// <seealso cref="System.Diagnostics.TraceListener"/>
        /// <seealso cref="SystemDiagnosticsTraceListenerData"/>
        /// <seealso cref="TraceOptions"/>
        ILoggingConfigurationSendToSystemDiagnosticsTraceListener WithTraceOptions(TraceOptions traceOptions);
    }
}
