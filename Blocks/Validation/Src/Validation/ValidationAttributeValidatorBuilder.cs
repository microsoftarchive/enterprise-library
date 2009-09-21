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
using System.ComponentModel.DataAnnotations;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation
{
    /// <summary>
    /// Builder of validators from attributes inheriting from <see cref="ValidationAttribute"/>.
    /// </summary>
    /// <seealso cref="ValidationAttributeValidator"/>
    public class ValidationAttributeValidatorBuilder : ValidatorBuilderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationAttributeValidatorBuilder"/> class with the default 
        /// <see cref="MemberAccessValidatorBuilderFactory"/>.
        /// </summary>
        public ValidationAttributeValidatorBuilder()
            : base()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationAttributeValidatorBuilder"/> class with the supplied
        /// <see cref="MemberAccessValidatorBuilderFactory"/>.
        /// </summary>        
        /// <param name="memberAccessValidatorFactory">A factory to create member accessors if necessary.</param>
        public ValidationAttributeValidatorBuilder(MemberAccessValidatorBuilderFactory memberAccessValidatorFactory)
            : base(memberAccessValidatorFactory)
        { }

        /// <summary>
        /// Creates a validator for the supplied type.
        /// </summary>
        /// <param name="type">The type for which the validator should be created.</param>
        /// <returns>A validator.</returns>
        public Validator CreateValidator(Type type)
        {
            return CreateValidator(new ValidationAttributeValidatedType(type));
        }
    }
}
