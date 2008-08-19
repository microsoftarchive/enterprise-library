//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
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
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Properties;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design
{
    /// <summary>
    /// Represents a collection of <see cref="CallHandlerNode"/> instances in a hierarchy.
    /// </summary>
    [Image(typeof(CallHandlersCollectionNode))]
    [SelectedImage(typeof(CallHandlersCollectionNode))]
    public class CallHandlersCollectionNode : ConfigurationNode
    {
        /// <summary>
        /// Gets or sets the name for the configuration node.
        /// </summary>
		/// <remarks>
		/// The name for a <see cref="CallHandlersCollectionNode"/> is fixed.
		/// </remarks>
        [ReadOnly(true)]
        public override string Name
        {
            get
            {
                return Resources.CallHandlersCollectionNodeName;
            }
            set
            {
            }
        }

		/// <summary>
		/// Gets if children added to the node are sorted.
		/// </summary>
		/// <value><see langword="false"/> as nodes in the collection should not be sorted.</value>
		public override bool SortChildren
        {
            get
            {
                return false;
            }
        }
    }
}