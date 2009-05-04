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
using System.Diagnostics;
using System.Security.Cryptography;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    /// <summary>
    /// Represents a command for adding a <see cref="HashAlgorithmProviderNode"/>.
    /// </summary>
    public class AddHashAlgorithmProviderNodeCommand : AddChildNodeCommand
    {
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="AddHashAlgorithmProviderNodeCommand"/> class with an <see cref="IServiceProvider"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The service provider to get service objects.</para>
        /// </param>
        public AddHashAlgorithmProviderNodeCommand(IServiceProvider serviceProvider) : base(serviceProvider, typeof(HashAlgorithmProviderNode))
        {
        }

        /// <summary>
        /// <para>Creates an instance of the child node class and adds it as a child of the parent node. The node will be a <see cref="HashAlgorithmProviderNode"/>.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The parent node to add the newly created <see cref="AddChildNodeCommand.ChildNode"/>.</para>
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            INodeNameCreationService service = ServiceHelper.GetNameCreationService(ServiceProvider);
            Debug.Assert(service != null, "Could not find the INodeNameCreationService");

            TypeSelectorUI selector = new TypeSelectorUI(
                typeof(SHA1Managed),
                typeof(HashAlgorithm),
                TypeSelectorIncludes.None
                );

            DialogResult typeResult = selector.ShowDialog();
            if (typeResult == DialogResult.OK)
            {

                Type selectedAlgorithmType = selector.SelectedType;
                if (selector.SelectedType.IsSubclassOf(typeof(KeyedHashAlgorithm)))
                {
                    CryptographicKeyWizard keyWizard = new CryptographicKeyWizard(new KeyedHashAlgorithmKeyCreator(selector.SelectedType));
                    DialogResult keyResult = keyWizard.ShowDialog();

                    if (keyResult == DialogResult.OK)
                    {
                        KeyedHashAlgorithmProviderNode providerNode = new KeyedHashAlgorithmProviderNode();

                        providerNode.Key = keyWizard.KeySettings;
                        providerNode.AlgorithmType = selector.SelectedType;

                        node.AddNode(providerNode);
                        providerNode.Name = service.GetUniqueName(selector.SelectedType.Name, providerNode, providerNode.Parent);
                    }
                }
                else
                {
                    base.ExecuteCore(node);
                    HashAlgorithmProviderNode providerNode = (HashAlgorithmProviderNode)ChildNode;
                    providerNode.AlgorithmType = selectedAlgorithmType;
                    providerNode.Name = service.GetUniqueName(selector.SelectedType.Name, providerNode, providerNode.Parent);
                }
            }
        }
    }
}
