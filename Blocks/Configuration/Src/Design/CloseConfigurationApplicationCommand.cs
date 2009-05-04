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
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Represents a command that will close an <see cref="ConfigurationApplicationNode"/> and remove the <see cref="IConfigurationUIHierarchy"/> that it represents.
    /// </summary>
    public class CloseConfigurationApplicationCommand : ConfigurationNodeCommand
    {
        /// <summary>
		/// Initialize a new instance of the <see cref="CloseConfigurationApplicationCommand"/> class with an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        public CloseConfigurationApplicationCommand(IServiceProvider serviceProvider) : this(serviceProvider, true)
        {
        }

        /// <summary>
		/// Initialize a new instance of the <see cref="CloseConfigurationApplicationCommand"/> class with an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider to get service objects.
        /// </param>
        /// <param name="clearErrorService">
        /// Determines if the <see cref="IErrorLogService"/> when the command has executed.
        /// </param>
        public CloseConfigurationApplicationCommand(IServiceProvider serviceProvider, bool clearErrorService) : base(serviceProvider, clearErrorService)
        {
        }

        /// <summary>
        /// Closes the application configuration.
        /// </summary>
        /// <param name="node">
        /// The node to execute the command upon.
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            try
            {
                UIService.BeginUpdate();
                
				if (UIService.IsDirty(node.Hierarchy))
                {
                    DialogResult result = UIService.ShowMessage(Resources.SaveApplicationRequest, Resources.SaveApplicationCaption, MessageBoxButtons.YesNo);
                    if (DialogResult.Yes == result)
                    {
                        if (!TryAndSaveApplication(node))
                        {
                            return;
                        }
                    }
                }
                if (ErrorLogService.ConfigurationErrorCount > 0)
                {
                    UIService.DisplayErrorLog(ErrorLogService);
                    DialogResult result = UIService.ShowMessage(Resources.SaveApplicationErrorRequestMessage, Resources.SaveApplicationCaption, MessageBoxButtons.YesNo);
                    if (result == DialogResult.No) return;
                }
                ConfigurationUIHierarchyService.RemoveHierarchy(node.Hierarchy.Id);
            }
            finally
            {
                UIService.EndUpdate();
            }
        }

        private bool TryAndSaveApplication(ConfigurationNode node)
        {
			SaveConfigurationApplicationNodeCommand cmd = new SaveConfigurationApplicationNodeCommand(ServiceProvider);
            cmd.Execute(node);
            return cmd.SaveSucceeded;
        }
    }
}
