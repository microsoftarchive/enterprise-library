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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters
{
    /// <summary>
    /// Represents a collection of <see cref="FormatterData"/> configuration elements.
    /// </summary>
    [Image(typeof (FormatterCollectionNode))]
    public sealed class FormatterCollectionNode : ConfigurationNode
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="FormatterCollectionNode"/> class.
        /// </summary>
        public FormatterCollectionNode()
            : base(Resources.Formatters)
        {            
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
