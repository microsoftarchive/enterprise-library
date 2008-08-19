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

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design
{
    /// <summary>
    /// Represents a remoting injection configuration node.
    /// </summary>
    [Image(typeof(PolicyInjectionSettingsNode))]
    public class RemotingInjectorNode : InjectorNode
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="RemotingInjectorNode"/> class.
        /// </summary>
        public RemotingInjectorNode()
            : base(new RemotingInjectorData(Resources.RemotingInjectorNodeName)) {}

        /// <summary>
        /// Initialize a new instance of the <see cref="RemotingInjectorNode"/> class with the <see cref="RemotingInjectorData"/> to use.
        /// </summary>
        /// <param name="data">
        /// The <see cref="RemotingInjectorData"/> to use.
        /// </param>
        public RemotingInjectorNode(RemotingInjectorData data)
            : base(data) {}

        /// <summary>
        /// Gets the configuration data for this node.
        /// </summary>
        /// <returns>The configuration data for this node.</returns>
        public override InjectorData GetConfigurationData()
        {
            return new RemotingInjectorData(Name);
        }
    }
}