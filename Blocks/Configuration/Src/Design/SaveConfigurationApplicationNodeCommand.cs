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
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Represents a command that will save an application configuration.
    /// </summary>
    public class SaveConfigurationApplicationNodeCommand : ConfigurationNodeCommand
    {
        private bool saveSucceeded;

        /// <summary>
		/// Initialize a new instance of the <see cref="SaveConfigurationApplicationNodeCommand"/> class with an <see cref="System.IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        public SaveConfigurationApplicationNodeCommand(IServiceProvider serviceProvider) : this(serviceProvider, true)
        {
        }

        /// <summary>
		/// Initialize a new instance of the <see cref="SaveConfigurationApplicationNodeCommand"/> class with an <see cref="System.IServiceProvider"/> and if the error service should be cleared after the command executes.
        /// </summary>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
        /// <param name="clearErrorLog">
        /// Determines if all the messages in the <see cref="IErrorLogService"/> should be cleared when the command has executed.
        /// </param>
        public SaveConfigurationApplicationNodeCommand(IServiceProvider serviceProvider, bool clearErrorLog) : base(serviceProvider, clearErrorLog)
        {
        }

        /// <summary>
        /// Saves the application configuration.
        /// </summary>
        /// <param name="node">
        /// The node to execute the command upon.
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
        {            
            if (!DoValidationCommand())
            {
                return;
            }
            DoApplicationSave();
        }

        /// <summary>
        /// Determines if the saving of the application was suseccesful.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the saving of the application was suseccesful; otherwise, <see langword="false"/>.
        /// </value>
        public bool SaveSucceeded
        {
            get { return saveSucceeded; }
        }

        private bool DoValidationCommand()
        {
            using(ValidateNodeCommand cmd = new ValidateNodeCommand(ServiceProvider, false, false))
            {
                cmd.Execute(ServiceHelper.GetCurrentRootNode(ServiceProvider));
                if (!cmd.ValidationSucceeded)
                {
                    UIService.ShowMessage(Resources.ValidationErrorsMessage, Resources.SaveApplicationCaption, MessageBoxButtons.OK);
                    saveSucceeded = false;
                    return false;
                }
                return true;
            }
        }

        private void DoApplicationSave()
        {
            // we need to create the meta data for everybody first
            if (!CreateConfigurationFile()) return;			
            CreateStorageEntries();
            if (!CheckAndDisplayErrors())
            {
                return;
            }
            SaveHierarchy();
        }

        private bool CreateConfigurationFile()
        {
			// set the configuration file here for the service
			IStorageService service = ServiceHelper.GetCurrentStorageService(ServiceProvider);
			service.ConfigurationFile = ServiceHelper.GetCurrentRootNode(ServiceProvider).ConfigurationFile;
			ConfigurationFileStorageCreationCommand cmd = new ConfigurationFileStorageCreationCommand(service.ConfigurationFile, ServiceProvider);
			cmd.Execute();
			if (cmd.CreationCancled)
			{
				return false;
			}
            return true;
        }

        private void SaveHierarchy()
        {
            CurrentHierarchy.Save();
            CheckAndDisplayErrors();
        }

        private bool CheckAndDisplayErrors()
        {
            saveSucceeded = true;
            if (ErrorLogService.ConfigurationErrorCount > 0)
            {
                UIService.DisplayErrorLog(ErrorLogService);
                UIService.ShowError(Resources.SaveApplicationErrorMessage, Resources.SaveApplicationCaption);
                saveSucceeded = false;
            }
            return saveSucceeded;
        }

		private void CreateStorageEntries()
		{
			using (StorageCreationNodeCommand cmd = new StorageCreationNodeCommand(ServiceProvider))
			{
				cmd.Execute(ServiceHelper.GetCurrentRootNode(ServiceProvider));
			}			
			IStorageService storage = CurrentHierarchy.StorageService;
			storage.ForEach(new Action<StorageCreationCommand>(ExecuteStorageCommand));
		}

		private void ExecuteStorageCommand(StorageCreationCommand cmd)
		{
			cmd.Execute();
		}
    }
}
