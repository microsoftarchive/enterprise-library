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
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    /// <summary>
    /// Represents a collection of <see cref="DbProviderMapping"/> configuration elements.
    /// </summary>
    [Image(typeof(ProviderMappingsNode))]
    public sealed class ProviderMappingsNode : ConfigurationNode
    {	

		/// <summary>
		/// Initialize a new instance of the <see cref="ProviderMappingsNode"/> class.
		/// </summary>
		public ProviderMappingsNode()
			: base(Resources.ProviderMappingsNodeName)
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
            get { return base.Name; }
        }        
    }
}