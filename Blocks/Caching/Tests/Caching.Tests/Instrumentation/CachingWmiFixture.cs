//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation.Tests
{
    [TestClass]
    public class CachingWmiFixture
    {
        const string instanceName = "test";
        const string exceptionMessage = "exception message";
        const string errorMessage = "error message";
        const string key = "key";

        NoPrefixNameFormatter formatter;

        [TestInitialize]
        public void SetUp()
        {
            formatter = new NoPrefixNameFormatter();
        }

        [TestMethod]
        public void CacheScavengingWithInstrumentationDisabledDoesNotFireWmiEvent()
        {
            CachingInstrumentationListener listener
                = new CachingInstrumentationListener(instanceName, false, false, false, formatter);

            using (WmiEventWatcher eventListener = new WmiEventWatcher(1))
            {
                CacheScavengedEventArgs args = new CacheScavengedEventArgs(10);

                listener.CacheScavenged(null, args);

                eventListener.WaitForEvents();
                Assert.AreEqual(0, eventListener.EventsReceived.Count);
            }
        }

        [TestMethod]
        public void CacheScavengingWithInstrumentationEnabledDoesFireWmiEvent()
        {
            CachingInstrumentationListener listener
                = new CachingInstrumentationListener(instanceName, false, false, true, formatter);

            using (WmiEventWatcher eventListener = new WmiEventWatcher(1))
            {
                CacheScavengedEventArgs args = new CacheScavengedEventArgs(10);

                listener.CacheScavenged(null, args);

                eventListener.WaitForEvents();
                Assert.AreEqual(1, eventListener.EventsReceived.Count);
                Assert.AreEqual("CacheScavengedEvent", eventListener.EventsReceived[0].ClassPath.ClassName);
                Assert.AreEqual(instanceName, eventListener.EventsReceived[0].GetPropertyValue("InstanceName"));
                Assert.AreEqual(10L, eventListener.EventsReceived[0].GetPropertyValue("ItemsScavenged"));
            }
        }

        [TestMethod]
        public void CacheFailureWithInstrumentationDisabledDoesNotFireWmiEvent()
        {
            CachingInstrumentationListener listener
                = new CachingInstrumentationListener(instanceName, false, false, false, formatter);
            Exception exception = new Exception(exceptionMessage);

            using (WmiEventWatcher eventListener = new WmiEventWatcher(1))
            {
                CacheFailureEventArgs args = new CacheFailureEventArgs(errorMessage, exception);

                listener.CacheFailed(null, args);

                eventListener.WaitForEvents();
                Assert.AreEqual(0, eventListener.EventsReceived.Count);
            }
        }

        [TestMethod]
        public void CacheFailureWithInstrumentationEnabledDoesFireWmiEvent()
        {
            CachingInstrumentationListener listener
                = new CachingInstrumentationListener(instanceName, false, false, true, formatter);
            Exception exception = new Exception(exceptionMessage);

            using (WmiEventWatcher eventListener = new WmiEventWatcher(1))
            {
                CacheFailureEventArgs args = new CacheFailureEventArgs(errorMessage, exception);

                listener.CacheFailed(null, args);

                eventListener.WaitForEvents();
                Assert.AreEqual(1, eventListener.EventsReceived.Count);
                Assert.AreEqual("CacheFailureEvent", eventListener.EventsReceived[0].ClassPath.ClassName);
                Assert.AreEqual(errorMessage, eventListener.EventsReceived[0].GetPropertyValue("ErrorMessage"));
                Assert.IsTrue(eventListener.EventsReceived[0].GetPropertyValue("ExceptionMessage").ToString().IndexOf(exceptionMessage) > -1);
            }
        }

        [TestMethod]
        public void CacheCallbackFailureWithInstrumentationDisabledDoesNotFireWmiEvent()
        {
            CachingInstrumentationListener listener
                = new CachingInstrumentationListener(instanceName, false, false, false, formatter);
            Exception exception = new Exception(exceptionMessage);

            using (WmiEventWatcher eventListener = new WmiEventWatcher(1))
            {
                CacheCallbackFailureEventArgs args = new CacheCallbackFailureEventArgs(key, exception);

                listener.CacheCallbackFailed(null, args);

                eventListener.WaitForEvents();
                Assert.AreEqual(0, eventListener.EventsReceived.Count);
            }
        }

        [TestMethod]
        public void CacheCallbackFailureWithInstrumentationEnabledDoesFireWmiEvent()
        {
            CachingInstrumentationListener listener
                = new CachingInstrumentationListener(instanceName, false, false, true, formatter);
            Exception exception = new Exception(exceptionMessage);

            using (WmiEventWatcher eventListener = new WmiEventWatcher(1))
            {
                CacheCallbackFailureEventArgs args = new CacheCallbackFailureEventArgs(key, exception);

                listener.CacheCallbackFailed(null, args);

                eventListener.WaitForEvents();
                Assert.AreEqual(1, eventListener.EventsReceived.Count);
                Assert.AreEqual("CacheCallbackFailureEvent", eventListener.EventsReceived[0].ClassPath.ClassName);
                Assert.AreEqual(key, eventListener.EventsReceived[0].GetPropertyValue("Key"));
                Assert.IsTrue(eventListener.EventsReceived[0].GetPropertyValue("ExceptionMessage").ToString().IndexOf(exceptionMessage) > -1);
            }
        }
    }
}
