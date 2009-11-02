using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{

    /// <summary/>
    public static class ValidationDesignTime
    {
        ///<summary/>
        public static class ViewModelTypeNames
        {
            ///<summary/>
            public const string ValidationSectionViewModel = "Console.Wpf.ViewModel.BlockSpecifics.ValidationSectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            ///<summary/>
            public const string DomainConfigurationElementViewModel =
                "Console.Wpf.ViewModel.BlockSpecifics.DomainConfigurationElementViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }

        /// <summary/>
        public static class CommandTypeNames
        {
            /// <summary/>
            public const string AddValidatedTypeCommand = "Console.Wpf.ViewModel.BlockSpecifics.ValidationTypeReferenceAddCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }
    }
}
