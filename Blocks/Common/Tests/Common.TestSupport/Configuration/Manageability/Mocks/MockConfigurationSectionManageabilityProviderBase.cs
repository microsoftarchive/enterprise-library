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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;

namespace Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.Manageability.Mocks
{
    public abstract class MockConfigurationSectionManageabilityProviderBase
        : ConfigurationSectionManageabilityProvider
    {
        public bool called = false;
        public ConfigurationElement configurationObject;
        public bool readGroupPolicies;
        public IRegistryKey machineKey;
        public IRegistryKey userKey;
        public bool generateWmiObjects;

        public const string ValuePropertyName = "value";

        public MockConfigurationSectionManageabilityProviderBase(IDictionary<Type, ConfigurationElementManageabilityProvider> subProviders)
            : base(subProviders)
        { }

        public override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
            ConfigurationSection configurationObject,
            IConfigurationSource configurationSource,
            String applicationName)
        { }

        public override bool OverrideWithGroupPoliciesAndGenerateWmiObjects(ConfigurationSection configurationObject,
            bool readGroupPolicies, IRegistryKey machineKey, IRegistryKey userKey,
            bool generateWmiObjects, ICollection<ConfigurationSetting> wmiSettings)
        {
            called = true;
            this.configurationObject = configurationObject;
            this.readGroupPolicies = readGroupPolicies;
            this.machineKey = machineKey;
            this.userKey = userKey;
            this.generateWmiObjects = generateWmiObjects;

            IRegistryKey policyKey = GetPolicyKey(machineKey, userKey);
            if (policyKey != null)
            {
                if (!policyKey.GetBoolValue(PolicyValueName).Value)
                {
                    return false;
                }

                TestsConfigurationSection section = configurationObject as TestsConfigurationSection;
                if (section != null)
                {
                    try
                    {
                        section.Value = policyKey.GetStringValue(ValuePropertyName);
                    }
                    catch (RegistryAccessException)
                    { }
                }
            }

            if (generateWmiObjects)
            {
                TestConfigurationSettings setting = new TestConfigurationSettings(configurationObject.ToString());
                setting.SourceElement = configurationObject;

                wmiSettings.Add(setting);
            }

            return true;
        }

        public new ConfigurationElementManageabilityProvider GetSubProvider(Type configurationObjectType)
        {
            return base.GetSubProvider(configurationObjectType);
        }
    }
}
