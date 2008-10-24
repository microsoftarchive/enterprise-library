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

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Manageability;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Configuration.Design.Tests
{
    [TestClass]
    public class ManageableConfigurationSourceElementBuilderFixture
    {
        [TestMethod]
        public void CanCreateDataCollectionForEmptyMappings()
        {
            ConfigurationSectionManageabilityProviderAttribute[] sectionProviders
                = new ConfigurationSectionManageabilityProviderAttribute[0];
            ConfigurationElementManageabilityProviderAttribute[] elementProviders
                = new ConfigurationElementManageabilityProviderAttribute[0];

            List<ConfigurationSectionManageabilityProviderData> providersDataCollection
                = ConvertToList(ManageableConfigurationSourceElementBuilder.BuildSectionManageabilityProvidersData(sectionProviders, elementProviders));

            Assert.IsNotNull(providersDataCollection);
            Assert.AreEqual(0, providersDataCollection.Count);
        }

        [TestMethod]
        public void CanCreateDataCollectionForSingleSectionMapping()
        {
            ConfigurationSectionManageabilityProviderAttribute[] sectionProviders
                = new ConfigurationSectionManageabilityProviderAttribute[1];
            sectionProviders[0] = new ConfigurationSectionManageabilityProviderAttribute("section1", typeof(MockConfigurationSectionManageabilityProvider));
            ConfigurationElementManageabilityProviderAttribute[] elementProviders
                = new ConfigurationElementManageabilityProviderAttribute[0];

            List<ConfigurationSectionManageabilityProviderData> providersDataCollection
                = ConvertToList(ManageableConfigurationSourceElementBuilder.BuildSectionManageabilityProvidersData(sectionProviders, elementProviders));

            Assert.IsNotNull(providersDataCollection);
            Assert.AreEqual(1, providersDataCollection.Count);
            ConfigurationSectionManageabilityProviderData data1 = GetProviderData(providersDataCollection, "section1");
            Assert.IsNotNull(data1);
            Assert.AreSame(typeof(MockConfigurationSectionManageabilityProvider), data1.Type);
            Assert.AreEqual(0, data1.ManageabilityProviders.Count);
        }

        [TestMethod]
        public void CanCreateDataCollectionForMultipleSectionMapping()
        {
            ConfigurationSectionManageabilityProviderAttribute[] sectionProviders
                = new ConfigurationSectionManageabilityProviderAttribute[2];
            sectionProviders[0] = new ConfigurationSectionManageabilityProviderAttribute("section1", typeof(MockConfigurationSectionManageabilityProvider));
            sectionProviders[1] = new ConfigurationSectionManageabilityProviderAttribute("section2", typeof(MockConfigurationSectionManageabilityProviderAlt));
            ConfigurationElementManageabilityProviderAttribute[] elementProviders
                = new ConfigurationElementManageabilityProviderAttribute[0];

            List<ConfigurationSectionManageabilityProviderData> providersDataCollection
                = ConvertToList(ManageableConfigurationSourceElementBuilder.BuildSectionManageabilityProvidersData(sectionProviders, elementProviders));

            Assert.IsNotNull(providersDataCollection);
            Assert.AreEqual(2, providersDataCollection.Count);
            ConfigurationSectionManageabilityProviderData data1 = GetProviderData(providersDataCollection, "section1");
            Assert.IsNotNull(data1);
            Assert.AreSame(typeof(MockConfigurationSectionManageabilityProvider), data1.Type);
            Assert.AreEqual(0, data1.ManageabilityProviders.Count);
            ConfigurationSectionManageabilityProviderData data2 = GetProviderData(providersDataCollection, "section1");
            Assert.IsNotNull(data2);
            Assert.AreSame(typeof(MockConfigurationSectionManageabilityProvider), data2.Type);
            Assert.AreEqual(0, data2.ManageabilityProviders.Count);
        }

        [TestMethod]
        public void CanCreateDataCollectionWithElementProviders()
        {
            ConfigurationSectionManageabilityProviderAttribute[] sectionProviders
                = new ConfigurationSectionManageabilityProviderAttribute[1];
            sectionProviders[0] = new ConfigurationSectionManageabilityProviderAttribute("section1", typeof(MockConfigurationSectionManageabilityProvider));
            ConfigurationElementManageabilityProviderAttribute[] elementProviders
                = new ConfigurationElementManageabilityProviderAttribute[1];
            elementProviders[0] = new ConfigurationElementManageabilityProviderAttribute(typeof(MockConfigurationElementManageabilityProvider), typeof(String), typeof(MockConfigurationSectionManageabilityProvider));

            List<ConfigurationSectionManageabilityProviderData> providersDataCollection
                = ConvertToList(ManageableConfigurationSourceElementBuilder.BuildSectionManageabilityProvidersData(sectionProviders, elementProviders));

            Assert.IsNotNull(providersDataCollection);
            Assert.AreEqual(1, providersDataCollection.Count);
            ConfigurationSectionManageabilityProviderData data1 = GetProviderData(providersDataCollection, "section1");
            Assert.IsNotNull(data1);
            Assert.AreSame(typeof(MockConfigurationSectionManageabilityProvider), data1.Type);
            Assert.AreEqual(1, data1.ManageabilityProviders.Count);
            ConfigurationElementManageabilityProviderData elementData11
                = GetProviderData<ConfigurationElementManageabilityProviderData>(data1.ManageabilityProviders, "String");
            Assert.IsNotNull(elementData11);
            Assert.AreSame(typeof(MockConfigurationElementManageabilityProvider), elementData11.Type);
            Assert.AreSame(typeof(String), elementData11.TargetType);
        }

        [TestMethod]
        public void CanCreateElementFromNode()
        {
            ManageableConfigurationSourceElement originalElement = new ManageableConfigurationSourceElement("name", "test.config", "app", true, false);
            ManageableConfigurationSourceElementNode node = new ManageableConfigurationSourceElementNode(originalElement);

            String[] assemblyNames = new String[] { Assembly.GetExecutingAssembly().GetName().Name + ".dll" };
            ManageableConfigurationSourceElementBuilder builder = new ManageableConfigurationSourceElementBuilder(node, new ConfigurationManageabilityProviderAttributeRetriever(assemblyNames));

            ManageableConfigurationSourceElement createdElement = builder.Build();

            Assert.AreEqual(originalElement.Name, createdElement.Name);
            Assert.AreEqual(originalElement.FilePath, createdElement.FilePath);
            Assert.AreEqual(originalElement.ApplicationName, createdElement.ApplicationName);
            Assert.AreEqual(originalElement.EnableGroupPolicies, createdElement.EnableGroupPolicies);
            Assert.AreEqual(originalElement.EnableWmi, createdElement.EnableWmi);
        }

        [TestMethod]
        public void CanCreateElementFromNewNode()
        {
            ManageableConfigurationSourceElementNode node = new ManageableConfigurationSourceElementNode();
            node.Name = "name";
            node.File = "test.config";
            node.ApplicationName = "app";
            node.EnableGroupPolicies = true;
            node.EnableWmi = false;

            String[] assemblyNames = new String[] { typeof(MockConfigurationSectionManageabilityProvider).Assembly.GetName().Name + ".dll" };
            ManageableConfigurationSourceElementBuilder builder
                = new ManageableConfigurationSourceElementBuilder(node, new ConfigurationManageabilityProviderAttributeRetriever(assemblyNames));

            ManageableConfigurationSourceElement createdElement = builder.Build();

            Assert.AreEqual(node.Name, createdElement.Name);
            Assert.AreEqual(node.File, createdElement.FilePath);
            Assert.AreEqual(node.ApplicationName, createdElement.ApplicationName);
            Assert.AreEqual(node.EnableGroupPolicies, createdElement.EnableGroupPolicies);
            Assert.AreEqual(node.EnableWmi, createdElement.EnableWmi);
            Assert.AreEqual(1, createdElement.ConfigurationManageabilityProviders.Count);
        }

        List<T> ConvertToList<T>(IEnumerable<T> providersData)
        {
            List<T> result = new List<T>();

            foreach (T data in providersData)
            {
                result.Add(data);
            }

            return result;
        }

        T GetProviderData<T>(IEnumerable<T> providersDataCollection,
                             String name)
            where T : NamedConfigurationElement
        {
            foreach (T data in providersDataCollection)
            {
                if (name.Equals(data.Name, StringComparison.Ordinal))
                {
                    return data;
                }
            }

            return null;
        }
    }
}
