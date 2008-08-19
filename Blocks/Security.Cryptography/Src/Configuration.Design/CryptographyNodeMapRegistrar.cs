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
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design
{
    class CryptographyNodeMapRegistrar : NodeMapRegistrar
    {
        public CryptographyNodeMapRegistrar(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }


        public override void Register()
        {

            AddMultipleNodeMap(Resources.CustomHashProviderNodeName, 
                typeof(CustomHashProviderNode), typeof(CustomHashProviderData));

            AddMultipleNodeMap(Resources.HashAlgorithmProviderNodeName,
                typeof(HashAlgorithmProviderNode), typeof(HashAlgorithmProviderData));

            AddMultipleNodeMap(Resources.KeyedHashProviderNodeName,
                typeof(KeyedHashAlgorithmProviderNode), typeof(KeyedHashAlgorithmProviderData));

            AddMultipleNodeMap(Resources.SymmetricAlgorithmProviderNodeName,
                typeof(SymmetricAlgorithmProviderNode), typeof(SymmetricAlgorithmProviderData));

            AddMultipleNodeMap(Resources.DpapiSymmetricCryptoProviderNodeName,
                typeof(DpapiSymmetricCryptoProviderNode), typeof(DpapiSymmetricCryptoProviderData));

            AddMultipleNodeMap(Resources.CustomSymmetricCryptoProviderNodeName,
                typeof(CustomSymmetricCryptoProviderNode), typeof(CustomSymmetricCryptoProviderData));
        }
    }
}
