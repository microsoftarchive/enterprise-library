//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
	/// <summary>
	/// Add the Security Application Block to the current application.
	/// </summary>
    public class AddSecuritySettingsNodeCommand : AddChildNodeCommand
    {
		/// <summary>
		/// Initialize a new instance of the <see cref="AddSecuritySettingsNodeCommand"/> class with an <see cref="IServiceProvider"/>.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
		/// </param>
        public AddSecuritySettingsNodeCommand(IServiceProvider serviceProvider)
            : base(serviceProvider, typeof(SecuritySettingsNode))
        {
        }

		/// <summary>
		/// After the <see cref="SecuritySettingsNode"/> is added, adds the default nodes.
		/// </summary>
		/// <param name="node">The <see cref="SecuritySettingsNode"/>.S</param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            base.ExecuteCore(node);
            SecuritySettingsNode securitySettingsNode = (SecuritySettingsNode)ChildNode;

            securitySettingsNode.AddNode(new AuthorizationProviderCollectionNode());
            securitySettingsNode.AddNode(new SecurityCacheProviderCollectionNode());
        }
    }
}
