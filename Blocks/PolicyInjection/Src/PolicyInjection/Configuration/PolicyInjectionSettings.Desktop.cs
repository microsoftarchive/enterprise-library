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
using System.Linq;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A <see cref="ConfigurationSection"/> that stores the policy set in configuration.
    /// </summary>
    [ViewModel(PolicyInjectionDesignTime.ViewModelTypeNames.PolicyInjectionSectionViewModel)]
    [ResourceDescription(typeof(DesignResources), "PolicyInjectionSettingsDescription")]
    [ResourceDisplayName(typeof(DesignResources), "PolicyInjectionSettingsDisplayName")]
    public partial class PolicyInjectionSettings : SerializableConfigurationSection
    {
        //private const string InjectorsPropertyName = "injectors";
        private const string PoliciesPropertyName = "policies";

        /// <summary>
        /// Gets or sets the collection of Policies from configuration.
        /// </summary>
        /// <value>The <see cref="PolicyData"/> collection.</value>
        [ConfigurationProperty(PoliciesPropertyName)]
        [ConfigurationCollection(typeof(PolicyData))]
        [ResourceDescription(typeof(DesignResources), "PolicyInjectionSettingsPoliciesDescription")]
        [ResourceDisplayName(typeof(DesignResources), "PolicyInjectionSettingsPoliciesDisplayName")]
        public NamedElementCollection<PolicyData> Policies
        {
            get { return (NamedElementCollection<PolicyData>)base[PoliciesPropertyName]; }
            set { base[PoliciesPropertyName] = value; }
        }

        /// <summary>
        /// Gets a value indicating whether an unknown element is encountered during deserialization.
        /// </summary>
        /// <param name="elementName">The name of the unknown subelement.</param>
        /// <param name="reader">The <see cref="XmlReader"/> being used for deserialization.</param>
        /// <returns>true when an unknown element is encountered while deserializing; otherwise, false.</returns>
        /// <remarks>This class will ignore an element named "injectors".</remarks>
        protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
        {
            if (elementName == "injectors")
            {
                reader.Skip();
                return true;
            }

            return base.OnDeserializeUnrecognizedElement(elementName, reader);
        }
    }
}
