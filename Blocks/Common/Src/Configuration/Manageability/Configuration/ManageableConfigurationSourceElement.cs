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
using System.ComponentModel;
using System.Configuration;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Configuration
{
    /// <summary>
    /// Represents the configuration settings that describe a <see cref="ManageableConfigurationSource"/>.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "ManageableConfigurationSourceElementDescription")]
    [ResourceDisplayName(typeof(DesignResources), "ManageableConfigurationSourceElementDisplayName")]
    [Browsable(true)]
    [Command(CommonDesignTime.CommandTypeNames.ExportAdmTemplateCommand)]
    [ViewModel(CommonDesignTime.ViewModelTypeNames.ManageableConfigurationSourceViewModel)]
    [EnvironmentalOverrides(false)]
    public class ManageableConfigurationSourceElement : ConfigurationSourceElement
    {
        private const String filePathPropertyName = "filePath";
        private const String applicationNamePropertyName = "applicationName";
        private const String enableGroupPoliciesPropertyName = "enableGroupPolicies";
        private const String manageabilityProvidersCollectionPropertyName = "manageabilityProviders";

        /// <summary>
        /// Represents the minimum application name length allowed.
        /// </summary>
        public const int MinimumApplicationNameLength = 1;

        /// <summary>
        /// Represents the maximumapplication name length allowed.
        /// </summary>
        public const int MaximumApplicationNameLength = 255;		// maximum allowable registry key name


        /// <summary>
        /// Initializes a new instance of the <see cref="ManageableConfigurationSourceElement"/> class with default values.
        /// </summary>
        public ManageableConfigurationSourceElement()
            : base(Manageability.Properties.Resources.ManageableConfigurationSourceName, typeof(ManageableConfigurationSource))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageableConfigurationSourceElement"/> class.
        /// </summary>
        /// <param name="name">The instance name.</param>
        /// <param name="filePath">The path to the configuration file.</param>
        /// <param name="applicationName">The name that identifies the application consuming the configuration information.</param>
        public ManageableConfigurationSourceElement(String name, String filePath, String applicationName)
            : this(name, filePath, applicationName, true)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageableConfigurationSourceElement"/> class.
        /// </summary>
        /// <param name="name">The instance name.</param>
        /// <param name="filePath">The path to the configuration file.</param>
        /// <param name="applicationName">The name that identifies the application consuming the configuration information.</param>
        /// <param name="enableGroupPolicies"><see langword="true"/> if Group Policy overrides must be appliedby the represented 
        /// configuration source; otherwise, <see langword="false"/>.</param>
        public ManageableConfigurationSourceElement(String name, String filePath, String applicationName,
            Boolean enableGroupPolicies)
            : base(name, typeof(ManageableConfigurationSource))
        {
            this.FilePath = filePath;
            this.ApplicationName = applicationName;
            this.EnableGroupPolicies = enableGroupPolicies;
        }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        [ConfigurationProperty(filePathPropertyName, IsRequired = true)]
        [ResourceDescription(typeof(DesignResources), "ManageableConfigurationSourceElementFilePathDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ManageableConfigurationSourceElementFilePathDisplayName")]
        [Editor(CommonDesignTime.EditorTypes.FilteredFilePath, CommonDesignTime.EditorTypes.UITypeEditor)]
        [FilteredFileNameEditor(typeof(DesignResources), "ManageableConfigurationSourceElementFilePathFilter", CheckFileExists=false)]
        [Validation(CommonDesignTime.ValidationTypeNames.PathExistsValidator)]
        public String FilePath
        {
            get { return (String)this[filePathPropertyName]; }
            set { this[filePathPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the application. This is a required field.
        /// </summary>
        [ConfigurationProperty(applicationNamePropertyName, IsRequired = true, DefaultValue = "Application")]
        [StringValidator(MinLength = MinimumApplicationNameLength, MaxLength = MaximumApplicationNameLength)]
        [ResourceDescription(typeof(DesignResources), "ManageableConfigurationSourceElementApplicationNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ManageableConfigurationSourceElementApplicationNameDisplayName")]
        public String ApplicationName
        {
            get { return (String)this[applicationNamePropertyName]; }
            set { this[applicationNamePropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the value for GP enablement.
        /// </summary>
        [ConfigurationProperty(enableGroupPoliciesPropertyName, DefaultValue = true)]
        [ResourceDescription(typeof(DesignResources), "ManageableConfigurationSourceElementEnableGroupPoliciesDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ManageableConfigurationSourceElementEnableGroupPoliciesDisplayName")]
        public bool EnableGroupPolicies
        {
            get { return (bool)this[enableGroupPoliciesPropertyName]; }
            set { this[enableGroupPoliciesPropertyName] = value; }
        }

        /// <summary>
        /// Gets the collection of registered <see cref="ConfigurationSectionManageabilityProvider"/> types
        /// necessary to provide manageability by the represented configuration source.
        /// </summary>
        [ConfigurationProperty(manageabilityProvidersCollectionPropertyName)]
        [ConfigurationCollection(typeof(ConfigurationSectionManageabilityProviderData))]
        [Browsable(false)]
        public NamedElementCollection<ConfigurationSectionManageabilityProviderData> ConfigurationManageabilityProviders
        {
            get
            {
                return (NamedElementCollection<ConfigurationSectionManageabilityProviderData>)this[manageabilityProvidersCollectionPropertyName];
            }
        }

        /// <summary>
        /// Returns a new <see cref="ManageableConfigurationSource"/> configured with the receiver's settings.
        /// </summary>
        /// <returns>A new configuration source.</returns>
        public override IConfigurationSource CreateSource()
        {
            IDictionary<String, ConfigurationSectionManageabilityProvider> manageabilityProviders = new Dictionary<String, ConfigurationSectionManageabilityProvider>(this.ConfigurationManageabilityProviders.Count);

            ManageabilityProviderBuilder providerBuilder = new ManageabilityProviderBuilder();

            foreach (ConfigurationSectionManageabilityProviderData data in this.ConfigurationManageabilityProviders)
            {
                ConfigurationSectionManageabilityProvider provider
                    = providerBuilder.CreateConfigurationSectionManageabilityProvider(data);

                manageabilityProviders.Add(data.Name, provider);
            }

            return new ManageableConfigurationSource(this.FilePath, manageabilityProviders, this.EnableGroupPolicies, this.ApplicationName);
        }

        /// <summary>
        /// Gets a value indicating whether an unknown attribute is encountered during deserialization.
        /// </summary>
        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            return string.Equals("enableWmi", name, StringComparison.Ordinal)
                || base.OnDeserializeUnrecognizedAttribute(name, value);
        }

        ///<summary>
        /// Returns a new <see cref="IDesignConfigurationSource"/> configured based on this configuration element.
        ///</summary>
        ///<returns>Returns a new <see cref="IDesignConfigurationSource"/> or null if this source does not have design-time support.</returns>
        public override IDesignConfigurationSource CreateDesignSource(IDesignConfigurationSource rootSource)
        {
            return DesignConfigurationSource.CreateDesignSource(rootSource, FilePath);
        }
    }
}
