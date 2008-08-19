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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Properties;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides
{
    class OpenEnvironmentDeltaCommand : ConfigurationNodeCommand
    {
        public OpenEnvironmentDeltaCommand(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        protected override void ExecuteCore(ConfigurationNode node)
        {
            IUIService uiService = ServiceHelper.GetUIService(ServiceProvider);

            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = Resources.EnvironmentDeltaFileDialogFilter;
                fileDialog.CheckFileExists = true;
                fileDialog.CheckPathExists = true;
                fileDialog.AddExtension = true;
                fileDialog.DefaultExt = Resources.DefaultEnvironmentDeltaFileExtension;
                fileDialog.RestoreDirectory = true;

                if (DialogResult.OK == uiService.ShowOpenDialog(fileDialog))
                {

                    uiService.BeginUpdate();

                    try
                    {
                        EnvironmentNodeBuilder nodeBuilder = new EnvironmentNodeBuilder(ServiceProvider);
                        EnvironmentNode childNode = nodeBuilder.Build(fileDialog.FileName, node.Hierarchy);

                        node.AddNode(childNode);
                        uiService.SetUIDirty(node.Hierarchy);
                        uiService.ActivateNode(childNode);
                    }
                    catch (Exception e)
                    {
                        uiService.ShowError(e, Resources.ErrorOpeningEnvironmentMergeFile);
                    }
                    finally
                    {
                        uiService.EndUpdate();
                    }
                }
            }
        }

       
    }
}
