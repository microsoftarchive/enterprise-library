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

using System.Windows.Markup;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    [ContentProperty("Domain")]
    partial class DomainValidatorData
    {
        private NamedElementCollection<DomainConfigurationElement> domain = new NamedElementCollection<DomainConfigurationElement>();
        /// <summary>
        /// Gets the collection of elements for the domain for the represented <see cref="DomainValidator{T}"/>.
        /// </summary>
        public NamedElementCollection<DomainConfigurationElement> Domain
        {
            get { return this.domain; }
        }
    }
}
