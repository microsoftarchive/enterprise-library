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
using System.IO;
using System.Windows.Forms;


using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Manageability
{
    /// <summary>
    /// 
    /// </summary>
    public class ExportAdmTemplateNodeCommand : ConfigurationNodeCommand
    {
        private static IDictionary<Type, ConfigurationElementManageabilityProvider> NoProviders
            = new Dictionary<Type, ConfigurationElementManageabilityProvider>(0);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ExportAdmTemplateNodeCommand(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            ManageableConfigurationSourceElementNode sourceNode = node as ManageableConfigurationSourceElementNode;
            if (sourceNode != null)
            {
                // check current configuration == selected configuration?

                // check for dirty and ask for saving
                if (UIService.IsDirty(node.Hierarchy))
                {
                    DialogResult result
                        = UIService.ShowMessage(Resources.SaveApplicationBeforeExportingAdmRequest,
                            Resources.SaveApplicationCaption,
                            MessageBoxButtons.YesNo);
                    if (DialogResult.Yes == result)
                    {
                        if (!TryAndSaveApplication(node))
                        {
                            return;
                        }
                    }
                }

                TryAndExportAdmTemplate(sourceNode);
            }
        }

        private bool TryAndSaveApplication(ConfigurationNode node)
        {
            SaveConfigurationApplicationNodeCommand cmd
                = new SaveConfigurationApplicationNodeCommand(ServiceProvider);
            cmd.Execute(node);
            return cmd.SaveSucceeded;
        }

        private static void TryAndExportAdmTemplate(ManageableConfigurationSourceElementNode sourceNode)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.Filter = Resources.AdmTemplateDialogFilter;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                IDictionary<String, ConfigurationSectionManageabilityProvider> manageabilityProviders
                    = GetManageabilityProviders(sourceNode.Site);
                try
                {
                    AdmContent content = AdministrativeTemplateGenerator.GenerateAdministrativeTemplateContent(
                        sourceNode.ConfigurationSource,
                        sourceNode.ApplicationName,
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

        private static IDictionary<String, ConfigurationSectionManageabilityProvider> GetManageabilityProviders(
            IServiceProvider serviceProvider)
        {
            IDictionary<String, ConfigurationSectionManageabilityProvider> manageabilityProviders
                = new Dictionary<String, ConfigurationSectionManageabilityProvider>();

            ConfigurationManageabilityProviderAttributeRetriever retriever
                = ConfigurationManageabilityProviderAttributeRetriever.CreateInstance(serviceProvider);

            foreach (ConfigurationSectionManageabilityProviderAttribute sectionAttribute
                in retriever.SectionManageabilityProviderAttributes)
            {
                IDictionary<Type, ConfigurationElementManageabilityProvider> subProviders
                    = new Dictionary<Type, ConfigurationElementManageabilityProvider>();

                foreach (ConfigurationElementManageabilityProviderAttribute elementAttribute
                    in retriever.ElementManageabilityProviderAttributes)
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
