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

using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Manageability
{
	/// <summary>
	/// Represents the configuration information from a 
	/// <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.LoggingExceptionHandlerData"/> instance.
	/// </summary>
	/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.LoggingExceptionHandlerData"/>
	[ManagementEntity]
	public class LoggingExceptionHandlerSetting : ExceptionHandlerSetting
	{
		private int eventId;
		private string formatterType;
		private string logCategory;
		private int priority;
		private string severity;
		private string title;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="eventId"></param>
		/// <param name="formatterType"></param>
		/// <param name="logCategory"></param>
		/// <param name="priority"></param>
		/// <param name="severity"></param>
		/// <param name="title"></param>
		/// <param name="sourceElement"></param>
		public LoggingExceptionHandlerSetting(ConfigurationElement sourceElement,
		                                      string name,
		                                      int eventId,
		                                      string formatterType,
		                                      string logCategory,
		                                      int priority,
		                                      string severity,
		                                      string title)
			: base(sourceElement, name)
		{
			this.eventId = eventId;
			this.formatterType = formatterType;
			this.logCategory = logCategory;
			this.priority = priority;
			this.severity = severity;
			this.title = title;
		}

		/// <summary>
		/// Gets the event id for the represented configuration object.
		/// </summary>
		/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.LoggingExceptionHandlerData.EventId">LoggingExceptionHandlerData.EventId</seealso>
		[ManagementConfiguration]
		public int EventId
		{
			get { return eventId; }
			set { eventId = value; }
		}

		/// <summary>
		/// Gets the name of the formatter type for the represented configuration object.
		/// </summary>
		/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.LoggingExceptionHandlerData.FormatterType">LoggingExceptionHandlerData.FormatterType</seealso>
		[ManagementConfiguration]
		public string FormatterType
		{
			get { return formatterType; }
			set { formatterType = value; }
		}

		/// <summary>
		/// Gets the log category for the represented configuration object.
		/// </summary>
		/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.LoggingExceptionHandlerData.LogCategory">LoggingExceptionHandlerData.LogCategory</seealso>
		[ManagementConfiguration]
		public string LogCategory
		{
			get { return logCategory; }
			set { logCategory = value; }
		}

		/// <summary>
		/// Gets the priority for the represented configuration object.
		/// </summary>
		/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.LoggingExceptionHandlerData.Priority">LoggingExceptionHandlerData.Priority</seealso>
		[ManagementConfiguration]
		public int Priority
		{
			get { return priority; }
			set { priority = value; }
		}

		/// <summary>
		/// Gets the severity for the represented configuration object.
		/// </summary>
		/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.LoggingExceptionHandlerData.Severity">LoggingExceptionHandlerData.Severity</seealso>
		[ManagementConfiguration]
		public string Severity
		{
			get { return severity; }
			set { severity = value; }
		}

		/// <summary>
		/// Gets the title for the represented configuration object.
		/// </summary>
		/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.LoggingExceptionHandlerData.Title">LoggingExceptionHandlerData.Title</seealso>
		[ManagementConfiguration]
		public string Title
		{
			get { return title; }
			set { title = value; }
		}

		/// <summary>
		/// Returns an enumeration of the published <see cref="LoggingExceptionHandlerSetting"/> instances.
		/// </summary>
		/// <returns></returns>
		[ManagementEnumerator]
		public static IEnumerable<LoggingExceptionHandlerSetting> GetInstances()
		{
			return GetInstances<LoggingExceptionHandlerSetting>();
		}

		/// <summary>
		/// Returns the <see cref="LoggingExceptionHandlerSetting"/> instance corresponding to the provided values for the key properties.
		/// </summary>
		/// <param name="ApplicationName">The value for the ApplicationName key property.</param>
		/// <param name="SectionName">The value for the SectionName key property.</param>
		/// <param name="Policy"></param>
		/// <param name="ExceptionType"></param>
		/// <param name="Name">The value for the Name key property.</param>
		/// <returns>The published <see cref="LoggingExceptionHandlerSetting"/> instance specified by the values for the key properties,
		/// or <see langword="null"/> if no such an instance is currently published.</returns>
		[ManagementBind]
		public static LoggingExceptionHandlerSetting BindInstance(string ApplicationName,
		                                                          string SectionName,
		                                                          string Policy,
		                                                          string ExceptionType,
		                                                          string Name)
		{
			return BindInstance<LoggingExceptionHandlerSetting>(ApplicationName, SectionName, Policy, ExceptionType, Name);
		}

		/// <summary>
		/// Saves the changes on the <see cref="LoggingExceptionHandlerSetting"/> to its corresponding configuration object.
		/// </summary>
		/// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
		/// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return LoggingExceptionHandlerDataWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}