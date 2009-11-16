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
using System.Reflection;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// Represents an attribute that will validate a property or field.  The class is abstract.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public abstract class ValidationAttribute : Attribute
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ValidationAttribute"/> class.</para>
        /// </summary>
        protected ValidationAttribute()
        {
        }

        /// <summary>
        /// Validate the given <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">
        /// The instance to validate.
        /// </param>
        /// <param name="errors">
        /// The collection to add any errors that occur during the validation.
        /// </param>
        /// <param name="provider">The mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>		
        public void Validate(object instance, IList<ValidationError> errors)
        {
            if (null == instance) throw new ArgumentNullException("instance");
            if (null == errors) throw new ArgumentNullException("errors");

            ValidateCore(instance, errors);
        }

        /// <summary>
        /// Validate the given <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">
        /// The instance to validate.
        /// </param>
        /// <param name="errors">
        /// The collection to add any errors that occur during the validation.
        /// </param>		
        protected abstract void ValidateCore(object instance, IList<ValidationError> errors);
    }
}
