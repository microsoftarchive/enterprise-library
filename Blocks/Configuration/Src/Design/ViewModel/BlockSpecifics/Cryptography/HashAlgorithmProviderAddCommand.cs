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
using System.Security.Cryptography;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{

#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class HashAlgorithmProviderAddCommand : TypePickingCollectionElementAddCommand
    {
        ProtectedKeySettings keySettings;

        public HashAlgorithmProviderAddCommand(IUIServiceWpf uiService, IAssemblyDiscoveryService discoveryService, TypePickingCommandAttribute commandAttribute, ConfigurationElementType configurationElementType, ElementCollectionViewModel elementCollectionModel)
            : base(uiService, discoveryService, commandAttribute, configurationElementType, elementCollectionModel)
        { }

        protected override bool AfterSelectType(Type selectedType)
        {
            if (selectedType.IsSubclassOf(typeof(KeyedHashAlgorithm)))
            {
                CreatedElementType = typeof(KeyedHashAlgorithmProviderData);
                CryptographicKeyWizard keyManager = new CryptographicKeyWizard(new KeyedHashAlgorithmKeyCreator(selectedType));
                DialogResult keyResult = keyManager.ShowDialog();

                keySettings = keyManager.KeySettings;
                return keyResult == DialogResult.OK;
            }
            else
            {
                CreatedElementType = typeof(HashAlgorithmProviderData);
                return true;
            }
        }

        protected override void SetProperties(ElementViewModel createdElement, Type selectedType)
        {
            base.SetProperties(createdElement, selectedType);

            if (createdElement.ConfigurationType == typeof(KeyedHashAlgorithmProviderData))
            {
                createdElement.Property("Key").Value = keySettings;
            }
        }
    }

#pragma warning restore 1591
}
