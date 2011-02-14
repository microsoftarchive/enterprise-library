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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    /// <summary>
    /// Base class for configuration objects describing validators.
    /// </summary>
    public partial class ValidatorData : IValidatorDescriptor
    {
        /// <summary>
        /// Creates the <see cref="Validator"/> described by the configuration object.
        /// </summary>
        /// <param name="targetType">The type of object that will be validated by the validator.</param>
        /// <param name="ownerType">The type of the object from which the value to validate is extracted.</param>
        /// <param name="memberValueAccessBuilder">The <see cref="MemberValueAccessBuilder"/> to use for validators that
        /// require access to properties.</param>
        /// <param name="validatorFactory">Factory to use when building nested validators.</param>
        /// <returns>The created <see cref="Validator"/>.</returns>
        Validator IValidatorDescriptor.CreateValidator(
            Type targetType,
            Type ownerType,
            MemberValueAccessBuilder memberValueAccessBuilder,
            ValidatorFactory validatorFactory)
        {
            Validator validator = DoCreateValidator(targetType, ownerType, memberValueAccessBuilder, validatorFactory);
            validator.Tag = string.IsNullOrEmpty(this.Tag) ? null : this.Tag;
            validator.MessageTemplate = GetMessageTemplate();

            return validator;
        }

        /// <summary>
        /// Creates the <see cref="Validator"/> described by the configuration object.
        /// </summary>
        /// <param name="targetType">The type of object that will be validated by the validator.</param>
        /// <param name="ownerType">The type of the object from which the value to validate is extracted.</param>
        /// <param name="memberValueAccessBuilder">The <see cref="MemberValueAccessBuilder"/> to use for validators that
        /// require access to properties.</param>
        /// <param name="validatorFactory">Factory to use when building nested validators.</param>
        /// <returns>The created <see cref="Validator"/>.</returns>
        /// <remarks>
        /// The default implementation invokes <see cref="ValidatorData.DoCreateValidator(Type)"/>. Subclasses requiring access to all
        /// the parameters or this method may override it instead of <see cref="ValidatorData.DoCreateValidator(Type)"/>.
        /// </remarks>
        protected virtual Validator DoCreateValidator(
            Type targetType,
            Type ownerType, MemberValueAccessBuilder
            memberValueAccessBuilder,
            ValidatorFactory validatorFactory)
        {
            return DoCreateValidator(targetType);
        }

        /// <summary>
        /// Creates the <see cref="Validator"/> described by the configuration object.
        /// </summary>
        /// <param name="targetType">The type of object that will be validated by the validator.</param>
        /// <remarks>This operation must be overriden by subclasses.</remarks>
        /// <returns>The created <see cref="Validator"/>.</returns>
        protected virtual Validator DoCreateValidator(Type targetType)
        {
            throw new NotImplementedException(Resources.MustImplementOperation);
        }

        /// <summary>
        /// Returns the message template for the represented validator.
        /// </summary>
        /// <remarks>
        /// The textual message is given precedence over the resource based mechanism.
        /// </remarks>
        public string GetMessageTemplate()
        {
            if (!string.IsNullOrEmpty(this.MessageTemplate))
            {
                return this.MessageTemplate;
            }
            Type messageTemplateResourceType = this.GetMessageTemplateResourceType();
            if (null != messageTemplateResourceType)
            {
                return ResourceStringLoader.LoadString(messageTemplateResourceType.FullName,
                    this.MessageTemplateResourceName,
                    messageTemplateResourceType.Assembly);
            }

            return null;
        }

        private Type GetMessageTemplateResourceType()
        {
            if (!string.IsNullOrEmpty(this.MessageTemplateResourceTypeName))
            {
                return Type.GetType(this.MessageTemplateResourceTypeName);
            }

            return null;
        }
    }
}
