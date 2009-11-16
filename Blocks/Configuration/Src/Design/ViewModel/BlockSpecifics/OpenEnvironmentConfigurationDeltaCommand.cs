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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class OpenEnvironmentConfigurationDeltaCommand : CommandModel
    {
        IUIServiceWpf uiService;
        ConfigurationSourceModel sourceModel;

        public OpenEnvironmentConfigurationDeltaCommand(IUIServiceWpf uiService, ConfigurationSourceModel sourceModel)
        {
            this.uiService = uiService;
            this.sourceModel = sourceModel;
        }

        public override void Execute(object parameter)
        {
            OpenFileDialog openEnvironmentConfigurationDeltaDialog = new OpenFileDialog
            {
                Filter = "Environment Delta Configuration Files (*.dconfig)|*.dconfig|All files (*.*)|*.*",
                Title = "Open Environment Delta File (dconfig)"
            };

            var openFileResult = uiService.ShowFileDialog(openEnvironmentConfigurationDeltaDialog);
            if (openFileResult.DialogResult != true)
            {
                return;
            }

            var configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = openFileResult.FileName }, ConfigurationUserLevel.None);
            EnvironmentMergeSection mergeSection = (EnvironmentMergeSection)configuration.GetSection(EnvironmentMergeSection.EnvironmentMergeData);
            
            sourceModel.LoadEnvironment(mergeSection, openFileResult.FileName);
        }
    }
}
