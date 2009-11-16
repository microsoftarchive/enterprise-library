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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    ///<summary>
    /// Provides validation for <see cref="Property"/> values by using validators
    /// supplied by <see cref="ConfigurationValidatorAttribute"/>
    /// 
    ///</summary>
    public class ValidationService
    {
        public IEnumerable<ValidationError> Validate(Property property)
        {
            var validations = GetDefaultPropertyValidators(property)
                .Union(property.Attributes.OfType<ValidationAttribute>());

            var results = new List<ValidationError>();

            foreach (var validation in validations)
            {
                validation.Validate(property, results);
            }

            return results;
        }

        private IEnumerable<ValidationAttribute> GetDefaultPropertyValidators(Property property)
        {
            yield return new DefaultPropertyValidator();
        }
    }
}
