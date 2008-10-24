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
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    /// <summary>
    /// Base class for <see cref="ConfigurationElementManageabilityProvider"/> implementations that provide manageability
    /// support for custom provider's configuration.
    /// </summary>
    /// <typeparam name="T">The custon provider's configuration element type.</typeparam>
    /// <remarks>
    /// The basic configuration for a custom provider includes the provider type and a collection of attributes.
    /// </remarks>
    public abstract class CustomProviderDataManageabilityProvider<T> : ConfigurationElementManageabilityProviderBase<T>
        where T : NameTypeConfigurationElement, ICustomProviderData
    {
        /// <summary>
        /// Name for the value holding the policy overrides for the custom provider's attributes.
        /// </summary>
        public const String AttributesPropertyName = "attributes";

        /// <summary>
        /// Name for the value holding the policy overrides for the custom provider's type.
        /// </summary>
        public const String ProviderTypePropertyName = "type";

        readonly String policyTemplate;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomProviderDataManageabilityProvider{T}"/> class with a 
        /// policy name template.
        /// </summary>
        /// <param name="policyTemplate">The template to use when generating the policy associated to a custom provider
        /// configuration instance.</param>
        protected CustomProviderDataManageabilityProvider(String policyTemplate)
        {
            this.policyTemplate = policyTemplate;
        }

        /// <summary>
        /// Gets the template for the name of the policy associated to the object.
        /// </summary>
        protected override string ElementPolicyNameTemplate
        {
            get { return policyTemplate; }
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
        /// Subclasses that manage custom provider's configuration objects with additional properties may
        /// override this method to add the corresponding parts.
        /// </remarks>
        /// <seealso cref="ConfigurationElementManageabilityProviderBase{T}.AddAdministrativeTemplateDirectives(AdmContentBuilder, T, IConfigurationSource, String)"/>
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
                                                                      T configurationObject,
                                                                      IConfigurationSource configurationSource,
                                                                      String elementPolicyKeyName)
        {
            contentBuilder.AddEditTextPart(Resources.CustomProviderTypePartName,
                                           ProviderTypePropertyName,
                                           configurationObject.Type.AssemblyQualifiedName,
                                           1024,
                                           true);

            contentBuilder.AddEditTextPart(Resources.CustomProviderAttributesPartName,
                                           AttributesPropertyName,
                                           GenerateAttributesString(configurationObject.Attributes),
                                           1024,
                                           false);
        }

        /// <summary>
        /// Returns a string with the encoded key/value pairs that represent the <paramref name="attributes"/> collection.
        /// </summary>
        /// <param name="attributes">The collection of attributes.</param>
        /// <returns>The encoded representation of the attributes collection.</returns>
        protected static String GenerateAttributesString(NameValueCollection attributes)
        {
            KeyValuePairEncoder encoder = new KeyValuePairEncoder();

            foreach (String key in attributes.AllKeys)
            {
                encoder.AppendKeyValuePair(key, attributes[key]);
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
        /// <remarks>Subclasses that manage custom provider's configuration objects with additional properties may
        /// override this method to override these properties.</remarks>
        protected override void OverrideWithGroupPolicies(T configurationObject,
                                                          IRegistryKey policyKey)
        {
            Type providerTypeOverride = policyKey.GetTypeValue(ProviderTypePropertyName);
            String attributesOverride = policyKey.GetStringValue(AttributesPropertyName);

            configurationObject.Type = providerTypeOverride;

            configurationObject.Attributes.Clear();
            Dictionary<String, String> attributesDictionary = new Dictionary<string, string>();
            KeyValuePairParser.ExtractKeyValueEntries(attributesOverride, attributesDictionary);
            foreach (KeyValuePair<String, String> kvp in attributesDictionary)
            {
                configurationObject.Attributes.Add(kvp.Key, kvp.Value);
            }
        }
    }
}
