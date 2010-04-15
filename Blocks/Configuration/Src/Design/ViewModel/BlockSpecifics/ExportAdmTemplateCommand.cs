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
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
#pragma warning disable 1591

    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class ExportAdmTemplateCommand : CommandModel
    {
        ApplicationViewModel applicationViewModel;
        ElementViewModel element;
        ConfigurationManageabilityProviderAttributeRetriever attributeRetriever;

        public ExportAdmTemplateCommand(CommandAttribute commandAttribute,
                                        ApplicationViewModel applicationViewModel,
                                        IUIServiceWpf uiService,
                                        ElementViewModel element,
                                        AssemblyLocator assemblyLocator)
            : base(commandAttribute, uiService)
        {
            this.applicationViewModel = applicationViewModel;
            this.element = element;
            this.attributeRetriever = new ConfigurationManageabilityProviderAttributeRetriever(assemblyLocator);
        }

        protected override bool InnerCanExecute(object parameter)
        {
            return true;
        }

        public override string Title
        {
            get { return Resources.GenerateAdmTemplateCommandText; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void InnerExecute(object parameter)
        {
            // check for dirty and ask for saving
            if (applicationViewModel.IsDirty)
            {
                DialogResult result
                    = UIService.ShowMessage(Resources.SaveApplicationBeforeExportingAdmRequest,
                        Resources.SaveApplicationCaption,
                        MessageBoxButtons.YesNo);
                if (DialogResult.Yes == result)
                {
                    if (!applicationViewModel.Save())
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            TryAndExportAdmTemplate();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void TryAndExportAdmTemplate()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.Filter = Resources.AdmTemplateDialogFilter;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                IDictionary<String, ConfigurationSectionManageabilityProvider> manageabilityProviders
                    = GetManageabilityProviders();
                try
                {
                    using (var configurationSource = new FileConfigurationSource(this.ConfigurationFilePath, false))
                    {
                        AdmContent content = AdministrativeTemplateGenerator.GenerateAdministrativeTemplateContent(
                            configurationSource,
                            element.Property("ApplicationName").Value as string,
                            manageabilityProviders);

                        using (StreamWriter fileWriter = new StreamWriter(dialog.OpenFile()))
                        {
                            fileWriter.WriteLine("CLASS MACHINE");
                            content.Write(fileWriter);
                            fileWriter.WriteLine("CLASS USER");
                            content.Write(fileWriter);
                            fileWriter.Flush();
                            fileWriter.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(String.Format(CultureInfo.CurrentCulture, Resources.ErrorGeneratingAdmFile, e.Message),
                        Resources.AdmGenerationDialogErrorTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button1);
                }
            }
        }

        private IDictionary<String, ConfigurationSectionManageabilityProvider> GetManageabilityProviders()
        {
            IDictionary<String, ConfigurationSectionManageabilityProvider> manageabilityProviders
                = new Dictionary<String, ConfigurationSectionManageabilityProvider>();

            foreach (ConfigurationSectionManageabilityProviderAttribute sectionAttribute
                in attributeRetriever.SectionManageabilityProviderAttributes)
            {
                IDictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                    = new Dictionary<Type, ConfigurationElementManageabilityProvider>();

                foreach (ConfigurationElementManageabilityProviderAttribute elementAttribute
                    in attributeRetriever.ElementManageabilityProviderAttributes)
                {
                    if (elementAttribute.SectionManageabilityProviderType
                        == sectionAttribute.ManageabilityProviderType)
                    {
                        subProviders.Add(elementAttribute.TargetType,
                            (ConfigurationElementManageabilityProvider)Activator.CreateInstance(elementAttribute.ManageabilityProviderType));
                    }
                }

                manageabilityProviders.Add(sectionAttribute.SectionName,
                    (ConfigurationSectionManageabilityProvider)Activator.CreateInstance(sectionAttribute.ManageabilityProviderType, subProviders));
            }

            return manageabilityProviders;
        }

        private string ConfigurationFilePath
        {
            get
            {
                var manageableConfigurationSourceElement =
                    this.element.ConfigurationElement as ManageableConfigurationSourceElement;
                return manageableConfigurationSourceElement != null
                    ? manageableConfigurationSourceElement.FilePath
                    : this.applicationViewModel.ConfigurationFilePath;
            }
        }
    }
#pragma warning restore 1591
}
