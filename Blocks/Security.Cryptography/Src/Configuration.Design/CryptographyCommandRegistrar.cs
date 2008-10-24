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
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{

    class CryptographyCommandRegistrar : CommandRegistrar
    {
        public CryptographyCommandRegistrar(IServiceProvider serviceProvider)
            :base(serviceProvider)
        {
        }

        public override void Register()
        {
            AddCryptographySettingsCommand();
            AddDefaultCommands(typeof(CryptographySettingsNode));

            AddHashAlgortihmProviderCommand();
            AddDefaultCommands(typeof(HashAlgorithmProviderNode));
			AddDefaultCommands(typeof(KeyedHashAlgorithmProviderNode));
            AddExportCryptographicKeyCommand(typeof(KeyedHashAlgorithmProviderNode));

            AddCustomHashProviderCommand();
            AddDefaultCommands(typeof(CustomHashProviderNode));

            AddDpapiSymmetricCryptoProviderCommand();
            AddDefaultCommands(typeof(DpapiSymmetricCryptoProviderNode));

            AddCustomSymmetricCryptoProviderCommand();
            AddDefaultCommands(typeof(CustomSymmetricCryptoProviderNode));

            AddSymmetricAlgorithmProviderCommand();
            AddDefaultCommands(typeof(SymmetricAlgorithmProviderNode));
            AddExportCryptographicKeyCommand(typeof(SymmetricAlgorithmProviderNode));
        }

        private void AddExportCryptographicKeyCommand(Type nodeType)
        {
            ConfigurationUICommand command = new ConfigurationUICommand(
                ServiceProvider, Resources.ExportKeyCommandText,
                Resources.ExportKeyLongCommandText, CommandState.Enabled, 
                new ExportKeyCommand(ServiceProvider), System.Windows.Forms.Shortcut.None, 
                InsertionPoint.Action, null);
            AddUICommand(command, nodeType);
        }

        private void AddSymmetricAlgorithmProviderCommand()
        {
            ConfigurationUICommand cmd = ConfigurationUICommand.CreateMultipleUICommand(ServiceProvider,
                Resources.SymmetricAlgorithmProviderNodeName,
                string.Format(Resources.Culture, Resources.GenericCreateStatusText, Resources.SymmetricAlgorithmProviderNodeName),
                new AddSymmetricAlgorithmProviderNodeCommand(ServiceProvider), typeof(SymmetricAlgorithmProviderNode));

            AddUICommand(cmd, typeof(SymmetricCryptoProviderCollectionNode));
        }

        private void AddCustomSymmetricCryptoProviderCommand()
        {
            AddMultipleChildNodeCommand(Resources.CustomSymmetricCryptoProviderNodeName,
                string.Format(Resources.Culture, Resources.GenericCreateStatusText, Resources.CustomSymmetricCryptoProviderNodeName),
                typeof(CustomSymmetricCryptoProviderNode),
                typeof(SymmetricCryptoProviderCollectionNode));
        }

        private void AddDpapiSymmetricCryptoProviderCommand()
        {
            AddMultipleChildNodeCommand(Resources.DpapiSymmetricCryptoProviderNodeName,
                string.Format(Resources.Culture, Resources.GenericCreateStatusText, Resources.DpapiSymmetricCryptoProviderNodeName),
                typeof(DpapiSymmetricCryptoProviderNode),
                typeof(SymmetricCryptoProviderCollectionNode));
        }

        private void AddCustomHashProviderCommand()
        {
            AddMultipleChildNodeCommand(Resources.CustomHashProviderNodeName,
                string.Format(Resources.Culture, Resources.GenericCreateStatusText, Resources.CustomHashProviderNodeName),
                typeof(CustomHashProviderNode),
                typeof(HashProviderCollectionNode));
        }

        private void AddHashAlgortihmProviderCommand()
        {
            ConfigurationUICommand cmd = ConfigurationUICommand.CreateMultipleUICommand(ServiceProvider,
                Resources.HashAlgorithmProviderNodeName,
                string.Format(Resources.Culture, Resources.GenericCreateStatusText, Resources.HashAlgorithmProviderNodeName),
                new AddHashAlgorithmProviderNodeCommand(ServiceProvider), typeof(HashAlgorithmProviderNode));
            AddUICommand(cmd, typeof(HashProviderCollectionNode));

        }

        private void AddCryptographySettingsCommand()
        {
            ConfigurationUICommand cmd = ConfigurationUICommand.CreateSingleUICommand(ServiceProvider,
                Resources.CryptographySectionCommandName,
                string.Format(Resources.Culture, Resources.GenericCreateStatusText, Resources.CryptographySectionCommandName), 
                new AddCryptographySettingsNodeCommand(ServiceProvider),
                typeof(CryptographySettingsNode));
            AddUICommand(cmd, typeof(ConfigurationApplicationNode));
        }
    }
}
