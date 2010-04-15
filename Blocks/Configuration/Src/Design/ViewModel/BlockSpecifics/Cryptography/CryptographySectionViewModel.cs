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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.Unity;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{

#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class CryptographySectionViewModel : SectionViewModel
    {
        public CryptographySectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section)
        {

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

            using (Stream keyOutput = File.OpenWrite(protectedKeySettings.FileName))
            {
                KeyManager.Write(keyOutput, protectedKeySettings.ProtectedKey.EncryptedKey, protectedKeySettings.Scope);
            }
        }


        protected override object CreateBindable()
        {
            var hashProviders = DescendentElements().Where(x => x.ConfigurationType == typeof(NameTypeConfigurationElementCollection<HashProviderData, CustomHashProviderData>)).First();
            var symmetricProviders = DescendentElements().Where(x => x.ConfigurationType == typeof(NameTypeConfigurationElementCollection<SymmetricProviderData, CustomSymmetricCryptoProviderData>)).First();

            return new HorizontalListLayout(
                new HeaderedListLayout(hashProviders), 
                new HeaderedListLayout(symmetricProviders));
        }
    }

#pragma warning restore 1591
}
