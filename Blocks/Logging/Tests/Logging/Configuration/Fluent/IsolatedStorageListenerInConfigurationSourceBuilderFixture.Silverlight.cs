//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    public abstract class Given_IsolatedListenerInConfigurationSourceBuilder : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        protected ILoggingConfigurationSendToIsolatedStorageTraceListener IsolatedListenerBuilder;
        private string traceListenerName = "isolated listener";

        protected override void Arrange()
        {
            base.Arrange();

            IsolatedListenerBuilder = base.CategorySourceBuilder.SendTo.IsolatedStorage(traceListenerName);
        }

        protected IsolatedStorageTraceListenerData GetIsolatedStorageTraceListenerData()
        {
            return base.GetLoggingConfiguration().TraceListeners.OfType<IsolatedStorageTraceListenerData>()
                .Where(x => x.Name == traceListenerName).First();
        }
    }

    [TestClass]
    public class When_CallingSendToIsolatedListenerOnLogToCategoryConfigurationBuilder : Given_IsolatedListenerInConfigurationSourceBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();
        }

        [TestMethod]
        public void ThenMaxSizeIsDefault()
        {
            Assert.AreEqual(256, GetIsolatedStorageTraceListenerData().MaxSizeInKilobytes);
        }
    }

    [TestClass]
    public class When_CallingSendToIsolatedListenerPassingNullForName : Given_LoggingCategorySourceInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_SendToIsolatedStorage_TrowsArgumentException()
        {
            base.CategorySourceBuilder.SendTo.IsolatedStorage(null);
        }
    }

    [TestClass]
    public class When_CallingWithRepositoryNameOnIsolatedStorageTraceListener : Given_IsolatedListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.IsolatedListenerBuilder.WithRepositoryName("test");
        }

        [TestMethod]
        public void ThenUseAuthenticationIsTrue()
        {
            Assert.AreEqual("test", base.GetIsolatedStorageTraceListenerData().RepositoryName);
        }
    }

    [TestClass]
    public class When_CallingSetMaxSizeInKilobytesOnIsolatedStorageTraceListener : Given_IsolatedListenerInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.IsolatedListenerBuilder.SetMaxSizeInKilobytes(123);
        }

        [TestMethod]
        public void ThenUseAuthenticationIsTrue()
        {
            Assert.AreEqual(123, base.GetIsolatedStorageTraceListenerData().MaxSizeInKilobytes);
        }
    }
}
