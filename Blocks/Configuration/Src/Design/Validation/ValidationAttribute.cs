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
		private IServiceProvider serviceProvider;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ValidationAttribute"/> class.</para>
        /// </summary>
        protected ValidationAttribute()
        {            
        }

		/// <summary>
		/// Validate the given <paramref name="instance"/> and the <paramref name="propertyInfo"/>.
		/// </summary>
		/// <param name="instance">
		/// The instance to validate.
		/// </param>
		/// <param name="propertyInfo">
		/// The property containing the value to validate.
		/// </param>
		/// <param name="errors">
		/// The collection to add any errors that occur during the validation.
		/// </param>
		/// <param name="provider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>		
		public void Validate(object instance, PropertyInfo propertyInfo, IList<ValidationError> errors, IServiceProvider provider)
		{
			if (null == instance) throw new ArgumentNullException("instance");
			if (null == propertyInfo) throw new ArgumentNullException("propertyInfo");
			if (null == errors) throw new ArgumentNullException("errors");
			if (null == provider) throw new ArgumentNullException("serviceProvider");

			this.serviceProvider = provider;
			ValidateCore(instance, propertyInfo, errors);
		}

		/// <summary>
		/// Gets the a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
		/// </summary>
		/// <value>
		/// The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.
		/// </value>
		protected IServiceProvider ServiceProvider
		{
			get { return serviceProvider;  }
		}

		/// <summary>
		/// Validate the given <paramref name="instance"/> and the <paramref name="propertyInfo"/>.
		/// </summary>
		/// <param name="instance">
		/// The instance to validate.
		/// </param>
		/// <param name="propertyInfo">
		/// The property containing the value to validate.
		/// </param>
		/// <param name="errors">
		/// The collection to add any errors that occur during the validation.
		/// </param>		
		protected abstract void ValidateCore(object instance, PropertyInfo propertyInfo, IList<ValidationError> errors);
    }
}
