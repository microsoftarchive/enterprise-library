//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Tests
{
    [TestClass]
    public class DefaultExceptionHandlingInstrumentationProviderFixture
    {
        [TestMethod]
        [Ignore]    // TODO use a different instrumentation mechanism to test
        public void GetsHookedUpAndRaisesEvents()
        {
            //var manager = new ExceptionManagerImpl(
            //    new Dictionary<string, ExceptionPolicyImpl>(),
            //    new DefaultExceptionHandlingEventLogger(true, true, "ApplicationInstanceName"));

            //using (var eventListener = new WmiEventWatcher(1))
            //{
            //    try
            //    {
            //        manager.HandleException(new Exception(), "non-existing policy");
            //    }
            //    catch (ExceptionHandlingException)
            //    {
            //        eventListener.WaitForEvents();
            //        Assert.AreEqual(1, eventListener.EventsReceived.Count);
            //        Assert.AreEqual(typeof(ExceptionHandlingFailureEvent).Name,
            //            eventListener.EventsReceived[0].ClassPath.ClassName);
            //        Assert.AreEqual("non-existing policy", eventListener.EventsReceived[0].GetPropertyValue("InstanceName"));
            //    }
            //}
        }

        [TestMethod]
        [Ignore]    // TODO use a different instrumentation mechanism to test
        public void GetsHookedUpAndRaisesEventsWhenResolvedFromContainer()
        {
            //var exceptionSection = new ExceptionHandlingSettings();
            //var instrumentationSection = new InstrumentationConfigurationSection
            //{
            //    ApplicationInstanceName = "ApplicationInstanceName",
            //    EventLoggingEnabled = true,
            //    PerformanceCountersEnabled = true,
            //};

            //var config = new DictionaryConfigurationSource();
            //config.Add(ExceptionHandlingSettings.SectionName, exceptionSection);
            //config.Add(InstrumentationConfigurationSection.SectionName, instrumentationSection);

            //var manager = EnterpriseLibraryContainer.CreateDefaultContainer(config).GetInstance<ExceptionManager>();

            //using (var eventListener = new WmiEventWatcher(1))
            //{
            //    try
            //    {
            //        manager.HandleException(new Exception(), "non-existing policy");
            //    }
            //    catch (ExceptionHandlingException)
            //    {
            //        eventListener.WaitForEvents();
            //        Assert.AreEqual(1, eventListener.EventsReceived.Count);
            //        Assert.AreEqual(typeof(ExceptionHandlingFailureEvent).Name,
            //            eventListener.EventsReceived[0].ClassPath.ClassName);
            //        Assert.AreEqual("non-existing policy", eventListener.EventsReceived[0].GetPropertyValue("InstanceName"));
            //    }
            //}
        }
    }
}
