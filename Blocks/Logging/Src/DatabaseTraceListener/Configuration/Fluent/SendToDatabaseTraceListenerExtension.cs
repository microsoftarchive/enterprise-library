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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Extension methods to support configuration of <see cref="FormattedDatabaseTraceListener"/>.
    /// </summary>
    /// <seealso cref="FormattedDatabaseTraceListener"/>
    /// <seealso cref="FormattedDatabaseTraceListenerData"/>
    public static class SendToDatabaseListenerExtensions
    {
        /// <summary>
        /// Adds a new <see cref="FormattedDatabaseTraceListener"/> to the logging settings and creates
        /// a reference to this Trace Listener for the current category source.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="listenerName">The name of the <see cref="FormattedDatabaseTraceListener"/>.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="FormattedDatabaseTraceListenerData"/>. </returns>
        /// <seealso cref="FormattedDatabaseTraceListener"/>
        /// <seealso cref="FormattedDatabaseTraceListenerData"/>
        public static ILoggingConfigurationSendToDatabaseTraceListener Database(this ILoggingConfigurationSendTo context, string listenerName)
        {
            if (string.IsNullOrEmpty(listenerName))
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "listenerName");

            return new SendToDatabaseTraceListenerBuilder(context, listenerName);
        }

        private class SendToDatabaseTraceListenerBuilder : SendToTraceListenerExtension, ILoggingConfigurationSendToDatabaseTraceListener
        {
            FormattedDatabaseTraceListenerData databaseTraceListener;
            public SendToDatabaseTraceListenerBuilder(ILoggingConfigurationSendTo context, string listenerName)
                : base(context)
            {
                databaseTraceListener = new FormattedDatabaseTraceListenerData
                {
                    Name = listenerName
                };

                base.AddTraceListenerToSettingsAndCategory(databaseTraceListener);
            }


            public ILoggingConfigurationSendToDatabaseTraceListener FormatWith(IFormatterBuilder formatBuilder)
            {
                if (formatBuilder == null) throw new ArgumentNullException("formatBuilder");

                FormatterData formatter = formatBuilder.GetFormatterData();
                databaseTraceListener.Formatter = formatter.Name;
                LoggingSettings.Formatters.Add(formatter);

                return this;
            }

            public ILoggingConfigurationSendToDatabaseTraceListener FormatWithSharedFormatter(string formatterName)
            {
                databaseTraceListener.Formatter = formatterName;

                return this;
            }

            public ILoggingConfigurationSendToDatabaseTraceListener WithTraceOptions(TraceOptions traceOptions)
            {
                databaseTraceListener.TraceOutputOptions = traceOptions;

                return this;
            }

            public ILoggingConfigurationSendToDatabaseTraceListener Filter(SourceLevels sourceLevel)
            {
                databaseTraceListener.Filter = sourceLevel;

                return this;
            }

            public ILoggingConfigurationSendToDatabaseTraceListener WithAddCategoryStoredProcedure(string addCategoryStoredProcedureName)
            {
                if (string.IsNullOrEmpty(addCategoryStoredProcedureName))
                    throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "addCategoryStoredProcedureName");

                databaseTraceListener.AddCategoryStoredProcName = addCategoryStoredProcedureName;

                return this;
            }

            public ILoggingConfigurationSendToDatabaseTraceListener WithWriteLogStoredProcedure(string writeLogStoredProcedureName)
            {
                if (string.IsNullOrEmpty(writeLogStoredProcedureName)) 
                    throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "writeLogStoredProcedureName");

                databaseTraceListener.WriteLogStoredProcName = writeLogStoredProcedureName;

                return this;
            }


            public ILoggingConfigurationSendToDatabaseTraceListener UseDatabase(string databaseInstanceName)
            {
                databaseTraceListener.DatabaseInstanceName = databaseInstanceName;

                return this;
            }
        }
    }
}
