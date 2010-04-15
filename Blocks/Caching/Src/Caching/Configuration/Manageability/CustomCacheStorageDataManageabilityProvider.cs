//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability
{
    /// <summary>
    /// Provides a default implementation for <see cref="CustomCacheStorageData"/> that
    /// processes policy overrides, performing appropriate logging of 
    /// policy processing errors.
    /// </summary>
    public class CustomCacheStorageDataManageabilityProvider
        : CustomProviderDataManageabilityProvider<CustomCacheStorageData>
    {
        /// <summary>
        /// The name of the attributes property.
        /// </summary>
        public new const String AttributesPropertyName = CustomProviderDataManageabilityProvider<CustomCacheStorageData>.AttributesPropertyName;

        /// <summary>
        /// The name of the provider type property.
        /// </summary>
        public new const String ProviderTypePropertyName = CustomProviderDataManageabilityProvider<CustomCacheStorageData>.ProviderTypePropertyName;

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomCacheStorageDataManageabilityProvider"/> class.
        /// </summary>
        public CustomCacheStorageDataManageabilityProvider()
            : base("")
        { }

        /// <summary>
        /// Adds the ADM instructions that describe the policies that can be used to override the properties of
        /// a specific instance of the configuration element type managed by the receiver.
        /// </summary>
        /// <param name="contentBuilder">The <see cref="AdmContentBuilder"/> to which the Adm instructions are to be appended.</param>
        /// <param name="configurationObject">The configuration object instance.</param>
        /// <param name="configurationSource">The configuration source from where to get additional configuration
        /// information, if necessary.</param>
        /// <param name="elementPolicyKeyName">The key for the element's policies.</param>
        /// <remarks>
        /// The default implementation for this method creates a policy, using 
        /// <see cref="ConfigurationElementManageabilityProviderBase{T}.ElementPolicyNameTemplate"/> to create the policy name and invoking
        /// <see cref="ConfigurationElementManageabilityProviderBase{T}.AddElementAdministrativeTemplateParts(AdmContentBuilder, T, IConfigurationSource, String)"/>
        /// to add the policy parts.
        /// Subclasses managing objects that must not create a policy must override this method to just add the parts.
        /// </remarks>
        protected override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
                                                                    CustomCacheStorageData configurationObject,
                                                                    IConfigurationSource configurationSource,
                                                                    String elementPolicyKeyName)
        {
            // parts for stores are part of their cache manager's policies
            AddElementAdministrativeTemplateParts(contentBuilder,
                                                  configurationObject,
                                                  configurationSource,
                                                  elementPolicyKeyName);
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
                                                                      CustomCacheStorageData configurationObject,
                                                                      IConfigurationSource configurationSource,
                                                                      String elementPolicyKeyName)
        {
            contentBuilder.AddEditTextPart(Resources.CustomProviderTypePartName,
                                           elementPolicyKeyName,
                                           ProviderTypePropertyName,
                                           configurationObject.Type.AssemblyQualifiedName,
                                           1024,
                                           true);

            contentBuilder.AddEditTextPart(Resources.CustomProviderAttributesPartName,
                                           elementPolicyKeyName,
                                           AttributesPropertyName,
                                           GenerateAttributesString(configurationObject.Attributes),
                                           1024,
                                           false);
        }
    }
}
