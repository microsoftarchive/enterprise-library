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
using System.IO;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Represents a command that will open an application configuration.
    /// </summary>
    public class OpenConfigurationApplicationNodeCommand : ConfigurationNodeCommand
    {
        /// <summary>
		/// Initialize a new instance of the <see cref="OpenConfigurationApplicationNodeCommand"/> class with an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        public OpenConfigurationApplicationNodeCommand(IServiceProvider serviceProvider) : this(serviceProvider, true)
        {
        }

        /// <summary>
		/// Initialize a new instance of the <see cref="OpenConfigurationApplicationNodeCommand"/> class with an <see cref="IServiceProvider"/> and if the error service should be cleared after the command executes.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        /// <param name="clearErrorLog">
        /// Determines if all the messages in the <see cref="IErrorLogService"/> should be cleared when the command has executed.
        /// </param>
        public OpenConfigurationApplicationNodeCommand(IServiceProvider serviceProvider, bool clearErrorLog) : base(serviceProvider, clearErrorLog)
        {
        }

        /// <summary>
        /// Opens a previously saved configuration.
        /// </summary>
        /// <param name="node">
        /// The node to execute the command upon.
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            try
            {
                UIService.BeginUpdate();
                OpenFile();
                if (ErrorLogService.ConfigurationErrorCount > 0)
                {
                    UIService.DisplayErrorLog(ErrorLogService);
                    UIService.ShowMessage(Resources.OpenApplicationErrorMessage, Resources.OpenApplicationCaption);
                }
            }
            finally
            {
                UIService.EndUpdate();
            }
        }

        private void OpenFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = Resources.ConfigurationFileDialogFilter;
            fileDialog.CheckFileExists = true;
            fileDialog.CheckPathExists = true;
            fileDialog.AddExtension = true;
            fileDialog.DefaultExt = ".config";
            fileDialog.RestoreDirectory = true;
            DialogResult result = UIService.ShowOpenDialog(fileDialog);

            if (result == DialogResult.OK)
            {
                string file = fileDialog.FileName;
				ConfigurationApplicationFile data = new ConfigurationApplicationFile(Path.GetDirectoryName(file), file);
				IConfigurationUIHierarchy hierarchy = new ConfigurationUIHierarchy(new ConfigurationApplicationNode(data), ServiceProvider);
                ConfigurationUIHierarchyService.AddHierarchy(hierarchy);
                hierarchy.Open();
            }
        }
    }
}