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

using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Instrumentation
{
    [TestClass]
    public class IntegrationFixture
    {
        [TestMethod]
        [Ignore] // TODO try a different instrumentation mechanism
        public void CallingThroughFacadeFiresWmiEvents()
        {
            using (WmiEventWatcher eventWatcher = new WmiEventWatcher(1))
            {
                Validation.Validate(this);

                eventWatcher.WaitForEvents();

                Assert.AreEqual(1, eventWatcher.EventsReceived.Count);
                Assert.AreEqual("ValidationSucceededEvent", eventWatcher.EventsReceived[0].ClassPath.ClassName);
            }
        }
    }
}
