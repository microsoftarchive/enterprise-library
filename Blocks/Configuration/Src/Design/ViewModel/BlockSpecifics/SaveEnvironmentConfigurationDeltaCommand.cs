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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using System.IO;


namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class SaveEnvironmentConfigurationDeltaCommand : CommandModel
    {
        IUIServiceWpf uiService;
        EnvironmentalOverridesViewModel overridesViewModel;
        IApplicationModel applictionModel;
        
        public SaveEnvironmentConfigurationDeltaCommand(IUIServiceWpf uiService, IApplicationModel applictionModel, ElementViewModel overridesViewModel)
        {
            this.applictionModel = applictionModel;
            this.uiService = uiService;
            this.overridesViewModel = (EnvironmentalOverridesViewModel) overridesViewModel;
        }
        
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override string Title
        {
            get
            {
                return "Save Environment Delta File";
            }
        }

        public override void Execute(object parameter)
        {
            if (String.IsNullOrEmpty(overridesViewModel.EnvironmentDeltaFile))
            {
                SaveFileDialog saveEnvironmentDeltaDialog = new SaveFileDialog()
                {
                    Filter = "Environment Delta Configuration Files (*.dconfig)|*.dconfig|All files (*.*)|*.*",
                    Title = "Save Environment Delta File (dconfig)"
                };

                var dialogResult = uiService.ShowFileDialog(saveEnvironmentDeltaDialog);
                if (dialogResult.DialogResult != true)
                {
                    return;
                }
                overridesViewModel.EnvironmentDeltaFile = dialogResult.FileName;
            }

            if (applictionModel.EnsureCanSaveConfigurationFile(overridesViewModel.EnvironmentDeltaFile))
            {
                DesignConfigurationSource source = new DesignConfigurationSource(overridesViewModel.EnvironmentDeltaFile);
                overridesViewModel.Save(source);
            }
        }
    }
}
