﻿//===============================================================================
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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation
{
    /// <summary>
    /// Represents the description of how validation must be performed on a language element as defined by 
    /// <see cref="ValidationAttribute"/> attached to it.
    /// </summary>
    /// <seealso cref="ValidationAttribute"/>
    /// <seealso cref="IValidatedElement"/>
    public class ValidationAttributeValidatedElement : IValidatedElement
    {
        private MemberInfo memberInfo;
        private Type targetType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationAttributeValidatedElement"/> class for a property.
        /// </summary>
        /// <param name="propertyInfo">The property to represent.</param>
        public ValidationAttributeValidatedElement(PropertyInfo propertyInfo)
            : this(propertyInfo, propertyInfo.PropertyType)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationAttributeValidatedElement"/> class for a field.
        /// </summary>
        /// <param name="fieldInfo">The field to represent.</param>
        public ValidationAttributeValidatedElement(FieldInfo fieldInfo)
            : this(fieldInfo, fieldInfo.FieldType)
        { }

        /// <summary>
        /// Initializes a new instance of hte <see cref="ValidationAttributeValidatedElement"/> class for a 
        /// <see cref="MemberInfo"/> of a target <see cref="Type"/>.
        /// </summary>
        /// <param name="memberInfo">The member info to represent.</param>
        /// <param name="targetType">The target type for member info.</param>
        protected ValidationAttributeValidatedElement(MemberInfo memberInfo, Type targetType)
        {
            this.memberInfo = memberInfo;
            this.targetType = targetType;
        }

        IEnumerable<IValidatorDescriptor> IValidatedElement.GetValidatorDescriptors()
        {
            var attributes =
                ValidationReflectionHelper.GetCustomAttributes(this.memberInfo, typeof(ValidationAttribute), true);
            if (attributes.Length == 0)
            {
                return new IValidatorDescriptor[0];
            }
            else
            {
                return new IValidatorDescriptor[]
                {
                    new ValidationAttributeValidatorDescriptor(attributes.Cast<ValidationAttribute>())
                };
            }
        }

        CompositionType IValidatedElement.CompositionType
        {
            get { return CompositionType.And; }
        }

        string IValidatedElement.CompositionMessageTemplate
        {
            get { return null; }
        }

        string IValidatedElement.CompositionTag
        {
            get { return null; }
        }

        bool IValidatedElement.IgnoreNulls
        {
            get { return false; }
        }

        string IValidatedElement.IgnoreNullsMessageTemplate
        {
            get { return null; }
        }

        string IValidatedElement.IgnoreNullsTag
        {
            get { return null; }
        }

        MemberInfo IValidatedElement.MemberInfo
        {
            get { return this.memberInfo; }
        }

        Type IValidatedElement.TargetType
        {
            get { return this.targetType; }
        }

        internal class ValidationAttributeValidatorDescriptor : IValidatorDescriptor
        {
            private readonly IEnumerable<ValidationAttribute> attributes;

            internal ValidationAttributeValidatorDescriptor(IEnumerable<ValidationAttribute> attributes)
            {
                this.attributes = attributes;
            }

            public Validator CreateValidator(Type targetType, Type ownerType, MemberValueAccessBuilder memberValueAccessBuilder)
            {
                return new ValidationAttributeValidator(this.attributes);
            }
        }
    }
}
