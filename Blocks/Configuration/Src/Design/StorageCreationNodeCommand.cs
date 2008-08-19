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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Creates the list of storage commands for the nodes.
    /// </summary>
    public class StorageCreationNodeCommand : ConfigurationNodeCommand
    {

		/// <summary>
		/// Initialize a new instance of the <see cref="StorageCreationNodeCommand"/> class with an <see cref="IServiceProvider"/>.
		/// </summary>
		/// <param name="serviceProvider">
		/// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
		/// </param>
        public StorageCreationNodeCommand(IServiceProvider serviceProvider) : base(serviceProvider, true)
        {            
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="StorageCreationNodeCommand"/> class with an <see cref="IServiceProvider"/> and if the error service should be cleared after the command executes.
		/// </summary>
		/// <param name="serviceProvider">
		/// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
		/// </param>
		/// <param name="clearErrorLog">
		/// Determines if all the messages in the <see cref="IErrorLogService"/> should be cleared when the command has executed.
		/// </param>
        public StorageCreationNodeCommand(IServiceProvider serviceProvider, bool clearErrorLog) : base(serviceProvider, clearErrorLog)
        {
        }
		
        /// <summary>
        /// Creates the commands and adds them to the <see cref="IStorageService"/>.
        /// </summary>
        /// <param name="node">The node to execute the command upon.</param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
			// clear out the service first since we are going to refill it
			IStorageService storageService = ServiceHelper.GetCurrentStorageService(ServiceProvider);
			storageService.Clear();
            CreateCommands(node);            
        }

        private void CreateCommands(ConfigurationNode node)
        {
            Type t = node.GetType();
            PropertyInfo[] properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
			CreateCommandsOnNode(node, properties);
			CreateCommandsOnChildNodeProperties(node);			
        }

        private void CreateCommandsOnNode(ConfigurationNode node, PropertyInfo[] properties)
        {
			IStorageService storageService = ServiceHelper.GetCurrentStorageService(ServiceProvider);
            foreach (PropertyInfo property in properties)
            {
				StorageCreationAttribute[] storageCreationAttributes = (StorageCreationAttribute[])property.GetCustomAttributes(typeof(StorageCreationAttribute), true);
				foreach (StorageCreationAttribute storageCreationAttribute in storageCreationAttributes)
                {					
					StorageCreationCommand cmd = storageCreationAttribute.CreateCommand(node, property, ServiceProvider);
					if (!storageService.Contains(cmd.Name))
					{
						storageService.Add(cmd);	
					}
                    
                }
            }
        }

        private void CreateCommandsOnChildNodeProperties(ConfigurationNode node)
        {
            foreach (ConfigurationNode childNode in node.Nodes)
            {
				CreateCommands(childNode);
            }
        }
    }
}