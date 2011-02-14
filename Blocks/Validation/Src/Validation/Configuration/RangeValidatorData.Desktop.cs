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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System.ComponentModel;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    [ResourceDisplayName(typeof(DesignResources), "RangeValidatorDataDisplayName")]
    [ResourceDescription(typeof(DesignResources), "RangeValidatorDataDescription")]
	partial class RangeValidatorData
	{
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RangeValidatorData"/> class.</para>
		/// </summary>
		public RangeValidatorData() { Type = typeof(RangeValidator); }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RangeValidatorData"/> class with a name.</para>
		/// </summary>
		public RangeValidatorData(string name)
			: base(name, typeof(RangeValidator))
		{ }

        private const string CulturePropertyName = "culture";

        /// <summary>
        /// Gets or sets the name of the culture that will be used to read lower and upperbound from the configuration file.
        /// </summary>
        [ConfigurationProperty(CulturePropertyName)]
        [TypeConverter(typeof(ConfigurationCultureInfoConverter))]
        [ViewModel(ValidationDesignTime.ViewModelTypeNames.RangeValidatorCultureProperty)]
        [ResourceDescription(typeof(DesignResources), "RangeValidatorDataCultureDescription")]
        [ResourceDisplayName(typeof(DesignResources), "RangeValidatorDataCultureDisplayName")]
        public CultureInfo Culture
        {
            get { return (CultureInfo)this[CulturePropertyName]; }
            set { this[CulturePropertyName] = value; }
        }
	}
}
