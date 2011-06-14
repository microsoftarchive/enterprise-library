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

using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Configuration
{
    [TestClass]
    public class GivenEmptyLoggingSettings
    {
        private LoggingSettings settings;

        [TestInitialize]
        public void Setup()
        {
            settings = new LoggingSettings();
        }

        [TestMethod]
        public void ThenHasDefaultValues()
        {
            Assert.IsTrue(settings.LogWarningWhenNoCategoriesMatch);
            Assert.IsTrue(settings.RevertImpersonation);
            Assert.IsTrue(settings.TracingEnabled);
        }
    }
}
