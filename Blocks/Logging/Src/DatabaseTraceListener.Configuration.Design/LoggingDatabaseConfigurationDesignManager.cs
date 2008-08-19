//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design
{
	/// <summary>
	/// Represents the design manager for the database trace listener.
	/// </summary>
    public sealed class LoggingDatabaseConfigurationDesignManager : ConfigurationDesignManager
    {
		/// <summary>
		/// Initialize a new instance of the <see cref="LoggingDatabaseConfigurationDesignManager"/> class.
		/// </summary>
		public LoggingDatabaseConfigurationDesignManager()
        {
        }

		/// <summary>
		/// Register the commands and node maps needed for the design manager into the design time.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		public override void Register(IServiceProvider serviceProvider)
        {
			LoggingDatabaseCommandRegistrar cmdRegistrar = new LoggingDatabaseCommandRegistrar(serviceProvider);
			cmdRegistrar.Register();
			LoggingDatabaseNodeMapRegistrar nodeRegistrar = new LoggingDatabaseNodeMapRegistrar(serviceProvider);
			nodeRegistrar.Register();
        }

		/// <summary>
		/// Initializes the database listener and adds it to the logging settings.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <param name="rootNode">The root node of the application.</param>
		/// <param name="section">The <see cref="ConfigurationSection"/> that was opened from the <see cref="IConfigurationSource"/>.</param>
		protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, ConfigurationSection section)
        {
			IConfigurationUIHierarchy hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
			foreach (LoggingDatabaseNode loggingDatabaseNode in hierarchy.FindNodesByType(typeof(LoggingDatabaseNode)))
			{
				foreach (ConnectionStringSettingsNode connectionStringNode in hierarchy.FindNodesByType(typeof(ConnectionStringSettingsNode)))
				{
					if (connectionStringNode.Name == ((FormattedDatabaseTraceListenerData)loggingDatabaseNode.TraceListenerData).DatabaseInstanceName)
					{
						loggingDatabaseNode.DatabaseInstance = connectionStringNode;
						break;
					}
				}
			}			
        }		
    }
}