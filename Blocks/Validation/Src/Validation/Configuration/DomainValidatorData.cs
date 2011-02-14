//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    /// <summary>
    /// Configuration object to describe an instance of class <see cref="DomainValidatorData"/>.
    /// </summary>
    public partial class DomainValidatorData : ValueValidatorData
    {
        /// <summary>
        /// Creates the <see cref="DomainValidator{T}"/> described by the configuration object.
        /// </summary>
        /// <param name="targetType">The type of object that will be validated by the validator.</param>
        /// <returns>The created <see cref="DomainValidator{T}"/>.</returns>	
        protected override Validator DoCreateValidator(Type targetType)
        {
            List<object> domainObjects = new List<object>();

            foreach (var domainConfigurationElement in this.Domain)
            {
                object domainObject = null;
                if (targetType != null)
                {
                    domainObject = Convert.ChangeType(domainConfigurationElement.Name, targetType, CultureInfo.InvariantCulture);
                }

                if (domainObject != null)
                {
                    domainObjects.Add(domainObject);
                }
                else
                {	
                    domainObjects.Add(domainConfigurationElement.Name);
                }
            }

            return new DomainValidator<object>(domainObjects, Negated);
        }
    }
}
