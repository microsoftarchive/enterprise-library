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
    /// Fluent interface used to specify settings on a <see cref="RollingFlatFileTraceListenerData"/>.
    /// </summary>
    /// <seealso cref="RollingFlatFileTraceListener"/>
    /// <seealso cref="RollingFlatFileTraceListenerData"/>
    public interface ILoggingConfigurationSendToRollingFileTraceListener : ILoggingConfigurationContd, ILoggingConfigurationCategoryContd, IFluentInterface
    {
        /// <summary>
        /// Specifies the time interval used for rolling of the <see cref="RollingFlatFileTraceListener"/>.<br/>
        /// </summary>
        /// <param name="interval">The time interval used for rolling.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="RollingFlatFileTraceListenerData"/>. </returns>
        /// <seealso cref="RollingFlatFileTraceListener"/>
        /// <seealso cref="RollingFlatFileTraceListenerData"/>
        ILoggingConfigurationSendToRollingFileTraceListener RollEvery(RollInterval interval);

        /// <summary>
        /// Specifies the behavior that should be used when a file already exists.<br/>
        /// </summary>
        /// <param name="behavior">The behavior that should be used when a file already exists.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="RollingFlatFileTraceListenerData"/>. </returns>
        /// <seealso cref="RollingFlatFileTraceListener"/>
        /// <seealso cref="RollingFlatFileTraceListenerData"/>
        ILoggingConfigurationSendToRollingFileTraceListener WhenRollFileExists(RollFileExistsBehavior behavior);

        /// <summary>
        /// Specifies the threshold in file size used for rolling of the <see cref="RollingFlatFileTraceListener"/>.<br/>
        /// </summary>
        /// <param name="rollSizeInKB">The threshold in file size used for rolling.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="RollingFlatFileTraceListenerData"/>. </returns>
        /// <seealso cref="RollingFlatFileTraceListener"/>
        /// <seealso cref="RollingFlatFileTraceListenerData"/>
        ILoggingConfigurationSendToRollingFileTraceListener RollAfterSize(int rollSizeInKB);

        /// <summary>
        /// Specifies the timestamp pattern used to create an archived file by the <see cref="RollingFlatFileTraceListener"/>.<br/>
        /// </summary>
        /// <param name="timeStampPattern">The timestamp pattern used to create an archived file when logging.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="RollingFlatFileTraceListenerData"/>. </returns>
        /// <seealso cref="RollingFlatFileTraceListener"/>
        /// <seealso cref="RollingFlatFileTraceListenerData"/>
        ILoggingConfigurationSendToRollingFileTraceListener UseTimeStampPattern(string timeStampPattern);

        /// <summary>
        /// Specifies the footer used when logging by the <see cref="RollingFlatFileTraceListener"/>.<br/>
        /// </summary>
        /// <param name="footer">The footer used by logging.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="RollingFlatFileTraceListenerData"/>. </returns>
        /// <seealso cref="RollingFlatFileTraceListener"/>
        /// <seealso cref="RollingFlatFileTraceListenerData"/>
        ILoggingConfigurationSendToRollingFileTraceListener WithFooter(string footer);

        /// <summary>
        /// Specifies the header used when logging by the <see cref="RollingFlatFileTraceListener"/>.<br/>
        /// </summary>
        /// <param name="header">The header used by logging.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="RollingFlatFileTraceListenerData"/>. </returns>
        /// <seealso cref="RollingFlatFileTraceListener"/>
        /// <seealso cref="RollingFlatFileTraceListenerData"/>
        ILoggingConfigurationSendToRollingFileTraceListener WithHeader(string header);

        /// <summary>
        /// Specifies the filename used to log to by the <see cref="RollingFlatFileTraceListener"/>.<br/>
        /// The default is rolling.log.
        /// </summary>
        /// <param name="filename">The filename used to log.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="RollingFlatFileTraceListenerData"/>. </returns>
        /// <seealso cref="RollingFlatFileTraceListener"/>
        /// <seealso cref="RollingFlatFileTraceListenerData"/>
        ILoggingConfigurationSendToRollingFileTraceListener ToFile(string filename);

        /// <summary>
        /// Specifies the formatter used to format log messages send by this <see cref="RollingFlatFileTraceListener"/>.<br/>
        /// </summary>
        /// <param name="formatBuilder">The <see cref="FormatterBuilder"/> used to create an <see cref="LogFormatter"/> .</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="RollingFlatFileTraceListenerData"/>. </returns>
        /// <seealso cref="RollingFlatFileTraceListener"/>
        /// <seealso cref="RollingFlatFileTraceListenerData"/>
        ILoggingConfigurationSendToRollingFileTraceListener FormatWith(IFormatterBuilder formatBuilder);

        /// <summary>
        /// Specifies the formatter used to format log messages send by this <see cref="RollingFlatFileTraceListener"/>.<br/>
        /// </summary>
        /// <param name="formatterName">The name of a <see cref="FormatterData"/> configured elsewhere in this section.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="RollingFlatFileTraceListenerData"/>. </returns>
        /// <seealso cref="RollingFlatFileTraceListener"/>
        /// <seealso cref="RollingFlatFileTraceListenerData"/>
        ILoggingConfigurationSendToRollingFileTraceListener FormatWithSharedFormatter(string formatterName);

        /// <summary>
        /// Specifies the <see cref="SourceLevels"/> that should be used to filter trace output by this <see cref="RollingFlatFileTraceListener"/>.
        /// </summary>
        /// <param name="sourceLevel">The <see cref="SourceLevels"/> that should be used to filter trace output .</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="RollingFlatFileTraceListenerData"/>. </returns>
        /// <seealso cref="RollingFlatFileTraceListener"/>
        /// <seealso cref="RollingFlatFileTraceListenerData"/>
        /// <seealso cref="SourceLevels"/>
        ILoggingConfigurationSendToRollingFileTraceListener Filter(SourceLevels sourceLevel);

        /// <summary>
        /// Specifies which options, or elements, should be included in messages send by this <see cref="RollingFlatFileTraceListener"/>.<br/>
        /// </summary>
        /// <param name="traceOptions">The options that should be included in the trace output.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="RollingFlatFileTraceListenerData"/>. </returns>
        /// <seealso cref="RollingFlatFileTraceListener"/>
        /// <seealso cref="RollingFlatFileTraceListenerData"/>
        /// <seealso cref="TraceOptions"/>
        ILoggingConfigurationSendToRollingFileTraceListener WithTraceOptions(TraceOptions traceOptions);

        /// <summary>
        /// Specifies the maximum number of archived files for this <see cref="RollingFlatFileTraceListener"/>.
        /// </summary>
        /// <param name="maximumArchivedFiles">the maximum number of archived files for this <see cref="RollingFlatFileTraceListener"/>.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="RollingFlatFileTraceListenerData"/>. </returns>
        /// <seealso cref="RollingFlatFileTraceListener"/>
        /// <seealso cref="RollingFlatFileTraceListenerData"/>
        ILoggingConfigurationSendToRollingFileTraceListener CleanUpArchivedFilesWhenMoreThan(int maximumArchivedFiles);
    }
}
