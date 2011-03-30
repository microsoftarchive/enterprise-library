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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    /// <summary>
    /// Describes a <see cref="DateTimeRangeValidator"/>.
    /// </summary>
    public partial class DateTimeRangeValidatorData : RangeValidatorData<DateTime>
    {
        /// <summary>
        /// Overridden in order to add DateTimeTypeConverter attribute.
        /// </summary>
        [TypeConverter(typeof(DateTimeTypeConverter))]
        public override DateTime LowerBound
        {
            get { return base.LowerBound; }
            set { base.LowerBound = value; }
        }
        
        /// <summary>
        /// Overridden in order to add DateTimeTypeConverter attribute.
        /// </summary>
        [TypeConverter(typeof(DateTimeTypeConverter))]
        public override DateTime  UpperBound
        {
	         get { return base.UpperBound; }
            set { base.UpperBound = value; }
        }
    }
}
