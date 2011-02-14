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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.Unity.Configuration.Design.Validation
{
    internal class ExtensionTypeValidator : TypeNameValidatorBase
    {
        protected override void ValidateCore(Property property, string value, IList<ValidationResult> errors)
        {
            Initialize(property, errors);

            Type extensionType = ResolveTypeName(value);
            if (extensionType == null) return;

            if (!TypeIsContainerExtension(extensionType))
            {
                ReportError(DesignResources.NotAnExtensionType, value);
            }
        }

        private static bool TypeIsContainerExtension(Type extensionType)
        {
            return typeof (UnityContainerExtension).IsAssignableFrom(extensionType);
        }
    }
}
