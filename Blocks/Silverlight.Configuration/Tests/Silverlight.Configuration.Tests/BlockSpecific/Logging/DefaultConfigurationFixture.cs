//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Silverlight Design-Time Configuration
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Tests.VSTS.BlockSpecific.Logging
{
    [TestClass]
    public class DefaultConfigurationFixture
    {
        [TestMethod]
        public void IsolatedStorageTraceListenerDataHasProperDefaultValues()
        {
            var data = new IsolatedStorageTraceListenerData();
            Assert.AreEqual(typeof(IsolatedStorageTraceListener), data.Type);
            Assert.AreEqual(256, data.MaxSizeInKilobytes);
            Assert.IsTrue(string.IsNullOrEmpty(data.RepositoryName));
        }

        [TestMethod]
        public void RemoteServiceTraceListenerDataHasProperDefaultValues()
        {
            var data = new RemoteServiceTraceListenerData();
            Assert.AreEqual(typeof(RemoteServiceTraceListener), data.Type);
            Assert.AreEqual(TimeSpan.FromMinutes(1), data.SubmitInterval);
            Assert.AreEqual(0, data.IsolatedStorageBufferMaxSizeInKilobytes);
            Assert.AreEqual(100, data.MaxElementsInBuffer);
            Assert.IsTrue(string.IsNullOrEmpty(data.LoggingServiceFactory));
            Assert.IsFalse(data.SendImmediately);
        }
    }
}
