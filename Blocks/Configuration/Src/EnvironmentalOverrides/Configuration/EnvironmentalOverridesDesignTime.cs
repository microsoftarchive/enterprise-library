//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration
{
    internal static class EnvironmentalOverridesDesignTime
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Used to organize constants.")]
        public static class ViewModelTypeNames
        {
            public const string EnvironmentalOverridesViewModel = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.EnvironmentSourceViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "Used to organize constants.")]
        public static class ValidatorTypeName
        {
            public const string EnvironmentalOverridesSectionMergeFileValidator = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.EnvironmentalOverridesSectionMergeFileValidator, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";

            public const string EnvironmentalOverridesSectionNameValidator = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.EnvironmentalOverridesSectionNameValidator, Microsoft.Practices.EnterpriseLibrary.Configuration.DesignTime";
        }
    }
}
