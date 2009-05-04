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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    /// <summary>
    /// Represents the connection strings section in configuration.
    /// </summary>
    [Image(typeof(ConnectionStringsSectionNode))]
    public sealed class ConnectionStringsSectionNode : ConfigurationNode
    {
		/// <summary>
		/// Initialize a new instance of the <see cref="ConnectionStringsSectionNode"/> class.
		/// </summary>
		public ConnectionStringsSectionNode()
			: base(Resources.ConnectionStringsNodeName)
		{

		}	

        /// <summary>
        /// <para>Gets the name for the node.</para>
        /// </summary>
        /// <value>
        /// <para>The display name for the node.</para>
        /// </value>
        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }            
        }		
    }
}
