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
using System.Configuration;
using System.Globalization;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.EnvironmentalOverrides;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using Microsoft.Win32;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{

#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class OpenEnvironmentConfigurationDeltaCommand : CommandModel
    {
        readonly ApplicationViewModel sourceModel;
        private readonly bool canExecute;

        public OpenEnvironmentConfigurationDeltaCommand(IUIServiceWpf uiService, ApplicationViewModel sourceModel, bool canExecute)
            :base(uiService)
        {
            this.sourceModel = sourceModel;
            this.canExecute = canExecute;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected override void InnerExecute(object parameter)
        {

            OpenFileDialog openEnvironmentConfigurationDeltaDialog = new OpenFileDialog
                                                                         {
                                                                             Filter = DesignResources.DeltaDialogFilter,
                                                                             Title = DesignResources.OpenDeltaDialogTitle
                                                                         };

            var openFileResult = UIService.ShowFileDialog(openEnvironmentConfigurationDeltaDialog);
            if (openFileResult.DialogResult != true)
            {
                return;
            }

            try
            {
                var configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = openFileResult.FileName }, ConfigurationUserLevel.None);
                EnvironmentalOverridesSection mergeSection = (EnvironmentalOverridesSection)configuration.GetSection(EnvironmentalOverridesSection.EnvironmentallyOverriddenProperties);

                sourceModel.LoadEnvironment(mergeSection, openFileResult.FileName);
            }
            catch (Exception ex)
            {
                ConfigurationLogWriter.LogException(ex);
                UIService.ShowMessageWpf(string.Format(CultureInfo.CurrentCulture, Resources.ErrorLoadingDeltaFile, ex.Message),
                                         Resources.ErrorTitle,
                                         MessageBoxButton.OK);
            }
        }

        protected override bool InnerCanExecute(object parameter)
        {
            return canExecute;
        }
    }

#pragma warning restore 1591
}
