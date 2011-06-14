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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Container = Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Container;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Configuration settings for client-side logging applications.
    /// </summary>
    partial class LoggingSettings
    {
        private const string ErrorsTraceSourceKey = "___ERRORS";
        private const string AllTraceSourceKey = "___ALL";
        private const string NoMatchesTraceSourceKey = "___NO_MATCHES";

        /// <summary>
        /// Configuration section name for logging client settings.
        /// </summary>
        public const string SectionName = BlockSectionNames.Logging;

        /// <summary>
        /// Initialize a new instance of the <see cref="LoggingSettings"/> using the given name.
        /// </summary>
        /// <param name="name">The name to use for this instance</param>
        public LoggingSettings(string name)
            : this(name, true, string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="LoggingSettings"/> using the given values.
        /// </summary>
        /// <param name="name">The name to use for this instance</param>
        /// <param name="tracingEnabled">Should tracing be enabled?</param>
        /// <param name="defaultCategory">The default category to use.</param>
        public LoggingSettings(string name, bool tracingEnabled, string defaultCategory)
        {
            Name = name;
            TracingEnabled = tracingEnabled;
            DefaultCategory = defaultCategory;
        }

        /// <summary>
        /// Retrieves the <see cref="LoggingSettings"/> section from the configuration source.
        /// </summary>
        /// <param name="configurationSource">The <see cref="IConfigurationSource"/> to get the section from.</param>
        /// <returns>The logging section.</returns>
        public static LoggingSettings GetLoggingSettings(IConfigurationSource configurationSource)
        {
            if (configurationSource == null) throw new ArgumentNullException("configurationSource");
            return (LoggingSettings)configurationSource.GetSection(SectionName);
        }

        /// <summary>
        /// Return the <see cref="TypeRegistration"/> objects needed to configure
        /// the container.
        /// </summary>
        /// <returns>The sequence of <see cref="TypeRegistration"/> objects.</returns>
        public IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            return GetRegistrationsCore(configurationSource);
        }

        /// <summary>
        /// Return the <see cref="TypeRegistration"/> objects needed to reconfigure
        /// the container after a configuration source has changed.
        /// </summary>
        /// <remarks>If there are no reregistrations, return an empty sequence.</remarks>
        /// <param name="configurationSource">The <see cref="IConfigurationSource"/> containing
        /// the configuration information.</param>
        /// <returns>The sequence of <see cref="TypeRegistration"/> objects.</returns>
        public IEnumerable<TypeRegistration> GetUpdatedRegistrations(IConfigurationSource configurationSource)
        {
            return GetRegistrationsCore(configurationSource);
        }

        private static TypeRegistration CreateLogSourceRegistration(TraceSourceData traceSourceData, string name)
        {
            var registration = (traceSourceData ?? new TraceSourceData()).GetRegistrations();
            registration.Name = name;
            return registration;
        }

        private TypeRegistration CreateLogWriterStructureHolderRegistration()
        {
            return
                new TypeRegistration<LogWriterStructureHolder>(() =>
                    new LogWriterStructureHolder(
                        Container.ResolvedEnumerable<ILogFilter>(LogFilters.Select(lfd => lfd.Name)),
                        TraceSources.Select(tsd => tsd.Name).ToArray(),
                        Container.ResolvedEnumerable<LogSource>(TraceSources.Select(tsd => tsd.Name)),
                        Container.Resolved<LogSource>(AllTraceSourceKey),
                        Container.Resolved<LogSource>(NoMatchesTraceSourceKey),
                        Container.Resolved<LogSource>(ErrorsTraceSourceKey),
                        DefaultCategory,
                        TracingEnabled,
                        LogWarningWhenNoCategoriesMatch,
                        RevertImpersonation))
                {
                    Lifetime = TypeRegistrationLifetime.Transient,
                    IsDefault = true
                };
        }
    }
}
