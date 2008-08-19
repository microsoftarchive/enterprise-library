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
using Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design
{
    sealed class DataCacheStorageCommandRegistrar: CommandRegistrar
    {
        
		public DataCacheStorageCommandRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider) 
        { }

        
        public override void Register()
        {
            AddDataCaheStorageCommand();
            AddDefaultCommands(typeof(DataCacheStorageNode));           
        }

        
		private void AddDataCaheStorageCommand()
        {
			ConfigurationUICommand cmd = ConfigurationUICommand.CreateSingleUICommand(ServiceProvider, 
				Resources.DataCacheStorageCommandText,
				Resources.DataCacheStorageCommandLongText,
				new AddDataCacheStorageCommand(ServiceProvider),
				typeof(CacheStorageNode));
			AddUICommand(cmd, typeof(CacheManagerNode));                
        }
    }
}
