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
    /// A baseclass for Validators used in the configuration designer.
    /// </summary>
    public abstract class Validator
    {
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="Validator"/> class.</para>
        /// </summary>
        protected Validator()
        {
        }

        /// <summary>
        /// When implemented in a derived class, validates <paramref name="value"/> as part of the <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <param name="value">Value to validate</param>
        /// <param name="results">The collection to wich any results that occur during the validation can be added.</param>		
        protected abstract void ValidateCore(object instance, string value, IList<ValidationResult> results);

        /// <summary>
        /// Validates <paramref name="value"/> as part of the <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <param name="value">Value to validate.</param>
        /// <param name="results">The collection to wich any results that occur during the validation can be added.</param>		
        public void Validate(object instance, string value, IList<ValidationResult> results)
        {
            if (null == instance) throw new ArgumentNullException("instance");
            if (null == results) throw new ArgumentNullException("results");

            ValidateCore(instance, value, results);
        }
    }

    
}
