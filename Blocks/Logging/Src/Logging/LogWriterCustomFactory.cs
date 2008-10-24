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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the process to build a <see cref="LogWriter"/> described by the <see cref="LoggingSettings"/> configuration section.
	/// </summary>
	/// <remarks>
	/// This is used by the <see cref="ConfiguredObjectStrategy"/> when an instance of the <see cref="LogWriter"/> class is requested to 
	/// a properly configured <see cref="IBuilder"/> instance.
	/// </remarks>
	public class LogWriterCustomFactory : ICustomFactory
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds a <see cref="LogWriter"/> described by the <see cref="LoggingSettings"/> configuration section.
		/// </summary>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="name">The name of the instance to build. It is part of the <see cref="ICustomFactory.CreateObject(IBuilderContext, string, IConfigurationSource, ConfigurationReflectionCache)"/> method, but it is not used in this implementation.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A fully initialized instance of <see cref="LogWriter"/>.</returns>
		public object CreateObject(IBuilderContext context,
								   string name,
								   IConfigurationSource configurationSource,
								   ConfigurationReflectionCache reflectionCache)
		{
			LogWriterStructureHolder structureHolder
				=
				(LogWriterStructureHolder)
				LogWriterStructureHolderCustomFactory.Instance.CreateObject(context, name, configurationSource, reflectionCache);

			LogWriterStructureHolderUpdater structureHolderUpdater = new LogWriterStructureHolderUpdater(configurationSource);
			LogWriter createdObject = new LogWriter(structureHolder, structureHolderUpdater);
			structureHolderUpdater.SetLogWriter(createdObject);

			// add the writer to the locator, if necessary.
			if (context.Locator != null)
			{
				context.Locator.Add(new NamedTypeBuildKey(typeof(LogWriter), name), createdObject);
			}
			if (context.Lifetime != null)
			{
				context.Lifetime.Add(createdObject);
			}

			return createdObject;
		}
	}
}
