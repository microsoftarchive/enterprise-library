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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    /// <summary>
    /// A <see cref="TraceListener"/> that writes an email message, formatting the output with an <see cref="ILogFormatter"/>.
    /// </summary>
    [ConfigurationElementType(typeof(EmailTraceListenerData))]
    public class EmailTraceListener : FormattedTraceListenerBase
    {
        private string toAddress = String.Empty;
        private string fromAddress = String.Empty;
        private string subjectLineStarter = String.Empty;
        private string subjectLineEnder = String.Empty;
        private string smtpServer = String.Empty;
        private int smtpPort = 25;
        private EmailAuthenticationMode authenticationMode = EmailAuthenticationMode.None;
        private string userName = string.Empty;
        private string password = string.Empty;
        private bool useSSL = false;

        /// <summary>
        /// Initializes a new instance of <see cref="EmailTraceListener"/> with a toaddress, fromaddress, 
        /// subjectlinestarter, subjectlinender, smtpserver, and a formatter
        /// a <see cref="ILogFormatter"/>.
        /// </summary>
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="formatter">The Formatter <see cref="ILogFormatter"/> which determines how the 
        /// email message should be formatted</param>
        public EmailTraceListener(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, ILogFormatter formatter)
            : this(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, 25, formatter)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EmailTraceListener"/> with a toaddress, fromaddress, 
        /// subjectlinestarter, subjectlinender, smtpserver, smtpport, and a formatter
        /// a <see cref="ILogFormatter"/>.
        /// </summary>
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        /// <param name="formatter">The Formatter <see cref="ILogFormatter"/> which determines how the 
        /// email message should be formatted</param>
        public EmailTraceListener(
            string toAddress,
            string fromAddress,
            string subjectLineStarter,
            string subjectLineEnder,
            string smtpServer,
            int smtpPort,
            ILogFormatter formatter)
            : base(formatter)
        {
            Guard.ArgumentNotNullOrEmpty(toAddress, "toAddress");
            Guard.ArgumentNotNullOrEmpty(fromAddress, "fromAddress");
            Guard.ArgumentNotNullOrEmpty(subjectLineStarter, "subjectLineStarter");
            Guard.ArgumentNotNullOrEmpty(subjectLineEnder, "subjectLineEnder");
            Guard.ArgumentNotNullOrEmpty(smtpServer, "smtpServer");

            this.toAddress = toAddress;
            this.fromAddress = fromAddress;
            this.subjectLineStarter = subjectLineStarter;
            this.subjectLineEnder = subjectLineEnder;
            this.smtpServer = smtpServer;
            this.smtpPort = smtpPort;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EmailTraceListener"/> with a toaddress, fromaddress, 
        /// subjectlinestarter, subjectlinender, smtpserver, smtpport, and a formatter
        /// a <see cref="ILogFormatter"/>.
        /// </summary>
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        /// <param name="formatter">The Formatter <see cref="ILogFormatter"/> which determines how the 
        /// email message should be formatted</param>
        /// <param name="authenticationMode">Authentication style to use when connecting to SMTP server.</param>
        /// <param name="userName">User name to use for authentication if using username/password authentication.</param>
        /// <param name="password">Password to use for authentication if using username/password authentication.</param>
        /// <param name="useSSL">Use SSL to connect to mail server if true, regular socket if false.</param>

        public EmailTraceListener(
            string toAddress,
            string fromAddress,
            string subjectLineStarter,
            string subjectLineEnder,
            string smtpServer,
            int smtpPort,
            ILogFormatter formatter,
            EmailAuthenticationMode authenticationMode,
            string userName,
            string password,
            bool useSSL)
            : base(formatter)
        {
            Guard.ArgumentNotNullOrEmpty(toAddress, "toAddress");
            Guard.ArgumentNotNullOrEmpty(fromAddress, "fromAddress");
            Guard.ArgumentNotNullOrEmpty(subjectLineStarter, "subjectLineStarter");
            Guard.ArgumentNotNullOrEmpty(subjectLineEnder, "subjectLineEnder");
            Guard.ArgumentNotNullOrEmpty(smtpServer, "smtpServer");

            if (authenticationMode == EmailAuthenticationMode.UserNameAndPassword)
            {
                Guard.ArgumentNotNullOrEmpty(userName, "userName");
            }

            this.toAddress = toAddress;
            this.fromAddress = fromAddress;
            this.subjectLineStarter = subjectLineStarter;
            this.subjectLineEnder = subjectLineEnder;
            this.smtpServer = smtpServer;
            this.smtpPort = smtpPort;
            this.authenticationMode = authenticationMode;
            this.userName = userName;
            this.password = password;
            this.useSSL = useSSL;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EmailTraceListener"/> with a toaddress, fromaddress, 
        /// subjectlinestarter, subjectlinender, smtpserver, smtpport, and a formatter
        /// a <see cref="ILogFormatter"/>.
        /// </summary>
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        public EmailTraceListener(
            string toAddress,
            string fromAddress,
            string subjectLineStarter,
            string subjectLineEnder,
            string smtpServer)
            : this(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, 25)
        { }

        /// <summary>
        /// Initializes a new instance of <see cref="EmailTraceListener"/> with a toaddress, fromaddress, 
        /// subjectlinestarter, subjectlinender, smtpserver, smtpport.
        /// </summary>
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        public EmailTraceListener(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort)
            : this(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, EmailAuthenticationMode.None, null, null, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EmailTraceListener"/> with a toaddress, fromaddress, 
        /// subjectlinestarter, subjectlinender, smtpserver, smtpport and authentication information.
        /// </summary>
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        /// <param name="authenticationMode">Authentication style to use when connecting to SMTP server.</param>
        /// <param name="userName">User name to use for authentication if using username/password authentication.</param>
        /// <param name="password">Password to use for authentication if using username/password authentication.</param>
        /// <param name="useSSL">Use SSL to connect to mail server if true, regular socket if false.</param>
        public EmailTraceListener(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort,
            EmailAuthenticationMode authenticationMode, string userName, string password, bool useSSL)
        {
            Guard.ArgumentNotNullOrEmpty(toAddress, "toAddress");
            Guard.ArgumentNotNullOrEmpty(fromAddress, "fromAddress");
            Guard.ArgumentNotNullOrEmpty(subjectLineStarter, "subjectLineStarter");
            Guard.ArgumentNotNullOrEmpty(subjectLineEnder, "subjectLineEnder");
            Guard.ArgumentNotNullOrEmpty(smtpServer, "smtpServer");

            if (authenticationMode == EmailAuthenticationMode.UserNameAndPassword)
            {
                Guard.ArgumentNotNullOrEmpty(userName, "userName");
            }

            this.toAddress = toAddress;
            this.fromAddress = fromAddress;
            this.subjectLineStarter = subjectLineStarter;
            this.subjectLineEnder = subjectLineEnder;
            this.smtpServer = smtpServer;
            this.smtpPort = smtpPort;
            this.authenticationMode = authenticationMode;
            this.userName = userName;
            this.password = password;
            this.useSSL = useSSL;
        }

        /// <summary>
        /// Sends an email message given a predefined string
        /// </summary>
        /// <param name="message">The string to write as the email message</param>
        public override void Write(string message)
        {
            EmailMessage mailMessage =
                new EmailMessage(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, message, this.Formatter, authenticationMode, userName, password, useSSL);
            mailMessage.Send();
        }

        /// <summary>
        /// Sends an email message given a predefined string
        /// </summary>
        /// <param name="message">The string to write as the email message</param>
        public override void WriteLine(string message)
        {
            Write(message);
        }

        /// <summary>
        /// Delivers the trace data as an email message.
        /// </summary>
        /// <param name="eventCache">The context information provided by <see cref="System.Diagnostics"/>.</param>
        /// <param name="source">The name of the trace source that delivered the trace data.</param>
        /// <param name="eventType">The type of event.</param>
        /// <param name="id">The id of the event.</param>
        /// <param name="data">The data to trace.</param>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
            {
                if (data is LogEntry)
                {
                    EmailMessage message = new EmailMessage(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, data as LogEntry, this.Formatter, authenticationMode, userName, password, useSSL);
                    message.Send();
                }
                else if (data is string)
                {
                    Write(data);
                }
                else
                {
                    base.TraceData(eventCache, source, eventType, id, data);
                }
            }
        }

        /// <summary>
        /// Declare the supported attributes for <see cref="EmailTraceListener"/>
        /// </summary>
        protected override string[] GetSupportedAttributes()
        {
            return new string[7] { "formatter", "toAddress", "fromAddress", "subjectLineStarter", "subjectLineEnder", "smtpServer", "smtpPort" };
        }
    }
}
