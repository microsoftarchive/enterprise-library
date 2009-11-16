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
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.SecuritySectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }

        ///<summary/>
        public static class EditorTypeNames
        {

            ///<summary/>
            public const string SecurityExpressionEditor = "Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.ExpressionEditor, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }
    }
}
