//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design
{
    /// <summary>
    /// Represents a command for adding a symmetric cache storage encryption provider.
    /// </summary>
    public class AddSymmetricStorageEncryptionCommand : AddChildNodeCommand
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="AddSymmetricStorageEncryptionCommand"/> class with an <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public AddSymmetricStorageEncryptionCommand(IServiceProvider serviceProvider)
            : base(serviceProvider, typeof(SymmetricStorageEncryptionProviderNode)) {}

        /// <summary>
        /// <para>Adds the <see cref="SymmetricStorageEncryptionProviderNode"/> to the current <see cref="CacheStorageNode"/>. 
        /// If the Cryptography Application Block configuration is not added to the current application, it is also added.</para>
        /// </summary>
        /// <param name="e"><para>An <see cref="EventArgs"/> containing the event data.</para></param>
        protected override void OnExecuted(EventArgs e)
        {
            base.OnExecuted(e);
            SymmetricStorageEncryptionProviderNode node = ChildNode as SymmetricStorageEncryptionProviderNode;
            Debug.Assert(null != node, "Expected a SymmetricStorageEncryptionNode");

            if (null == CurrentHierarchy.FindNodeByType(typeof(CryptographySettingsNode)))
            {
                new AddCryptographySettingsNodeCommand(ServiceProvider).Execute(CurrentHierarchy.RootNode);
            }
        }
    }
}
