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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides
{
    /// <summary>
    /// Represents the logic to create a <see cref="EnvironmentNode"/> from an Environment Delta file (.dconfig).
    /// </summary>
    public class EnvironmentNodeBuilder : NodeBuilder
    {
        /// <summary>
        /// Intializes a new intance of <see cref="EnvironmentNodeBuilder"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider to get service objects.</param>
        public EnvironmentNodeBuilder(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        /// <summary>
        /// Builds an <see cref="EnvironmentNode"/> given a Environment Delta file (.dconfig) and a <see cref="ConfigurationUIHierarchy"/>.
        /// </summary>
        /// <param name="environmentFileName">The path of the Environment Delta file (.dconfig) that should be used to construct the <see cref="EnvironmentNode"/>.</param>
        /// <param name="uiHierarchy">The <see cref="IConfigurationUIHierarchy"/> the created <see cref="EnvironmentNode"/> belongs to.</param>
        /// <returns>An instance of <see cref="EnvironmentNode"/> that represents the Environment Delta file (.dconfig) passed as <paramref name="environmentFileName"/>.</returns>
        public EnvironmentNode Build(string environmentFileName, IConfigurationUIHierarchy uiHierarchy)
        {
            EnvironmentMergeSection mergeSection = GetEnvrinmentMergeSection(environmentFileName);
            if (mergeSection != null)
            {
                EnvironmentMergeData data = ReadEnvironmentMergeData(mergeSection, uiHierarchy);
                data.EnvironmentName = mergeSection.EnvironmentName;
                data.EnvironmentDeltaFile = environmentFileName;
                data.EnvironmentConfigurationFile = mergeSection.EnvironmentDeltaFile;

                EnvironmentNode environmentNode = new EnvironmentNode(data);
                if (mergeSection.SectionInformation.IsProtected && mergeSection.SectionInformation.ProtectionProvider != null)
                {
                    environmentNode.ProtectionProvider = mergeSection.SectionInformation.ProtectionProvider.Name;
                }

                return environmentNode;
            }
            return new EnvironmentNode();
        }

        EnvironmentMergeSection GetEnvrinmentMergeSection(string filename)
        {
            ExeConfigurationFileMap configurationFileMap = new ExeConfigurationFileMap();
            configurationFileMap.ExeConfigFilename = filename;

            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configurationFileMap, ConfigurationUserLevel.None);
            return config.GetSection(EnvironmentMergeSection.EnvironmentMergeData) as EnvironmentMergeSection;
        }

        EnvironmentMergeData ReadEnvironmentMergeData(EnvironmentMergeSection environmentSection, IConfigurationUIHierarchy configurationHierarchy)
        {
            EnvironmentMergeData mergeData = new EnvironmentMergeData();

            foreach (EnvironmentNodeMergeElement mergeElement in environmentSection.MergeElements)
            {
                ConfigurationNodeMergeData propertyDictionaty = new ConfigurationNodeMergeData();
                foreach (string key in mergeElement.OverriddenProperties.AllKeys)
                {
                    string overriddenValue = mergeElement.OverriddenProperties[key].Value;
                    propertyDictionaty.SetPropertyValue(key, new UnserializedPropertyValue(overriddenValue));
                }
                ConfigurationNodeMergeData configurationNodeMergeData = new ConfigurationNodeMergeData(mergeElement.OverrideProperties, propertyDictionaty);
                string fullNodePath = configurationHierarchy.RootNode.Path + mergeElement.ConfigurationNodePath;
                ConfigurationNode node = configurationHierarchy.FindNodeByPath(fullNodePath);
                if (node != null)
                {
                    mergeData.UpdateMergeData(node, configurationNodeMergeData);
                }
            }

            return mergeData;
        }
    }
}
