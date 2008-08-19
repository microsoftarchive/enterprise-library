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
using System.Net.Mail;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	/// <summary>
	/// Represents a <see cref="EmailMessage"/>.
	/// Encapsulates a System.Net.MailMessage with functions to accept a LogEntry, Formatting, and sending of emails
	/// </summary>
	public class EmailMessage
	{
		private EmailTraceListenerData configurationData;
		private ILogFormatter formatter;
		private LogEntry logEntry;

		/// <summary>
		/// Initializes a <see cref="EmailMessage"/> with email configuration data, logentry, and formatter 
		/// </summary>
		/// <param name="configurationData">The configuration data <see cref="EmailTraceListenerData"/> 
		/// that represents how to create the email message</param>
		/// <param name="logEntry">The LogEntry <see cref="LogEntry"/> to send via email.</param>
		/// <param name="formatter">The Formatter <see cref="ILogFormatter"/> which determines how the 
		/// email message should be formatted</param>
		public EmailMessage(EmailTraceListenerData configurationData, LogEntry logEntry, ILogFormatter formatter)
		{
			this.configurationData = configurationData;
			this.logEntry = logEntry;
			this.formatter = formatter;
		}

		/// <summary>
		/// Initializes a <see cref="EmailMessage"/> with the raw data to create and email, the logentry, and the formatter 
		/// </summary>
		/// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
		/// <param name="fromAddress">Represents from whom the email is sent.</param>
		/// <param name="subjectLineStarter">Starting text for the subject line.</param>
		/// <param name="subjectLineEnder">Ending text for the subject line.</param>
		/// <param name="smtpServer">The name of the SMTP server.</param>
		/// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
		/// <param name="logEntry">The LogEntry <see cref="LogEntry"/> to send via email.</param>
		/// <param name="formatter">The Formatter <see cref="ILogFormatter"/> which determines how the 
		/// email message should be formatted</param>
		public EmailMessage(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, LogEntry logEntry, ILogFormatter formatter)
		{
			this.configurationData = new EmailTraceListenerData(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, string.Empty);
			this.logEntry = logEntry;
			this.formatter = formatter;
		}

		/// <summary>
		/// Initializes a <see cref="EmailMessage"/> with the raw data to create and email, a message, and the formatter 
		/// </summary>
		/// <param name="toAddress">A semicolon delimited string the represents to whom the email should be sent.</param>
		/// <param name="fromAddress">Represents from whom the email is sent.</param>
		/// <param name="subjectLineStarter">Starting text for the subject line.</param>
		/// <param name="subjectLineEnder">Ending text for the subject line.</param>
		/// <param name="smtpServer">The name of the SMTP server.</param>
		/// <param name="smtpPort">The port on the SMTP server to use for sending the email.</param>
		/// <param name="message">Represents the message to send via email.</param>
		/// <param name="formatter">The Formatter <see cref="ILogFormatter"/> which determines how the 
		/// email message should be formatted</param>
		public EmailMessage(string toAddress, string fromAddress, string subjectLineStarter, string subjectLineEnder, string smtpServer, int smtpPort, string message, ILogFormatter formatter)
		{
			this.configurationData = new EmailTraceListenerData(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, string.Empty);
			this.logEntry = new LogEntry();
			logEntry.Message = message;
			this.formatter = formatter;
		}

		/// <summary>
		/// Determines whether the string is <see langword="null"/> or empty
		/// </summary>
		/// <param name="subjectLineMarker">string to evaluate</param>
		/// <returns>Boolean value that returns true if the string is <see langword="null"/> or empty</returns>
		private bool IsEmpty(string subjectLineMarker)
		{
			return subjectLineMarker == null || subjectLineMarker.Length == 0;
		}

		/// <summary>
		/// Creates the prefix for the subject line
		/// </summary>
		/// <param name="subjectLineField">string to add as the subject line prefix (plus whitespace) if it is not empty.</param>
		/// <returns>modified string to use as subject line prefix</returns>
		private string GenerateSubjectPrefix(string subjectLineField)
		{
			return IsEmpty(subjectLineField)
				? ""
				: subjectLineField + " ";
		}

		/// <summary>
		/// Creates the suffix for the subject line.
		/// </summary>
		/// <param name="subjectLineField">string to add as the subject line suffix (plus whitespace) if it is not empty.</param>
		/// <returns>modified string to use as subject line suffix</returns>
		private string GenerateSubjectSuffix(string subjectLineField)
		{
			return IsEmpty(subjectLineField)
				? ""
				: " " + subjectLineField;
		}

		/// <summary>
		/// Creates a <see cref="MailMessage"/> from the configuration data which was used to create the instance of this object.
		/// </summary>
		/// <returns>A new <see cref="MailMessage"/>.</returns>
		protected MailMessage CreateMailMessage()
		{
			string header = GenerateSubjectPrefix(configurationData.SubjectLineStarter);
			string footer = GenerateSubjectSuffix(configurationData.SubjectLineEnder);

			string sendToSmtpSubject = header + logEntry.Severity.ToString() + footer;

			MailMessage message = new MailMessage();
			string[] toAddresses = configurationData.ToAddress.Split(';');
			foreach (string toAddress in toAddresses)
			{
				message.To.Add(new MailAddress(toAddress));
			}

			message.From = new MailAddress(configurationData.FromAddress);

			message.Body = (formatter != null) ? formatter.Format(logEntry) : logEntry.Message;
			message.Subject = sendToSmtpSubject;
			message.BodyEncoding = Encoding.UTF8;

			return message;
		}

		/// <summary>
		/// Uses the settings for the SMTP server and SMTP port to send the new mail message
		/// </summary>
		public virtual void Send()
		{
			using (MailMessage message = CreateMailMessage())
			{
				SendMessage(message);
			}
		}

		/// <summary>
		/// Uses the settings for the SMTP server and SMTP port to send the MailMessage that it is passed
		/// </summary>
		/// <param name="message">MailMessage to send via SMTP</param>
		public virtual void SendMessage(MailMessage message)
		{
			SmtpClient smtpClient = new SmtpClient(configurationData.SmtpServer, configurationData.SmtpPort);
			smtpClient.Send(message);
		}
	}
}
