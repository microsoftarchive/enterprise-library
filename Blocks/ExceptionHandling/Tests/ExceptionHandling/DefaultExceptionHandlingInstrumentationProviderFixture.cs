//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Tests;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
	[TestClass]
	public class DefaultExceptionHandlingInstrumentationProviderFixture
	{
		private ExceptionManagerImpl manager;

		[TestInitialize]
		public void SetUp()
		{
			manager = new ExceptionManagerImpl(new Dictionary<string, ExceptionPolicyImpl>());

			ReflectionInstrumentationAttacher attacher
				= new ReflectionInstrumentationAttacher(
					((IInstrumentationEventProvider)manager).GetInstrumentationEventProvider(),
					typeof(DefaultExceptionHandlingEventLogger),
					new object[] { true, true, true, "ApplicationInstanceName" });
			attacher.BindInstrumentation();
		}

		[TestMethod]
		public void GetsHookedUpAndRaisesEvents()
		{
			using (WmiEventWatcher eventListener = new WmiEventWatcher(1))
			{
				try
				{
					manager.HandleException(new Exception(), "non-existing policy");
				}
				catch (ExceptionHandlingException)
				{
					eventListener.WaitForEvents();
					Assert.AreEqual(1, eventListener.EventsReceived.Count);
					Assert.AreEqual(typeof(ExceptionHandlingFailureEvent).Name,
						eventListener.EventsReceived[0].ClassPath.ClassName);
					Assert.AreEqual("non-existing policy", eventListener.EventsReceived[0].GetPropertyValue("InstanceName"));
				}
			}
		}
	}
}
