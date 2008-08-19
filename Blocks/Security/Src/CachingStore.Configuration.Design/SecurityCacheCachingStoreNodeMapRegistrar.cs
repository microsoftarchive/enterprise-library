//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cache.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.Configuration.Design
{
    sealed class SecurityCacheCachingStoreNodeMapRegistrar : NodeMapRegistrar
    {
        public SecurityCacheCachingStoreNodeMapRegistrar(IServiceProvider serviceProvider)
            : base(serviceProvider) {}

        public override void Register()
        {
            AddMultipleNodeMap(Resources.SecurityInstance,
                               typeof(CachingStoreProviderNode),
                               typeof(CachingStoreProviderData));
        }
    }
}