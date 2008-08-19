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
using System.Configuration;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	   
	/// <summary>
	/// Represents a <see cref="DatabaseSettings"/> configuration section.
	/// </summary>
    [Image(typeof(DatabaseSectionNode))]
    public sealed class DatabaseSectionNode : ConfigurationSectionNode
    {        
		private ConnectionStringSettingsNode connectionStringSettingsNode;
		private EventHandler<ConfigurationNodeChangedEventArgs> connectionStringNodeRemovedHandler;		        

        /// <summary>
        /// Initialize a new instance of the <see cref="DatabaseSectionNode"/> class.
        /// </summary>
		public DatabaseSectionNode()
			: base(Resources.DataUICommandText)
        {
			this.connectionStringNodeRemovedHandler = new EventHandler<ConfigurationNodeChangedEventArgs>(OnConnectionStringNodeRemoved);            
        }

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="DatabaseSectionNode"/> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
		/// </param>
		protected override void Dispose(bool disposing)
		{			
			if (disposing)
			{
				if (connectionStringSettingsNode != null)
				{
					connectionStringSettingsNode.Removed -= new EventHandler<ConfigurationNodeChangedEventArgs>(OnConnectionStringNodeRemoved);				
				}				
			}
			base.Dispose(disposing);
		}

        /// <summary>
        /// Gets the name of the node.
        /// </summary>
		/// <value>
		/// The name of the node.
		/// </value>
		/// <remarks>
		/// Overridden to make readonly for the design tool. 
		/// </remarks>
        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }            
        }

		/// <summary>
		/// Gets or sets the default database connection to use when none is specified.
		/// </summary>
		/// <value>
		/// The default database connection to use when none is specified.
		/// </value>
		[Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
		[ReferenceType(typeof(ConnectionStringSettingsNode))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("DefaultDatabaseDescription", typeof(Resources))]
		public ConnectionStringSettingsNode DefaultDatabase
		{
			get { return connectionStringSettingsNode; }
			set
			{
				connectionStringSettingsNode = LinkNodeHelper.CreateReference<ConnectionStringSettingsNode>(connectionStringSettingsNode,
																								 value,
																								 connectionStringNodeRemovedHandler,
																								 null);				
			}
		}        
		
        private void OnConnectionStringNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
			connectionStringSettingsNode = null;            
        }        
    }
}