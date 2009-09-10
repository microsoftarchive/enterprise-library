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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface used to configure a <see cref="FormattedDatabaseTraceListener"/> instance.
    /// </summary>
    /// <seealso cref="FormattedDatabaseTraceListener"/>
    /// <seealso cref="FormattedDatabaseTraceListenerData"/>
    public interface ILoggingConfigurationSendToDatabaseTraceListener : ILoggingConfigurationContd, ILoggingConfigurationCategoryContd, IFluentInterface
    {

        /// <summary>
        /// Specifies the formatter used to format database log messages send by this <see cref="FormattedDatabaseTraceListener"/>.<br/>
        /// </summary>
        /// <param name="formatBuilder">The <see cref="FormatterBuilder"/> used to create an <see cref="LogFormatter"/> .</param>
        /// <returns>Fluent interface that can be used to further configure the current <see cref="FormattedDatabaseTraceListener"/> instance. </returns>
        /// <seealso cref="FormattedDatabaseTraceListener"/>
        /// <seealso cref="FormattedDatabaseTraceListenerData"/>
        ILoggingConfigurationSendToDatabaseTraceListener FormatWith(IFormatterBuilder formatBuilder);


        /// <summary>
        /// Specifies the formatter used to format log messages send by this <see cref="FormattedDatabaseTraceListener"/>.<br/>
        /// </summary>
        /// <returns>Fluent interface that can be used to further configure the current <see cref="FormattedDatabaseTraceListener"/> instance. </returns>
        /// <seealso cref="FormattedDatabaseTraceListener"/>
        /// <seealso cref="FormattedDatabaseTraceListenerData"/>
        ILoggingConfigurationSendToDatabaseTraceListener FormatWithSharedFormatter(string formatterName);

        /// <summary>
        /// Specifies which options, or elements, should be included in messages send by this <see cref="FormattedDatabaseTraceListener"/>.<br/>
        /// </summary>
        /// <returns>Fluent interface that can be used to further configure the current <see cref="FormattedDatabaseTraceListener"/> instance. </returns>
        /// <seealso cref="FormattedDatabaseTraceListener"/>
        /// <seealso cref="FormattedDatabaseTraceListenerData"/>
        /// <seealso cref="TraceOptions"/>
        ILoggingConfigurationSendToDatabaseTraceListener WithTraceOptions(TraceOptions traceOptions);

        /// <summary>
        /// Specifies the <see cref="SourceLevels"/> that should be used to filter trace output by this <see cref="FormattedDatabaseTraceListener"/>.
        /// </summary>
        /// <returns>Fluent interface that can be used to further configure the current <see cref="FormattedDatabaseTraceListener"/> instance. </returns>
        /// <seealso cref="FormattedDatabaseTraceListener"/>
        /// <seealso cref="FormattedDatabaseTraceListenerData"/>
        /// <seealso cref="SourceLevels"/>
        ILoggingConfigurationSendToDatabaseTraceListener Filter(SourceLevels sourceLevel);

        /// <summary>
        /// Specifies the name of the stored procedure that should be used to add a new log category.
        /// </summary>
        /// <param name="addCategoryStoredProcedureName">The name of the stored procedure that should be used to add a new log category.</param>
        /// <returns>Fluent interface that can be used to further configure the current <see cref="FormattedDatabaseTraceListener"/> instance. </returns>
        /// <seealso cref="FormattedDatabaseTraceListener"/>
        /// <seealso cref="FormattedDatabaseTraceListenerData"/>
        ILoggingConfigurationSendToDatabaseTraceListener WithAddCategoryStoredProcedure(string addCategoryStoredProcedureName);


        /// <summary>
        /// Specifies the name of the stored procedure that should be used when writing a log entry.
        /// </summary>
        /// <param name="writeLogStoredProcedureName">The name of the stored procedure that should be used when writing a log entry.</param>
        /// <returns>Fluent interface that can be used to further configure the current <see cref="FormattedDatabaseTraceListener"/> instance. </returns>
        /// <seealso cref="FormattedDatabaseTraceListener"/>
        /// <seealso cref="FormattedDatabaseTraceListenerData"/>
        ILoggingConfigurationSendToDatabaseTraceListener WithWriteLogStoredProcedure(string writeLogStoredProcedureName);

        /// <summary>
        /// Specifies which database instance, or connection string, should be used to send log messages to.
        /// </summary>
        /// <param name="databaseInstanceName">The name of the database instance, or connection string, should be used to send log messages to.</param>
        /// <returns>Fluent interface that can be used to further configure the current <see cref="FormattedDatabaseTraceListener"/> instance. </returns>
        /// <seealso cref="FormattedDatabaseTraceListener"/>
        /// <seealso cref="FormattedDatabaseTraceListenerData"/>
        ILoggingConfigurationSendToDatabaseTraceListener UseDatabase(string databaseInstanceName);
    }
}
