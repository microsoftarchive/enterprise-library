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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    [ResourceDescription(typeof(DesignResources), "ObjectValidatorDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "ObjectValidatorDataDisplayName")]
    partial class ObjectValidatorData
    {
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ObjectValidatorData"/> class.</para>
        /// </summary>
        public ObjectValidatorData()
        {
            this.Type = typeof(ObjectValidator);
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ObjectValidatorData"/> class with a name.</para>
        /// </summary>
        /// <param name="name">The name for the instance.</param>
        public ObjectValidatorData(string name)
            : base(name, typeof(ObjectValidator))
        { }

        private const string TargetRulesetPropertyName = "targetRuleset";
        /// <summary>
        /// Gets or sets the name for the target ruleset for the represented validator.
        /// </summary>
        [ConfigurationProperty(TargetRulesetPropertyName)]
        [ResourceDescription(typeof(DesignResources), "ObjectValidatorDataTargetRulesetDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ObjectValidatorDataTargetRulesetDisplayName")]
        public string TargetRuleset
        {
            get { return (string)this[TargetRulesetPropertyName]; }
            set { this[TargetRulesetPropertyName] = value; }
        }

        private const string ValidateActualTypePropertyName = "validateActualType";
        /// <summary>
        /// Gets or sets the value indicating whether to validate based on the static type or the actual type.
        /// </summary>
        [ConfigurationProperty(ValidateActualTypePropertyName, DefaultValue = false)]
        [ResourceDescription(typeof(DesignResources), "ObjectValidatorDataValidateActualTypeDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ObjectValidatorDataValidateActualTypeDisplayName")]
        public bool ValidateActualType
        {
            get { return (bool)this[ValidateActualTypePropertyName]; }
            set { this[ValidateActualTypePropertyName] = value; }
        }
    }
}
