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
    class MapToNameValidator : TypeNameValidatorBase
    {
        protected override void ValidateCore(Property property, string value, IList<ValidationResult> errors)
        {
            Initialize(property, errors);

            Type typeNameType = GetTypeNameType(property);
            if(typeNameType == null)
            {
                return;
            }

            var mapToType = ResolveTypeName(value);

            bool valid = ValidateTypeIsResolved(mapToType, value) &&
                ValidateMappingIsValid(typeNameType, mapToType);
        }

        private Type GetTypeNameType(Property mapToProperty)
        {
            var elementProperty = mapToProperty as ElementProperty;
            if(elementProperty == null) return null;

            var registerElement = elementProperty.DeclaringElement;
            
            if(registerElement == null)
                return null;

            var typeNameValue = (string)registerElement.Property("TypeName").Value;

            var type = ResolveTypeName(typeNameValue);
            return type;
        }

        private bool ValidateTypeIsResolved(Type mapToType, string value)
        {
            if (mapToType == null)
            {
                ReportWarning(DesignResources.CouldNotResolveTypeName, value);
                return false;
            }
            return true;
        }

        private bool ValidateMappingIsValid(Type fromType, Type toType)
        {
            if(!fromType.IsAssignableFrom(toType))
            {
                ReportError(DesignResources.MapToTypeIsInvalid, toType, fromType);
                return false;
            }
            return true;
        }
    }
}
