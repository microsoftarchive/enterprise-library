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
using System.IO;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides
{
    /// <summary>
    /// Represents a command for merging environmental specific configuration and generating a new deployable 
    /// configuration file specific for an environment.
    /// </summary>
    public class SaveMergedEnvironmentCommand : ConfigurationNodeCommand
    {
        private bool mergeSucceeded;
        private string mergedConfigurationFile;

        /// <summary>
        /// Initialize a new instance of <see cref="SaveMergedEnvironmentCommand"/>.
        /// </summary>
        /// <param name="serviceProvider">The a mechanism for retrieving a service object; 
        /// that is, an object that provides custom support to other objects.</param>
        public SaveMergedEnvironmentCommand(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        /// <summary>
        /// Returns <see langword="true"/> if the command executed successfully, otherwise <see langword="false"/>.
        /// </summary>
        public bool MergeSucceeded
        {
            get { return mergeSucceeded; }
        }

        /// <summary>
        /// Gets the path to the file that was used to write the merged configuration.
        /// </summary>
        public string MergedConfigurationFile
        {
            get { return mergedConfigurationFile; }
        }

        /// <summary>
        /// Performs the merging of configuration, given a specific <see cref="EnvironmentNode"/>.
        /// </summary>
        /// <param name="node">The <see cref="EnvironmentNode"/> this command should be executed on.</param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            EnvironmentNode environmentNode = node as EnvironmentNode;
            if (environmentNode != null)
            {
                if (EnsureMainHierarchyIsSaved(node.Hierarchy, node.Site))
                {
                    EnvironmentMergeAware environmentMergeService
                        = node.Site.GetService(typeof(EnvironmentMergeAware)) as EnvironmentMergeAware;

                    string originalFilePath = Path.GetFullPath(node.Hierarchy.RootNode.ConfigurationFile);
                    string originalDirectoryPath = Path.GetDirectoryName(originalFilePath);
                    environmentMergeService.SetEnvironmentalMergeInProgress(true);

                    try
                    {
                        UIService.BeginUpdate();

                        Dictionary<string, ConfigurationNodeMergeData> mergeDataByPath
                            = environmentNode.EnvironmentMergeData.UnfoldMergeData(environmentNode.Hierarchy, false);

                        string temporaryFilepath = Path.GetTempFileName();
                        bool temporarySaveSuccessful = false;
                        try
                        {
                            UpdateConfigurationSource(node.Hierarchy, temporaryFilepath);

                            SaveConfigurationApplicationNodeCommand saveConfigurationNodeCommand
                                = new SaveConfigurationApplicationNodeCommand(ServiceProvider);
                            saveConfigurationNodeCommand.Execute(ServiceHelper.GetCurrentRootNode(ServiceProvider));
                            temporarySaveSuccessful = saveConfigurationNodeCommand.SaveSucceeded;
                        }
                        finally
                        {
                            UpdateConfigurationSource(node.Hierarchy, originalFilePath);
                            node.Hierarchy.ConfigurationSource = null;
                            node.Hierarchy.ConfigurationParameter = null;
                        }

                        if (temporarySaveSuccessful)
                        {
                            using (TemporaryConfigurationHierarchy tempHierarchy
                                = new TemporaryConfigurationHierarchy(ServiceProvider, temporaryFilepath))
                            {
                                MergeHierarchy(tempHierarchy.Hierarchy, mergeDataByPath);

                                SaveConfigurationApplicationNodeCommand saveConfigurationApplication
                                    = new SaveConfigurationApplicationNodeCommand(ServiceProvider);

                                string environmentConfigurationFile
                                    = CreateMergedConfigurationFile(environmentNode, originalFilePath, originalDirectoryPath);

                                UpdateConfigurationSource(tempHierarchy.Hierarchy, environmentConfigurationFile);
                                RemoveConfigurationSourceElements(tempHierarchy.Hierarchy);

                                saveConfigurationApplication.Execute(tempHierarchy.Hierarchy.RootNode);

                                mergeSucceeded = saveConfigurationApplication.SaveSucceeded;
                                mergedConfigurationFile = environmentConfigurationFile;

                            }

                            try
                            {
                                File.Delete(temporaryFilepath);
                            }
                            catch (IOException)
                            {
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        IUIService uiService = ServiceHelper.GetUIService(ServiceProvider);
                        uiService.ShowError(e, Resources.ErrorSavingMergedConfigurationCaption);
                    }
                    finally
                    {
                        IConfigurationUIHierarchyService hierarchyService = ServiceHelper.GetUIHierarchyService(ServiceProvider);
                        hierarchyService.SelectedHierarchy = node.Hierarchy;

                        UIService.EndUpdate();

                        environmentMergeService.SetEnvironmentalMergeInProgress(false);
                    }
                }
            }
        }

        private string CreateMergedConfigurationFile(EnvironmentNode environmentNode, string originalFilePath, string originalDirectoryPath)
        {
            string environmentConfigurationFile = Path.Combine(originalDirectoryPath, environmentNode.EnvironmentConfigurationFile);
            EnsureDirectoryExists(Path.GetDirectoryName(environmentConfigurationFile));
            File.Copy(originalFilePath, environmentConfigurationFile, true);

            // make the target file writable (it may have inherited the read-only attribute from the original file)
            FileAttributes attributes = File.GetAttributes(environmentConfigurationFile);
            File.SetAttributes(environmentConfigurationFile, attributes & ~FileAttributes.ReadOnly);

            return environmentConfigurationFile;
        }

        private void EnsureDirectoryExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private bool EnsureMainHierarchyIsSaved(IConfigurationUIHierarchy configurationHierarchy, IServiceProvider serviceProvider)
        {
            if (String.IsNullOrEmpty(configurationHierarchy.RootNode.ConfigurationFile))
            {
                IUIService uiService = ServiceHelper.GetUIService(serviceProvider);

                if (DialogResult.Yes
                    == uiService.ShowMessage(
                        Resources.SaveConfigurationFileBeforeEnvironmentMerge,
                        Resources.GenericDialogCaption,
                        MessageBoxButtons.YesNo))
                {
                    SaveConfigurationApplicationNodeCommand saveApplicationNodeCommand
                        = new SaveConfigurationApplicationNodeCommand(serviceProvider);
                    saveApplicationNodeCommand.Execute(configurationHierarchy.RootNode);

                    return saveApplicationNodeCommand.SaveSucceeded;
                }
                return false;
            }
            return true;
        }

        private static void UpdateConfigurationSource(IConfigurationUIHierarchy configurationUIHierarchy, string filename)
        {
            FileConfigurationSource.ResetImplementation(filename, false);

            configurationUIHierarchy.ConfigurationSource = new FileConfigurationSource(filename);
            configurationUIHierarchy.ConfigurationParameter = new FileConfigurationParameter(filename);
            configurationUIHierarchy.StorageService.ConfigurationFile = filename;

            if (configurationUIHierarchy.RootNode != null)
            {
                configurationUIHierarchy.RootNode.ConfigurationFile = filename;
            }
        }

        private void RemoveConfigurationSourceElements(IConfigurationUIHierarchy configurationUIHierarchy)
        {
            ConfigurationSourceSectionNode configurationSourcesNode
                = configurationUIHierarchy.FindNodeByType(typeof(ConfigurationSourceSectionNode)) as ConfigurationSourceSectionNode;
            if (configurationSourcesNode != null)
            {
                configurationSourcesNode.Remove();
            }
        }

        private void MergeHierarchy(IConfigurationUIHierarchy configurationHierarhcy, Dictionary<string, ConfigurationNodeMergeData> mergeDataByPath)
        {
            string rootNodeName = configurationHierarhcy.RootNode.Name;
            foreach (String configurationNodePath in mergeDataByPath.Keys)
            {
                ConfigurationNodeMergeData mergeData = mergeDataByPath[configurationNodePath];
                if (mergeData.OverrideProperties)
                {
                    ConfigurationNode node = configurationHierarhcy.FindNodeByPath(rootNodeName + configurationNodePath);
                    if (node != null)
                    {
                        PropertyDescriptorCollection propertiesOnNode = TypeDescriptor.GetProperties(node);
                        foreach (PropertyDescriptor property in propertiesOnNode)
                        {
                            object propertyValue
                                = mergeData.GetPropertyValue(property.Name, property.PropertyType, property.GetValue(node), configurationHierarhcy);
                            property.SetValue(node, propertyValue);
                        }
                    }
                }
            }
        }

        private class TemporaryConfigurationHierarchy : IDisposable
        {
            IConfigurationUIHierarchy previouslySelectedHierarchy;
            IConfigurationUIHierarchy hierarchy;
            IConfigurationUIHierarchyService hierarchyService;

            public TemporaryConfigurationHierarchy(IServiceProvider serviceProvider, string filePath)
            {
                hierarchyService = ServiceHelper.GetUIHierarchyService(serviceProvider);

                previouslySelectedHierarchy = hierarchyService.SelectedHierarchy;

                ConfigurationApplicationFile configurationFile = new ConfigurationApplicationFile(Path.GetDirectoryName(filePath), filePath);
                ConfigurationApplicationNode configurationRootNode = new ConfigurationApplicationNode(configurationFile);
                hierarchy = new ConfigurationUIHierarchy(configurationRootNode, serviceProvider);

                hierarchyService.SelectedHierarchy = hierarchy;


                SaveMergedEnvironmentCommand.UpdateConfigurationSource(hierarchy, filePath);
                hierarchy.Open();
            }

            public IConfigurationUIHierarchy Hierarchy
            {
                get { return hierarchy; }
            }

            #region IDisposable Members

            public void Dispose()
            {
                hierarchyService.SelectedHierarchy = previouslySelectedHierarchy;
                hierarchy.Dispose();
            }

            #endregion
        }
    }
}
