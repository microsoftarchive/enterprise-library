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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources
{
    /// <summary>
	/// Represents is the group for error, not processed, and all events.
    /// </summary>
    [Image(typeof(SpecialTraceSourcesNode))]
    public sealed class SpecialTraceSourcesNode : ConfigurationNode
    {
		/// <summary>
        /// Initialize a new instance of the <see cref="SpecialTraceSourcesNode"/> class.
        /// </summary>
        public SpecialTraceSourcesNode()
            : base(Resources.SpecialTraceSourcesNodeName)
        {
        }

		/// <summary>
		/// Determines if the child nodes are sorted.  These nodes are not sorted.
		/// </summary>
		/// <value>
		/// <see langword="true"/> if the children is sorted; otherwise <see langword="false"/>.
		/// </value>
		[Browsable(false)]
		public override bool SortChildren
		{
			get { return false; }
		}

		/// <summary>
		/// Gets the name of the node.
		/// </summary>
		/// <value>
		/// The name of the node.
		/// </value>
		[ReadOnly(true)]
		public override string Name
		{
			get { return base.Name; }			
		}        
    }
}