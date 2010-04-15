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
    internal static class SecurityDesignTime
    {
        public static class ViewModelTypeNames
        {
            public const string SecuritySectionViewModel =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.SecuritySectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string AuthorizationRuleDataViewModel =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.AuthorizationRuleDataViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string AuthorizationProviderDataViewModel =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.AuthorizationProviderDataViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
 
            public const string AuthorizationRuleProviderDataViewModel =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.AuthorizationRuleProviderDataViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
        }

        public static class EditorTypeNames
        {
            public const string SecurityExpressionEditor = "Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.ExpressionEditor, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
        }

        public static class ValidatorTypes
        {
            public const string NameValueCollectionValidator = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.NameValueCollectionValidator, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
        }
    }
}
