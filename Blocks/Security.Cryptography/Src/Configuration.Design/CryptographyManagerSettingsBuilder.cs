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
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    class CryptographyManagerSettingsBuilder
    {
        private CryptographySettings cryptographySettings;
        private CryptographySettingsNode cryptographySettingsNode;
        private IConfigurationUIHierarchy hierarchy;
		
        public CryptographyManagerSettingsBuilder(IServiceProvider serviceProvider, CryptographySettingsNode cryptographySettingsNode) 
		{
            this.cryptographySettingsNode = cryptographySettingsNode;
			hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
            cryptographySettings = new CryptographySettings();
		}

        public CryptographySettings Build()
        {
            BuildHashProviders();
            BuildSymmetricCryptoProviders();

			if (!cryptographySettingsNode.RequirePermission)	// don't set if false
				cryptographySettings.SectionInformation.RequirePermission = cryptographySettingsNode.RequirePermission;
			cryptographySettings.DefaultHashProviderName = (cryptographySettingsNode.DefaultHashProvider != null) ? cryptographySettingsNode.DefaultHashProvider.Name : null;
            cryptographySettings.DefaultSymmetricCryptoProviderName = (cryptographySettingsNode.DefaultSymmetricCryptoProvider != null) ? cryptographySettingsNode.DefaultSymmetricCryptoProvider.Name : null;

            return cryptographySettings;
        }

        private void BuildSymmetricCryptoProviders()
        {
            foreach (HashProviderNode hashProviderNode in hierarchy.FindNodesByType(cryptographySettingsNode, typeof(HashProviderNode)))
            {
                cryptographySettings.HashProviders.Add(hashProviderNode.HashProviderData);
            }
        }

        private void BuildHashProviders()
        {
            foreach (SymmetricCryptoProviderNode symmetricCryptoProviderNode in hierarchy.FindNodesByType(cryptographySettingsNode, typeof(SymmetricCryptoProviderNode)))
            {
                cryptographySettings.SymmetricCryptoProviders.Add(symmetricCryptoProviderNode.SymmetricCryptoProviderData);
            }
        }
    }
}
