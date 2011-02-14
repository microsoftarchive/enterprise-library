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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    [ResourceDescription(typeof(DesignResources), "DateTimeRangeValidatorDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "DateTimeRangeValidatorDataDisplayName")]
    partial class DateTimeRangeValidatorData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeRangeValidatorData"/> class.
        /// </summary>
        public DateTimeRangeValidatorData() { Type = typeof(DateTimeRangeValidator); }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeRangeValidatorData"/> class.
        /// </summary>
        /// <param name="name">The configuration object name.</param>
        public DateTimeRangeValidatorData(string name)
            : base(name, typeof(DateTimeRangeValidator))
        { }

        /// <summary>
        /// Overridden in order to add Editor Attribute.
        /// </summary>
        [EditorAttribute(CommonDesignTime.EditorTypes.DatePickerEditor, CommonDesignTime.EditorTypes.FrameworkElement)]
        [Validation(ValidationDesignTime.Validators.RangeBoundValidator)]
        public override DateTime LowerBound
        {
            get { return base.LowerBound; }
            set { base.LowerBound = value; }
        }

        /// <summary>
        /// Overridden in order to add Editor Attribute.
        /// </summary>
        [EditorAttribute(CommonDesignTime.EditorTypes.DatePickerEditor, CommonDesignTime.EditorTypes.FrameworkElement)]
        [Validation(ValidationDesignTime.Validators.RangeBoundValidator)]
        public override DateTime UpperBound
        {
            get { return base.UpperBound; }
            set { base.UpperBound = value; }
        }
    }
}
