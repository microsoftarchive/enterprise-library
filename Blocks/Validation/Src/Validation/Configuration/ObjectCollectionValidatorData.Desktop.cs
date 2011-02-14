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
using System.ComponentModel;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    [ResourceDescription(typeof(DesignResources), "ObjectCollectionValidatorDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "ObjectCollectionValidatorDataDisplayName")]
    partial class ObjectCollectionValidatorData
    {
        private static readonly AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ObjectCollectionValidatorData"/> class.</para>
        /// </summary>
        public ObjectCollectionValidatorData() { Type = typeof(ObjectCollectionValidator); }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ObjectCollectionValidatorData"/> class with a name.</para>
        /// </summary>
        /// <param name="name">The name for the instance.</param>
        public ObjectCollectionValidatorData(string name)
            : base(name, typeof(ObjectCollectionValidator))
        { }

        /// <summary>
        /// Gets or sets the target element type.
        /// </summary>
        /// <value>
        /// The target element type.
        /// </value>
        public Type TargetType
        {
            get { return (Type)typeConverter.ConvertFrom(TargetTypeName); }
            set { TargetTypeName = typeConverter.ConvertToString(value); }
        }

        private const string TargetTypePropertyName = "targetType";
        /// <summary>
        /// Gets or sets the name of the target element type for the represented validator.
        /// </summary>
        /// <seealso cref="ObjectCollectionValidatorData.TargetTypeName"/>
        [ConfigurationProperty(TargetTypePropertyName)]
        [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
        [ResourceDescription(typeof(DesignResources), "ObjectCollectionValidatorDataTargetTypeNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ObjectCollectionValidatorDataTargetTypeNameDisplayName")]
        public string TargetTypeName
        {
            get { return (string)this[TargetTypePropertyName]; }
            set { this[TargetTypePropertyName] = value; }
        }

        private const string TargetRulesetPropertyName = "targetRuleset";
        /// <summary>
        /// Gets or sets the name for the target ruleset for the represented validator.
        /// </summary>
        [ConfigurationProperty(TargetRulesetPropertyName, DefaultValue = "")]
        [ResourceDescription(typeof(DesignResources), "ObjectCollectionValidatorDataTargetRulesetDescription")]
        [ResourceDisplayName(typeof(DesignResources), "ObjectCollectionValidatorDataTargetRulesetDisplayName")]
        public string TargetRuleset
        {
            get { return (string)this[TargetRulesetPropertyName]; }
            set { this[TargetRulesetPropertyName] = value; }
        }
    }
}
