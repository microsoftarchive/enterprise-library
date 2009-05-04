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
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    /// <summary>
	/// Represents the <see cref="OracleConnectionSettings"/> configuration section.
    /// </summary>    
	[Image(typeof(OracleConnectionElementNode))]
	public sealed class OracleConnectionElementNode : ConfigurationNode
    {
		/// <summary>
		/// Initialize a new instance of the <see cref="OracleConnectionElementNode"/> class.
		/// </summary>
		public OracleConnectionElementNode()
			: base(Resources.OraclePackagesNodeName)
		{
			
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
			get
			{
				return base.Name;
			}			
		}		
	}
}
