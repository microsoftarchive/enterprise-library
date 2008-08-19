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
    /// Represents a command that will export a cryptographic key.
    /// </summary>
    public class ExportKeyCommand : ConfigurationNodeCommand
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="ChooseKeyFileControl"/>, with a <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public ExportKeyCommand(IServiceProvider serviceProvider)
            :base(serviceProvider)
        {
        }

        /// <summary>
        /// Shows an dialog that allows the user to export an cryptographic key.
        /// </summary>
        /// <param name="node">
        /// The <see cref="ConfigurationNode"/> of which an cryptographic key should be exported.
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            ICryptographicKeyConfigurationNode cryptographicNode = node as ICryptographicKeyConfigurationNode;
            if (cryptographicNode != null)
            {
                ExportKeyUI exportKeyDialog = new ExportKeyUI(cryptographicNode.KeySettings.ProtectedKey);
                exportKeyDialog.ShowDialog();
            }
        }
    }
}
