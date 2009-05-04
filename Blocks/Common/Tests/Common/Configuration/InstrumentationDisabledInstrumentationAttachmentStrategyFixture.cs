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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
	[TestClass]
	public class InstrumentationDisabledInstrumentationAttachmentStrategyFixture
	{
		[TestInitialize]
		public void ClearStaticState()
		{
			ShouldNeverBeInstantiatedListener.WasInstantiated = false;
		}

		[TestMethod]
		public void InstrumentationIsNotAttachedIfAllConfigOptionsAreFalse()
		{
			DictionaryConfigurationSource dictionary = new DictionaryConfigurationSource();
			dictionary.Add(InstrumentationConfigurationSection.SectionName,
						   new InstrumentationConfigurationSection(false, false, false));

			InstrumentationAttachmentStrategy attacher = new InstrumentationAttachmentStrategy();
			ConfigurationReflectionCache reflectionCache = new ConfigurationReflectionCache();

			SourceObject sourceObject = new SourceObject();
			attacher.AttachInstrumentation(sourceObject, dictionary, reflectionCache);

			Assert.IsFalse(ShouldNeverBeInstantiatedListener.WasInstantiated);
		}

		[TestMethod]
		public void InstrumentationIsNotAttachedIfConfigurationIsMissing()
		{
			DictionaryConfigurationSource dictionary = new DictionaryConfigurationSource();
			InstrumentationAttachmentStrategy attacher = new InstrumentationAttachmentStrategy();
			ConfigurationReflectionCache reflectionCache = new ConfigurationReflectionCache();

			SourceObject sourceObject = new SourceObject();
			attacher.AttachInstrumentation(sourceObject, dictionary, reflectionCache);

			Assert.IsFalse(ShouldNeverBeInstantiatedListener.WasInstantiated);
		}

		[InstrumentationListener(typeof(ShouldNeverBeInstantiatedListener))]
		public class SourceObject : IInstrumentationEventProvider
		{
			object IInstrumentationEventProvider.GetInstrumentationEventProvider()
			{
				return this;
			}
		}

		public class ShouldNeverBeInstantiatedListener
		{
			public static bool WasInstantiated = false;
		}
	}
}
