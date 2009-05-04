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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.Manageability.Mocks;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Manageability;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Manageability.Design.Tests
{
    [TestClass]
    public class ConfigurationManageabilityProviderAttributeRetrieverFixture
    {
        [TestMethod]
        public void CanLoadAttributesFromAssembly()
        {
            List<ConfigurationSectionManageabilityProviderAttribute> sectionManageabilityProviderAttributes
                = new List<ConfigurationSectionManageabilityProviderAttribute>();
            List<ConfigurationElementManageabilityProviderAttribute> elementManageabilityProviderAttributes
                = new List<ConfigurationElementManageabilityProviderAttribute>();

            String[] assemblyNames 
                = new String[] { typeof(MockConfigurationSectionManageabilityProvider).Assembly.GetName().Name + ".dll" };
            ConfigurationManageabilityProviderAttributeRetriever retriever
                = new ConfigurationManageabilityProviderAttributeRetriever(assemblyNames);

            sectionManageabilityProviderAttributes.AddRange(retriever.SectionManageabilityProviderAttributes);
            elementManageabilityProviderAttributes.AddRange(retriever.ElementManageabilityProviderAttributes);

            Assert.AreEqual(1, sectionManageabilityProviderAttributes.Count);
            Assert.AreEqual("section1", sectionManageabilityProviderAttributes[0].SectionName);
            Assert.AreSame(typeof(MockConfigurationSectionManageabilityProvider), sectionManageabilityProviderAttributes[0].ManageabilityProviderType);
            Assert.AreEqual(1, elementManageabilityProviderAttributes.Count);
        }
    }
}
