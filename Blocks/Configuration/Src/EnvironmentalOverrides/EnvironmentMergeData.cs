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
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Properties;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides
{

    /// <summary>
    /// Represents information specific to a logic environment and its overridden configuration settings at designtime.
    /// </summary>
    public class EnvironmentMergeData
    {
        private MergeDataDictionary mergeDataDictionary = new MergeDataDictionary();
        private string environmentName;
        private string environmentDeltaFile;
        private string environemntConfigurationFile;

        /// <summary>
        /// Initialize a new instance of <see cref="EnvironmentMergeData"/>.
        /// </summary>
        public EnvironmentMergeData()
        {
            environmentName = Resources.DefaultEnvironmentName;
            environmentDeltaFile = Resources.DefaultEnvironmentDeltaFile;
            environemntConfigurationFile = string.Empty;
        }

        /// <summary>
        /// Gets or sets the name of the logical environment.
        /// </summary>
        public string EnvironmentName
        {
            get { return environmentName; }
            set { environmentName = value; }
        }

        /// <summary>
        /// Gets or sets the location of the delta configuration file (.dconfig) that can
        /// be used to merge configuration and create a new deployable configuration file.
        /// </summary>
        public string EnvironmentDeltaFile
        {
            get { return environmentDeltaFile; }
            set { environmentDeltaFile = value; }
        }

        /// <summary>
        /// Gets or sets the location of the configuration file that is generated as a result of merging configuration.
        /// </summary>
        public string EnvironmentConfigurationFile
        {
            get { return environemntConfigurationFile; }
            set { environemntConfigurationFile = value; }
        }

        /// <summary>
        /// Returns all the instances of <see cref="ConfigurationNodeMergeData"/> contained in this environment, indexed by the path of the configuration they apply to.
        /// <seealso cref="ConfigurationNodeMergeData"/>
        /// </summary>
        /// <param name="hierarchy"></param>
        /// <param name="useFullPathNames"></param>
        /// <returns></returns>
        public Dictionary<string, ConfigurationNodeMergeData> UnfoldMergeData(IConfigurationUIHierarchy hierarchy, bool useFullPathNames) 
        {
            Dictionary<string, ConfigurationNodeMergeData> result = new Dictionary<string, ConfigurationNodeMergeData>();

            foreach (Guid configurationNodeId in mergeDataDictionary.Keys)
            {
                ConfigurationNode configurationNode = hierarchy.FindNodeById(configurationNodeId);
                string rootNodePath = hierarchy.RootNode.Path;

                if (configurationNode != null)
                {
                    string configurationNodePath = configurationNode.Path;
                    if (!useFullPathNames)
                    {
                        configurationNodePath = SerializationUtility.CreatePathRelativeToRootNode(configurationNodePath, hierarchy);
                    }

                    result.Add(configurationNodePath, mergeDataDictionary[configurationNodeId]);
                }
            }

            return result;
        }

        /// <summary>
        /// Updates the <see cref="ConfigurationNodeMergeData"/> for a specific instance of <see cref="ConfigurationNode"/>.
        /// </summary>
        /// <param name="configurationNode">The <see cref="ConfigurationNode"/> whose <see cref="ConfigurationNodeMergeData"/> should be updated.</param>
        /// <param name="mergeData">The <see cref="ConfigurationNodeMergeData"/> that apply on the given <see cref="ConfigurationNode"/>.</param>
        public void UpdateMergeData(ConfigurationNode configurationNode, ConfigurationNodeMergeData mergeData)
        {
            mergeDataDictionary[configurationNode.Id] = mergeData;
        }


        /// <summary>
        /// Gets the <see cref="ConfigurationNodeMergeData"/> for a specific <see cref="ConfigurationNode"/>.
        /// </summary>
        /// <param name="configurationNode">The <see cref="ConfigurationNode"/> for which the <see cref="ConfigurationNodeMergeData"/> should be returned.</param>
        /// <returns>The <see cref="ConfigurationNodeMergeData"/> that belongs to <paramref name="configurationNode"/>. If no <see cref="ConfigurationNodeMergeData"/> is found, en empty isntance of <see cref="ConfigurationNodeMergeData"/> is returned.</returns>
        public ConfigurationNodeMergeData GetMergeData(ConfigurationNode configurationNode)
        {
            ConfigurationNodeMergeData mergeData = null;

            if (mergeDataDictionary.TryGetValue(configurationNode.Id, out mergeData))
            {
                return mergeData;
            }
            return new ConfigurationNodeMergeData();
        }

        private class MergeDataDictionary : Dictionary<Guid, ConfigurationNodeMergeData>
        {

        }
    }
}
