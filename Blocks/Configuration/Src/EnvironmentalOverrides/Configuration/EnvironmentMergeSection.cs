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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration
{
    /// <summary>
    /// Represents a configuration section that contains the difference (or delta) between 2 deployable configuration files.
    /// </summary>
    /// <remarks>
    /// This configuration section is used by the Environmental Overrides extension to the configuration tool at designtime.
    /// </remarks>
    [ViewModel(EnvironmentalOverridesDesignTime.ViewModelTypeNames.EnvironmentalOverridesViewModel)]
    [ResourceDescription(typeof(DesignResources), "EnvironmentMergeSectionDescription")]
    [ResourceDisplayName(typeof(DesignResources), "EnvironmentMergeSectionDisplayName")]
    [NameProperty("EnvironmentName")]
    public class EnvironmentMergeSection : ConfigurationSection
    {
        /// <summary>
        /// The name of the serialized <see cref="EnvironmentMergeSection"/> in configuration.
        /// </summary>
        public const string EnvironmentMergeData = "EnvironmentMergeData";
        private const string EnvironmentNamePropertyName = "environmentName";
        private const string EnvironmentConfigurationFilePropertyName = "environmentConfigurationFile";
        private const string MergeElementsPropertyName = "mergeElements";

        /// <summary>
        /// Gets or sets a collection of <see cref="EnvironmentNodeMergeElement"/>.
        /// </summary>
        [ConfigurationProperty(MergeElementsPropertyName, IsDefaultCollection=true)]
        [Browsable(false)]
        public EnvironmentNodeMergeElementCollection MergeElements
        {
            get { return (EnvironmentNodeMergeElementCollection)base[MergeElementsPropertyName]; }
            set { base[MergeElementsPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the environment.
        /// </summary>
        [ConfigurationProperty(EnvironmentNamePropertyName)]
        [ResourceDescription(typeof(DesignResources), "EnvironmentMergeSectionEnvironmentNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "EnvironmentMergeSectionEnvironmentNameDisplayName")]
        [EnvironmentalOverrides(false)]
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
        [FilteredFileNameEditor(typeof(DesignResources), "EnvironmentMergeSectionEnvironmentDeltaFileFilter")]
        [Editor(CommonDesignTime.EditorTypes.FilteredFilePath, CommonDesignTime.EditorTypes.UITypeEditor)]
        [EnvironmentalOverrides(false)]
        public string EnvironmentConfigurationFile
        {
            get { return (string)base[EnvironmentConfigurationFilePropertyName]; }
            set { base[EnvironmentConfigurationFilePropertyName] = value; }
        }
    }

    
}
