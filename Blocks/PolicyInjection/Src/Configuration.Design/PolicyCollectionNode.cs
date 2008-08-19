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

using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design
{
    /// <summary>
    /// Represents a configuration node for a policy collection.
    /// </summary>
    [Image(typeof(PolicyCollectionNode))]
    public class PolicyCollectionNode : ConfigurationNode
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="PolicyCollectionNode"/> class.
        /// </summary>
        public PolicyCollectionNode()
            : base(Resources.PolicyCollectionNodeName) {}

        /// <summary>
        /// Gets or sets the name of the node.
        /// </summary>
        /// <value>
        /// The name of the node.
        /// </value>
        [ReadOnly(true)]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("PolicyCollectionNodeDescription", typeof(Resources))]
        public override string Name
        {
            get { return base.Name; }
        }
    }
}