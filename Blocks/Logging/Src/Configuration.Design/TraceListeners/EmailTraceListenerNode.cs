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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners
{
    /// <summary>
    /// Represents a <see cref="EmailTraceListenerData"/> configuration element.
    /// </summary>
    public class EmailTraceListenerNode: TraceListenerNode
    {
        private string formatterName;
        private FormatterNode formatterNode;
		private string toAddress;
		private string fromAddress;
		private string subjectLineStarter;
		private string subjectLineEnder;
		private string smtpServer;
		private int smtpPort;

        /// <summary>
        /// Initialize a new instance of the <see cref="EmailTraceListenerNode"/> class.
        /// </summary>
        public EmailTraceListenerNode()
            :this(new EmailTraceListenerData(Resources.EmailTraceListenerNode, DefaultValues.EmailListenerToAddress, DefaultValues.EmailListenerFromAddress, string.Empty, string.Empty, DefaultValues.EmailListenerSmtpAddress, DefaultValues.EmailListenerSmtpPort, string.Empty))
        {
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="EmailTraceListenerNode"/> class with a <see cref="EmailTraceListenerData"/> class.
		/// </summary>
		/// <param name="emailTraceListenerData">A <see cref="EmailTraceListenerData"/> class.</param>
        public EmailTraceListenerNode(EmailTraceListenerData emailTraceListenerData)
        {
			if (null == emailTraceListenerData) throw new ArgumentNullException("emailTraceListenerData");

			Rename(emailTraceListenerData.Name);
			TraceOutputOptions = emailTraceListenerData.TraceOutputOptions;
            Filter = emailTraceListenerData.Filter;
            this.formatterName = emailTraceListenerData.Formatter;
			this.toAddress = emailTraceListenerData.ToAddress;
			this.fromAddress = emailTraceListenerData.FromAddress;
			this.subjectLineStarter = emailTraceListenerData.SubjectLineStarter;
			this.subjectLineEnder = emailTraceListenerData.SubjectLineEnder;
			this.smtpServer = emailTraceListenerData.SmtpServer;
			this.smtpPort = emailTraceListenerData.SmtpPort;
        }

        /// <summary>
        /// Gets or sets the to address for the email.
        /// </summary>
		/// <value>
		/// The to address for the email.
		/// </value>
        [Required]
        [SRDescription("EmailSinkToAddressDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string ToAddress
        {
            get { return toAddress; }
            set { toAddress = value; }
        }

        /// <summary>
        /// Gets or sets the from address for the email.
        /// </summary>
		/// <value>
		/// The from address for the email.
		/// </value>
        [Required]
        [SRDescription("EmailTraceListenerFromAddressDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string FromAddress
        {
            get { return fromAddress; }
            set { fromAddress = value; }
        }

        /// <summary>
        /// Gets or sets the subject line starter for the email.
        /// </summary>
		/// <value>
		/// The subject line starter for the email.
		/// </value>
        [SRDescription("EmailSinkSubjectLineStarterDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string SubjectLineStarter
        {
            get { return subjectLineStarter; }
            set { subjectLineStarter = value; }
        }

		/// <summary>
		/// Gets or sets the subject line ender for the email.
		/// </summary>
		/// <value>
		/// The subject line ender for the email.
		/// </value>
        [SRDescription("EmailTraceListenerSubjectLineEnderDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string SubjectLineEnder
        {
            get { return subjectLineEnder; }
            set { subjectLineEnder = value; }
        }

        /// <summary>
        /// Gets or sets the SMTP server to send messages through.
        /// </summary>
		/// <value>
		/// The SMTP server to send messages through.
		/// </value>
        [Required]
        [SRDescription("EmailTraceListenerSmtpServerDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string SmtpServer
        {
            get { return smtpServer; }
            set { smtpServer = value; }
        }

		/// <summary>
		/// Gets or sets the SMTP port to send messages through.
		/// </summary>
		/// <value>
		/// The SMTP port to send messages through.
		/// </value>
        [Required]
        [SRDescription("EmailTraceListenerSmtpPortDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public int SmtpPort
        {
            get { return smtpPort; }
            set { smtpPort = value; }
        }

        /// <summary>
        /// Gets or sets the formatter for the email.
        /// </summary>
		/// <value>
		/// The formatter for the email.
		/// </value>
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(FormatterNode))]
        [SRDescription("FormatDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public FormatterNode Formatter
        {
            get { return formatterNode; }
            set
            {
                formatterNode = LinkNodeHelper.CreateReference<FormatterNode>(formatterNode,
                    value,
                    OnFormatterNodeRemoved,
                    OnFormatterNodeRenamed);

                formatterName = formatterNode == null ? String.Empty : formatterNode.Name;
            }
        }

		/// <summary>
		/// Gets the <see cref="EmailTraceListenerData"/> that this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="EmailTraceListenerData"/> that this node represents.
		/// </value>
		public override TraceListenerData TraceListenerData
		{
			get
			{
				EmailTraceListenerData data = new EmailTraceListenerData(Name, toAddress, fromAddress, subjectLineStarter, 
					subjectLineEnder, smtpServer, smtpPort, formatterName);
				data.TraceOutputOptions = TraceOutputOptions;
				return data;
			}
		}

		/// <summary>
		/// Sets the formatter to use for this listener.
		/// </summary>
		/// <param name="formatterNodeReference">
		/// A <see cref="FormatterNode"/> reference or <see langword="null"/> if no formatter is defined.
		/// </param>
		protected override void SetFormatterReference(ConfigurationNode formatterNodeReference)
		{
			if (formatterName == formatterNodeReference.Name) Formatter = (FormatterNode)formatterNodeReference;
		}		

        private void OnFormatterNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            formatterNode = null;
        }

        private void OnFormatterNodeRenamed(object sender, ConfigurationNodeChangedEventArgs e)
        {
            formatterName = e.Node.Name;
        }
    }
}
