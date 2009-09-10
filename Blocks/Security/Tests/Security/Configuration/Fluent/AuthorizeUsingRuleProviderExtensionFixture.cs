//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests.Configuration.Fluent
{
    public abstract class Given_AuthorizationRuleProviderInConfigurationSourceBuilder : ArrangeActAssert
    {
        IConfigurationSourceBuilder configurationSourceBuilder;
        protected string ruleProviderName = "rule provider";
        protected IAuthorizeUsingRuleProvider ConfigureRuleProvider;

        protected override void Arrange()
        {
            configurationSourceBuilder = new ConfigurationSourceBuilder();
            ConfigureRuleProvider = configurationSourceBuilder.ConfigureSecurity()
                    .AuthorizeUsingRuleProviderNamed(ruleProviderName);
        }


        protected SecuritySettings GetSecuritySettings()
        {
            IConfigurationSource source = new DictionaryConfigurationSource();
            configurationSourceBuilder.UpdateConfigurationWithReplace(source);

            return (SecuritySettings) source.GetSection(SecuritySettings.SectionName);
        }

        protected AuthorizationRuleProviderData GetRuleProviderData()
        {
            return GetSecuritySettings().AuthorizationProviders.OfType<AuthorizationRuleProviderData>()
                        .FirstOrDefault();
        }
    }

    [TestClass]
    public class When_ConfiguringAuthorizationRuleProviderPassingNullForName : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_AuthorizeUsingRuleProviderNamed_ThrowsArgumentException()
        {
            ConfigureSecuritySettings.AuthorizeUsingRuleProviderNamed(null);
        }
    }


    [TestClass]
    public class When_ConfiguringAuthorizationRuleProvider : Given_AuthorizationRuleProviderInConfigurationSourceBuilder
    {
        [TestMethod]
        public void Then_AuthorizationRuleProviderDataIsAddedToSecurityConfiguration()
        {
            Assert.IsNotNull(GetRuleProviderData());
        }

        [TestMethod]
        public void Then_AuthorizationRuleProviderDatahasAppropriateName()
        {
            Assert.AreEqual(ruleProviderName, GetRuleProviderData().Name);
        }

        [TestMethod]
        public void Then_AuthorizationRuleProviderDatahasCorrectType()
        {
            Assert.AreEqual(typeof(AuthorizationRuleProvider), GetRuleProviderData().Type);
        }
    }


    [TestClass]
    public class When_SpecifyingAuthorizationRuleProviderAsDefault : Given_AuthorizationRuleProviderInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            ConfigureRuleProvider.SetAsDefault();
        }

        [TestMethod]
        public void Then_SecurityConfigurationHasDefaultAuthZProvider()
        {
            Assert.AreEqual(ruleProviderName, GetSecuritySettings().DefaultAuthorizationProviderName);
        }
    }

    [TestClass]
    public class When_AddingRulesToAuthorizationRuleProvider : Given_AuthorizationRuleProviderInConfigurationSourceBuilder
    {
        private string rule1 = "rule1";
        private string rule1Expression = "NOT I:?";

        private string rule2 = "rule2";
        private string rule2Expression = "R:admin OR I:user43";


        protected override void Act()
        {
            ConfigureRuleProvider.SpecifyRule(rule1, rule1Expression)
                                 .SpecifyRule(rule2, rule2Expression);
        }

        [TestMethod]
        public void Then_RulesAreContainedInConfiguration()
        {
            var authZRuleProvider = GetRuleProviderData();

            Assert.IsNotNull(authZRuleProvider.Rules.Get(rule1));
            Assert.AreEqual(rule1Expression, authZRuleProvider.Rules.Get(rule1).Expression);

            Assert.IsNotNull(authZRuleProvider.Rules.Get(rule2));
            Assert.AreEqual(rule2Expression, authZRuleProvider.Rules.Get(rule2).Expression);
        }
    }


    [TestClass]
    public class When_SpecifyingRulePassingNullName : Given_AuthorizationRuleProviderInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_SpecifyRule_ThrowsArgumentException()
        {
            ConfigureRuleProvider.SpecifyRule(null, "I:?");
        }
    }

    [TestClass]
    public class When_AddingTwoAuthorizationRuleProviders : Given_AuthorizationRuleProviderInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            ConfigureRuleProvider.AuthorizeUsingRuleProviderNamed("another rule provider");
        }

        [TestMethod]
        public void Then_ConfigurationContainsTwoAuthZRuleProviders()
        {
            var securityConfiguration = GetSecuritySettings();
            Assert.AreEqual(2, securityConfiguration.AuthorizationProviders.OfType<AuthorizationRuleProviderData>().Count());
        }
    }
}
