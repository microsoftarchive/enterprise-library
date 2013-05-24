#region license
//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Enterprise Application Block Library
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
#endregion
namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Represents a default element in the Enterprise Library configuration designer.
    /// </summary>
    public class DefaultElementConfigurationProperty : ElementProperty
    {
        ///<summary>
        /// Initializes a new instance of the <see cref="DefaultElementConfigurationProperty"/> class.
        ///</summary>
        ///<param name="serviceProvider">The service provider used to locate certain services for the configuration system.</param>
        ///<param name="parent">The parent <see cref="ElementViewModel"/> that owns the property.</param>
        ///<param name="declaringProperty">The description of the property.</param>
        [InjectionConstructor]
        public DefaultElementConfigurationProperty(IServiceProvider serviceProvider, ElementViewModel parent, PropertyDescriptor declaringProperty)
            : base(serviceProvider, parent, declaringProperty)
        {
        }

        ///<summary>
        /// Initializes a new instance of the <see cref="DefaultElementConfigurationProperty"/> class.
        ///</summary>
        ///<param name="serviceProvider">The service provider used to locate certain services for the configuration system.</param>
        ///<param name="parent">The parent <see cref="ElementViewModel"/> that owns the property.</param>
        ///<param name="declaringProperty">The description of the property.</param>
        ///<param name="additionalAttributes">Additional attributes made available to the property.</param>
        public DefaultElementConfigurationProperty(IServiceProvider serviceProvider, ElementViewModel parent, PropertyDescriptor declaringProperty, IEnumerable<Attribute> additionalAttributes)
            : base(serviceProvider, parent, declaringProperty, additionalAttributes)
        {
        }

        /// <summary>
        /// Gets the set of default property validators for this <see cref="Property"/>.
        /// </summary>
        /// <returns>The set of default validators for the property.</returns>
        protected override IEnumerable<Validator> GetDefaultPropertyValidators()
        {
            if (typeof(ConfigurationElementCollection).IsAssignableFrom(this.PropertyType))
            {
                yield break;
            }

            yield return new DefaultConfigurationPropertyValidator();
        }
    }
}
