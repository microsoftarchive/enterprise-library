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

using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Manageability
{
    /// <summary>
    /// Represents the configuration information from a 
    /// <see cref="Microsoft.Practices.EnterpriseLibrary.Security.Configuration.AuthorizationRuleProviderData"/> instance.
    /// </summary>
    /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Configuration.AuthorizationRuleProviderData"/>
    /// <seealso cref="NamedConfigurationSetting"/>
    /// <seealso cref="ConfigurationSetting"/>	
    [ManagementEntity]
    public partial class AuthorizationRuleProviderSetting : AuthorizationProviderSetting
    {
        string[] rules;

        /// <summary>
        /// Initialize a new instance of the <see cref="AuthorizationRuleProviderSetting"/> class with a configuraiton source element,
        /// the name of the rule provider, and the rules.
        /// </summary>
        /// <param name="sourceElement">The configuration source element.</param>
        /// <param name="name">The provider name.</param>
        /// <param name="rules">The rules for the provider.</param>
        public AuthorizationRuleProviderSetting(ConfigurationElement sourceElement,
                                                string name,
                                                string[] rules)
            : base(sourceElement, name)
        {
            this.rules = rules;
        }

        /// <summary>
        /// Gets the collection of rules represented as a 
        /// <see cref="string"/> array of key/value pairs for the represented configuration object.
        /// </summary>
        /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Security.Configuration.AuthorizationRuleProviderData.Rules">
        /// AuthorizationRuleProviderData.Rules</seealso>
        [ManagementConfiguration]
        public string[] Rules
        {
            get { return rules; }
            set { rules = value; }
        }

        /// <summary>
        /// Returns the <see cref="AuthorizationRuleProviderSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="AuthorizationRuleProviderSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static AuthorizationRuleProviderSetting BindInstance(string ApplicationName,
                                                                    string SectionName,
                                                                    string Name)
        {
            return BindInstance<AuthorizationRuleProviderSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="AuthorizationRuleProviderSetting"/> instances.
        /// </summary>
        [ManagementEnumerator]
        public static IEnumerable<AuthorizationRuleProviderSetting> GetInstances()
        {
            return GetInstances<AuthorizationRuleProviderSetting>();
        }

        /// <summary>
        /// Saves the changes on the <see cref="AuthorizationRuleProviderSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return AuthorizationRuleProviderDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
