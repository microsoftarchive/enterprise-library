//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Extensions to <see cref="IExceptionConfigurationAddExceptionHandlers"/> that support logging exceptions.
    /// </summary>
    public static class ExceptionHandlingLoggingConfigurationSourceBuilderExtensions
    {
        /// <summary>
        /// Category name to log <see cref="Exception"/> under.  This should align with a category name defined through the <see cref="LoggingConfigurationSourceBuilderExtensions"/> extensions.
        /// </summary>
        /// <param name="context">Interface to extend to provide this handler fluent interface.</param>
        /// <param name="categoryName">Name of the category.</param>
        /// <returns></returns>
        public static IExceptionConfigurationLoggingProvider LogToCategory(this IExceptionConfigurationAddExceptionHandlers context, string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName)) 
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "categoryName");

            return new ExceptionConfigurationLoggingProviderBuilder(context, categoryName);
        }

        private class ExceptionConfigurationLoggingProviderBuilder : ExceptionHandlerConfigurationExtension, IExceptionConfigurationLoggingProvider
        {
            private LoggingExceptionHandlerData logHandler;
            
            public ExceptionConfigurationLoggingProviderBuilder(IExceptionConfigurationAddExceptionHandlers context, string categoryName)
                :base(context)
            {
                logHandler = new LoggingExceptionHandlerData
                {
                    Name = categoryName,
                    LogCategory = categoryName,
                    FormatterType = typeof(TextExceptionFormatter)
                };

                base.CurrentExceptionTypeData.ExceptionHandlers.Add(logHandler);
            }

            public IExceptionConfigurationLoggingProvider UsingTitle(string title)
            {
                if (string.IsNullOrEmpty(title))
                    throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "title");

                logHandler.Title = title;
                
                return this;
            }

            public IExceptionConfigurationLoggingProvider UsingExceptionFormatter(Type exceptionFormatterType)
            {
                if (exceptionFormatterType == null)
                    throw new ArgumentNullException("exceptionFormatterType");

                logHandler.FormatterType = exceptionFormatterType;

                return this;
            }

            public IExceptionConfigurationLoggingProvider UsingExceptionFormatter<T>()
            {
                return UsingExceptionFormatter(typeof (T));
            }

            public IExceptionConfigurationLoggingProvider WithSeverity(TraceEventType severity)
            {
                logHandler.Severity = severity;

                return this;
            }

            public IExceptionConfigurationLoggingProvider WithPriority(int priority)
            {
                logHandler.Priority = priority;

                return this;
            }

            public IExceptionConfigurationLoggingProvider UsingEventId(int eventId)
            {
                logHandler.EventId = eventId;

                return this;
            }
        }
    }
}
