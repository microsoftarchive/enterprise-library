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
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using ValidationRangeBoundaryType = Microsoft.Practices.EnterpriseLibrary.Validation.Validators.RangeBoundaryType;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes
{
    /// <summary>
    /// Respresents the designtime configuration node for a <see cref="StringLengthValidatorData"/>.
    /// </summary>
    public class StringLengthValidatorNode : ValueValidatorNode
    {
        private int lowerBound;
        private ValidationRangeBoundaryType lowerBoundType;
        private int upperBound;
        private ValidationRangeBoundaryType upperBoundType;

        /// <summary>
        /// Creates an instance of <see cref="StringLengthValidatorNode"/> based on default values.
        /// </summary>
        public StringLengthValidatorNode()
            : this(new StringLengthValidatorData(Resources.StringLengthValidatorNodeName))
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="StringLengthValidatorNode"/> based on runtime configuration data.
        /// </summary>
        /// <param name="validatorData">The corresponding runtime configuration data.</param>
        public StringLengthValidatorNode(StringLengthValidatorData validatorData)
            : base(validatorData)
        {
			lowerBound = validatorData.LowerBound;
			lowerBoundType = validatorData.LowerBoundType;
			upperBound = validatorData.UpperBound;
			upperBoundType = validatorData.UpperBoundType;
		}

        /// <summary>
        /// Gets or sets the LowerBound value for this validator.
        /// </summary>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("LowerBoundDescription", typeof(Resources))]
        public int LowerBound
        {
            get { return lowerBound; }
            set { lowerBound = value; }
        }

        /// <summary>
        /// Gets or sets the LowerBoundType value for this validator.
        /// </summary>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("RangeBoundaryTypeDescription", typeof(Resources))]
        public ValidationRangeBoundaryType LowerBoundType
        {
            get { return lowerBoundType; }
            set { lowerBoundType = value; }
        }

        /// <summary>
        /// Gets or sets the UpperBound value for this validator.
        /// </summary>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("UpperBoundDescription", typeof(Resources))]
        public int UpperBound
        {
            get { return upperBound; }
            set { upperBound = value; }
        }

        /// <summary>
        /// Gets or sets the UpperBoundType value for this validator.
        /// </summary>
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("UpperBoundTypeDescription", typeof(Resources))]
        public ValidationRangeBoundaryType UpperBoundType
        {
            get { return upperBoundType; }
            set { upperBoundType = value; }
        }

        /// <summary>
        /// Returns the runtime configuration data that is represented by this node.
        /// </summary>
        /// <returns>An instance of <see cref="StringLengthValidatorData"/> that can be persisted to a configuration file.</returns>
        public override ValidatorData CreateValidatorData()
        {
            StringLengthValidatorData validatorData = new StringLengthValidatorData(Name);
            SetValidatorBaseProperties(validatorData);

            validatorData.LowerBound = lowerBound;
            validatorData.LowerBoundType = lowerBoundType;
            validatorData.UpperBound = upperBound;
            validatorData.UpperBoundType = upperBoundType;
			validatorData.Negated = Negated;

            return validatorData;
        }
    }
}
