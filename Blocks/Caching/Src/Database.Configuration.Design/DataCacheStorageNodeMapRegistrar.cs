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
using Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design
{
    class DataCacheStorageNodeMapRegistrar: NodeMapRegistrar
    {
        public DataCacheStorageNodeMapRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
        {
        }

        public override void Register()
        {
            AddMultipleNodeMap(Resources.DataCacheStorage,
                typeof(DataCacheStorageNode),
				typeof(DataCacheStorageData));
        }
    }
}
