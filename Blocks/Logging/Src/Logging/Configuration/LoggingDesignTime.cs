using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary/>
    public static class LoggingDesignTime
    {
        ///<summary/>
        public static class ViewModelTypeNames
        {
            ///<summary/>
            public const string LogggingSectionViewModel = "Console.Wpf.ViewModel.BlockSpecifics.LoggingSectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            ///<summary/>
            public const string TraceListenerElementCollectionViewModel = "Console.Wpf.ViewModel.BlockSpecifics.TraceListenerElementCollectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            ///<summary/>
            public const string TraceListenerReferenceElementViewModel = "Console.Wpf.ViewModel.BlockSpecifics.TraceListenerReferenceViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            ///<summary/>
            public const string CategoryFilterViewModel =
                "Console.Wpf.ViewModel.BlockSpecifics.CategoryFilterEntryViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }

        /// <summary/>
        public static class CommandTypeNames
        {
            /// <summary/>
            public const string AddLoggingBlockCommand = "Console.Wpf.ViewModel.BlockSpecifics.AddLoggingBlockCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }
    }
}
