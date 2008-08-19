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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters
{
    /// <summary>
	/// Represents a collection of <see cref="LogFilterData"/> configuration objects.
    /// </summary>
    [Image(typeof(LogFilterCollectionNode))]
    public sealed class LogFilterCollectionNode : ConfigurationNode
    {

        /// <summary>
        /// Initialize a new instance of the <see cref="LogFilterCollectionNode"/> class.
        /// </summary>
        public LogFilterCollectionNode()
            : base(Resources.FilterCollectionNodeName)
        {
        }

        /// <summary>
        /// Gets or sets the name of the node.
        /// </summary>
		/// <value>
		/// The name of the node.
		/// </value>
        [ReadOnly(true)]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("LogFilterCollectionNameDesciption", typeof(Resources))]
        public override string Name
        {
            get{return base.Name;}            
        }


		/// <summary>
		/// Gets if children added to the node are sorted. The children under this node are not sorted.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if nodes add are sorted; otherwise <see langword="false"/>. The default is <see langword="true"/>.
		/// </value>
        public override bool SortChildren
        {
            get{return false;}
        }        
    }
}