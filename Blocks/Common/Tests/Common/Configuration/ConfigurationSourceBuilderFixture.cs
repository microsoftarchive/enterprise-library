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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
    public abstract class Given_EmptyConfigurationSourceBuilder : ArrangeActAssert
    {
        protected IConfigurationSourceBuilder ConfigurationSourceBuilder;

        protected override void Arrange()
        {
            ConfigurationSourceBuilder = new ConfigurationSourceBuilder();
        }

        protected IConfigurationSource GetConfigurationSource()
        {
            var configSource = new DictionaryConfigurationSource();
            ConfigurationSourceBuilder.UpdateConfigurationWithReplace(configSource);
            return configSource;
        }
    }

    [TestClass]
    public class When_AccessingConfigurationSourceBuilderMembers : ArrangeActAssert
    {
        [TestMethod]
        public void Then_ObjectMembersAreHiddenFromIntellisense()
        {
            Assert.IsTrue(typeof(IFluentInterface).IsAssignableFrom(typeof(IConfigurationSourceBuilder)));
        }
    }

    [TestClass]
    public class When_AddingSectionsToSourceBuilder : Given_EmptyConfigurationSourceBuilder
    {
        private MockConfigurationSection section;
        private const string sectionName = "MockSection";

        protected override void Act()
        {
            section = new MockConfigurationSection();
            base.ConfigurationSourceBuilder.AddSection(sectionName, section);
        }

        [TestMethod]
        public void Then_CanRetrieveAddedSection()
        {
            Assert.AreSame(section, ConfigurationSourceBuilder.Get(sectionName));
        }

        [TestMethod]
        public void Then_CanRetrieveAddedSectionViaGeneric()
        {
            Assert.AreSame(section, ConfigurationSourceBuilder.Get<MockConfigurationSection>(sectionName));
        }

        [TestMethod]
        public void Then_ReturnsNullIfCannotFind()
        {
            Assert.IsNull(ConfigurationSourceBuilder.Get("unknown section name"));
        }

        [TestMethod]
        public void Then_ReturnsNullIfCannotFindViaGeneric()
        {
            Assert.IsNull(ConfigurationSourceBuilder.Get<MockConfigurationSection>("unknown section name"));
        }
    }
}
