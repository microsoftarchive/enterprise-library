//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Instrumentation
{
    [TestClass]
    public class EventLogFixture
    {
        [TestInitialize]
        public void TestInitialize()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        [TestMethod, DeploymentItem("errornous.config")]
        public void ConfigurationErrorsAreLoggedInEventLog()
        {
            FileConfigurationSource configurationSourceWithInvalidConfiguration = new FileConfigurationSource("errornous.config");

            using (EventLog eventLog = GetEventLog())
            {
                int eventCount = eventLog.Entries.Count;

                try
                {
                    ValidationFactory.CreateValidator<EventLogFixture>(configurationSourceWithInvalidConfiguration);
                    Assert.Fail();
                }
                catch (ConfigurationErrorsException e)
                {
                    Assert.AreEqual(eventCount + 1, eventLog.Entries.Count);
                    Assert.IsTrue(eventLog.Entries[eventCount].Message.IndexOf(e.Message) > -1);
                }
            }
        }

        static EventLog GetEventLog()
        {
            return new EventLog("Application", ".", "Enterprise Library Validation");
        }
    }
}
