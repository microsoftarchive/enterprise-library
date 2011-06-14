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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Windows.Markup;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    /// <summary>
    /// Represents validation information for a type and its members.
    /// </summary>
    /// <seealso cref="ValidationRulesetData"/>
    [ContentProperty("Rulesets")]
    public class ValidatedTypeReference : NamedConfigurationElement
    {
        /// <summary>
        /// <para>Creates a new instance of the <see cref="ValidatedTypeReference"/> class with a type.</para>
        /// </summary>
        /// <param name="type">The represented type.</param>
        public static ValidatedTypeReference Create(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            return new ValidatedTypeReference { Name = type.FullName };
        }

        private readonly NamedElementCollection<ValidationRulesetData> rulesets = new NamedElementCollection<ValidationRulesetData>();
        /// <summary>
        /// Gets the collection with the validation rulesets configured the represented type.
        /// </summary>
        public NamedElementCollection<ValidationRulesetData> Rulesets
        {
            get { return this.rulesets; }
        }

        /// <summary>
        /// Gets or sets the default ruleset for the represented type.
        /// </summary>
        public string DefaultRuleset { get; set; }
    }
}
