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
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources
{
    /// <summary>
    /// A collection of trace source nodes.
    /// </summary>
    [Image(typeof(CategoryTraceSourceCollectionNode))]
    public sealed class CategoryTraceSourceCollectionNode : ConfigurationNode
    {
		/// <summary>
        /// Initialize a new instance of the <see cref="CategoryTraceSourceCollectionNode"/> class.
        /// </summary>        
        public CategoryTraceSourceCollectionNode()
            : base(Resources.CategorySources)
        {
        }
		        
        /// <summary>
        /// Gets the name of the node.
        /// </summary>
		/// <value>
		/// The name of the node.
		/// </value>
        [ReadOnly(false)]
        public override string Name
        {
            get{return base.Name;}            
        }
    }
}
