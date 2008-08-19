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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;
using ValidationRangeBoundaryType = Microsoft.Practices.EnterpriseLibrary.Validation.Validators.RangeBoundaryType;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes
{
    /// <summary>
    /// Respresents the designtime configuration node for any <see cref="RangeValidatorData{T}"/>.
    /// </summary>
    public abstract class RangeValidatorNodeBase<T> : ValueValidatorNode
        where T : IComparable<T>
    {
        private T lowerBound;
        private ValidationRangeBoundaryType lowerBoundType;
        private T upperBound;
        private ValidationRangeBoundaryType upperBoundType;



        /// <summary>
        /// Creates an instance of <see cref="RangeValidatorNodeBase{T}"/> based on runtime configuration data.
        /// </summary>
        /// <param name="validatorData">The corresponding runtime configuration data.</param>
        protected RangeValidatorNodeBase(RangeValidatorData<T> validatorData)
            : base(validatorData)
        {
            lowerBound = validatorData.LowerBound;
            lowerBoundType = validatorData.LowerBoundType;
            upperBound = validatorData.UpperBound;
            upperBoundType = validatorData.UpperBoundType;
        }

        /// <summary>
        /// Gets or sets the LowerBound value for this <see cref="RangeValidatorNodeBase{T}"/>.
        /// </summary>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("LowerBoundDescription", typeof(Resources))]
        public T LowerBound
        {
            get { return lowerBound; }
            set { lowerBound = value; }
        }

        /// <summary>
        /// Gets or sets the LowerBoundType value for this  <see cref="RangeValidatorNodeBase{T}"/>.
        /// </summary>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("RangeBoundaryTypeDescription", typeof(Resources))]
        public ValidationRangeBoundaryType LowerBoundType
        {
            get { return lowerBoundType; }
            set { lowerBoundType = value; }
        }

        /// <summary>
        /// Gets or sets the UpperBound value for this <see cref="RangeValidatorNodeBase{T}"/>.
        /// </summary>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("UpperBoundDescription", typeof(Resources))]
        public T UpperBound
        {
            get { return upperBound; }
            set { upperBound = value; }
        }
        /// <summary>
        /// Gets or sets the UpperBoundType value for this  <see cref="RangeValidatorNodeBase{T}"/>.
        /// </summary>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("UpperBoundTypeDescription", typeof(Resources))]
        public ValidationRangeBoundaryType UpperBoundType
        {
            get { return upperBoundType; }
            set { upperBoundType = value; }
        }


        /// <summary>
        /// Copies properties declared on this node to a runtime configuration data.
        /// </summary>
        /// <param name="rangeValidatorData">The runtime configuration data which should be updated with settings in this node.</param>
        protected void SetRangeValidatorBaseProperties(RangeValidatorData<T> rangeValidatorData)
        {
            base.SetValidatorBaseProperties(rangeValidatorData);
			
            rangeValidatorData.LowerBound = lowerBound;
            rangeValidatorData.LowerBoundType = lowerBoundType;
            rangeValidatorData.UpperBound = upperBound;
            rangeValidatorData.UpperBoundType = upperBoundType;
        }

        /// <summary>
        /// Performs validation for this node.
        /// </summary>
        /// <param name="errors">The list of errors to add any validation errors.</param>
        public override void Validate(IList<ValidationError> errors)
        {
            if (UpperBound != null && UpperBoundType != ValidationRangeBoundaryType.Ignore)
            {
                if (LowerBound != null && LowerBoundType != ValidationRangeBoundaryType.Ignore)
                {
                    if (LowerBound.CompareTo(UpperBound) > 0)
                    {
                        errors.Add(new ValidationError(this, "UpperBound", Resources.UpperBoundShouldBeGeaterThanLowerBound));
                    }
                }
            }
            base.Validate(errors);
        }
    }
}
