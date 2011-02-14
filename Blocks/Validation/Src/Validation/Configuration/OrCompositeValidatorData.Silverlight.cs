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
    /// <summary>
    /// Configuration object to describe an instance of class <see cref="OrCompositeValidator"/>.
    /// </summary>
    /// <seealso cref="OrCompositeValidator"/>
    /// <seealso cref="ValidatorData"/>
    [ContentProperty("Validators")]
    partial class OrCompositeValidatorData
    {
        private NamedElementCollection<ValidatorData> validators = new NamedElementCollection<ValidatorData>();
        /// <summary>
        /// Gets the collection with the definitions for the validators composed by 
        /// the represented <see cref="OrCompositeValidator"/>.
        /// </summary>
        public NamedElementCollection<ValidatorData> Validators
        {
            get { return this.validators; }
        }
    }
}
