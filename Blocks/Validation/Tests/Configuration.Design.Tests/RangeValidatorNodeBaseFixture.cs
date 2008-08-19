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

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ValidationBoundaryType = Microsoft.Practices.EnterpriseLibrary.Validation.Validators.RangeBoundaryType;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Tests
{
    [TestClass]
    public class RangeValidatorNodeBaseFixture
    {
        [TestMethod]
        public void RangeValidatorPropertiesAreCopiedOnNode()
        {
            MyRangeValidatorData validationData = new MyRangeValidatorData();
            validationData.LowerBound = 0;
            validationData.LowerBoundType = ValidationBoundaryType.Inclusive;
            validationData.UpperBound = 10;
            validationData.UpperBoundType = ValidationBoundaryType.Exclusive;

            MyRangeValidatorNode validationNode = new MyRangeValidatorNode(validationData);
            Assert.AreEqual(0, validationNode.LowerBound);
            Assert.AreEqual(ValidationBoundaryType.Inclusive, validationNode.LowerBoundType);
            Assert.AreEqual(10, validationNode.UpperBound);
            Assert.AreEqual(ValidationBoundaryType.Exclusive, validationNode.UpperBoundType);
        }

        [TestMethod]
        public void RangeValidatorPropertiesAreCopiedOnConfigurationData()
        {
            MyRangeValidatorNode validationNode = new MyRangeValidatorNode(new MyRangeValidatorData());
            validationNode.LowerBound = 0;
            validationNode.LowerBoundType = ValidationBoundaryType.Inclusive;
            validationNode.UpperBound = 10;
            validationNode.UpperBoundType = ValidationBoundaryType.Exclusive;

            MyRangeValidatorData validationData = validationNode.CreateValidatorData() as MyRangeValidatorData;

            Assert.IsNotNull(validationData);
            Assert.AreEqual(0, validationData.LowerBound);
            Assert.AreEqual(ValidationBoundaryType.Inclusive, validationData.LowerBoundType);
            Assert.AreEqual(10, validationData.UpperBound);
            Assert.AreEqual(ValidationBoundaryType.Exclusive, validationData.UpperBoundType);
        }

        [TestMethod]
        public void RangeValidationWithUpperBoundLessThanLowerBoundIsInvalid()
        {
            MyRangeValidatorNode validationNode = new MyRangeValidatorNode(new MyRangeValidatorData());
            validationNode.LowerBound = 10;
            validationNode.LowerBoundType = ValidationBoundaryType.Inclusive;
            validationNode.UpperBound = 0;
            validationNode.UpperBoundType = ValidationBoundaryType.Inclusive;

            List<ValidationError> validationErrors = new List<ValidationError>();
            validationNode.Validate(validationErrors);

            Assert.AreEqual(1, validationErrors.Count);
        }

        class MyRangeValidatorNode : RangeValidatorNodeBase<int>
        {
            public MyRangeValidatorNode(MyRangeValidatorData configurationData)
                : base(configurationData) {}

            public override ValidatorData CreateValidatorData()
            {
                MyRangeValidatorData validatorData = new MyRangeValidatorData(Name);
                SetRangeValidatorBaseProperties(validatorData);

                return validatorData;
            }
        }

        class MyRangeValidatorData : RangeValidatorData<int>
        {
            public MyRangeValidatorData()
                : this("MyRangeValidatorData") {}

            public MyRangeValidatorData(string name)
                : base(name, typeof(MyRangeValidatorData)) {}
        }
    }
}