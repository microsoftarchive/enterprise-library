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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;

#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Instrumentation
{
	[TestClass]
	public class DefaultEventLoggerFixture
	{
		[TestMethod]
		public void CanCreateThroughObjectBuilder()
		{
			DictionaryConfigurationSource configSource = new DictionaryConfigurationSource();
			InstrumentationConfigurationSection configSection = new InstrumentationConfigurationSection();
			configSection.EventLoggingEnabled = true;
			configSource.Add(InstrumentationConfigurationSection.SectionName, configSection);
			
			DefaultEventLogger logger = EnterpriseLibraryFactory.BuildUp<DefaultEventLogger>(configSource);
			
			Assert.IsTrue(logger.EventLoggingEnabled);
		}
		
		[TestMethod]
		public void CanStillCreateLoggerWithoutConfigurationSectionPresent()
		{
			DictionaryConfigurationSource configSource = new DictionaryConfigurationSource();

			DefaultEventLogger logger = EnterpriseLibraryFactory.BuildUp<DefaultEventLogger>(configSource);

			Assert.IsFalse(logger.EventLoggingEnabled);
		}
	}
}
