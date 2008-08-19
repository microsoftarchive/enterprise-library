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
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides
{
    class EnvironmentalOverridesConfigurationDesignManager : ConfigurationDesignManager
    {
        protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, ConfigurationSection section)
        {
            rootNode.AddNode(new EnvironmentalOverridesNode());
        }

        public override void Register(IServiceProvider serviceProvider)
        {
            new EnvironmentalOverridesCommandRegistrar(serviceProvider).Register();

            if (serviceProvider.GetService(typeof(IEnvironmentMergeService)) == null)
            {
                IServiceContainer serviceContainer = (IServiceContainer)serviceProvider.GetService(typeof(IServiceContainer));

                EnvironmentMergeAware environmentMergeService = new EnvironmentMergeAware();
                serviceContainer.AddService(typeof(IEnvironmentMergeService), environmentMergeService);
                serviceContainer.AddService(typeof(EnvironmentMergeAware), environmentMergeService);
            }

            ConfigurationNode rootNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
            if (rootNode != null)
            {
                if (rootNode.Nodes[Resources.EnvironmentalOverridesNodeName] == null)
                {
                    rootNode.AddNode(new EnvironmentalOverridesNode());
                }
            }
        }

        public override void Save(IServiceProvider serviceProvider)
        {
            IErrorLogService errorService = ServiceHelper.GetErrorService(serviceProvider);
            IEnvironmentMergeService environmentMergeService = serviceProvider.GetService(typeof(IEnvironmentMergeService)) as IEnvironmentMergeService;
            if (environmentMergeService != null)
            {
                if (environmentMergeService.EnvironmentMergeInProgress) return;
            }
                
            IConfigurationUIHierarchy hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
            Debug.Assert(hierarchy != null);

            ConfigurationApplicationNode configurationRootNode = hierarchy.FindNodeByType(typeof(ConfigurationApplicationNode)) as ConfigurationApplicationNode;
            Debug.Assert(configurationRootNode != null);

            string configurationFileDirectory = Path.GetDirectoryName(configurationRootNode.ConfigurationFile);

            foreach (EnvironmentNode environmentNode in hierarchy.FindNodesByType(typeof(EnvironmentNode)))
            {
                string environmentDeltaFilePath = Path.Combine(configurationFileDirectory, environmentNode.EnvironmentDeltaFile);

                Dictionary<string, ConfigurationNodeMergeData> mergeDataByPath = environmentNode.EnvironmentMergeData.UnfoldMergeData(hierarchy, false);
                EnvironmentMergeSection environmentMergeSection = new EnvironmentMergeSection();
                environmentMergeSection.EnvironmentName = environmentNode.Name;
                environmentMergeSection.EnvironmentDeltaFile = environmentNode.EnvironmentConfigurationFile;

                CopyEnvironmentOverrides(environmentMergeSection, mergeDataByPath, hierarchy);
                string protectionProvider = GetProtectionProviderName(environmentNode);

                try
                {
                    FileConfigurationSource.ResetImplementation(environmentDeltaFilePath, false);
                    FileConfigurationSource fileConfigurationSource = new FileConfigurationSource(environmentDeltaFilePath);
                    if (!string.IsNullOrEmpty(protectionProvider))
                    {
                        fileConfigurationSource.Save(environmentDeltaFilePath, EnvironmentMergeSection.EnvironmentMergeData, environmentMergeSection, protectionProvider);
                    }
                    else
                    {
                        fileConfigurationSource.Save(environmentDeltaFilePath, EnvironmentMergeSection.EnvironmentMergeData, environmentMergeSection);
                    }
                }
                catch (ConfigurationErrorsException configurationErrors)
                {
                    errorService.LogErrors(configurationErrors);
                }
            }
        }

        private void CopyEnvironmentOverrides(EnvironmentMergeSection environmentMergeSection, Dictionary<string, ConfigurationNodeMergeData> mergeDataByPath, IConfigurationUIHierarchy configurationHierarchy)
        {
            foreach (string path in mergeDataByPath.Keys)
            {
                ConfigurationNodeMergeData mergeData = mergeDataByPath[path];

                EnvironmentNodeMergeElement mergeElement = new EnvironmentNodeMergeElement();
                mergeElement.ConfigurationNodePath = path;
                mergeElement.OverrideProperties = mergeData.OverrideProperties;

                foreach(string propertyName in mergeData.AllPropertyNames)
                {
                    object propertyValue = mergeData.GetPropertyValue(propertyName, typeof(string), null, configurationHierarchy);
                    string serializedRepresentation = SerializationUtility.SerializeToString(propertyValue, configurationHierarchy);

                    NameValueConfigurationElement keyValue = new NameValueConfigurationElement(propertyName, serializedRepresentation);
                    mergeElement.OverriddenProperties.Add(keyValue);
                }

                environmentMergeSection.MergeElements.Add(mergeElement);
            }
        }
    }
}
