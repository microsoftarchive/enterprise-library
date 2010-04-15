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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using System.Configuration;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Text.RegularExpressions;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{

    /// <summary>
    /// A validator class that validates whether a <see cref="Property"/>'s <see cref="Property.Value"/> is a valid <see cref="NameValueCollection"/>.
    /// </summary>
    public class NameValueCollectionValidator : Validator
    {
        /// <summary>
        /// Validates whether the <see cref="Property"/> <paramref name="instance"/> has a <see cref="NameValueCollection"/> that can be serialized as a set of XML attributes.
        /// </summary>
        /// <param name="instance">The <see cref="Property"/> instance to validate.</param>
        /// <param name="value">This value is being ignored by this validator.</param>
        /// <param name="results">The collection to wich any results that occur during the validation can be added.</param>		
        protected override void ValidateCore(object instance, string value, IList<ValidationResult> results)
        {
            var collection = ((Property)instance).Value as NameValueCollection;

            if (collection == null) return;

            foreach (var key in collection.AllKeys)
            {
                if (string.IsNullOrEmpty(key) &&
                    !string.IsNullOrEmpty(collection[key])
                    )
                {
                    results.Add(new PropertyValidationResult((Property)instance, Resources.InvalidKeyValueError));
                }

                if (!string.IsNullOrEmpty(key) &&
                    !AttributeKeyValidator.IsValid(key))
                {
                    results.Add(new PropertyValidationResult((Property)instance, Resources.InvalidKeyValueError));
                }
            }
        }

        private static class AttributeKeyValidator
        {
            private static readonly Regex Expression = new Regex(@"^[a-zA-Z_]\w*$");

            public static bool IsValid(string key)
            {
                return Expression.Match(key).Success;
            }
        }
    }
}
