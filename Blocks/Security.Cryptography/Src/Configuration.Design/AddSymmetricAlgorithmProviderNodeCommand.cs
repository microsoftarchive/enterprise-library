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
    /// Represents a command for adding a <see cref="SymmetricAlgorithmProviderNode"/>.
    /// </summary>
    public class AddSymmetricAlgorithmProviderNodeCommand : AddChildNodeCommand
    {
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="AddSymmetricAlgorithmProviderNodeCommand"/> class with an <see cref="IServiceProvider"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The service provider to get service objects.</para>
        /// </param>
        public AddSymmetricAlgorithmProviderNodeCommand(IServiceProvider serviceProvider)
            : base(serviceProvider, typeof(SymmetricAlgorithmProviderNode))
        {
        }

        /// <summary>
        /// <para>Creates an instance of the child node class and adds it as a child of the parent node. The node will be a <see cref="SymmetricAlgorithmProviderNode"/>.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The parent node to add the newly created <see cref="AddChildNodeCommand.ChildNode"/>.</para>
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            TypeSelectorUI selector = new TypeSelectorUI(
                typeof(RijndaelManaged),
                typeof(SymmetricAlgorithm),
                TypeSelectorIncludes.None
                );

            DialogResult typeResult = selector.ShowDialog();
            if (typeResult == DialogResult.OK)
            {
                Type algorithmType = selector.SelectedType;
                CryptographicKeyWizard keyManager = new CryptographicKeyWizard(new SymmetricAlgorithmKeyCreator(algorithmType));
                DialogResult keyResult = keyManager.ShowDialog();

                if (keyResult == DialogResult.OK)
                {
                    INodeNameCreationService service = ServiceHelper.GetNameCreationService(ServiceProvider);
                    Debug.Assert(service != null, "Could not find the INodeNameCreationService");
                    base.ExecuteCore(node);
                    SymmetricAlgorithmProviderNode providerNode = (SymmetricAlgorithmProviderNode)ChildNode;
                    providerNode.AlgorithmType = selector.SelectedType;
                    providerNode.Name = service.GetUniqueName(selector.SelectedType.Name, providerNode, providerNode.Parent);
                    providerNode.Key = keyManager.KeySettings;
                }
            }
        }
    }
}
