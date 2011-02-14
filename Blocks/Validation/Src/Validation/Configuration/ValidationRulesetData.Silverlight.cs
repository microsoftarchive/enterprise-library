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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Windows.Markup;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    /// <summary>
    /// Represents a ruleset for a validated type.
    /// </summary>
    /// <seealso cref="ValidatedTypeReference"/>
    /// <remarks>
    /// Self validation is not supported thorugh configuration.
    /// </remarks>
    /// <seealso cref="ValidatedTypeReference"/>
    [ContentProperty("Validators")]
    public class ValidationRulesetData : NamedConfigurationElement
    {
        private NamedElementCollection<ValidatorData> validators = new NamedElementCollection<ValidatorData>();
        /// <summary>
        /// Gets the collection of validators configured for the type owning the ruleset.
        /// </summary>
        public NamedElementCollection<ValidatorData> Validators
        {
            get { return this.validators; }
        }

        private NamedElementCollection<ValidatedFieldReference> fields = new NamedElementCollection<ValidatedFieldReference>();
        /// <summary>
        /// Gets the collection of validated fields for the type owning the ruleset.
        /// </summary>
        public NamedElementCollection<ValidatedFieldReference> Fields
        {
            get { return this.fields; }
        }

        private NamedElementCollection<ValidatedMethodReference> methods = new NamedElementCollection<ValidatedMethodReference>();
        /// <summary>
        /// Gets the collection of validated methods for the type owning the ruleset.
        /// </summary>
        public NamedElementCollection<ValidatedMethodReference> Methods
        {
            get { return this.methods; }
        }

        private NamedElementCollection<ValidatedPropertyReference> properties = new NamedElementCollection<ValidatedPropertyReference>();
        /// <summary>
        /// Gets the collection of validated properties for the type owning the ruleset.
        /// </summary>
        public NamedElementCollection<ValidatedPropertyReference> Properties
        {
            get { return this.properties; }
        }
    }
}
