//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
    [TestClass]
    public class CustomConfigurationElementCollectionFixture : ArrangeActAssert
    {
        string configurationFilePath;

        protected override void Arrange()
        {
            var resourceHelper = new ResourceHelper<CustomConfigurationElementCollectionFixture>();
            configurationFilePath = resourceHelper.DumpResourceFileToDisk("ConfigSourceWithInvalidType.config");
        }


        [TestMethod]
        public void ThenThrowsExceptionWhenTypeIsInvalid()
        {
            try
            {
                IConfigurationSource config = new FileConfigurationSource(configurationFilePath);
                var sources = config.GetSection("enterpriseLibrary.ConfigurationSource");
                Assert.Fail("Should have thrown");
            }
            catch (ConfigurationException ex)
            {
                Assert.IsTrue(ex.Message.Contains("The type 'Microsoft.Practices.EnterpriseLibrary.Common.Configuration.InvalidType, Microsoft.Practices.EnterpriseLibrary.Common' defined in the 'InvalidType' configuration source is invalid"));
            }
        }
    }
}
