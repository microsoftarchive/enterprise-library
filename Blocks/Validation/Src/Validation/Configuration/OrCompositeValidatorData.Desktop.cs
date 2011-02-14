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
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    /// <summary>
    /// Configuration object to describe an instance of class <see cref="OrCompositeValidator"/>.
    /// </summary>
    /// <seealso cref="OrCompositeValidator"/>
    /// <seealso cref="ValidatorData"/>
    [ResourceDescription(typeof(DesignResources), "OrCompositeValidatorDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "OrCompositeValidatorDataDisplayName")]
    partial class OrCompositeValidatorData
    {
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="OrCompositeValidatorData"/> class.</para>
        /// </summary>
        public OrCompositeValidatorData() { Type = typeof(OrCompositeValidator); }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="OrCompositeValidatorData"/> class with a name.</para>
        /// </summary>
        /// <param name="name">The name for the instance.</param>
        public OrCompositeValidatorData(string name)
            : base(name, typeof(OrCompositeValidator))
        { }

        private const string ValidatorsPropertyName = "";
        /// <summary>
        /// Gets the collection with the definitions for the validators composed by 
        /// the represented <see cref="OrCompositeValidator"/>.
        /// </summary>
        [ConfigurationProperty(ValidatorsPropertyName, IsDefaultCollection = true)]
        [ResourceDescription(typeof(DesignResources), "OrCompositeValidatorDataValidatorsDescription")]
        [ResourceDisplayName(typeof(DesignResources), "OrCompositeValidatorDataValidatorsDisplayName")]
        [PromoteCommands]
        public ValidatorDataCollection Validators
        {
            get { return (ValidatorDataCollection)this[ValidatorsPropertyName]; }
        }
    }
}
