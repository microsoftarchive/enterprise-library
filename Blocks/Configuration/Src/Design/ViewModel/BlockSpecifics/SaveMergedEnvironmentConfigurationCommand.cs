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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Win32;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class SaveMergedEnvironmentConfigurationCommand : CommandModel
    {
        IUIServiceWpf uiService;
        IApplicationModel application;
        EnvironmentalOverridesViewModel overridesViewModel;

        public SaveMergedEnvironmentConfigurationCommand(IUIServiceWpf uiService, IApplicationModel application, ElementViewModel overridesViewModel)
        {
            this.uiService = uiService;
            this.application = application;
            this.overridesViewModel = (EnvironmentalOverridesViewModel)overridesViewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override string Title
        {
            get
            {
                return "Export Merged Environment Configuration File";
            }
        }

        public override void Execute(object parameter)
        {
            if (application.IsDirty)
            {
                var saveDialogResult = uiService.ShowMessageWpf("In order to save the merged environment configuration, the main configuration must be saved. Would you like to proceed and save the connfiguration, prior to saving a merged configuration?", "Save Environment Configuration", System.Windows.MessageBoxButton.OKCancel);
                if (saveDialogResult == System.Windows.MessageBoxResult.Cancel)
                {
                    return;
                }
                if (!application.Save())
                {
                    return;
                }
            }

            string mergedConfigurationFile = overridesViewModel.Property("EnvironmentConfigurationFile").Value as string;
            if (string.IsNullOrEmpty(mergedConfigurationFile))
            {
                SaveFileDialog saveEnvrionmentDialog = new SaveFileDialog();
                saveEnvrionmentDialog.Title = "Save merged configuration";
                var saveFileResults = uiService.ShowFileDialog(saveEnvrionmentDialog);
                if (saveFileResults.DialogResult != true)
                {
                    return;
                }
                overridesViewModel.Property("EnvironmentConfigurationFile").Value = mergedConfigurationFile = saveFileResults.FileName;
            }

            if (!Path.IsPathRooted(mergedConfigurationFile))
            {
                string configurationFileDirectory = Path.GetDirectoryName(application.ConfigurationFilePath);
                mergedConfigurationFile = Path.Combine(configurationFileDirectory, mergedConfigurationFile);
            }

            ConfigurationMerger mergeComponent = new ConfigurationMerger();
            mergeComponent.MergeConfiguration(application.ConfigurationFilePath, (EnvironmentMergeSection)overridesViewModel.ConfigurationElement, mergedConfigurationFile);
        }
    }
}
