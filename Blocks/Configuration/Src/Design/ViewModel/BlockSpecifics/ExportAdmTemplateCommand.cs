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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Console;
using System.Windows.Forms.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class ExportAdmTemplateCommand : CommandModel
    {
        private static IDictionary<Type, ConfigurationElementManageabilityProvider> NoProviders
           = new Dictionary<Type, ConfigurationElementManageabilityProvider>(0);


        ApplicationViewModel applicationViewModel;
        IUIService uiService;
        ElementViewModel element;
        ConfigurationManageabilityProviderAttributeRetriever attributeRetriever;

        public ExportAdmTemplateCommand(CommandAttribute commandAttribute, 
                                        ApplicationViewModel applicationViewModel, 
                                        IUIService uiService, 
                                        ElementViewModel element,
                                        AssemblyLocator assemblyLocator)
            :base(commandAttribute)
        {
            this.applicationViewModel = applicationViewModel;
            this.uiService = uiService;
            this.element = element;
            this.attributeRetriever = new ConfigurationManageabilityProviderAttributeRetriever(assemblyLocator);
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override string Title
        {
            get{ return Resources.GenerateAdmTemplateCommandText; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Execute(object parameter)
        {
            // check for dirty and ask for saving
            if (applicationViewModel.IsDirty)
            {
                DialogResult result
                    = uiService.ShowMessage(Resources.SaveApplicationBeforeExportingAdmRequest,
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
                    AdmContent content = AdministrativeTemplateGenerator.GenerateAdministrativeTemplateContent(
                        new FileConfigurationSource(applicationViewModel.ConfigurationFilePath),
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
                catch (Exception e)
                {
                    MessageBox.Show(String.Format(Resources.Culture, Resources.ErrorGeneratingAdmFile, e.Message),
                        Resources.AdmGenerationDialogErrorTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
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
    }
}
