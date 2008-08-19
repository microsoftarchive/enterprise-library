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
using System.ComponentModel.Design;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Console.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Console
{
    /// <summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// Drives the process of applying the overrides from an environment file to the settings from a configuration file.
    /// </summary>
    public class ConfigurationMergeTool
    {
        string configurationMergeFile;
        string mainConfigurationFile;

        string mergedConfigurationFile;
        bool mergeSucceeded;
        ServiceContainer serviceContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationMergeTool"/> class.
        /// </summary>
        /// <param name="mainConfigurationFile">The path for the file containing the original configuration 
        /// information.</param>
        /// <param name="configurationMergeFile">The path for the file containing the environment overrides
        /// to apply.</param>
        public ConfigurationMergeTool(string mainConfigurationFile,
                                      string configurationMergeFile)
        {
            this.mainConfigurationFile = mainConfigurationFile;
            this.configurationMergeFile = configurationMergeFile;
            serviceContainer = ServiceBuilder.Build();
        }

        /// <summary>
        /// Gets the name of the file where the merged configuration was saved.
        /// </summary>
        public string MergedConfigurationFile
        {
            get { return mergedConfigurationFile; }
        }

        /// <summary>
        /// Gets a flag indicating whether the merge of configuration succeeded.
        /// </summary>
        public bool MergeSucceeded
        {
            get { return mergeSucceeded; }
        }

        EnvironmentNode LoadConfigurationMergeFile(IConfigurationUIHierarchy mainConfigurationHierarchy,
                                                   string configurationMergeFile,
                                                   IServiceProvider serviceProvider)
        {
            EnvironmentNodeBuilder nodeBuilder = new EnvironmentNodeBuilder(serviceProvider);
            EnvironmentNode environmentNode = nodeBuilder.Build(configurationMergeFile, mainConfigurationHierarchy);

            EnvironmentalOverridesNode environmentNodeContainer = mainConfigurationHierarchy.FindNodeByType(typeof(EnvironmentalOverridesNode)) as EnvironmentalOverridesNode;
            if (environmentNodeContainer == null)
            {
                throw new ApplicationException(Resources.ErrorNoEnvironmentContainer);
            }
            environmentNodeContainer.AddNode(environmentNode);

            return environmentNode;
        }

        ConfigurationUIHierarchy LoadMainConfiguration(string configurationFile,
                                                       IServiceProvider serviceProvider)
        {
            IConfigurationUIHierarchyService hierarchyService = (IConfigurationUIHierarchyService)serviceProvider.GetService(typeof(IConfigurationUIHierarchyService));

            ConfigurationApplicationFile data = new ConfigurationApplicationFile(Path.GetDirectoryName(configurationFile), configurationFile);
            ConfigurationUIHierarchy hierarchy = new ConfigurationUIHierarchy(new ConfigurationApplicationNode(data), serviceProvider);
            hierarchyService.AddHierarchy(hierarchy);
            hierarchyService.SelectedHierarchy = hierarchy;

            hierarchy.Open();
            return hierarchy;
        }

        /// <summary>
        /// Applies the overrides from an environment file to the settings from a configuration file, saving the
        /// resulting configuration in a new file.
        /// </summary>
        /// <param name="configurationFile">The name of the file on which the overriden configuration should be saved,
        /// or <see langword="null"/> save to the file specified in the environment file.</param>
        public void MergeAndSaveConfiguration(string configurationFile)
        {
            IConfigurationUIHierarchy mainConfigurationHierarchy = LoadMainConfiguration(mainConfigurationFile, serviceContainer);
            EnvironmentNode environmentNode = LoadConfigurationMergeFile(mainConfigurationHierarchy, configurationMergeFile, serviceContainer);
            if (!String.IsNullOrEmpty(configurationFile))
            {
                environmentNode.EnvironmentConfigurationFile = configurationFile;
            }
            SaveMergedEnvironmentCommand saveMergedConfigurationComand = new SaveMergedEnvironmentCommand(serviceContainer);
            saveMergedConfigurationComand.Execute(environmentNode);

            mergeSucceeded = saveMergedConfigurationComand.MergeSucceeded;
            mergedConfigurationFile = saveMergedConfigurationComand.MergedConfigurationFile;
        }

        class ServiceBuilder
        {
            public static ServiceContainer Build()
            {
                ServiceContainer container = new ServiceContainer();
                NodeNameCreationService nodeNameCreationService = new NodeNameCreationService();
                ConfigurationUIHierarchyService configurationUIHierarchy = new ConfigurationUIHierarchyService();
                container.AddService(typeof(INodeNameCreationService), nodeNameCreationService);
                container.AddService(typeof(IConfigurationUIHierarchyService), configurationUIHierarchy);
                container.AddService(typeof(IUIService), new ConsoleUIService());
                container.AddService(typeof(IErrorLogService), new ErrorLogService());
                container.AddService(typeof(INodeCreationService), new NodeCreationService());
                container.AddService(typeof(IUICommandService), new UICommandService(configurationUIHierarchy));
                container.AddService(typeof(IStorageService), new StorageService());
                container.AddService(typeof(IPluginDirectoryProvider), new AppDomainBasePluginDirectoryProvider());
                return container;
            }

            class AppDomainBasePluginDirectoryProvider : IPluginDirectoryProvider
            {
                public string PluginDirectory
                {
                    get { return AppDomain.CurrentDomain.BaseDirectory; }
                }
            }
        }
    }
}