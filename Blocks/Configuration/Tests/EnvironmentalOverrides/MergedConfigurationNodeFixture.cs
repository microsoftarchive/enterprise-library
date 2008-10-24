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

using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Tests
{
    [TestClass]
    public class MergedConfigurationNodeFixture
    {
        [TestMethod]
        public void MergedConfigurationNodeHidesReadonlyProperties()
        {
            TestConfigurationNode testnode = new TestConfigurationNode();
            MergedConfigurationNode mergedNode = new MergedConfigurationNode(testnode, new ConfigurationNodeMergeData());

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(mergedNode);

            Assert.IsNull(properties["ReadonlyProperty"]);
        }

        [TestMethod]
        public void MergedConfigurationNodeHidesNonBrowsableProperties()
        {
            TestConfigurationNode testnode = new TestConfigurationNode();
            MergedConfigurationNode mergedNode = new MergedConfigurationNode(testnode, new ConfigurationNodeMergeData());

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(mergedNode);

            Assert.IsNull(properties["NonBrosableProperty"]);
        }

        [TestMethod]
        public void MergedConfigurationNodeHidesPropertiesMarkedAsNonOverridable()
        {
            TestConfigurationNode testnode = new TestConfigurationNode();
            MergedConfigurationNode mergedNode = new MergedConfigurationNode(testnode, new ConfigurationNodeMergeData());

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(mergedNode);

            Assert.IsNull(properties["OverridesHiddenProperty"]);
        }

        [TestMethod]
        public void MergedConfigurationNodeHidesNameProperty()
        {
            TestConfigurationNode testnode = new TestConfigurationNode();
            MergedConfigurationNode mergedNode = new MergedConfigurationNode(testnode, new ConfigurationNodeMergeData());

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(mergedNode);

            Assert.IsNull(properties["Name"]);
        }

        [TestMethod]
        public void MergedConfigurationNodeReturnsRegularProperties()
        {
            TestConfigurationNode testnode = new TestConfigurationNode();
            MergedConfigurationNode mergedNode = new MergedConfigurationNode(testnode, new ConfigurationNodeMergeData());

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(mergedNode);

            Assert.IsNotNull(properties["Property1"]);
        }

        [TestMethod]
        public void NonSerializableTypesAreHidden()
        {
            TestConfigurationNode testnode = new TestConfigurationNode();
            MergedConfigurationNode mergedNode = new MergedConfigurationNode(testnode, new ConfigurationNodeMergeData());

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(mergedNode);

            Assert.IsNull(properties["NonSerializableProperty"]);
        }

        [TestMethod]
        public void ReturnedPropertiesAreMergedConfigurationProperty()
        {
            TestConfigurationNode testnode = new TestConfigurationNode();
            MergedConfigurationNode mergedNode = new MergedConfigurationNode(testnode, new ConfigurationNodeMergeData());

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(mergedNode);

            Assert.IsNotNull(properties["Property1"]);
            Assert.AreEqual(typeof(MergedConfigurationProperty), properties["Property1"].GetType());
        }

        [TestMethod]
        public void MergedConfigurationNodeConvertsToDisplayText()
        {
            TestConfigurationNode testnode = new TestConfigurationNode();
            MergedConfigurationNode mergedNode = new MergedConfigurationNode(testnode, new ConfigurationNodeMergeData());

            TypeConverter converter = TypeDescriptor.GetConverter(mergedNode);
            string displayText = converter.ConvertToInvariantString(mergedNode);

            Assert.AreEqual("Don't Override Properties", displayText);
        }

        [TestMethod]
        public void MergedConfigurationNodeConvertsToDisplayTextWhenOverridden()
        {
            ConfigurationNodeMergeData mergeData = new ConfigurationNodeMergeData(true, new ConfigurationNodeMergeData());
            TestConfigurationNode testnode = new TestConfigurationNode();

            MergedConfigurationNode mergedNode = new MergedConfigurationNode(testnode, mergeData);

            TypeConverter converter = TypeDescriptor.GetConverter(mergedNode);
            string displayText = converter.ConvertToInvariantString(mergedNode);

            Assert.AreEqual("Override Properties", displayText);
        }

        class TestConfigurationNode : ConfigurationNode
        {
            [Browsable(false)]
            public string NonBrosableProperty
            {
                get { return string.Empty; }
                set { }
            }

            public NonSerialableType NonSerializableProperty
            {
                get { return new NonSerialableType(); }
                set { }
            }

            [EnvironmentOverridableAttribute(false)]
            public string OverridesHiddenProperty
            {
                get { return string.Empty; }
                set { }
            }

            public string Property1
            {
                get { return string.Empty; }
                set { }
            }

            [ReadOnly(true)]
            public string ReadonlyProperty
            {
                get { return string.Empty; }
                set { }
            }

            public class NonSerialableType {}
        }
    }
}
