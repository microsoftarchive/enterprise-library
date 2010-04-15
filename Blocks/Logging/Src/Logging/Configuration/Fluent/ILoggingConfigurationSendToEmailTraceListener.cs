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
    /// Fluent interface used to specify settings on a <see cref="EmailTraceListener"/>.
    /// </summary>
    /// <seealso cref="EmailTraceListener"/>
    /// <seealso cref="EmailTraceListenerData"/>
    public interface ILoggingConfigurationSendToEmailTraceListener : ILoggingConfigurationContd, ILoggingConfigurationCategoryContd, IFluentInterface
    {
        /// <summary>
        /// Specifies the smtp server this <see cref="EmailTraceListener"/> uses to send email.<br/>
        /// The default smtp server is 127.0.0.1.
        /// </summary>
        /// <param name="smtpServer">The smtp server used to send email.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="EmailTraceListenerData"/>. </returns>
        /// <seealso cref="EmailTraceListener"/>
        /// <seealso cref="EmailTraceListenerData"/>
        ILoggingConfigurationSendToEmailTraceListener UsingSmtpServer(string smtpServer);


        /// <summary>
        /// Specifies the port on the smtp server used by this <see cref="EmailTraceListener"/> to send email.<br/>
        /// The default smtp server port is 25.
        /// </summary>
        /// <param name="smtpServerPort">The smtp server port used to send email.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="EmailTraceListenerData"/>. </returns>
        /// <seealso cref="EmailTraceListener"/>
        /// <seealso cref="EmailTraceListener"/>
        /// <seealso cref="EmailTraceListenerData"/>
        ILoggingConfigurationSendToEmailTraceListener UsingSmtpServerPort(int smtpServerPort);

        /// <summary>
        /// Specifies the email address of the recipient used by this <see cref="EmailTraceListener"/> to send email to.<br/>
        /// </summary>
        /// <param name="toEmailAddress">The email address of the email recipient.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="EmailTraceListenerData"/>. </returns>
        /// <seealso cref="EmailTraceListener"/>
        /// <seealso cref="EmailTraceListenerData"/>
        ILoggingConfigurationSendToEmailTraceListener To(string toEmailAddress);

        /// <summary>
        /// Specifies the email address of the recipient used by this <see cref="EmailTraceListener"/> to send email from.<br/>
        /// </summary>
        /// <param name="fromEmailAddress">The email address used to send email from.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="EmailTraceListenerData"/>. </returns>
        /// <seealso cref="EmailTraceListener"/>
        /// <seealso cref="EmailTraceListenerData"/>
        ILoggingConfigurationSendToEmailTraceListener From(string fromEmailAddress);

        /// <summary>
        /// Specifies the prefix of the subject set on emails send by this <see cref="EmailTraceListener"/>.<br/>
        /// </summary>
        /// <param name="subjectLineStart">The prefix used for subjects on emails.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="EmailTraceListenerData"/>. </returns>
        /// <seealso cref="EmailTraceListener"/>
        /// <seealso cref="EmailTraceListenerData"/>
        ILoggingConfigurationSendToEmailTraceListener WithSubjectStart(string subjectLineStart);

        /// <summary>
        /// Specifies the postfix of the subject set on emails send by this <see cref="EmailTraceListener"/>.<br/>
        /// </summary>
        /// <param name="subjectLineEnd">The postfix used for subjects on emails.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="EmailTraceListenerData"/>. </returns>
        /// <seealso cref="EmailTraceListener"/>
        /// <seealso cref="EmailTraceListenerData"/>
        ILoggingConfigurationSendToEmailTraceListener WithSubjectEnd(string subjectLineEnd);

        /// <summary>
        /// Specifies the formatter used to format email messages send by this <see cref="EmailTraceListener"/>.<br/>
        /// </summary>
        /// <param name="formatBuilder">The <see cref="FormatterBuilder"/> used to create an <see cref="LogFormatter"/> .</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="EmailTraceListenerData"/>. </returns>
        /// <seealso cref="EmailTraceListener"/>
        /// <seealso cref="EmailTraceListenerData"/>
        ILoggingConfigurationSendToEmailTraceListener FormatWith(IFormatterBuilder formatBuilder);

        /// <summary>
        /// Specifies the formatter used to format email messages send by this <see cref="EmailTraceListener"/>.<br/>
        /// </summary>
        /// <param name="formatterName">The name of a <see cref="FormatterData"/> configured elsewhere in this section.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="EmailTraceListenerData"/>. </returns>
        /// <seealso cref="EmailTraceListener"/>
        /// <seealso cref="EmailTraceListenerData"/>
        ILoggingConfigurationSendToEmailTraceListener FormatWithSharedFormatter(string formatterName);

        /// <summary>
        /// Specifies the <see cref="SourceLevels"/> that should be used to filter trace output by this <see cref="EmailTraceListener"/>.
        /// </summary>
        /// <param name="sourceLevel">The <see cref="SourceLevels"/> that should be used to filter trace output .</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="EmailTraceListenerData"/>. </returns>
        /// <seealso cref="EmailTraceListener"/>
        /// <seealso cref="EmailTraceListenerData"/>
        /// <seealso cref="SourceLevels"/>
        ILoggingConfigurationSendToEmailTraceListener Filter(SourceLevels sourceLevel);

        /// <summary>
        /// Specifies which options, or elements, should be included in messages send by this <see cref="EmailTraceListener"/>.<br/>
        /// </summary>
        /// <param name="traceOptions">The options that should be included in the trace output.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="EmailTraceListenerData"/>. </returns>
        /// <seealso cref="EmailTraceListener"/>
        /// <seealso cref="EmailTraceListenerData"/>
        /// <seealso cref="TraceOptions"/>
        ILoggingConfigurationSendToEmailTraceListener WithTraceOptions(TraceOptions traceOptions);

        /// <summary>
        /// Specifies if the <see cref="EmailTraceListener"/> should use SSL when connecting to the mail server.
        /// </summary>
        /// <param name="useSSL">true to use SSL to connect, false to use unsecured connection.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="EmailTraceListenerData"/>. </returns>
        ILoggingConfigurationSendToEmailTraceListener UseSSL(bool useSSL);

        /// <summary>
        /// Do not authenticate when logging into the mail server.
        /// </summary>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="EmailTraceListenerData"/>. </returns>
        ILoggingConfigurationSendToEmailTraceListener Unauthenticated();

        /// <summary>
        /// Send the current process Windows credentials when logging into the mail server.
        /// </summary>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="EmailTraceListenerData"/>. </returns>
        ILoggingConfigurationSendToEmailTraceListener WithWindowsCredentials();

        /// <summary>
        /// Authenticate against the mail server with this user name and password.
        /// </summary>
        /// <param name="userName">User name to send to mail server.</param>
        /// <param name="password">Password to send to mail server.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="EmailTraceListenerData"/>. </returns>
        ILoggingConfigurationSendToEmailTraceListener WithUserNameAndPassword(string userName, string password);
    }
}
