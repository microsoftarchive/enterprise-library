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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Tests
{
    [TestClass]
    public class ConfigurationNodeMergeDataFixture
    {
        [TestMethod]
        public void ConfigurationNodeMergeDataCanGetPropertiesThatHaveBeenSet()
        {
            ConfigurationNodeMergeData mergeData = new ConfigurationNodeMergeData();
            mergeData.SetPropertyValue("propertyName", "propertyValue");
            string propertyValue = mergeData.GetPropertyValue("propertyName", typeof(string), null, null) as string;

            Assert.AreEqual("propertyValue", propertyValue);
        }

        [TestMethod]
        public void GettingValueAfterResetReturnsDefaultValue()
        {
            ConfigurationNodeMergeData mergeData = new ConfigurationNodeMergeData();
            mergeData.SetPropertyValue("propertyName", "propertyValue");
            mergeData.ResetPropertyValue("propertyName");

            string propertyValue = mergeData.GetPropertyValue("propertyName", typeof(string), "default", null) as string;

            Assert.AreEqual("default", propertyValue);
        }

        [TestMethod]
        public void ValueIsDeserializedOnGet()
        {
            ConfigurationNodeMergeData mergeData = new ConfigurationNodeMergeData();
            mergeData.SetPropertyValue("propertyName", new UnserializedPropertyValue("123"));

            int propertyValue = (int)mergeData.GetPropertyValue("propertyName", typeof(int), 0, null);

            Assert.AreEqual(123, propertyValue);
        }
    }
}
