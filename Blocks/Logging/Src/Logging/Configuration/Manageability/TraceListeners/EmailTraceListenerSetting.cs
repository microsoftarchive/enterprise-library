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

using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
	/// <summary>
	/// Represents an instance of <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.EmailTraceListenerData"/>
	/// as an instrumentation class.
	/// </summary>
	[ManagementEntity]
	public class EmailTraceListenerSetting : TraceListenerSetting
	{
		string formatter;
		string fromAddress;
		int smtpPort;
		string smtpServer;
		string subjectLineEnder;
		string subjectLineStarter;
		string toAddress;

		/// <summary>
		/// Initialize a new instance of the <see cref="EmailTraceListenerSetting"/> class.
		/// </summary>
		/// <param name="sourceElement">The configuration for the listener.</param>
		/// <param name="name">The name of the listener.</param>
		/// <param name="formatter">The formatter to use.</param>
		/// <param name="fromAddress">The from address for the email.</param>
		/// <param name="smtpPort">The SMTP port.</param>
		/// <param name="smtpServer">The SMTP server.</param>
		/// <param name="subjectLineEnder">The subject line ender.</param>
		/// <param name="subjectLineStarter">The subject line starter.</param>
		/// <param name="toAddress">The to address for the email.</param>
		/// <param name="traceOutputOptions">The trace output options.</param>
		/// <param name="filter">The filter value.</param>
		public EmailTraceListenerSetting(EmailTraceListenerData sourceElement,
										   string name,
										   string formatter,
										   string fromAddress,
										   int smtpPort,
										   string smtpServer,
										   string subjectLineEnder,
										   string subjectLineStarter,
										   string toAddress,
										   string traceOutputOptions,
										   string filter)
			: base(sourceElement, name, traceOutputOptions, filter)
		{
			this.formatter = formatter;
			this.fromAddress = fromAddress;
			this.smtpPort = smtpPort;
			this.smtpServer = smtpServer;
			this.subjectLineEnder = subjectLineEnder;
			this.subjectLineStarter = subjectLineStarter;
			this.toAddress = toAddress;
		}

		/// <summary>
		/// Gets the name of the formatter for the represented configuration element.
		/// </summary>
		[ManagementConfiguration]
		public string Formatter
		{
			get { return formatter; }
			set { formatter = value; }
		}

		/// <summary>
		/// Gets the from address for the represented configuration element.
		/// </summary>
		[ManagementConfiguration]
		public string FromAddress
		{
			get { return fromAddress; }
			set { fromAddress = value; }
		}

		/// <summary>
		/// Gets the smtp port for the represented configuration element.
		/// </summary>
		[ManagementConfiguration]
		public int SmtpPort
		{
			get { return smtpPort; }
			set { smtpPort = value; }
		}

		/// <summary>
		/// Gets the smtp server for the represented configuration element.
		/// </summary>
		[ManagementConfiguration]
		public string SmtpServer
		{
			get { return smtpServer; }
			set { smtpServer = value; }
		}

		/// <summary>
		/// Gets the subject line ender for the represented configuration element.
		/// </summary>
		[ManagementConfiguration]
		public string SubjectLineEnder
		{
			get { return subjectLineEnder; }
			set { subjectLineEnder = value; }
		}

		/// <summary>
		/// Gets the subject line starter for the represented configuration element.
		/// </summary>
		[ManagementConfiguration]
		public string SubjectLineStarter
		{
			get { return subjectLineStarter; }
			set { subjectLineStarter = value; }
		}

		/// <summary>
		/// Gets the to address for the represented configuration element.
		/// </summary>
		[ManagementConfiguration]
		public string ToAddress
		{
			get { return toAddress; }
			set { toAddress = value; }
		}

		/// <summary>
		/// Returns the <see cref="EmailTraceListenerSetting"/> instance corresponding to the provided values for the key properties.
		/// </summary>
		/// <param name="ApplicationName">The value for the ApplicationName key property.</param>
		/// <param name="SectionName">The value for the SectionName key property.</param>
		/// <param name="Name">The value for the Name key property.</param>
		/// <returns>The published <see cref="EmailTraceListenerSetting"/> instance specified by the values for the key properties,
		/// or <see langword="null"/> if no such an instance is currently published.</returns>
		[ManagementBind]
		public static EmailTraceListenerSetting BindInstance(string ApplicationName,
															 string SectionName,
															 string Name)
		{
			return BindInstance<EmailTraceListenerSetting>(ApplicationName, SectionName, Name);
		}

		/// <summary>
		/// Returns an enumeration of the published <see cref="EmailTraceListenerSetting"/> instances.
		/// </summary>
		[ManagementEnumerator]
		public static IEnumerable<EmailTraceListenerSetting> GetInstances()
		{
			return GetInstances<EmailTraceListenerSetting>();
		}

		/// <summary>
		/// Saves the changes on the <see cref="EmailTraceListenerSetting"/> to its corresponding configuration object.
		/// </summary>
		/// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
		/// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return EmailTraceListenerDataWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}