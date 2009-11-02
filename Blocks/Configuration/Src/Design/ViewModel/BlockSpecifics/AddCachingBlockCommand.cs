using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Wpf.ViewModel.Commands;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    public class AddCachingBlockCommand : AddApplicationBlockCommand
    {
        public AddCachingBlockCommand(ConfigurationSourceModel configurationSourceModel, AddApplicationBlockCommandAttribute attribute)
            : base(configurationSourceModel, attribute)
        {
        }

        protected override System.Configuration.ConfigurationSection CreateConfigurationSection()
        {
            return new CacheManagerSettings
            {
                DefaultCacheManager = "Cache Manager",
                CacheManagers = 
                {{
                     new CacheManagerData
                     {
                         Name = "Cache Manager",
                         CacheStorage = String.Empty
                     }
                }},

            };
        }
    }
}
