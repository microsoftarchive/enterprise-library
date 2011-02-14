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
    [ResourceDescription(typeof(DesignResources), "PropertyComparisonValidatorDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "PropertyComparisonValidatorDataDisplayName")]
    partial class PropertyComparisonValidatorData
    {
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="PropertyComparisonValidator"/> class.</para>
        /// </summary>
        public PropertyComparisonValidatorData() { Type = typeof(PropertyComparisonValidator); }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="PropertyComparisonValidator"/> class with a name.</para>
        /// </summary>
        /// <param name="name"></param>
        public PropertyComparisonValidatorData(string name)
            : base(name, typeof(PropertyComparisonValidator))
        { }

        private const string ComparisonOperatorPropertyName = "operator";
        /// <summary>
        /// Gets or sets the <see cref="ComparisonOperator"/> describing the comparison that the represented <see cref="PropertyComparisonValidator"/>.
        /// </summary>
        [ConfigurationProperty(ComparisonOperatorPropertyName)]
        [ResourceDescription(typeof(DesignResources), "PropertyComparisonValidatorDataComparisonOperatorDescription")]
        [ResourceDisplayName(typeof(DesignResources), "PropertyComparisonValidatorDataComparisonOperatorDisplayName")]
        public ComparisonOperator ComparisonOperator
        {
            get { return (ComparisonOperator)this[ComparisonOperatorPropertyName]; }
            set { this[ComparisonOperatorPropertyName] = value; }
        }

        private const string PropertyToComparePropertyName = "propertyToCompare";
        /// <summary>
        /// Gets or sets the name of the property that the represented <see cref="PropertyComparisonValidator"/> will use to retrieve the value to compare.
        /// </summary>
        [ConfigurationProperty(PropertyToComparePropertyName, DefaultValue = "")]
        [ResourceDescription(typeof(DesignResources), "PropertyComparisonValidatorDataPropertyToCompareDescription")]
        [ResourceDisplayName(typeof(DesignResources), "PropertyComparisonValidatorDataPropertyToCompareDisplayName")]
        public string PropertyToCompare
        {
            get { return (string)this[PropertyToComparePropertyName]; }
            set { this[PropertyToComparePropertyName] = value; }
        }
    }
}
