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
using System.Text;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration
{
    /// <summary>
    /// Represents a configuration section that contains the difference (or delta) between 2 deployable configuration files.
    /// </summary>
    [ViewModel(EnvironmentalOverridesDesignTime.ViewModelTypeNames.EnvironmentalOverridesViewModel)]
    [ResourceDescription(typeof(DesignResources), "EnvironmentMergeSectionDescription")]
    [ResourceDisplayName(typeof(DesignResources), "EnvironmentMergeSectionDisplayName")]
    [NameProperty("EnvironmentName")]
    public class EnvironmentalOverridesSection : ConfigurationSection
    {
        /// <summary>
        /// The name of the serialized <see cref="EnvironmentalOverridesSection"/> in configuration.
        /// </summary>
        public const string EnvironmentallyOverriddenProperties = "environmentallyOverriddenProperties";
        private const string EnvironmentNamePropertyName = "environmentName";
        private const string EnvironmentConfigurationFilePropertyName = "environmentConfigurationFile";
        private const string MergeElementsPropertyName = "designtimeLogicalPropertyGroups";
        private const string OverriddenProtectionProvidersPropertyName = "overriddenProtectionProviders";

        /// <summary>
        /// Gets or sets a collection of <see cref="EnvironmentalOverridesElement"/>.
        /// </summary>
        [ConfigurationProperty(MergeElementsPropertyName)]
        [Browsable(false)]
        public EnvironmentalOverridesElementCollection MergeElements
        {
            get { return (EnvironmentalOverridesElementCollection)base[MergeElementsPropertyName]; }
            set { base[MergeElementsPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets a collection of <see cref="EnvironmentOverriddenProtectionProviderElement"/>.
        /// </summary>
        [ConfigurationProperty(OverriddenProtectionProvidersPropertyName)]
        [Browsable(false)]
        public EnvironmentOverriddenProtectionProviderElementCollection OverriddenProtectionProviders
        {
            get { return (EnvironmentOverriddenProtectionProviderElementCollection) base[OverriddenProtectionProvidersPropertyName]; }
            set { base[OverriddenProtectionProvidersPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the environment.
        /// </summary>
        [ConfigurationProperty(EnvironmentNamePropertyName)]
        [ResourceDescription(typeof(DesignResources), "EnvironmentMergeSectionEnvironmentNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "EnvironmentMergeSectionEnvironmentNameDisplayName")]
        [EnvironmentalOverrides(false)]
        [Validation(EnvironmentalOverridesDesignTime.ValidatorTypeName.EnvironmentalOverridesSectionNameValidator)]
        public string EnvironmentName
        {
            get { return (string)base[EnvironmentNamePropertyName]; }
            set { base[EnvironmentNamePropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the path to the deployable configuration that can be generated from this configuration.
        /// </summary>
        [ConfigurationProperty(EnvironmentConfigurationFilePropertyName)]
        [ResourceDescription(typeof(DesignResources), "EnvironmentMergeSectionEnvironmentDeltaFileDescription")]
        [ResourceDisplayName(typeof(DesignResources), "EnvironmentMergeSectionEnvironmentDeltaFileDisplayName")]
        [FilteredFileNameEditor(typeof(DesignResources), "EnvironmentMergeSectionEnvironmentDeltaFileFilter", CheckFileExists = false)]
        [Editor(CommonDesignTime.EditorTypes.FilteredFilePath, CommonDesignTime.EditorTypes.UITypeEditor)]
        [EnvironmentalOverrides(false)]
        [Validation(CommonDesignTime.ValidationTypeNames.PathExistsValidator)]
        [Validation(EnvironmentalOverridesDesignTime.ValidatorTypeName.EnvironmentalOverridesSectionMergeFileValidator)]
        public string EnvironmentConfigurationFile
        {
            get { return (string)base[EnvironmentConfigurationFilePropertyName]; }
            set { base[EnvironmentConfigurationFilePropertyName] = value; }
        }
    }

    
}
