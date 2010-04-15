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

    internal static class PolicyInjectionDesignTime
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public static class ViewModelTypeNames
        {
            public const string PolicyInjectionSectionViewModel = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.PolicyInjection.PolicyInjectionSectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string PolicyDataViewModel = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.PolicyInjection.PolicyDataViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Validators")]
        public static class Validators
        {
            public const string MatchCollectionPopulatedValidationType = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.PolicyInjection.MatchCollectionPopulatedValidator, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string NameValueCollectionValidator = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.NameValueCollectionValidator, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
        }
    }
}
