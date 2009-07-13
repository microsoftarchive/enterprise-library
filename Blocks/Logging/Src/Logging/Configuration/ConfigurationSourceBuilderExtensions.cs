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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary/>
    public static class ExceptionHandlingConfigurationSourceBuilderExtensions
    {
        /// <summary/>
        public static ILoggingConfigurationStart ConfigureLogging(this IConfigurationSourceBuilder configurationSourceBuilder)
        {
            LoggingSettings loggingSettings = new LoggingSettings();
            configurationSourceBuilder.AddSection(LoggingSettings.SectionName, loggingSettings);

            return new LoggingConfigurationBuilder(loggingSettings);
        }


        private class LoggingConfigurationBuilder :
            ILoggingConfigurationStart,
            ILoggingConfigurationSendTo,
            ILoggingConfigurationSendToEventLogTraceListener,
            ILoggingConfigurationCategoryStart,
            ILoggingConfigurationCategoryContd
        {
            LoggingSettings loggingSettings;
            TraceSourceData currentTraceSource;
            FormattedEventLogTraceListenerData currentEventLogTraceListener;

            public LoggingConfigurationBuilder(LoggingSettings loggingSettings)
            {
                this.loggingSettings = loggingSettings;
            }

            public ILoggingConfigurationSendToEventLogTraceListener EventLog(string listenerName)
            {
                currentEventLogTraceListener = new FormattedEventLogTraceListenerData()
                {
                    Name = listenerName
                };
                loggingSettings.TraceListeners.Add(currentEventLogTraceListener);
                currentTraceSource.TraceListeners.Add(new TraceListenerReferenceData(currentEventLogTraceListener.Name));

                return this;
            }

            public ILoggingConfigurationSendToEventLogTraceListener WithTraceOptions(TraceOptions options)
            {
                currentEventLogTraceListener.TraceOutputOptions = options;
                return this;
            }

            ILoggingConfigurationSendToEventLogTraceListener ILoggingConfigurationSendToEventLogTraceListener.FormatWith(FormatterBuilder builder)
            {
                FormatterData formatter = builder.GetFormatterData();
                currentEventLogTraceListener.Formatter = formatter.Name;
                loggingSettings.Formatters.Add(formatter);
                return this;
            }

            ILoggingConfigurationSendToEventLogTraceListener ILoggingConfigurationSendToEventLogTraceListener.FormatWithSharedFormatter(string formatterName)
            {
                currentEventLogTraceListener.Formatter = formatterName;
                return this;
            }

            ILoggingConfigurationSendToEventLogTraceListener ILoggingConfigurationSendToEventLogTraceListener.WithTraceOption(TraceOptions options)
            {
                currentEventLogTraceListener.TraceOutputOptions = options;
                return this;
            }

            /// <summary/>
            ILoggingConfigurationSendToEventLogTraceListener ILoggingConfigurationSendToEventLogTraceListener.WithEventSource(string source)
            {
                currentEventLogTraceListener.Source = source;
                return this;
            }

            ILoggingConfigurationSendToEventLogTraceListener ILoggingConfigurationSendToEventLogTraceListener.ToLog(string logName)
            {
                currentEventLogTraceListener.Log = logName;
                return this;
            }

            ILoggingConfigurationSendToEventLogTraceListener ILoggingConfigurationSendToEventLogTraceListener.ToMachine(string machineName)
            {
                currentEventLogTraceListener.MachineName = machineName;
                return this;
            }

            /// <summary/>
            ILoggingConfigurationSendToEventLogTraceListener ILoggingConfigurationSendToEventLogTraceListener.LogEventSourceLevel(SourceLevels sourceLevel)
            {
                currentEventLogTraceListener.Filter = sourceLevel;
                return this;
            }


            ILoggingConfigurationCategoryStart ILoggingConfigurationCategoryStart.ToSourceLevel(SourceLevels sourceLevels)
            {
                currentTraceSource.DefaultLevel = sourceLevels;

                return this;
            }

            ILoggingConfigurationCategoryStart ILoggingConfigurationCategoryStart.DoNotAutoFlushEntries()
            {
                throw new NotImplementedException();
            }

            ILoggingConfigurationStart ILoggingConfigurationStart.EnableTracing()
            {
                loggingSettings.TracingEnabled = true;

                return this;
            }

            ILoggingConfigurationCategoryStart ILoggingConfigurationContd.LogToCategoryNamed(string categoryName)
            {
                currentTraceSource = new TraceSourceData()
                {
                    Name = categoryName
                };

                loggingSettings.TraceSources.Add(currentTraceSource);
                return this;
            }

            ILoggingConfigurationCategoryStart ILoggingConfigurationSendTo.SharedListenerNamed(string listenerName)
            {
                currentTraceSource.TraceListeners.Add(new TraceListenerReferenceData(listenerName));
                return this;
            }


            ILoggingConfigurationSendTo ILoggingConfigurationCategoryContd.SendTo()
            {
                return this;
            }

            ILoggingConfigurationStart ILoggingConfigurationStart.DoNotRevertImpersonation()
            {
                throw new NotImplementedException();
            }

            ILoggingConfigurationStart ILoggingConfigurationStart.DoNotLogWarningsWhenNoCategoryExists()
            {
                throw new NotImplementedException();
            }


            ILoggingConfigurationCategoryStart ILoggingConfigurationCategoryStart.SetAsDefaultCategory()
            {
                throw new NotImplementedException();
            }
        }
    }

    /// <summary/>
    public interface ILoggingConfigurationStart : ILoggingConfigurationContd
    {
        /// <summary/>
        ILoggingConfigurationStart EnableTracing();

        /// <summary/>
        ILoggingConfigurationStart DoNotRevertImpersonation();

        /// <summary/>
        ILoggingConfigurationStart DoNotLogWarningsWhenNoCategoryExists();
    }

    /// <summary/>
    public interface ILoggingConfigurationContd
    {
        /// <summary/>
        ILoggingConfigurationCategoryStart LogToCategoryNamed(string categoryName);
    }

    /// <summary/>
    public interface ILoggingConfigurationCategoryStart : ILoggingConfigurationCategoryContd
    {
        /// <summary/>
        ILoggingConfigurationCategoryStart ToSourceLevel(SourceLevels sourceLevels);

        /// <summary/>
        ILoggingConfigurationCategoryStart DoNotAutoFlushEntries();

        /// <summary/>
        ILoggingConfigurationCategoryStart SetAsDefaultCategory();
    }

    /// <summary/>
    public interface ILoggingConfigurationCategoryContd : ILoggingConfigurationContd
    {
        /// <summary/>
        ILoggingConfigurationSendTo SendTo();
    }

    /// <summary/>
    public interface ILoggingConfigurationSendTo
    {
        /// <summary/>
        ILoggingConfigurationCategoryStart SharedListenerNamed(string listenerName);

        /// <summary/>
        ILoggingConfigurationSendToEventLogTraceListener EventLog(string listenerName);

    }

    /// <summary/>
    public interface ILoggingConfigurationSendToEventLogTraceListener : ILoggingConfigurationContd, ILoggingConfigurationCategoryContd
    {
        /// <summary/>
        ILoggingConfigurationSendToEventLogTraceListener ToLog(string logName);

        /// <summary/>
        ILoggingConfigurationSendToEventLogTraceListener ToMachine(string machineName);

        /// <summary/>
        ILoggingConfigurationSendToEventLogTraceListener FormatWith(FormatterBuilder formatBuilder);

        /// <summary/>
        ILoggingConfigurationSendToEventLogTraceListener FormatWithSharedFormatter(string formatterName);

        /// <summary/>
        ILoggingConfigurationSendToEventLogTraceListener WithTraceOption(TraceOptions traceOptions);

        /// <summary/>
        ILoggingConfigurationSendToEventLogTraceListener WithEventSource(string source);

        /// <summary/>
        ILoggingConfigurationSendToEventLogTraceListener LogEventSourceLevel(SourceLevels sourceLevel);
    }

    /// <summary/>
    public class FormatterBuilder : IFluentInterface
    {
        /// <summary/>
        public FormatterBuilder()
        {
        }
        /// <summary/>
        internal virtual FormatterData GetFormatterData()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary/>
    public static class TextFormatterBuilderExtensions
    {
        /// <summary/>
        public static TextFormatterBuilder TextFormatterNamed(this FormatterBuilder builder, string formatterName)
        {
            return new TextFormatterBuilder(formatterName);
        }
    }

    /// <summary/>
    public class TextFormatterBuilder : FormatterBuilder
    {
        TextFormatterData formatterData = new TextFormatterData();

        /// <summary/>
        public TextFormatterBuilder(string name)
        {
            formatterData.Name = name;
        }

        /// <summary/>
        public FormatterBuilder UsingTemplate(string template)
        {
            formatterData.Template = template;
            return this;
        }

        /// <summary/>
        internal override FormatterData GetFormatterData()
        {
            return formatterData;
        }
    }

}
