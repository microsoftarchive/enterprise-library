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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{

    /// <summary/>
    public static class ExceptionHandlingLoggingConfigurationSourceBuilderExtensions
    {
        /// <summary/>
        public static IExceptionConfigurationLoggingProvider LogToCategory(this IExceptionConfigurationAddExceptionHandlers context, string categoryName)
        {
            IExceptionHandlerExtension exceptionHandlerExtension = (IExceptionHandlerExtension)context;
            LoggingExceptionHandlerData loggingHandler = new LoggingExceptionHandlerData
            {
                Name = categoryName,
                LogCategory = categoryName,
                FormatterType = typeof(TextExceptionFormatter)
            };
            exceptionHandlerExtension.CurrentExceptionTypeData.ExceptionHandlers.Add(loggingHandler);

            return new ExceptionConfigurationLoggingProviderBuilder(
                (IExceptionConfigurationForExceptionTypeOrPostHandling)context,
                loggingHandler);

        }

        private class ExceptionConfigurationLoggingProviderBuilder : ExceptionConfigurationAddExceptionHandlers, IExceptionConfigurationLoggingProvider
        {
            LoggingExceptionHandlerData logHandler;
            
            public ExceptionConfigurationLoggingProviderBuilder(IExceptionConfigurationForExceptionTypeOrPostHandling context, LoggingExceptionHandlerData logHandler)
                :base(context)
            {
                this.logHandler = logHandler;
            }

            IExceptionConfigurationLoggingProvider IExceptionConfigurationLoggingProvider.UsingTitle(string title)
            {
                logHandler.Title = title;
                
                return this;
            }

            IExceptionConfigurationLoggingProvider IExceptionConfigurationLoggingProvider.UsingExceptionFormatter(Type exceptionFormatterType)
            {
                logHandler.FormatterType = exceptionFormatterType;

                return this;
            }

            IExceptionConfigurationLoggingProvider IExceptionConfigurationLoggingProvider.WithSeverity(TraceEventType severity)
            {
                logHandler.Severity = severity;

                return this;
            }

            IExceptionConfigurationLoggingProvider IExceptionConfigurationLoggingProvider.WithPriority(int priority)
            {
                logHandler.Priority = priority;

                return this;
            }

            IExceptionConfigurationLoggingProvider IExceptionConfigurationLoggingProvider.UsingEventId(int eventId)
            {
                logHandler.EventId = eventId;

                return this;
            }
        }
    }

    /// <summary/>
    public interface IExceptionConfigurationLoggingProvider : IExceptionConfigurationForExceptionTypeOrPostHandling
    {
        /// <summary/>
        IExceptionConfigurationLoggingProvider UsingTitle(string title);

        /// <summary/>
        IExceptionConfigurationLoggingProvider UsingEventId(int eventId);

        /// <summary/>
        IExceptionConfigurationLoggingProvider UsingExceptionFormatter(Type exceptionFormatterType);

        /// <summary/>
        IExceptionConfigurationLoggingProvider WithSeverity(TraceEventType severity);

        /// <summary/>
        IExceptionConfigurationLoggingProvider WithPriority(int priority);
    }
}
