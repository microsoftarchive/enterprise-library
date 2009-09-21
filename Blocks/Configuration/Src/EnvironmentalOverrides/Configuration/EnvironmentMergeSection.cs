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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration
{
    /// <summary>
    /// Represents a configuration section that contains the difference (or delta) between 2 deployable configuration files.
    /// </summary>
    /// <remarks>
    /// This configuration section is used by the Environmental Overrides extension to the configuration tool at designtime.
    /// </remarks>
    [ViewModel("Console.Wpf.ViewModel.BlockSpecifics.EnvironmentalOverridesViewModel, Console.Wpf")]
    public class EnvironmentMergeSection : ConfigurationSection
    {
        /// <summary>
        /// The name of the serialized <see cref="EnvironmentMergeSection"/> in configuration.
        /// </summary>
        public const string EnvironmentMergeData = "EnvironmentMergeData";
        private const string EnvironmentNamePropertyName = "environmentName";
        private const string EnvironmentDeltaFilePropertyName = "environmentDeltaFile";
        private const string MergeElementsPropertyName = "mergeElements";

        /// <summary>
        /// Gets or sets a collection of <see cref="EnvironmentNodeMergeElement"/>.
        /// </summary>
        [ConfigurationProperty(MergeElementsPropertyName, IsDefaultCollection=true)]
        public EnvironmentNodeMergeElementCollection MergeElements
        {
            get { return (EnvironmentNodeMergeElementCollection)base[MergeElementsPropertyName]; }
            set { base[MergeElementsPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the environment.
        /// </summary>
        [ConfigurationProperty(EnvironmentNamePropertyName)]
        public string EnvironmentName
        {
            get { return (string)base[EnvironmentNamePropertyName]; }
            set { base[EnvironmentNamePropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the path to the deployable configuration that can be generated from this configuration.
        /// </summary>
        [ConfigurationProperty(EnvironmentDeltaFilePropertyName)]
        public string EnvironmentDeltaFile
        {
            get { return (string)base[EnvironmentDeltaFilePropertyName]; }
            set { base[EnvironmentDeltaFilePropertyName] = value; }
        }
    }

    
}
