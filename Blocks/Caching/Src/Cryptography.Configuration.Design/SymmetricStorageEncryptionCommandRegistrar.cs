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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design
{
    class SymmetricStorageEncryptionCommandRegistrar : CommandRegistrar
    {
        public SymmetricStorageEncryptionCommandRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider) 
        { }

        public override void Register()
        {
            AddSymmetricStorageEncryptionCommand();
            AddDefaultCommands(typeof(SymmetricStorageEncryptionProviderNode));
        }


        private void AddSymmetricStorageEncryptionCommand()
        {
            ConfigurationUICommand cmd = ConfigurationUICommand.CreateSingleUICommand(ServiceProvider,
                Resources.SymmetricStorageEncryptionProviderCommandText,
                Resources.SymmetricStorageEncryptionProviderCommandLongText,
                new AddSymmetricStorageEncryptionCommand(ServiceProvider),
                typeof(CacheStorageEncryptionNode));

            AddUICommand(cmd, typeof(IsolatedStorageCacheStorageNode));
            AddUICommand(cmd, typeof(CustomCacheStorageNode));
            AddUICommand(cmd, typeof(DataCacheStorageNode)); 
        }
    }
}
