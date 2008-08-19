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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners
{
    /// <summary>
	/// Represents a collection of <see cref="TraceListenerData"/> configuration elements.
    /// </summary>
    [Image(typeof(TraceListenerCollectionNode))]
    public class TraceListenerCollectionNode : ConfigurationNode
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="TraceListenerCollectionNode"/> class.
        /// </summary>
        public TraceListenerCollectionNode()
            : base(Resources.TraceListenerCollectionNode)
        {
        }

        /// <summary>
        /// Gets the name of the node.
        /// </summary>
		/// <value>
		/// The name of the node.
		/// </value>
        [ReadOnly(true)]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("LogFilterCollectionNameDesciption", typeof(Resources))]
        public override string Name
        {
            get { return base.Name; }            
        }
    }
}