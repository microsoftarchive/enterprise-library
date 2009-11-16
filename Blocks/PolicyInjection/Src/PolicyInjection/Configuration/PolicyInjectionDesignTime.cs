//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{

    ///<summary/>
    public static class PolicyInjectionDesignTime
    {
        ///<summary/>
        public static class ViewModelTypeNames
        {
            ///<summary/>
            public const string PiabSectionViewModel = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.PiabSectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            ///<summary/>
            public const string PiabParameterTypeMatchDataViewModel = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.PiabParameterTypeMatchDataCollectionElementViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            ///<summary/>
            public const string PiabPropertyMatchDataViewModel = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.PiabPropertyMatchDataViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            ///<summary/>
            public const string PiabTypeMatchDataViewModel = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.PiabTypeMatchDataViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            ///<summary/>
            public const string PiabNamespaceMatchDataViewModel = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.PiabNamespaceMatchDataViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            ///<summary/>
            public const string PiabMethodSignatureTypesViewModel = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.PiabMethodSignatureTypesViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            ///<summary/>
            public const string PiabMemberNameViewModel = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.PiabMemberNameViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

        }
    }
}
