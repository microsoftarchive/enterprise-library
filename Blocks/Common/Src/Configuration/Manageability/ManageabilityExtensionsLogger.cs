//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	/// <summary>
	/// Provides logging services to the Enterprise Library Manageability Extensions.
	/// </summary>
	[HasInstallableResources()]
	[EventLogDefinition("Application", EventLogSourceName)]
	public static class ManageabilityExtensionsLogger
	{
		internal const String EventLogSourceName = "Enterprise Library Manageability Extensions";

		/// <summary>
		/// Logs an error detected while overriding a configuration object with policy values.
		/// </summary>
		/// <param name="exception">The exception representing the error.</param>
		public static void LogExceptionWhileOverriding(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException("exception");

			LogException(exception, Resources.ExceptionErrorWhileOverriding);
		}

		/// <summary>
		/// Logs an error.
		/// </summary>
		/// <param name="exception">The exception representing the error.</param>
		/// <param name="title">The title that describes the error.</param>
		public static void LogException(Exception exception, String title)
		{
            if (exception == null) throw new ArgumentNullException("exception");

			StringBuilder entryTextBuilder = new StringBuilder();
			entryTextBuilder.AppendLine(title);
			entryTextBuilder.Append(exception.Message);

			try
			{
				EventLog.WriteEntry(EventLogSourceName, entryTextBuilder.ToString(), EventLogEntryType.Error);
			}
			catch
			{
				// cannot write to the event log
			}
		}
	}
}
