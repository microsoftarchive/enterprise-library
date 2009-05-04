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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
	/// <summary>
	/// Base for custom factories used to create default event loggers specific to individual blocks.
	/// These default event loggers are used when events need to be logged from static classes and methods
	/// that cannot participate in the normal instrumentation attachment process.
	/// </summary>
	public abstract class DefaultEventLoggerCustomFactoryBase : ICustomFactory
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// </summary>
		/// <param name="context">Represents the context in which a build-up or tear-down runs.</param>
		/// <param name="name">Unused parameter</param>
		/// <param name="configurationSource">Represents a source for getting configuration</param>
		/// <param name="reflectionCache">Unused parameter.</param>
		/// <returns>Fully initialized instance of a default event logging object</returns>
		public object CreateObject(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			InstrumentationConfigurationSection objectConfiguration	= GetConfiguration(configurationSource);

			object createdObject = DoCreateObject((objectConfiguration));

			return createdObject;
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds an initialized default event logging object.
		/// </summary>
		/// <param name="instrumentationConfigurationSection">The instrumentation section that is used as configuration.</param>
		/// <returns>A fully initialized instance of a default event logging object.</returns>
		protected abstract object DoCreateObject(InstrumentationConfigurationSection instrumentationConfigurationSection);

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
