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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build a <see cref="LogWriterStructureHolder"/> described by the <see cref="LoggingSettings"/> configuration section.
	/// </summary>
	public class LogWriterStructureHolderCustomFactory : ICustomFactory
	{
		/// <summary>
		/// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// </summary>
		public static LogWriterStructureHolderCustomFactory Instance = new LogWriterStructureHolderCustomFactory();

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds a <see cref="LogWriterStructureHolder"/> described by the <see cref="LoggingSettings"/> configuration section.
		/// </summary>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="name">The name of the instance to build. It is part of the <see cref="ICustomFactory.CreateObject(IBuilderContext, string, IConfigurationSource, ConfigurationReflectionCache)"/> method, but it is not used in this implementation.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A fully initialized instance of <see cref="LogWriterStructureHolder"/>.</returns>
		public object CreateObject(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			LoggingSettings loggingSettings = LoggingSettings.GetLoggingSettings(configurationSource);
			ValidateLoggingSettings(loggingSettings);

			TraceListenerCustomFactory.TraceListenerCache traceListenerCache
				= TraceListenerCustomFactory.CreateTraceListenerCache(loggingSettings.TraceListeners.Count);

			ICollection<ILogFilter> logFilters = new List<ILogFilter>();
			foreach (LogFilterData logFilterData in loggingSettings.LogFilters)
			{
				logFilters.Add(LogFilterCustomFactory.Instance.Create(context, logFilterData, configurationSource, reflectionCache));
			}

			IDictionary<string, LogSource> traceSources = new Dictionary<string, LogSource>();
			foreach (TraceSourceData traceSourceData in loggingSettings.TraceSources)
			{
				traceSources.Add(traceSourceData.Name, LogSourceCustomFactory.Instance.Create(context, traceSourceData, configurationSource, reflectionCache, traceListenerCache));
			}

			LogSource allEventsTraceSource
				= LogSourceCustomFactory.Instance.Create(context, loggingSettings.SpecialTraceSources.AllEventsTraceSource, configurationSource, reflectionCache, traceListenerCache);
			LogSource notProcessedTraceSource
				= LogSourceCustomFactory.Instance.Create(context, loggingSettings.SpecialTraceSources.NotProcessedTraceSource, configurationSource, reflectionCache, traceListenerCache);
			LogSource errorsTraceSource
				= LogSourceCustomFactory.Instance.Create(context, loggingSettings.SpecialTraceSources.ErrorsTraceSource, configurationSource, reflectionCache, traceListenerCache);

			LogWriterStructureHolder createdObject
				= new LogWriterStructureHolder(
					logFilters,
					traceSources,
					allEventsTraceSource,
					notProcessedTraceSource,
					errorsTraceSource,
					loggingSettings.DefaultCategory,
					loggingSettings.TracingEnabled,
					loggingSettings.LogWarningWhenNoCategoriesMatch,
                    loggingSettings.RevertImpersonation);

			return createdObject;
		}

		private void ValidateLoggingSettings(LoggingSettings loggingSettings)
		{
			if (loggingSettings == null)
			{
				throw new System.Configuration.ConfigurationErrorsException(Resources.ExceptionLoggingSectionNotFound);
			}
		}
	}
}
