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

    public abstract class Validator
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="Validator"/> class.</para>
        /// </summary>
        protected Validator()
        {
        }

        /// <summary>
        /// Validate the given <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">
        /// The instance to validate.
        /// </param>
        /// <param name="value">Value to validate</param>
        /// <param name="errors">
        /// The collection to add any errors that occur during the validation.
        /// </param>		
        protected abstract void ValidateCore(object instance, string value, IList<ValidationError> errors);

        public void Validate(object instance, string value, IList<ValidationError> errors)
        {
            if (null == instance) throw new ArgumentNullException("instance");
            if (null == errors) throw new ArgumentNullException("errors");

            ValidateCore(instance, value, errors);
        }
    }
}
