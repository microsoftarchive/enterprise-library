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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Provides instrumentation specific configuration extensions to <see cref="IConfigurationSourceBuilder"/>
    /// </summary>
    /// <seealso cref="ConfigurationSourceBuilder"/>
    public static class ConfigurationSourceBuilderExtensions
    {
        /// <summary>
        /// Configure Instrumentation for Enterprise Library.
        /// </summary>
        /// <param name="configurationSourceBuilderRoot">Extends the <see cref="IConfigurationSourceBuilder"/></param>
        /// <returns></returns>
        public static IInstrumentationConfiguration ConfigureInstrumentation(this IConfigurationSourceBuilder configurationSourceBuilderRoot)
        {
            return new InstrumentationConfigurationSectionBuilder(configurationSourceBuilderRoot);
        }

        private class InstrumentationConfigurationSectionBuilder :
            IInstrumentationConfiguration
        {

            IConfigurationSourceBuilder root;
            InstrumentationConfigurationSection section = new InstrumentationConfigurationSection(false, false);

            internal InstrumentationConfigurationSectionBuilder(IConfigurationSourceBuilder configurationSourceBuilderRoot)
            {

                if (configurationSourceBuilderRoot.Contains(InstrumentationConfigurationSection.SectionName))
                    throw new InvalidOperationException("ConfigurationSourceBuilder already contains instrumentation settings");

                configurationSourceBuilderRoot.AddSection(InstrumentationConfigurationSection.SectionName, section);
                this.root = configurationSourceBuilderRoot;
            }


            IInstrumentationConfiguration IInstrumentationConfiguration.EnableLogging()
            {
                section.EventLoggingEnabled = true;
                return this;
            }


            IInstrumentationConfiguration IInstrumentationConfiguration.EnablePerformanceCounters()
            {
                section.PerformanceCountersEnabled = true;
                return this;
            }


            IInstrumentationConfiguration IInstrumentationConfiguration.ForApplicationInstance(string application)
            {
                section.ApplicationInstanceName = application;
                return this;
            }
        }
    }

    /// <summary>
    /// Defines instrumentation configuration options.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IInstrumentationConfiguration : IFluentInterface
    {
        /// <summary>
        /// Enable logging 
        /// </summary>
        /// <returns></returns>
        IInstrumentationConfiguration EnableLogging();

        /// <summary>
        /// Enable performance counters for instrumentation
        /// </summary>
        /// <returns></returns>
        IInstrumentationConfiguration EnablePerformanceCounters();

        /// <summary>
        /// Set application instance for instrumentation.
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        IInstrumentationConfiguration ForApplicationInstance(string application);
    }
}
