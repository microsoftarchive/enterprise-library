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

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A <see cref="ConfigurationSection"/> that stores the policy set in configuration.
    /// </summary>
    public class PolicyInjectionSettings : SerializableConfigurationSection
    {
        private const string InjectorsPropertyName = "injectors";
        private const string PoliciesPropertyName = "policies";

        /// <summary>
        /// Section name as it appears in the config file.
        /// </summary>
        public const string SectionName = "policyInjection";

        /// <summary>
        /// Gets or sets the collections of injectors from configuration.
        /// </summary>
        /// <value>The <see cref="InjectorDataCollection"/>.</value>
        [ConfigurationProperty(InjectorsPropertyName)]
        public InjectorDataCollection Injectors
        {
            get { return (InjectorDataCollection)base[InjectorsPropertyName]; }
            set { base[InjectorsPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the collection of Policies from configuration.
        /// </summary>
        /// <value>The <see cref="PolicyData"/> collection.</value>
        [ConfigurationProperty(PoliciesPropertyName)]
        public NamedElementCollection<PolicyData> Policies
        {
            get { return (NamedElementCollection<PolicyData>)base[PoliciesPropertyName]; }
            set { base[PoliciesPropertyName] = value; }
        }
    }
}
