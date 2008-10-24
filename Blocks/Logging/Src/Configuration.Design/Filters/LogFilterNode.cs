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
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters
{
    /// <summary>
    /// Represents a <see cref="LogFilterData"/> configuration element. This class is abstract.
    /// </summary>
    [Image(typeof(LogFilterNode))]
    public abstract class LogFilterNode : ConfigurationNode
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="LogFilterNode"/> class with a name.
        /// </summary>
        /// <param name="name">
		/// The name of the node.
		/// </param>
        protected LogFilterNode(string name)
            : base(name)
        {            
        }

		/// <summary>
		/// Gets the <see cref="LogFilterData"/> this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="LogFilterData"/> this node represents.
		/// </value>
		[Browsable(false)]
		public abstract LogFilterData LogFilterData { get; }
    }
}
