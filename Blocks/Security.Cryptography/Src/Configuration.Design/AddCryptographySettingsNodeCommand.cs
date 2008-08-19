//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
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
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Represents a command for adding the Cryptography Application Block to the current application.
    /// </summary>
    public class AddCryptographySettingsNodeCommand : AddChildNodeCommand
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="AddCryptographySettingsNodeCommand"/> class with an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public AddCryptographySettingsNodeCommand(IServiceProvider serviceProvider)
			: base(serviceProvider, typeof(CryptographySettingsNode))
		{
		}

        /// <summary>
        /// <para>Adds the <see cref="CryptographySettingsNode"/> to the current application, with its default childnodes.</para>
        /// </summary>
        /// <param name="e"><para>An <see cref="EventArgs"/> containing the event data.</para></param>
        protected override void OnExecuted(EventArgs e)
        {
            base.OnExecuted(e);
            CryptographySettingsNode node = ChildNode as CryptographySettingsNode;
            Debug.Assert(null != node, "The added node should be a CryptographySettingsNode");

            node.AddNode(new HashProviderCollectionNode());
            node.AddNode(new SymmetricCryptoProviderCollectionNode());
        }
    }
}
