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
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Extension methods to support configuration of <see cref="EmailTraceListener"/>.
    /// </summary>
    public static class SendToEmailTraceListenerExtensions
    {
        /// <summary>
        /// Adds a new <see cref="EmailTraceListener"/> to the logging settings and creates
        /// a reference to this Trace Listener for the current category source.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="listenerName">The name of the <see cref="EmailTraceListener"/>.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="EmailTraceListenerData"/>. </returns>
        /// <seealso cref="EmailTraceListenerData"/>
        public static ILoggingConfigurationSendToEmailTraceListener Email(this ILoggingConfigurationSendTo context, string listenerName)
        {
            if (string.IsNullOrEmpty(listenerName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "listenerName");

            return new SendToEmailTraceListenerBuilder(context, listenerName);
        }

        private class SendToEmailTraceListenerBuilder : SendToTraceListenerExtension, ILoggingConfigurationSendToEmailTraceListener
        {
            EmailTraceListenerData emailTraceListener;

            public SendToEmailTraceListenerBuilder(ILoggingConfigurationSendTo context, string listenerName)
                : base(context)
            {
                emailTraceListener = new EmailTraceListenerData
                {
                    Name = listenerName
                };

                base.AddTraceListenerToSettingsAndCategory(emailTraceListener);
            }

            public ILoggingConfigurationSendToEmailTraceListener UsingSmtpServer(string smtpServer)
            {
                emailTraceListener.SmtpServer = smtpServer;

                return this;
            }

            public ILoggingConfigurationSendToEmailTraceListener UsingSmtpServerPort(int smtpServerPort)
            {
                emailTraceListener.SmtpPort = smtpServerPort;

                return this;
            }

            public ILoggingConfigurationSendToEmailTraceListener To(string toEmailAddress)
            {
                if (string.IsNullOrEmpty(toEmailAddress)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "toEmailAddress");

                emailTraceListener.ToAddress = toEmailAddress;

                return this;
            }

            public ILoggingConfigurationSendToEmailTraceListener From(string fromEmailAddress)
            {
                if (string.IsNullOrEmpty(fromEmailAddress)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "fromEmailAddress");

                emailTraceListener.FromAddress = fromEmailAddress;

                return this;
            }

            public ILoggingConfigurationSendToEmailTraceListener WithSubjectStart(string subjectLineStart)
            {
                emailTraceListener.SubjectLineStarter = subjectLineStart;

                return this;
            }

            public ILoggingConfigurationSendToEmailTraceListener WithSubjectEnd(string subjectLineEnd)
            {
                emailTraceListener.SubjectLineEnder = subjectLineEnd;

                return this;
            }

            public ILoggingConfigurationSendToEmailTraceListener FormatWith(IFormatterBuilder formatBuilder)
            {
                if (formatBuilder == null) throw new ArgumentNullException("formatBuilder");

                FormatterData formatter = formatBuilder.GetFormatterData();
                emailTraceListener.Formatter = formatter.Name;
                LoggingSettings.Formatters.Add(formatter);

                return this;
            }

            public ILoggingConfigurationSendToEmailTraceListener FormatWithSharedFormatter(string formatterName)
            {
                emailTraceListener.Formatter = formatterName;

                return this;
            }

            public ILoggingConfigurationSendToEmailTraceListener Filter(SourceLevels sourceLevel)
            {
                emailTraceListener.Filter = sourceLevel;

                return this;
            }

            public ILoggingConfigurationSendToEmailTraceListener WithTraceOptions(TraceOptions traceOptions)
            {
                emailTraceListener.TraceOutputOptions = traceOptions;

                return this;
            }

            public ILoggingConfigurationSendToEmailTraceListener UseSSL(bool useSSL)
            {
                emailTraceListener.UseSSL = useSSL;
                return this;
            }

            public ILoggingConfigurationSendToEmailTraceListener Unauthenticated()
            {
                emailTraceListener.AuthenticationMode = EmailAuthenticationMode.None;
                return this;
            }

            public ILoggingConfigurationSendToEmailTraceListener WithWindowsCredentials()
            {
                emailTraceListener.AuthenticationMode = EmailAuthenticationMode.WindowsCredentials;
                return this;
            }

            public ILoggingConfigurationSendToEmailTraceListener WithUserNameAndPassword(string userName, string password)
            {
                emailTraceListener.AuthenticationMode = EmailAuthenticationMode.UserNameAndPassword;
                emailTraceListener.UserName = userName;
                emailTraceListener.Password = password;

                return this;
            }
        }
    }
}
