#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Semantic Logging Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Etw.Configuration;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Tests.Etw
{
    [TestClass]
    public class given_eventSourceSettings
    {
        [TestMethod]
        [ExpectedException(typeof(ConfigurationException))]
        public void when_creating_instance_with_no_values()
        {
            new EventSourceSettings();
        }

        [TestMethod]
        public void when_creating_instance_with_name_only()
        {
            var sut = new EventSourceSettings(MyCompanyEventSource.Log.Name);

            Assert.AreEqual(MyCompanyEventSource.Log.Name, sut.Name);
            Assert.AreEqual(MyCompanyEventSource.Log.Guid, sut.EventSourceId);
            Assert.AreEqual(EventLevel.LogAlways, sut.Level);
            Assert.AreEqual(Keywords.All, sut.MatchAnyKeyword);
        }

        [TestMethod]
        public void when_creating_instance_with_id_only()
        {
            var sut = new EventSourceSettings(eventSourceId: MyCompanyEventSource.Log.Guid);

            Assert.AreEqual(MyCompanyEventSource.Log.Guid.ToString(), sut.Name);
            Assert.AreEqual(MyCompanyEventSource.Log.Guid, sut.EventSourceId);
            Assert.AreEqual(EventLevel.LogAlways, sut.Level);
            Assert.AreEqual(Keywords.All, sut.MatchAnyKeyword);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationException))]
        public void when_creating_instance_with_both_name_and_id()
        {
            new EventSourceSettings(MyCompanyEventSource.Log.Name, MyCompanyEventSource.Log.Guid);
        }
    }
}
