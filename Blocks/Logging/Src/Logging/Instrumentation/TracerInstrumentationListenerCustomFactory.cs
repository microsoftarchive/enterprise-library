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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation
{
    /// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build a <see cref="TracerInstrumentationListener"/> according to the instrumentation configuration.
	/// </summary>
    public class TracerInstrumentationListenerCustomFactory : ICustomFactory
    {
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds a <see cref="TracerInstrumentationListener"/>.
		/// </summary>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="name">The name of the instance to build. It is part of the <see cref="ICustomFactory.CreateObject(IBuilderContext, string, IConfigurationSource, ConfigurationReflectionCache)"/> method, but it is not used in this implementation.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A fully initialized instance of <see cref="TracerInstrumentationListener"/>.</returns>
		public object CreateObject(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            InstrumentationConfigurationSection objectConfiguration
                = GetConfiguration(configurationSource);

            TracerInstrumentationListener createdObject
                = new TracerInstrumentationListener(objectConfiguration.PerformanceCountersEnabled);

            return createdObject;
        }

        private InstrumentationConfigurationSection GetConfiguration(IConfigurationSource configurationSource)
        {
            InstrumentationConfigurationSection configurationSection
                = (InstrumentationConfigurationSection)configurationSource.GetSection(InstrumentationConfigurationSection.SectionName);
            if (configurationSection == null) configurationSection
                = new InstrumentationConfigurationSection(false, false, false);

            return configurationSection;
        }
    }
}
