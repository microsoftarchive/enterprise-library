//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.TestSupport
{
    public class CallHandlerDataFixtureBase
    {
        public static CallHandlerData SerializeAndDeserializeHandler(CallHandlerData handlerData)
        {
            PolicyData policy = new PolicyData("policy");
            policy.Handlers.Add(handlerData);

            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(policy);

            Dictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections.Add(PolicyInjectionSettings.SectionName, settings);

            IConfigurationSource configurationSource =
                ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            PolicyInjectionSettings deserializedSection =
                configurationSource.GetSection(PolicyInjectionSettings.SectionName) as PolicyInjectionSettings;
            Assert.IsNotNull(deserializedSection);

            PolicyData deserializedPolicy = deserializedSection.Policies.Get(0);
            Assert.IsNotNull(deserializedPolicy);
            return deserializedPolicy.Handlers.Get(0);
        }
    }
}
