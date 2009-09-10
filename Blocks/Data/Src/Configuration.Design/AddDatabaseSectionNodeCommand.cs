//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
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
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	/// <summary>
	/// Command to add the Data Application Block to the application's configuration.
	/// </summary>
	public class AddDatabaseSectionNodeCommand : AddChildNodeCommand
	{
		private bool addDefaultConnectionString;

		/// <summary>
		/// Initialize a new instance of the <see cref="AddDatabaseSectionNodeCommand"/> class with an <see cref="IServiceProvider"/>.
		/// </summary>
		/// <param name="serviceProvider">
		/// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
		/// </param>
		public AddDatabaseSectionNodeCommand(IServiceProvider serviceProvider)
			: this(serviceProvider, true)
        {
        }

		/// <summary>
		/// Initialize a new instance of the <see cref="AddDatabaseSectionNodeCommand"/> class with an <see cref="IServiceProvider"/> and if the default connection string should be added.
		/// </summary>
		/// <param name="serviceProvider">
		/// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
		/// </param>
		/// <param name="addDefaultConnectionString">Determines if the default connection string should be added.</param>
		public AddDatabaseSectionNodeCommand(IServiceProvider serviceProvider, bool addDefaultConnectionString)
			: base(serviceProvider, typeof(DatabaseSectionNode))
		{
			this.addDefaultConnectionString = addDefaultConnectionString;
		}

		/// <summary>
        /// Add the Data Access Application Block to the current configuration 
		/// </summary>
		/// <param name="e">The <see cref="CommandExecutingEventArgs"/> containing the event data.</param>
		protected override void OnExecuted(EventArgs e)
		{
			DatabaseSectionNode node = ChildNode as DatabaseSectionNode;
			Debug.Assert(null != node, "Expected DatabaseSectionNode");
			if (addDefaultConnectionString)
			{
				new AddConnectionStringsSectionNodeCommand(ServiceProvider).Execute(node);				
				ConnectionStringSettingsNode defaultDatabaseNode = (ConnectionStringSettingsNode)CurrentHierarchy.FindNodeByType(node, typeof(ConnectionStringSettingsNode));
				node.DefaultDatabase = defaultDatabaseNode;
			}
			node.AddNode(new ProviderMappingsNode());
		}		
	}
}
