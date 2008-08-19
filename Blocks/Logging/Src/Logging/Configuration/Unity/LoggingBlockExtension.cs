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
using System.Diagnostics;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity
{
	/// <summary>
	/// Container extension to the policies required to create the Logging Application Block's
	/// objects described in the configuration file.
	/// </summary>
	public class LoggingBlockExtension : EnterpriseLibraryBlockExtension
	{
		private const string ErrorsTraceSourceKey = "___ERRORS";
		private const string AllTraceSourceKey = "___ALL";
		private const string NoMatchesTraceSourceKey = "___NO_MATCHES";

		/// <summary>
		/// Adds the policies describing the Logging Application Block's objects.
		/// </summary>
		protected override void Initialize()
		{
			LoggingSettings settings = (LoggingSettings)this.ConfigurationSource.GetSection(LoggingSettings.SectionName);
            InstrumentationConfigurationSection instrumentationSettings = (InstrumentationConfigurationSection)this.ConfigurationSource.GetSection(InstrumentationConfigurationSection.SectionName);

			if (settings == null)
			{
				// skip if no settings are available
				return;
			}

			// create policies for formatters
			CreateProvidersPolicies<ILogFormatter, FormatterData>(
				Context.Policies,
				null,
				settings.Formatters,
				ConfigurationSource);

			// create policies for filters
			CreateProvidersPolicies<ILogFilter, LogFilterData>(
				Context.Policies,
				null,
				settings.LogFilters,
				ConfigurationSource);

			// create policies for trace listeners
			CreateProvidersPolicies<TraceListener, TraceListenerData>(
				Context.Policies,
				null,
				settings.TraceListeners,
				ConfigurationSource);
			CreateTraceListenersAdditionalPolicies(
				Context.Policies,
				Context.Container,
				settings.TraceListeners);

			// create policies for trace sources
			CreateTraceSourcesPolicies(
				Context.Policies,
				settings.TraceSources,
				ConfigurationSource);
			CreateTraceSourcePolicies(
				Context.Policies,
				AllTraceSourceKey,
				settings.SpecialTraceSources.AllEventsTraceSource,
				ConfigurationSource);
			CreateTraceSourcePolicies(
				Context.Policies,
				NoMatchesTraceSourceKey,
				settings.SpecialTraceSources.NotProcessedTraceSource,
				ConfigurationSource);
			CreateTraceSourcePolicies(
				Context.Policies,
				ErrorsTraceSourceKey,
				settings.SpecialTraceSources.ErrorsTraceSource,
				ConfigurationSource);

			// create policies for log writer
			CreateLogWriterPolicies(
				Context.Policies,
				Context.Container,
				settings,
				ConfigurationSource);

            CreateTraceManagerPolicies(
                Context.Policies,
                instrumentationSettings);
		}

        private static void CreateTraceManagerPolicies(IPolicyList policyList,
            InstrumentationConfigurationSection instrumentationSettings
            )
        {
            new PolicyBuilder<TraceManager, InstrumentationConfigurationSection>(
                null,
                instrumentationSettings,
                c => new TraceManager(
                        Resolve.Reference<LogWriter>(null),
                        new TracerInstrumentationListener(GetPerformanceCountersEnabled(c)))
                    ).AddPoliciesToPolicyList(policyList);
        }

		private static void CreateLogWriterPolicies(IPolicyList policyList,
			IUnityContainer container,
			LoggingSettings settings,
			IConfigurationSource ConfigurationSource)
		{
			new PolicyBuilder<LogWriter, LoggingSettings>(
					null,
					settings,
					c => new LogWriter(
						Resolve.ReferenceCollection<List<ILogFilter>, ILogFilter>(from f in settings.LogFilters select f.Name),
						Resolve.ReferenceCollection<List<LogSource>, LogSource>(from ts in settings.TraceSources select ts.Name),
						Resolve.OptionalReference<LogSource>(AllTraceSourceKey),
						Resolve.OptionalReference<LogSource>(NoMatchesTraceSourceKey),
						Resolve.Reference<LogSource>(ErrorsTraceSourceKey),
						settings.DefaultCategory,
						settings.TracingEnabled,
						settings.LogWarningWhenNoCategoriesMatch))
				.AddPoliciesToPolicyList(policyList);

			container.RegisterType(typeof(LogWriter), new ContainerControlledLifetimeManager());
		}

		private static void CreateTraceListenersAdditionalPolicies(
			IPolicyList policyList,
			IUnityContainer container,
			TraceListenerDataCollection traceListenerDataCollection)
		{
			foreach (TraceListenerData data in traceListenerDataCollection)
			{
				new PolicyBuilder<TraceListener, TraceListenerData>(new NamedTypeBuildKey(data.Type, data.Name), data)
					.SetProperty(o => o.Name).To(c => c.Name)
					.SetProperty(o => o.TraceOutputOptions).To(c => c.TraceOutputOptions)
					.SetProperty(o => o.Filter).To(c => c.Filter != SourceLevels.All ? new EventTypeFilter(c.Filter) : null)
					.AddPoliciesToPolicyList(policyList);

				// REVIEW need to set the lifetime policy through the container
				// in order for proper lifetime management
				container.RegisterType(data.Type, data.Name, new ContainerControlledLifetimeManager());
			}
		}

		private static void CreateTraceSourcesPolicies(
			IPolicyList policyList,
			IEnumerable<TraceSourceData> traceSources,
			IConfigurationSource configurationSource)
		{
			foreach (TraceSourceData traceSourceData in traceSources)
			{
				CreateTraceSourcePolicies(policyList, traceSourceData.Name, traceSourceData, configurationSource);
			}
		}

		private static void CreateTraceSourcePolicies(
			IPolicyList policyList,
			string traceSourceName,
			TraceSourceData traceSourceData,
			IConfigurationSource configurationSource)
		{
			new PolicyBuilder<LogSource, TraceSourceData>(traceSourceName,
					traceSourceData,
					c => new LogSource(
						traceSourceData.Name,
						Resolve.ReferenceCollection<List<TraceListener>, TraceListener>(from r in traceSourceData.TraceListeners select r.Name),
						traceSourceData.DefaultLevel,
						traceSourceData.AutoFlush))
				.AddPoliciesToPolicyList(policyList);
		}

        private static bool GetPerformanceCountersEnabled(InstrumentationConfigurationSection instrumentationSettings)
        {
            bool performanceCountersEnabled = false;

            if (instrumentationSettings == null)
            {
                performanceCountersEnabled = false;
            }
            else
            {
                performanceCountersEnabled = instrumentationSettings.PerformanceCountersEnabled;
            }

            return performanceCountersEnabled;
        }

		/// <summary>
		/// Returns a default <see cref="IContainerPolicyCreator"/> implementation for <paramref name="targetType"/>.
		/// </summary>
		/// <param name="targetType">The type for which policies must be built.</param>
		/// <returns>An instance of <see cref="TraceListenerConstructorArgumentMatchingPolicyCreator"/> if
		/// <paramref name="targetType"/> derives from <see cref="TraceListener"/>, otherwise an instance of
		/// <see cref="ConstructorArgumentMatchingPolicyCreator"/>.</returns>
		protected override IContainerPolicyCreator GetDefaultContainerPolicyCreator(Type targetType)
		{
			return typeof(TraceListener).IsAssignableFrom(targetType)
				? new TraceListenerConstructorArgumentMatchingPolicyCreator(targetType)
				: base.GetDefaultContainerPolicyCreator(targetType);
		}
	}
}
