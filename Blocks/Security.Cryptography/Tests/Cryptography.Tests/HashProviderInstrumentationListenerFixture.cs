//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests
{
    [TestClass]
    public class HashProviderInstrumentationListenerFixture
    {
        [TestMethod]
        public void DoFailuresWithWmiEnabled()
        {
            int numberOfEvents = 50;
            using (WmiEventWatcher eventListener = new WmiEventWatcher(numberOfEvents))
            {
                HashAlgorithmProvider hashProvider = new HashAlgorithmProvider(typeof(SHA1Managed), false);
                InstrumentationAttacherFactory attacherFactory = new InstrumentationAttacherFactory();
                IInstrumentationAttacher binder = attacherFactory.CreateBinder(hashProvider.GetInstrumentationEventProvider(), new object[] { "foo", true, true, true, "fooInstanceName" }, new ConfigurationReflectionCache());
                binder.BindInstrumentation();

                for (int i = 0; i < numberOfEvents; i++)
                {
                    try
                    {
                        hashProvider.CreateHash(null);
                    }
                    catch (Exception) { }
                }

                eventListener.WaitForEvents();

                Assert.AreEqual(numberOfEvents, eventListener.EventsReceived.Count);
            }
        }
    }
}
