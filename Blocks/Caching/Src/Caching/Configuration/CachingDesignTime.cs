using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
    /// <summary/>
    public static class CachingDesignTime
    {
        ///<summary/>
        public static class ViewModelTypeNames
        {
            ///<summary/>
            public const string CacheManagerSectionViewModel =
                "Console.Wpf.ViewModel.BlockSpecifics.CacheManagerSectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }

        /// <summary/>
        public static class CommandTypeNames
        {
            /// <summary/>
            public const string AddCachingBlockCommand = "Console.Wpf.ViewModel.BlockSpecifics.AddCachingBlockCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }
    }
}
