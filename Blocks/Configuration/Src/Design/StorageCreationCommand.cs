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
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Creates the physical storage for configuration.
    /// </summary>
	public abstract class StorageCreationCommand
	{
	    private readonly IServiceProvider serviceProvider;
	    private readonly string name;

        /// <summary>
        /// Initialize a new instance of the <see cref="StorageCreationCommand"/> class with a name and <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="name">The name of the storage to create.</param>
        /// <param name="serviceProvider">
        /// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
        /// </param>
	    protected StorageCreationCommand(string name, IServiceProvider serviceProvider)
		{
            if (null == serviceProvider) throw new ArgumentNullException("serviceProvider");        

	        this.name = name;
	        this.serviceProvider = serviceProvider;
		}

        /// <summary>
        /// Gets the name of the storage to create.
        /// </summary>
        /// <value>
        /// The name of the storage to create.
        /// </value>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets or sets the <see cref="IServiceProvider"/> for the command.
        /// </summary>
        /// <value>
        /// The <see cref="IServiceProvider"/> for the command.
        /// </value>
	    protected IServiceProvider ServiceProvider
	    {
	        get { return serviceProvider; }
	    }

        /// <summary>
        /// When overriden by a class, executes the command to create the storage.
        /// </summary>
        /// <value>
        /// Executes the command to create the storage.
        /// </value>
        public abstract void Execute();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		protected void LogError(string message)
		{
			IConfigurationUIHierarchyService uiHierarchyService = ServiceHelper.GetUIHierarchyService(ServiceProvider);
			ServiceHelper.LogError(ServiceProvider, new ConfigurationError(uiHierarchyService.SelectedHierarchy.RootNode, message));
		}
	}
}
