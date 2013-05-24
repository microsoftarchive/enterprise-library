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

using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings that describe a <see cref="EmailTraceListener"/>.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "EmailTraceListenerDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "EmailTraceListenerDataDisplayName")]
    [ElementValidation(LoggingDesignTime.ValidatorTypes.EmailTraceListenerAuthenticationValidator)]
    public class EmailTraceListenerData : TraceListenerData
    {
        private const string toAddressProperty = "toAddress";
        private const string fromAddressProperty = "fromAddress";
        private const string subjectLineStarterProperty = "subjectLineStarter";
        private const string subjectLineEnderProperty = "subjectLineEnder";
        private const string smtpServerProperty = "smtpServer";
        private const string smtpPortProperty = "smtpPort";
        private const string formatterNameProperty = "formatter";
        private const string authenticationModeProperty = "authenticationMode";
        private const string useSSLProperty = "useSSL";
        private const string userNameProperty = "userName";
        private const string passwordProperty = "password";

        /// <summary>
        /// Initializes a <see cref="EmailTraceListenerData"/>.
        /// </summary>
        public EmailTraceListenerData()
            : base(typeof(EmailTraceListener))
        {
            ListenerDataType = typeof(EmailTraceListenerData);
        }

        /// <summary>
        /// Initializes a <see cref="EmailTraceListenerData"/> with a toaddress, 
        /// fromaddress, subjectLineStarter, subjectLineEnder, smtpServer, and a formatter name.
        /// Default value for the SMTP port is 25
        /// </summary>
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="formatterName">The name of the Formatter <see cref="ILogFormatter"/> which determines how the
        ///email message should be formatted</param>
        public EmailTraceListenerData(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, string formatterName)
            : this(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, 25, formatterName)
        {

        }

        /// <summary>
        /// Initializes a <see cref="EmailTraceListenerData"/> with a toaddress, 
        /// fromaddress, subjectLineStarter, subjectLineEnder, smtpServer, and a formatter name.
        /// </summary>
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        /// <param name="formatterName">The name of the Formatter <see cref="ILogFormatter"/> which determines how the
        ///email message should be formatted</param>
        public EmailTraceListenerData(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, string formatterName)
            : this("unnamed", toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, formatterName)
        {
        }

        /// <summary>
        /// Initializes a <see cref="EmailTraceListenerData"/> with a toaddress, 
        /// fromaddress, subjectLineStarter, subjectLineEnder, smtpServer, and a formatter name.
        /// </summary>
        /// <param name="name">The name of this listener</param>        
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        /// <param name="formatterName">The name of the Formatter <see cref="ILogFormatter"/> which determines how the
        ///email message should be formatted</param>
        public EmailTraceListenerData(string name, string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, string formatterName)
            : this(name, toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, formatterName, TraceOptions.None)
        {
        }

        /// <summary>
        /// Initializes a <see cref="EmailTraceListenerData"/> with a toaddress, 
        /// fromaddress, subjectLineStarter, subjectLineEnder, smtpServer, a formatter name and trace options.
        /// </summary>
        /// <param name="name">The name of this listener</param>        
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        /// <param name="formatterName">The name of the Formatter <see cref="ILogFormatter"/> which determines how the
        ///email message should be formatted</param>
        ///<param name="traceOutputOptions">The trace options.</param>
        public EmailTraceListenerData(string name, string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, string formatterName, TraceOptions traceOutputOptions)
            : base(name, typeof(EmailTraceListener), traceOutputOptions)
        {
            this.ToAddress = toAddress;
            this.FromAddress = fromAddress;
            this.SubjectLineStarter = subjectLineStarter;
            this.SubjectLineEnder = subjectLineEnder;
            this.SmtpServer = smtpServer;
            this.SmtpPort = smtpPort;
            this.Formatter = formatterName;
        }

        /// <summary>
        /// Initializes a <see cref="EmailTraceListenerData"/> with a toaddress, 
        /// fromaddress, subjectLineStarter, subjectLineEnder, smtpServer, a formatter name and trace options.
        /// </summary>
        /// <param name="name">The name of this listener</param>        
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        /// <param name="formatterName">The name of the Formatter <see cref="ILogFormatter"/> which determines how the
        ///email message should be formatted</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        /// <param name="filter">The filter to apply.</param>
        public EmailTraceListenerData(string name, string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, string formatterName, TraceOptions traceOutputOptions, SourceLevels filter)
            : base(name, typeof(EmailTraceListener), traceOutputOptions, filter)
        {
            this.ToAddress = toAddress;
            this.FromAddress = fromAddress;
            this.SubjectLineStarter = subjectLineStarter;
            this.SubjectLineEnder = subjectLineEnder;
            this.SmtpServer = smtpServer;
            this.SmtpPort = smtpPort;
            this.Formatter = formatterName;
        }

        /// <summary>
        /// Initializes a <see cref="EmailTraceListenerData"/> with a toaddress, 
        /// fromaddress, subjectLineStarter, subjectLineEnder, smtpServer, a formatter name, trace options
        /// and authentication information.
        /// </summary>
        /// <param name="name">The name of this listener</param>        
        /// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
        /// <param name="fromAddress">Represents from whom the email is sent.</param>
        /// <param name="subjectLineStarter">Starting text for the subject line.</param>
        /// <param name="subjectLineEnder">Ending text for the subject line.</param>
        /// <param name="smtpServer">The name of the SMTP server.</param>
        /// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
        /// <param name="formatterName">The name of the Formatter <see cref="ILogFormatter"/> which determines how the
        ///email message should be formatted</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        /// <param name="filter">The filter to apply.</param>
        /// <param name="authenticationMode">Authenticate mode to use.</param>
        /// <param name="userName">User name to pass to the server if using <see cref="EmailAuthenticationMode.UserNameAndPassword"/>.</param>
        /// <param name="password">Password to pass to the server if using <see cref="EmailAuthenticationMode.UserNameAndPassword"/>.</param>
        /// <param name="useSSL">Connect to the server using SSL?</param>
        public EmailTraceListenerData(string name,
            string toAddress, string fromAddress,
            string subjectLineStarter, string subjectLineEnder,
            string smtpServer, int smtpPort,
            string formatterName, TraceOptions traceOutputOptions, SourceLevels filter,
            EmailAuthenticationMode authenticationMode, string userName, string password, bool useSSL)
            : base(name, typeof(EmailTraceListener), traceOutputOptions, filter)
        {
            this.ToAddress = toAddress;
            this.FromAddress = fromAddress;
            this.SubjectLineStarter = subjectLineStarter;
            this.SubjectLineEnder = subjectLineEnder;
            this.SmtpServer = smtpServer;
            this.SmtpPort = smtpPort;
            this.Formatter = formatterName;
            this.AuthenticationMode = authenticationMode;
            this.UserName = userName;
            this.Password = password;
            this.UseSSL = useSSL;
        }

        /// <summary>
        /// Gets and sets the ToAddress.  One or more email semicolon separated addresses.
        /// </summary>
        [ConfigurationProperty(toAddressProperty, IsRequired = true)]
        [ResourceDescription(typeof(DesignResources), "EmailTraceListenerDataToAddressDescription")]
        [ResourceDisplayName(typeof(DesignResources), "EmailTraceListenerDataToAddressDisplayName")]
        [DesigntimeDefaultAttribute("to@example.com")]
        public string ToAddress
        {
            get { return (string)base[toAddressProperty]; }
            set { base[toAddressProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the FromAddress. Email address that messages will be sent from.
        /// </summary>
        [ConfigurationProperty(fromAddressProperty, IsRequired = true)]
        [ResourceDescription(typeof(DesignResources), "EmailTraceListenerDataFromAddressDescription")]
        [ResourceDisplayName(typeof(DesignResources), "EmailTraceListenerDataFromAddressDisplayName")]
        [DesigntimeDefaultAttribute("from@example.com")]
        public string FromAddress
        {
            get { return (string)base[fromAddressProperty]; }
            set { base[fromAddressProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the Subject prefix.
        /// </summary>
        [ConfigurationProperty(subjectLineStarterProperty)]
        [ResourceDescription(typeof(DesignResources), "EmailTraceListenerDataSubjectLineStarterDescription")]
        [ResourceDisplayName(typeof(DesignResources), "EmailTraceListenerDataSubjectLineStarterDisplayName")]
        public string SubjectLineStarter
        {
            get { return (string)base[subjectLineStarterProperty]; }
            set { base[subjectLineStarterProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the Subject suffix.
        /// </summary>
        [ConfigurationProperty(subjectLineEnderProperty)]
        [ResourceDescription(typeof(DesignResources), "EmailTraceListenerDataSubjectLineEnderDescription")]
        [ResourceDisplayName(typeof(DesignResources), "EmailTraceListenerDataSubjectLineEnderDisplayName")]
        public string SubjectLineEnder
        {
            get { return (string)base[subjectLineEnderProperty]; }
            set { base[subjectLineEnderProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the SMTP server to use to send messages.
        /// </summary>
        [ConfigurationProperty(smtpServerProperty, DefaultValue = "127.0.0.1")]
        [ResourceDescription(typeof(DesignResources), "EmailTraceListenerDataSmtpServerDescription")]
        [ResourceDisplayName(typeof(DesignResources), "EmailTraceListenerDataSmtpServerDisplayName")]
        public string SmtpServer
        {
            get { return (string)base[smtpServerProperty]; }
            set { base[smtpServerProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the SMTP port.
        /// </summary>
        [ConfigurationProperty(smtpPortProperty, DefaultValue = 25)]
        [ResourceDescription(typeof(DesignResources), "EmailTraceListenerDataSmtpPortDescription")]
        [ResourceDisplayName(typeof(DesignResources), "EmailTraceListenerDataSmtpPortDisplayName")]
        public int SmtpPort
        {
            get { return (int)base[smtpPortProperty]; }
            set { base[smtpPortProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the formatter name.
        /// </summary>
        [ConfigurationProperty(formatterNameProperty, IsRequired = false)]
        [Reference(typeof(NameTypeConfigurationElementCollection<FormatterData, CustomFormatterData>), typeof(FormatterData))]
        [ResourceDescription(typeof(DesignResources), "EmailTraceListenerDataFormatterDescription")]
        [ResourceDisplayName(typeof(DesignResources), "EmailTraceListenerDataFormatterDisplayName")]
        public string Formatter
        {
            get { return (string)base[formatterNameProperty]; }
            set { base[formatterNameProperty] = value; }
        }

        /// <summary>
        /// How do you authenticate against the email server?
        /// </summary>
        [ConfigurationProperty(authenticationModeProperty, IsRequired = false, DefaultValue = EmailAuthenticationMode.None)]
        [ResourceDisplayName(typeof(DesignResources), "EmailTraceListenerAuthenticationModeDisplayName")]
        [ResourceDescription(typeof(DesignResources), "EmailTraceListenerAuthenticationModeDescription")]
        public EmailAuthenticationMode AuthenticationMode
        {
            get { return (EmailAuthenticationMode)base[authenticationModeProperty]; }
            set { base[authenticationModeProperty] = value; }
        }

        /// <summary>
        /// Use SSL to connect to the email server?
        /// </summary>
        [ConfigurationProperty(useSSLProperty, IsRequired = false, DefaultValue = false)]
        [ResourceDisplayName(typeof(DesignResources), "EmailTraceListenerUseSSLDisplayName")]
        [ResourceDescription(typeof(DesignResources), "EmailTraceListenerUseSSLDescription")]
        public bool UseSSL
        {
            get { return (bool)base[useSSLProperty]; }
            set { base[useSSLProperty] = value; }
        }

        /// <summary>
        /// User name when authenticating with user name and password.
        /// </summary>
        [ConfigurationProperty(userNameProperty, IsRequired = false)]
        [ResourceDisplayName(typeof(DesignResources), "EmailTraceListenerUserNameDisplayName")]
        [ResourceDescription(typeof(DesignResources), "EmailTraceListenerUserNameDescription")]
        public string UserName
        {
            get { return (string)base[userNameProperty]; }
            set { base[userNameProperty] = value; }
        }

        /// <summary>
        /// Password when authenticating with user name and password.
        /// </summary>
        [ConfigurationProperty(passwordProperty, IsRequired = false)]
        [ResourceDisplayName(typeof(DesignResources), "EmailTraceListenerPasswordDisplayName")]
        [ResourceDescription(typeof(DesignResources), "EmailTraceListenerPasswordDescription")]
        [ViewModel(LoggingDesignTime.ViewModelTypeNames.EmailTraceListenerPropertyViewModel)]
        public string Password
        {
            get { return (string)base[passwordProperty]; }
            set { base[passwordProperty] = value; }
        }

        /// <summary>
        /// Builds the <see cref="TraceListener" /> object represented by this configuration object.
        /// </summary>
        /// <param name="settings">The logging configuration settings.</param>
        /// <returns>
        /// An <see cref="EmailTraceListener"/>.
        /// </returns>
        protected override TraceListener CoreBuildTraceListener(LoggingSettings settings)
        {
            var formatter = this.BuildFormatterSafe(settings, this.Formatter);

            return new EmailTraceListener(
                this.ToAddress,
                this.FromAddress,
                this.SubjectLineStarter,
                this.SubjectLineEnder,
                this.SmtpServer,
                this.SmtpPort,
                formatter,
                this.AuthenticationMode,
                this.UserName,
                this.Password,
                this.UseSSL);
        }
    }
}
