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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Manageability
{
    /// <summary>
    /// Represents a manageability provider for authorization rules.
    /// </summary>
    public class AuthorizationRuleProviderDataManageabilityProvider
        : ConfigurationElementManageabilityProviderBase<AuthorizationRuleProviderData>
    {
        /// <summary>
        /// The name of the rules property.
        /// </summary>
        public const String RulesPropertyName = "rules";

        /// <summary>
        /// Initialize a new instance of the <see cref="AuthorizationRuleProviderDataManageabilityProvider"/> class.
        /// </summary>
        public AuthorizationRuleProviderDataManageabilityProvider()
        { }

        /// <summary>
        /// Gets the template for the name of the policy associated to the object.
        /// </summary>
        /// <remarks>
        /// Elements that override 
        /// <see cref="ConfigurationElementManageabilityProviderBase{T}.AddAdministrativeTemplateDirectives(AdmContentBuilder, T, IConfigurationSource, String)"/>
        /// to avoid creating a policy must still override this property.
        /// </remarks>
        protected override string ElementPolicyNameTemplate
        {
            get { return Resources.AuthorizationProviderPolicyNameTemplate; }
        }

        /// <summary>
        /// Adds the ADM parts that represent the properties of
        /// a specific instance of the configuration element type managed by the receiver.
        /// </summary>
        /// <param name="contentBuilder">The <see cref="AdmContentBuilder"/> to which the Adm instructions are to be appended.</param>
        /// <param name="configurationObject">The configuration object instance.</param>
        /// <param name="configurationSource">The configuration source from where to get additional configuration
        /// information, if necessary.</param>
        /// <param name="elementPolicyKeyName">The key for the element's policies.</param>
        /// <remarks>
        /// Subclasses managing objects that must not create a policy will likely need to include the elements' keys when creating the parts.
        /// </remarks>
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
                                                                      AuthorizationRuleProviderData configurationObject,
                                                                      IConfigurationSource configurationSource,
                                                                      String elementPolicyKeyName)
        {
            contentBuilder.AddEditTextPart(Resources.AuthorizationRuleProviderRulesPartName,
                                           RulesPropertyName,
                                           GenerateRulesString(configurationObject.Rules),
                                           1024,
                                           true);
        }

        static String GenerateRulesString(NamedElementCollection<AuthorizationRuleData> rules)
        {
            KeyValuePairEncoder encoder = new KeyValuePairEncoder();

            foreach (AuthorizationRuleData ruleData in rules)
            {
                encoder.AppendKeyValuePair(ruleData.Name, ruleData.Expression);
            }

            return encoder.GetEncodedKeyValuePairs();
        }

        /// <summary>
        /// Overrides the <paramref name="configurationObject"/>'s properties with the Group Policy values from the 
        /// registry.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="policyKey">The <see cref="IRegistryKey"/> which holds the Group Policy overrides for the 
        /// configuration element.</param>
        /// <remarks>Subclasses implementing this method must retrieve all the override values from the registry
        /// before making modifications to the <paramref name="configurationObject"/> so any error retrieving
        /// the override values will cancel policy processing.</remarks>
        protected override void OverrideWithGroupPolicies(AuthorizationRuleProviderData configurationObject,
                                                          IRegistryKey policyKey)
        {
            String rulesOverride = policyKey.GetStringValue(RulesPropertyName);

            if (rulesOverride != null)
            {
                configurationObject.Rules.Clear();

                Dictionary<String, String> attributesDictionary = new Dictionary<string, string>();
                KeyValuePairParser.ExtractKeyValueEntries(rulesOverride, attributesDictionary);
                foreach (KeyValuePair<String, String> kvp in attributesDictionary)
                {
                    configurationObject.Rules.Add(new AuthorizationRuleData(kvp.Key, kvp.Value));
                }
            }
        }
    }
}
