using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration
{

    ///<summary/>
    public static class SecurityDesignTime
    {
        ///<summary/>
        public static class ViewModelTypeNames
        {
            ///<summary/>
            public const string SecuritySectionViewModel =
                "Console.Wpf.ViewModel.BlockSpecifics.SecuritySectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }

        ///<summary/>
        public static class EditorTypeNames
        {

            ///<summary/>
            public const string SecurityExpressionEditor = "Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.ExpressionEditor, Console.Wpf";
        }
    }
}
