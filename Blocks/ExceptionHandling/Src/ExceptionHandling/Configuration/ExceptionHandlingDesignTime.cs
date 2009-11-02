using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary/>
    public static class ExceptionHandlingDesignTime
    {
        /// <summary/>
        public static class ViewModelTypeNames
        {
            ///<summary>
            ///</summary>
            public const string ExceptionHandlingSectionViewModel =
                "Console.Wpf.ViewModel.BlockSpecifics.ExceptionHandlingSectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

        }
        /// <summary/>
        public static class CommandTypeNames
        {
            /// <summary/>
            public const string AddExceptionHandlingBlockCommand = "Console.Wpf.ViewModel.BlockSpecifics.AddExceptionHandlingBlockCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }
    }
}
