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
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    /// <summary>
    /// Configuration object to describe an instance of class <see cref="ObjectValidator"/>.
    /// </summary>
    public partial class ObjectValidatorData : ValidatorData
    {
        /// <summary>
        /// Creates the <see cref="ObjectValidator"/> described by the configuration object.
        /// </summary>
        /// <param name="targetType">The type of object that will be validated by the validator.</param>
        /// <remarks>This method must not be called on this class. Call 
        /// <see cref="ObjectValidatorData.DoCreateValidator(Type, Type, MemberValueAccessBuilder, ValidatorFactory)"/>.</remarks>
        protected override Validator DoCreateValidator(Type targetType)
        {
            throw new NotImplementedException(Resources.ExceptionShouldNotCall);
        }

        /// <summary>
        /// Creates the <see cref="ObjectValidator"/> described by the configuration object.
        /// </summary>
        /// <param name="targetType">The type of object that will be validated by the validator.</param>
        /// <param name="ownerType">The type of the object from which the value to validate is extracted.</param>
        /// <param name="memberValueAccessBuilder">The <see cref="MemberValueAccessBuilder"/> to use for validators that
        /// require access to properties.</param>
        /// <param name="validatorFactory">Factory to use when building nested validators.</param>
        /// <returns>The created <see cref="ObjectValidator"/>.</returns>
        /// <seealso cref="ObjectValidator"/>
        protected override Validator DoCreateValidator(
            Type targetType,
            Type ownerType,
            MemberValueAccessBuilder memberValueAccessBuilder,
            ValidatorFactory validatorFactory)
        {
            if (this.ValidateActualType)
            {
                return new ObjectValidator(validatorFactory, this.TargetRuleset);
            }
            else
            {
                return new ObjectValidator(targetType, validatorFactory, this.TargetRuleset);
            }
        }
    }
}
