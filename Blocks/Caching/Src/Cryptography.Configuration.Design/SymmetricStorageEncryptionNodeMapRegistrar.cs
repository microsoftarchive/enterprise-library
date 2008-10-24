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
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design
{
    class SymmetricStorageEncryptionNodeMapRegistrar : NodeMapRegistrar
    {
        public SymmetricStorageEncryptionNodeMapRegistrar(IServiceProvider serviceProvider)
            :base(serviceProvider)
        {
        }

        public override void Register()
        {
            AddMultipleNodeMap(Resources.SymmetricStorageEncryptionProvider,
                typeof(SymmetricStorageEncryptionProviderNode),
                typeof(SymmetricStorageEncryptionProviderData));
        }
    }
}
