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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Security.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Tests
{
    [TestClass]
    public class AuthorizationConfigurationSerializationFixture
    {
        const string authorizationName1 = "authorization1";
        const string authorizationName2 = "authorization2";

        const string ruleName11 = "rule11";
        const string expression11 = "expression 11";
        const string ruleName12 = "rule12";
        const string expression12 = "expression 12";

        [TestInitialize]
        public void TestInitialize()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            SecuritySettings settings = new SecuritySettings();

            AuthorizationRuleProviderData authorizationData1 = new AuthorizationRuleProviderData(authorizationName1);
            authorizationData1.Rules.Add(new AuthorizationRuleData(ruleName11, expression11));
            authorizationData1.Rules.Add(new AuthorizationRuleData(ruleName12, expression12));

            CustomAuthorizationProviderData authorizationData2 = new CustomAuthorizationProviderData(authorizationName2, typeof(MockCustomAuthorizationProvider));

            settings.AuthorizationProviders.Add(authorizationData1);
            settings.AuthorizationProviders.Add(authorizationData2);
            settings.DefaultAuthorizationProviderName = authorizationName1;

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[SecuritySettings.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            SecuritySettings roSettigs = (SecuritySettings)configurationSource.GetSection(SecuritySettings.SectionName);

            Assert.IsNotNull(roSettigs);
            Assert.AreEqual(2, roSettigs.AuthorizationProviders.Count);

            Assert.IsNotNull(roSettigs.AuthorizationProviders.Get(authorizationName1));
            Assert.AreSame(typeof(AuthorizationRuleProviderData), roSettigs.AuthorizationProviders.Get(authorizationName1).GetType());
            Assert.AreEqual(2, ((AuthorizationRuleProviderData)roSettigs.AuthorizationProviders.Get(authorizationName1)).Rules.Count);
            Assert.IsNotNull(((AuthorizationRuleProviderData)roSettigs.AuthorizationProviders.Get(authorizationName1)).Rules.Get(ruleName11));
            Assert.AreEqual(expression11, ((AuthorizationRuleProviderData)roSettigs.AuthorizationProviders.Get(authorizationName1)).Rules.Get(ruleName11).Expression);

            Assert.IsNotNull(roSettigs.AuthorizationProviders.Get(authorizationName2));
            Assert.AreSame(typeof(CustomAuthorizationProviderData), roSettigs.AuthorizationProviders.Get(authorizationName2).GetType());
            Assert.AreSame(typeof(MockCustomAuthorizationProvider), ((CustomAuthorizationProviderData)roSettigs.AuthorizationProviders.Get(authorizationName2)).Type);
        }
    }
}