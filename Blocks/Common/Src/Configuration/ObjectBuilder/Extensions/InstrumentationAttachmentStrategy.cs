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

using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	/// <summary>
	/// Drives binding of instrumentation events to handler methods based on the attributes on the 
	/// source object.
	/// </summary>
	public class InstrumentationAttachmentStrategy
	{
		InstrumentationAttacherFactory attacherFactory = new InstrumentationAttacherFactory();

		/// <overloads>
		/// Attaches the instrumentation events in the <paramref name="createdObject"></paramref> to the 
		/// creating instance of the listener object, as defined by the <see cref="InstrumentationListenerAttribute"></see>
		/// on the source class.
		/// </overloads>
		/// <summary>
		/// Attaches the instrumentation events in the <paramref name="createdObject"></paramref> to the 
		/// creating instance of the listener object, as defined by the <see cref="InstrumentationListenerAttribute"></see>
		/// on the source class.
		/// </summary>
		/// <param name="createdObject">Source object used for instrumentation events.</param>
		/// <param name="configurationSource"><see cref="IConfigurationSource"></see> instance used to define whether
		/// instrumentation is enabled or disabled for application.</param>
		/// <param name="reflectionCache">Cache for instrumentation attributes discovered through reflection.</param>
		public void AttachInstrumentation(object createdObject, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			ArgumentGenerator arguments = new ArgumentGenerator();
			AttachInstrumentation(arguments, createdObject, configurationSource, reflectionCache);
		}

		/// <summary>
		/// Attaches the instrumentation events in the <paramref name="createdObject"></paramref> to the 
		/// creating instance of the listener object, as defined by the <see cref="InstrumentationListenerAttribute"></see>
		/// on the source class.
		/// </summary>
		/// <param name="instanceName">User-provided instance name given to the instrumenation listener during its instantiation.</param>
		/// <param name="createdObject">Source object used for instrumentation events.</param>
		/// <param name="configurationSource"><see cref="IConfigurationSource"></see> instance used to define whether
		/// instrumentation is enabled or disabled for application.</param>
		/// <param name="reflectionCache">Cache for instrumentation attributes discovered through reflection.</param>
		public void AttachInstrumentation(string instanceName, object createdObject, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			ArgumentGenerator arguments = new ArgumentGenerator(instanceName);
			AttachInstrumentation(arguments, createdObject, configurationSource, reflectionCache);
		}

		private void AttachInstrumentation(ArgumentGenerator arguments, object createdObject,
										   IConfigurationSource configurationSource,
										   ConfigurationReflectionCache reflectionCache)
		{
			InstrumentationConfigurationSection section = GetConfigurationSection(configurationSource);
			if (section.InstrumentationIsEntirelyDisabled) return;

			if (createdObject is IInstrumentationEventProvider)
			{
				createdObject = ((IInstrumentationEventProvider)createdObject).GetInstrumentationEventProvider();
			}

			object[] constructorArgs = arguments.ToArguments(section);

			BindInstrumentationTo(createdObject, constructorArgs, reflectionCache);
		}

		private void BindInstrumentationTo(object createdObject, object[] constructorArgs, ConfigurationReflectionCache reflectionCache)
		{
			IInstrumentationAttacher attacher = attacherFactory.CreateBinder(createdObject, constructorArgs, reflectionCache);
			attacher.BindInstrumentation();
		}

		private InstrumentationConfigurationSection GetConfigurationSection(IConfigurationSource configurationSource)
		{
			InstrumentationConfigurationSection section =
				(InstrumentationConfigurationSection)configurationSource.GetSection(InstrumentationConfigurationSection.SectionName);
			if (section == null) section = new InstrumentationConfigurationSection(false, false, false);
			
			return section;
		}

		private class ArgumentGenerator
		{
			private string instanceName;

			public ArgumentGenerator(string instanceName)
			{
				this.instanceName = instanceName;
			}

			public ArgumentGenerator() { }

			public object[] ToArguments(InstrumentationConfigurationSection configSection)
			{
				return instanceName == null
						?
					   new object[] { configSection.PerformanceCountersEnabled, configSection.EventLoggingEnabled, configSection.WmiEnabled, configSection.ApplicationInstanceName }
						:
					   new object[]
				       	{
				       		instanceName, configSection.PerformanceCountersEnabled, configSection.EventLoggingEnabled,
				       		configSection.WmiEnabled, configSection.ApplicationInstanceName
				       	};
			}
		}
	}
}
