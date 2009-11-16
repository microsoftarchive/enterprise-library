//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class CryptographySectionViewModel: PositionedSectionViewModel
    {
        public CryptographySectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section)
        {
            Positioning.PositionCollection("Hash Providers",
                            typeof(NameTypeConfigurationElementCollection<HashProviderData, CustomHashProviderData>),
                            typeof(HashProviderData),
                            new PositioningInstructions { FixedColumn = 0, FixedRow = 0 });

            Positioning.PositionCollection("Symmetric Crypto Providers",
                            typeof(NameTypeConfigurationElementCollection<SymmetricProviderData, CustomSymmetricCryptoProviderData>),
                            typeof(SymmetricProviderData),
                            new PositioningInstructions { FixedColumn = 1, FixedRow = 0 });

        }


        public override void Save(IDesignConfigurationSource configurationSource)
        {
            base.Save(configurationSource);

            foreach (ProtectedKeySettings keySettings in 
                DescendentElements().
                SelectMany(x => x.Properties).
                Where(x => typeof(ProtectedKeySettings) == x.PropertyType).
                Select(x => x.Value))
            {
                SaveProtectedKey(keySettings);
            }
        }


        private static void SaveProtectedKey(ProtectedKeySettings protectedKeySettings)
        {
            if (protectedKeySettings.ProtectedKey == null)
            {
                return;
            }

            using (Stream keyOutput = File.OpenWrite(protectedKeySettings.Filename))
            {
                KeyManager.Write(keyOutput, protectedKeySettings.ProtectedKey.EncryptedKey, protectedKeySettings.Scope);
            }
        }
    }
}
