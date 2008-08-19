//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design
{
    /// <summary>
    /// Respresents the designtime configuration container for validators that directly apply to the type being validated.
    /// </summary>
    [Image(typeof(SelfNode))]
    [SelectedImage(typeof(SelfNode))]
    public class SelfNode : ConfigurationNode
    {
        /// <summary>
        /// Creates an instance of <see cref="SelfNode"/>.
        /// </summary>
        public SelfNode()
            :base(Resources.SelfNodeName)
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
            get
            {
                return base.Name;
            }
        }

        /// <summary>
        /// Determines if the the child nodes are sorted by name.
        /// </summary>
        /// <value>
        /// Returns <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// Child nodes must be in order that they are added because they are handled in a chain.
        /// </remarks>
        public override bool SortChildren
        {
            get
            {
                return false;
            }
        }
    }
}
