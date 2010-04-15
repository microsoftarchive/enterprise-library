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

using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.EnvironmentalOverrides;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using Microsoft.Win32;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Windows;
using System.Globalization;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class SaveMergedEnvironmentConfigurationCommand : CommandModel
    {
        readonly IApplicationModel application;
        readonly EnvironmentSourceViewModel overridesViewModel;
        readonly ConfigurationSourceModel configurationSource;

        public SaveMergedEnvironmentConfigurationCommand(IUIServiceWpf uiService, ConfigurationSourceModel configurationSource, IApplicationModel application, ElementViewModel overridesViewModel)
            : base(uiService)
        {
            this.configurationSource = configurationSource;
            this.application = application;
            this.overridesViewModel = (EnvironmentSourceViewModel)overridesViewModel;
        }

        protected override bool InnerCanExecute(object parameter)
        {
            return true;
        }

        public override string Title
        {
            get
            {
                return DesignResources.ExportMergedEnvironmentTitle;
            }
        }

        protected override void InnerExecute(object parameter)
        {
            var configurationSourceSection = configurationSource.Sections.OfType<ConfigurationSourceSectionViewModel>().FirstOrDefault();
            if (configurationSourceSection != null)
            {
                ElementReferenceProperty selectedSourceProperty = (ElementReferenceProperty) configurationSourceSection.Property("SelectedSource");

                if (typeof(SystemConfigurationSourceElement) != selectedSourceProperty.ReferencedElement.ConfigurationType)
                {
                    UIService.ShowMessageWpf(DesignResources.ExportingEnvironemntConfigurationUsingNonSystemSource, DesignResources.ExportMergedEnvironmentTitle, MessageBoxButton.OK);
                    return;
                }
            }
            if (application.IsDirty)
            {
                var saveDialogResult = UIService.ShowMessageWpf(
                    DesignResources.ExportMergedUnsavedMainConfigurationMessage,
                    DesignResources.ExportMergedConfigurationTitle,
                    System.Windows.MessageBoxButton.OKCancel);

                if (saveDialogResult == System.Windows.MessageBoxResult.Cancel)
                {
                    return;
                }
                if (!application.Save())
                {
                    return;
                }
            }

            string mergedConfigurationFile = (string)overridesViewModel.Property("EnvironmentConfigurationFile").Value;

            if (string.IsNullOrEmpty(mergedConfigurationFile))
            {
                SaveFileDialog saveEnvrionmentDialog = new SaveFileDialog()
                    {
                        Title = DesignResources.ExportMergedConfigurationTitle,
                        DefaultExt = "*.config",
                        Filter = Resources.SaveConfigurationFileDialogFilter,
                        FilterIndex = 0
                    };

                var saveFileResults = UIService.ShowFileDialog(saveEnvrionmentDialog);
                if (saveFileResults.DialogResult != true)
                {
                    return;
                }
                mergedConfigurationFile = saveFileResults.FileName;
            }
            else
            {
                if (File.Exists(mergedConfigurationFile))
                {
                    var confirmationResult = UIService.ShowMessageWpf(
                        string.Format(CultureInfo.CurrentCulture, DesignResources.SaveOverwriteMergedFile, mergedConfigurationFile),
                        DesignResources.ExportMergedConfigurationTitle, MessageBoxButton.YesNo);

                    if (confirmationResult == MessageBoxResult.No)
                    {
                        return;
                    }
                }
            }

            if (!Path.IsPathRooted(mergedConfigurationFile))
            {
                string configurationFileDirectory = Path.GetDirectoryName(application.ConfigurationFilePath);
                mergedConfigurationFile = Path.Combine(configurationFileDirectory, mergedConfigurationFile);
            }

            ConfigurationMerger mergeComponent = new ConfigurationMerger(application.ConfigurationFilePath, (EnvironmentalOverridesSection)overridesViewModel.ConfigurationElement);
            mergeComponent.MergeConfiguration(mergedConfigurationFile);

            overridesViewModel.Property("EnvironmentConfigurationFile").Value = mergedConfigurationFile;
        }
    }
#pragma warning restore 1591
}
