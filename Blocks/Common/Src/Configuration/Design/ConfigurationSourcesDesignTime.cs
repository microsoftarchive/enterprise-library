using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design
{

    /// <summary/>
    public static class ConfigurationSourcesDesignTime
    {
        ///<summary/>
        public static class ViewModelTypeNames
        {
            ///<summary/>
            public const string ConfigurationSourcesSectionViewModel = "Console.Wpf.ViewModel.BlockSpecifics.ConfigurationSourceSectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            ///<summary/>
            public const string ConfigurationSourceSectionViewModel =
                "Console.Wpf.ViewModel.BlockSpecifics.ConfigurationSourceSectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }

        /// <summary/>
        public static class CommandTypeNames
        {
            /// <summary/>
            public const string AddConfigurationSourcesBlockCommand = "Console.Wpf.ViewModel.BlockSpecifics.AddConfigurationSourcesBlockCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }
    }

}
