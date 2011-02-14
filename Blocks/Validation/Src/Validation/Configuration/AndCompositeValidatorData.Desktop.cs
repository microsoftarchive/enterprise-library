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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    /// <summary>
    /// Configuration object to describe an instance of class <see cref="AndCompositeValidator"/>.
    /// </summary>
    /// <seealso cref="AndCompositeValidator"/>
    /// <seealso cref="ValidatorData"/>
    [ResourceDescription(typeof(DesignResources), "AndCompositeValidatorDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "AndCompositeValidatorDataDisplayName")]
    partial class AndCompositeValidatorData
    {
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="AndCompositeValidatorData"/> class.</para>
        /// </summary>
        public AndCompositeValidatorData() { Type = typeof(AndCompositeValidator); }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="AndCompositeValidatorData"/> class with a name.</para>
        /// </summary>
        /// <param name="name">The name for the instance.</param>
        public AndCompositeValidatorData(string name)
            : base(name, typeof(AndCompositeValidator))
        { }

        private const string ValidatorsPropertyName = "";
        /// <summary>
        /// Gets the collection with the definitions for the validators composed by 
        /// the represented <see cref="AndCompositeValidator"/>.
        /// </summary>
        [ConfigurationProperty(ValidatorsPropertyName, IsDefaultCollection = true)]
        [ResourceDescription(typeof(DesignResources), "AndCompositeValidatorDataValidatorsDescription")]
        [ResourceDisplayName(typeof(DesignResources), "AndCompositeValidatorDataValidatorsDisplayName")]
        [PromoteCommands]
        public ValidatorDataCollection Validators
        {
            get { return (ValidatorDataCollection)this[ValidatorsPropertyName]; }
        }

        /// <summary>
        /// Overridden in order to hide from the configuration designtime.
        /// </summary>
        [Browsable(false)]
        public override string MessageTemplate
        {
            get { return base.MessageTemplate; }
            set { base.MessageTemplate = value; }
        }

        /// <summary>
        /// Overridden in order to hide from the configuration designtime.
        /// </summary>
        [Browsable(false)]
        public override string MessageTemplateResourceName
        {
            get { return base.MessageTemplateResourceName; }
            set { base.MessageTemplateResourceName = value; }
        }
        /// <summary>
        /// Overridden in order to hide from the configuration designtime.
        /// </summary>
        [Browsable(false)]
        public override string MessageTemplateResourceTypeName
        {
            get { return base.MessageTemplateResourceTypeName; }
            set { base.MessageTemplateResourceTypeName = value; }
        }
        /// <summary>
        /// Overridden in order to hide from the configuration designtime.
        /// </summary>
        [Browsable(false)]
        public override string Tag
        {
            get { return base.Tag; }
            set { base.Tag = value; }
        }
    }
}
